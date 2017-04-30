namespace SlExport
{
  using System.Collections.Generic;

  internal class SisulizerStatBase
  {
    private readonly Dictionary<string, ProjectLanguage> languagesById = new Dictionary<string, ProjectLanguage>();

    public IEnumerable<IProjectLanguage> Languages => this.languagesById.Values;

    public void IncNative()
    {
      this.IncLanguage("native", LangStatus.Completed);
    }

    public virtual void IncLanguage(string language, LangStatus status)
    {
      ProjectLanguage projectLanguage;
      if (!this.languagesById.TryGetValue(language, out projectLanguage))
      {
        projectLanguage = new ProjectLanguage(language);
        this.languagesById.Add(language, projectLanguage);
      }

      projectLanguage.IncByStatus(status);
    }
  }
}