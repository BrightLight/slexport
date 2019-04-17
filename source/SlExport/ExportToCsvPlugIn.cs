namespace SlExport
{
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
      foreach (var project in sisulizerFile.Projects)
      {
        var count = project.Languages.Any() ? project.Languages.Max(x => x.NativeStringCount) : 0;
        stringBuilder.AppendLine($"{project.Name};{count}");
      }

      return stringBuilder.ToString();
    }
  }
}