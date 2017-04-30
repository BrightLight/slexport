namespace SlExport
{
  using System;
  using System.Collections.Generic;

  public interface IProjectLanguage
  {
    string Language { get; }

    int NativeCount { get; }

    IEnumerable<Tuple<LangStatus, int>> CountByStatus { get; }
  }
}