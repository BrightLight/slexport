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

    public string Name { get; private set; }

    public override void IncLanguage(string language, LangStatus status)
    {
      base.IncLanguage(language, status);
      this.sisulizerFile.IncLanguage(language, status);
    }
  }
}