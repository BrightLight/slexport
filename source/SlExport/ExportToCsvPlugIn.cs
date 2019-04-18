namespace SlExport
{
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.IO;
  using System.Linq;
  using System.Text;

  [Export(typeof(ISisulizerExportPlugin))]
  public class ExportToCsvPlugIn : ISisulizerExportPlugin
  {
    public string PlugInId => "Csv";

    public string PlugInName => "Export to CSV file";

    public void Execute(ISisulizerFile sisulizerFile, CommonExportOptions exportOptions)
    {
      if (exportOptions is CsvExportOptions csvExportOptions)
      {
        File.WriteAllText(csvExportOptions.OutputFilename, this.ToCsv(sisulizerFile));
      }
    }

    private string ToCsv(ISisulizerFile sisulizerFile)
    {
      var stringBuilder = new StringBuilder();
      
      // header
      stringBuilder.AppendLine($"Project;Language;Status;StringCount;InvalidStringCount;WordCount;InvalidWordCount");

      // totals over all projects in Sisulizer file
      this.ToCsv(stringBuilder, "total", sisulizerFile.Languages);

      // stats for each project individually
      foreach (var project in sisulizerFile.Projects)
      {
        this.ToCsv(stringBuilder, project.Name, project.Languages);
      }

      return stringBuilder.ToString();
    }

    private void ToCsv(StringBuilder stringBuilder, string projectname, IEnumerable<IProjectLanguage> projectLanguages)
    {
      foreach (var language in projectLanguages)
      {
        foreach (var (langStatus, langCount) in language.CountByStatus)
        {
          stringBuilder.AppendLine($"{projectname};{language.Language};{langStatus};{langCount.StringCount};{langCount.InvalidStringCount};{langCount.WordCount};{langCount.InvalidWordCount}");
        }
      }
    }
  }
}