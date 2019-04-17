namespace SlExport
{
  internal class SisulizerProject : SisulizerStatBase, ISisulizerProject
  {
    private readonly SisulizerFile sisulizerFile;

    public SisulizerProject(SisulizerFile sisulizerFile, string name)
    {
      this.sisulizerFile = sisulizerFile;
      this.Name = name;
    }

    public string Name { get; }

    public override void IncLanguage(string language, LangStatus status, bool isValid, string nativeText, string translatedText)
    {
      base.IncLanguage(language, status, isValid, nativeText, translatedText);
      this.sisulizerFile.IncLanguage(language, status, isValid, nativeText, translatedText);
    }
  }
}