namespace SlExport
{
  using System.Collections.Generic;

  public class SisulizerStatBase
  {
    private readonly Dictionary<string, ProjectLanguage> languagesById = new Dictionary<string, ProjectLanguage>();

    public IEnumerable<IProjectLanguage> Languages => this.languagesById.Values;

    public void IncNative(string nativeText, string translatedText)
    {
      this.IncLanguage("native", LangStatus.Completed, true, nativeText, translatedText);
    }

    public virtual void IncLanguage(string language, LangStatus status, bool isValid, string nativeText, string translatedText)
    {
      if (!this.languagesById.TryGetValue(language, out var projectLanguage))
      {
        projectLanguage = new ProjectLanguage(language);
        this.languagesById.Add(language, projectLanguage);
      }

      projectLanguage.IncByStatus(status, isValid, nativeText, translatedText);
    }

    public virtual void CalculateNotTranslated(int totalNativeStringCount, int totalNativeWordCount)
    {
      foreach (var langugae in this.languagesById.Values)
      {
        langugae.CalculateNotTranslated(totalNativeStringCount, totalNativeWordCount);
      }
    }
  }
}