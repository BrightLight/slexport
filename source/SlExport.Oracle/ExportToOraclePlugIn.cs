namespace SlExport.Oracle
{
  using System;
  using System.ComponentModel.Composition;
  using System.IO;
  using System.Linq;
  using global::Oracle.ManagedDataAccess.Client;

  /// <summary>
  /// Export plugin that writes the Sisulizer statistics into an Oracle database table
  /// </summary>
  /// <remarks>
  /// TODO check if table already exists. If not, create it
  /// </remarks>
  /// <seealso cref="SlExport.ISisulizerExportPlugin" />
  [Export(typeof(ISisulizerExportPlugin))]
  public class ExportToOraclePlugIn : ISisulizerExportPlugin
  {
    public string PlugInId => "Oracle";

    public string PlugInName => "Export to Oracle database using ODP.Net";

    public void Execute(ISisulizerFile sisulizerFile, CommonExportOptions exportOptions)
    {
      if (exportOptions is OracleExportOptions oracleExportOptions)
      {
        using (var oracleConnection = new OracleConnection(oracleExportOptions.ConnectionString))
        {
          var dbCommand = oracleConnection.CreateCommand();
          dbCommand.CommandText = $"insert into {oracleExportOptions.TableName} (source, product, version, build, createdon, languagecode, statistictype, nottranslated, bestguess, bestguessinvalid, autotranslated, autotranslatedinvalid, translated, translatedinvalid, forreview, forreviewinvalid, complete, completeinvalid, total)"
                                + $"values (:source, :product, :version, :build, :createdon, :languagecode, :statistictype, :nottranslated, :bestguess, :bestguessinvalid, :autotranslated, :autotranslatedinvalid, :translated, :translatedinvalid, :forreview, :forreviewinvalid, :complete, :completeinvalid, :total)";

          var createdOn = DateTime.Now;
          foreach (var languageStats in sisulizerFile.Languages)
          {
            foreach (var statType in new[] { StatType.Strings, StatType.Words })
            {
              this.CreateParameter(dbCommand, "source", OracleDbType.Varchar2).Value = Path.GetFileNameWithoutExtension(sisulizerFile.FileName);
              this.CreateParameter(dbCommand, "product", OracleDbType.Varchar2).Value = oracleExportOptions.Product;
              this.CreateParameter(dbCommand, "version", OracleDbType.Varchar2).Value = oracleExportOptions.Version;
              this.CreateParameter(dbCommand, "build", OracleDbType.Int32).Value = oracleExportOptions.Build;
              this.CreateParameter(dbCommand, "createdon", OracleDbType.Date).Value = createdOn;
              this.CreateParameter(dbCommand, "languagecode", OracleDbType.Varchar2).Value = languageStats.Language;
              this.CreateParameter(dbCommand, "statistictype", OracleDbType.Varchar2).Value = this.GetStatisticType(statType);
              this.CreateParameter(dbCommand, "nottranslated", OracleDbType.Varchar2).Value = this.GetNotTranslated(languageStats, statType);
              this.CreateParameter(dbCommand, "bestguess", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.BestGuess, statType, true);
              this.CreateParameter(dbCommand, "bestguessinvalid", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.BestGuess, statType, false);
              this.CreateParameter(dbCommand, "autotranslated", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.AutoTranslated, statType, true);
              this.CreateParameter(dbCommand, "autotranslatedinvalid", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.AutoTranslated, statType, false);
              this.CreateParameter(dbCommand, "translated", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.Translated, statType, true);
              this.CreateParameter(dbCommand, "translatedinvalid", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.Translated, statType, false);
              this.CreateParameter(dbCommand, "forreview", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.ForReview, statType, true);
              this.CreateParameter(dbCommand, "forreviewinvalid", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.ForReview, statType, false);
              this.CreateParameter(dbCommand, "complete", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.Completed, statType, true);
              this.CreateParameter(dbCommand, "completeinvalid", OracleDbType.Varchar2).Value = this.GetStatValue(languageStats, LangStatus.Completed, statType, false);
              this.CreateParameter(dbCommand, "total", OracleDbType.Varchar2).Value = this.GetNativeCount(languageStats, statType);
              var result = dbCommand.ExecuteNonQuery();
            }
          }
        }
      }
    }

    private string GetStatisticType(StatType statType)
    {
      switch (statType)
      {
        case StatType.Strings: return "Strings";
        case StatType.Words: return "Words";
        default: return string.Empty;
      }
    }

    private int GetNotTranslated(IProjectLanguage projectLanguage, StatType statType)
    {
      return this.GetNativeCount(projectLanguage, statType)
         - this.GetStatValue(projectLanguage, LangStatus.BestGuess, statType, true)
         - this.GetStatValue(projectLanguage, LangStatus.AutoTranslated, statType, true)
         - this.GetStatValue(projectLanguage, LangStatus.Translated, statType, true)
         - this.GetStatValue(projectLanguage, LangStatus.ForReview, statType, true)
         - this.GetStatValue(projectLanguage, LangStatus.Completed, statType, true);
    }

    private int GetNativeCount(IProjectLanguage projectLanguage, StatType statType)
    {
      return statType == StatType.Strings ? projectLanguage.NativeStringCount : projectLanguage.NativeWordCount;
    }

    private OracleParameter CreateParameter(OracleCommand dbCommand, string name, OracleDbType dbType)
    {
      var parameter = dbCommand.CreateParameter();
      parameter.ParameterName = name;
      parameter.Direction = System.Data.ParameterDirection.Input;
      parameter.OracleDbType = dbType;
      dbCommand.Parameters.Add(parameter);
      return parameter;
    }

    private LangCount GetLanguageCountByLanguageStatus(IProjectLanguage projectLanguage, LangStatus langStatus)
    {
      var langCount = projectLanguage.CountByStatus.FirstOrDefault(x => x.Item1 == langStatus);
      if (langCount != default)
      {
        return langCount.Item2;
      }

      return default;
    }

    private int GetStatValue(IProjectLanguage projectLanguage, LangStatus langStatus, StatType statType, bool valid)
    {
      var langCount = (this.GetLanguageCountByLanguageStatus(projectLanguage, langStatus));
      return this.GetStatValue(langCount, statType, valid);
    }

    private int GetStatValue(LangCount langCount, StatType statType, bool valid)
    {
      switch (statType)
      {
        case StatType.Strings:
          return valid ? langCount.StringCount : langCount.InvalidStringCount;
        case StatType.Words:
          return valid ? langCount.WordCount : langCount.InvalidWordCount;
        default: return 0;
      }
    }

    private enum StatType
    {
      Strings,
      Words,
    }
  }
}
