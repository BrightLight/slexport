namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  internal class ProjectLanguage : IProjectLanguage
  {
    private readonly Dictionary<LangStatus, (int StringCount, int WordCount)> countByStatus = new Dictionary<LangStatus, (int, int)>();

    public ProjectLanguage(string language)
    {
      this.Language = language;
    }

    public string Language { get; }

    public int NativeStringCount { get; private set; }

    public int NativeWordCount { get; private set; }

    public IEnumerable<(LangStatus, int StringCount, int WordCount)> CountByStatus
    {
      get { return this.countByStatus.Select(x => (x.Key, x.Value.StringCount, x.Value.WordCount)); }
    }

    public void IncByStatus(LangStatus status, string nativeText, string translatedText)
    {
      this.NativeStringCount++;
      this.NativeWordCount = DetermineWordCount(nativeText);

      if (!this.countByStatus.TryGetValue(status, out var count))
      {
        this.countByStatus.Add(status, (1, DetermineWordCount(translatedText)));
      }
      else
      {
        this.countByStatus[status] = (count.StringCount + 1, count.WordCount + DetermineWordCount(translatedText));
      }
    }

    private static int DetermineWordCount(string text)
    {
      return text.Split(' ').Length;
    }
  }
}