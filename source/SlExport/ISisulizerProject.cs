namespace SlExport
{
  using System.Collections.Generic;

  public interface ISisulizerProject
  {
    string Name { get; }

    IEnumerable<IProjectLanguage> Languages { get; }
  }
}