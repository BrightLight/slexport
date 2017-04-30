namespace SlExport
{
  public interface ISisulizerStatsExporer
  {
    string PlugInId { get; }

    string PlugInName { get; }

    void Execute(ISisulizerFile sisulizerFile);
  }
}