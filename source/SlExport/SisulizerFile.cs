namespace SlExport
{
  using System.Collections.Generic;

  internal class SisulizerFile : SisulizerStatBase, ISisulizerFile
  {
    private readonly List<SisulizerProject> projects = new List<SisulizerProject>();

    public SisulizerFile(string fileName)
    {
      this.FileName = fileName;
    }

    public string FileName { get; private set; }

    public SisulizerProject AddProject(string projectName)
    {
      var sisulizerProject = new SisulizerProject(this, projectName);
      this.projects.Add(sisulizerProject);
      return sisulizerProject;
    }

    public IEnumerable<ISisulizerProject> Projects => this.projects;
  }
}