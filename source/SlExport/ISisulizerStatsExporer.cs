namespace SlExport
{
  public interface ISisulizerExportPlugin
  {
    string PlugInId { get; }

    string PlugInName { get; }

    void Execute(ISisulizerFile sisulizerFile, CommonExportOptions exportOptions);
  }
}