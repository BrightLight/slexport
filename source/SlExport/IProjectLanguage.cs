namespace SlExport
{
  using System.Collections.Generic;

  public interface IProjectLanguage
  {
    string Language { get; }

    int NativeStringCount { get; }

    IEnumerable<(LangStatus, int StringCount, int WordCount)> CountByStatus { get; }
  }
}