namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Xml;

  internal class SisulizerFile : SisulizerStatBase, ISisulizerFile
  {
    private readonly List<SisulizerProject> projects = new List<SisulizerProject>();

    public SisulizerFile(string fileName)
    {
      this.FileName = fileName;
      this.ParseFile();
    }

    public string FileName { get; private set; }

    public IEnumerable<ISisulizerProject> Projects => this.projects;

    private SisulizerProject AddProject(string projectName)
    {
      var sisulizerProject = new SisulizerProject(this, projectName);
      this.projects.Add(sisulizerProject);
      return sisulizerProject;
    }

    private void ParseFile()
    {
      SisulizerProject sisulizerProject = null;
      using (var fileStream = new FileStream(this.FileName, FileMode.Open))
      using (var xr = XmlReader.Create(fileStream))
      {
        while (xr.Read())
        {
          switch (xr.NodeType)
          {
            case XmlNodeType.Element:
              if (xr.Name == "source")
              {
                var projectName = xr.GetAttribute("name");
                Console.WriteLine("Encountered new project " + projectName);
                sisulizerProject = this.AddProject(projectName);
              }

              if (xr.Name == "native")
              {
                sisulizerProject?.IncNative();
              }

              if (xr.Name == "lang")
              {
                var language = xr.GetAttribute("id");
                var status = GetStatusFromInt(xr.GetAttribute("status"));
                sisulizerProject?.IncLanguage(language, status);
              }

              break;
            case XmlNodeType.EndElement:
              break;
          }
        }
      }
    }

    private static LangStatus GetStatusFromInt(string statusAsString)
    {
      if (string.IsNullOrEmpty(statusAsString))
      {
        return LangStatus.Completed;
      }

      LangStatus status;
      if (Enum.TryParse(statusAsString, out status))
      {
        return status;
      }

      return LangStatus.Undefined;
    }
  }
}