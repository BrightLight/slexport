namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  internal class ProjectLanguage : IProjectLanguage
  {    
    private readonly Dictionary<LangStatus, int> countByStatus = new Dictionary<LangStatus, int>();

    public ProjectLanguage(string language)
    {
      this.Language = language;
    }

    public string Language { get; private set; }

    public int NativeCount { get; private set; }

    public IEnumerable<Tuple<LangStatus, int>> CountByStatus
    {
      get { return this.countByStatus.Select(x => new Tuple<LangStatus, int>(x.Key, x.Value)); }
    }

    public void IncByStatus(LangStatus status)
    {
      this.NativeCount++;

      int count;
      if (!this.countByStatus.TryGetValue(status, out count))
      {
        this.countByStatus.Add(status, 1);
      }
      else
      {
        this.countByStatus[status] = count + 1;
      }
    }
  }
}