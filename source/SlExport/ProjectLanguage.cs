namespace SlExport
{
  using System.Collections.Generic;
  using System.Linq;

  internal class ProjectLanguage : IProjectLanguage
  {
    private readonly Dictionary<LangStatus, LangCount> countByStatus = new Dictionary<LangStatus, LangCount>();

    public ProjectLanguage(string language)
    {
      this.Language = language;
    }

    public string Language { get; }

    public int NativeStringCount { get; private set; }

    public int NativeWordCount { get; private set; }

    public IEnumerable<(LangStatus, LangCount)> CountByStatus
    {
      get { return this.countByStatus.Select(x => (x.Key, x.Value)); }
    }

    public void IncByStatus(LangStatus status, bool isValid, string nativeText, string translatedText)
    {
      this.NativeStringCount++;
      this.NativeWordCount += DetermineWordCount(nativeText);

      if (!this.countByStatus.TryGetValue(status, out var langCount))
      {
        langCount = new LangCount();
        this.countByStatus.Add(status, langCount);
      }

      langCount.Increment(isValid, 1, DetermineWordCount(translatedText));
    }

    private static int DetermineWordCount(string text)
    {
      return text.Split(' ').Length;
    }
  }
}