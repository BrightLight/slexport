namespace SlExport
{
  using System.Collections.Generic;

  public interface ISisulizerFile
  {
    string FileName { get; }

    IEnumerable<ISisulizerProject> Projects { get; }

    IEnumerable<IProjectLanguage> Languages { get; }
  }
}