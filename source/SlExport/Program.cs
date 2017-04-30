namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.IO;
  using System.Xml;
  using System.ComponentModel.Composition.Hosting;
  using System.Reflection;

  class Program
  {
    private static readonly List<ISisulizerStatsExporer> exporter = new List<ISisulizerStatsExporer>();

    // ToDo: optional mittels Parameter das Herunterbrechen in einzelne Projekte unterdrücken (Ausgabe nur für die gesamte Datei)
    // ToDo: optional mittels Parameter die Ausgabe nur für bestimmte Sprachen beschränken
    static void Main(string[] args)
    {
      if (args.Length < 1)
      {
        Console.WriteLine("Usage: " + AppDomain.CurrentDomain.FriendlyName + "filename");
      }
      else
      {
        var filename = args[0];
        if (!File.Exists(filename))
        {
          Console.WriteLine($"Specified filename [{filename}] not found.");
        }

        RegisterExporter();
        var sisulizerFile = ParseFile(filename);

        Console.WriteLine($"Total projects in file: {sisulizerFile.Projects.Count()}");
        Console.ReadKey();

        foreach (var exportPlugIn in exporter)
        {
          exportPlugIn.Execute(sisulizerFile);
        }
      }
    }

    private static SisulizerFile ParseFile(string fileName)
    {
      var sisulizerFile = new SisulizerFile(fileName);
      SisulizerProject sisulizerProject = null;
      using (var fileStream = new FileStream(fileName, FileMode.Open))
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
                sisulizerProject = sisulizerFile.AddProject(projectName);
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

      return sisulizerFile;
    }

    private static void RegisterExporter()
    {
      var catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
      foreach (var foo in catalog.Parts)
      {
        Console.WriteLine(foo);
      }

      var container = new CompositionContainer(catalog);
      exporter.AddRange(container.GetExportedValues<ISisulizerStatsExporer>());      
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
