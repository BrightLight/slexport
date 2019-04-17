namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  internal class ProjectLanguage : IProjectLanguage
  {    
    private readonly Dictionary<LangStatus, int> stringCountByStatus = new Dictionary<LangStatus, int>();

    public ProjectLanguage(string language)
    {
      this.Language = language;
    }

    public string Language { get; private set; }

    public int NativeStringCount { get; private set; }

    public IEnumerable<Tuple<LangStatus, int>> StringCountByStatus
    {
      get { return this.stringCountByStatus.Select(x => new Tuple<LangStatus, int>(x.Key, x.Value)); }
    }

    public void IncByStatus(LangStatus status)
    {
      this.NativeStringCount++;

      if (!this.stringCountByStatus.TryGetValue(status, out var count))
      {
        this.stringCountByStatus.Add(status, 1);
      }
      else
      {
        this.stringCountByStatus[status] = count + 1;
      }
    }
  }
}