namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel.Composition;
  using System.IO;
  using System.Linq;
  using System.Text;

  [Export(typeof(ISisulizerExportPlugin))]
  public class ExportToXmlPlugIn : ISisulizerExportPlugin
  {
    public string PlugInId => "Xml";

    public string PlugInName => "Export to XML file";

    public void Execute(ISisulizerFile sisulizerFile, CommonExportOptions exportOptions)
    {
      if (exportOptions is XmlExportOptions xmlExportOptions)
      {
        File.WriteAllText(xmlExportOptions.OutputFilename, this.ToXml(sisulizerFile));
      }
    }

    private string ToXml(ISisulizerFile sisulizerFile)
    {
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"<TranslationStatus file=\"{sisulizerFile.FileName}\">");
      stringBuilder.Append(this.ToXml(sisulizerFile.Languages));
      foreach (var project in sisulizerFile.Projects)
      {
        stringBuilder.Append(this.ToXml(project));
      }

      stringBuilder.AppendLine("</TranslationStatus>");
      return stringBuilder.ToString();
    }

    private string ToXml(ISisulizerProject sisulizerProject)
    {
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"<Source>{sisulizerProject.Name}>");
      stringBuilder.Append(this.ToXml(sisulizerProject.Languages));
      stringBuilder.AppendLine("</Source>");
      return stringBuilder.ToString();
    }

    private string ToXml(IEnumerable<IProjectLanguage> languages)
    {
      var stringBuilder = new StringBuilder();
      foreach (var projectLanguage in languages.OrderByDescending(x => x.Language.Length).ThenBy(x => x.Language))
      {
        stringBuilder.Append(this.ToXml(projectLanguage));
      }

      return stringBuilder.ToString();
    }

    private string ToXml(IProjectLanguage projectLanguage)
    {
      // ToDo: NotTranslated berechnen aus "Native" - alle anderen
      var stringBuilder = new StringBuilder();
      foreach (var (languageStatus, stringCount) in projectLanguage.StringCountByStatus.OrderBy(x => x.Item1))
      {
        stringBuilder.AppendLine($"<language id=\"{projectLanguage.Language}\" status=\"{(int)languageStatus}\" statusText=\"{languageStatus}\">{stringCount}</language>");
      }

      return stringBuilder.ToString();
    }
  }
}