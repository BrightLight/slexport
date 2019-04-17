namespace SlExport
{
  using System;
  using System.Collections.Generic;

  public interface IProjectLanguage
  {
    string Language { get; }

    int NativeStringCount { get; }

    IEnumerable<Tuple<LangStatus, int>> StringCountByStatus { get; }
  }
}