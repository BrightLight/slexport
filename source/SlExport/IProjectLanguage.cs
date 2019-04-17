namespace SlExport
{
  using System.Collections.Generic;

  public interface IProjectLanguage
  {
    string Language { get; }

    int NativeStringCount { get; }

    int NativeWordCount { get; }

    IEnumerable<(LangStatus, LangCount)> CountByStatus { get; }
  }
}