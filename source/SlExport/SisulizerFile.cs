namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Xml;

  public class SisulizerFile : SisulizerStatBase, ISisulizerFile
  {
    private readonly List<SisulizerProject> projects = new List<SisulizerProject>();

    private readonly CommonExportOptions options;

    public SisulizerFile(string fileName, CommonExportOptions options)
    {
      this.FileName = fileName;
      this.options = options;
      this.ParseFile();
    }

    public SisulizerFile(Stream fileStream, CommonExportOptions options)
    {
      this.options = options;
      this.ParseStream(fileStream);
    }

    public string FileName { get; }

    public IEnumerable<ISisulizerProject> Projects => this.projects;

    private SisulizerProject AddProject(string projectName)
    {
      var sisulizerProject = new SisulizerProject(this, projectName);
      this.projects.Add(sisulizerProject);
      return sisulizerProject;
    }

    private void ParseFile()
    {
      using (var fileStream = new FileStream(this.FileName, FileMode.Open))
      {
        this.ParseStream(fileStream);
      }
    }

    private void ParseStream(Stream fileStream)
    {
      using (var xr = XmlReader.Create(fileStream))
      {
        SisulizerProject sisulizerProject = null;
        var mostRecentNativeText = string.Empty;
        var mostRecentTranslatedText = string.Empty;
        var mostRecentLanguage = string.Empty;

        // we are forward-only, so we need to store what we encounter and only act on it when the end-element is reached
        while (xr.Read())
        {
          switch (xr.NodeType)
          {
            case XmlNodeType.Element:
              if (xr.Name == "source")
              {
                var projectName = xr.GetAttribute("name");
                if (this.options.Verbose)
                {
                  Console.WriteLine("Encountered new project " + projectName);
                }

                sisulizerProject = this.AddProject(projectName);
              }

              if (xr.Name == "row")
              {
                mostRecentNativeText = xr.Value;
                sisulizerProject?.IncNative(mostRecentNativeText, string.Empty);
              }

              if (xr.Name == "native")
              {
                mostRecentNativeText = xr.Value;
              }

              if (xr.Name == "lang")
              {
                var language = xr.GetAttribute("id");
                var status = GetStatusFromInt(xr.GetAttribute("status"));
                var translatedText = xr.Value;
                sisulizerProject?.IncLanguage(language, status, mostRecentNativeText, translatedText);
              }

              break;
            case XmlNodeType.EndElement:
            {
              break;
            }
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

      if (Enum.TryParse(statusAsString, out LangStatus status))
      {
        return status;
      }

      return LangStatus.Undefined;
    }
  }
}