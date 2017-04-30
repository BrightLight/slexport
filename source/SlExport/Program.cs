namespace SlExport
{
  using System;
  using System.Linq;
  using System.IO;

  class Program
  {
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

        var sisulizerFile = new SisulizerFile(filename);
        Console.WriteLine($"Total projects in file: {sisulizerFile.Projects.Count()}");
        Console.ReadKey();

        foreach (var exportPlugIn in PlugInManager.Instance.ExportPlugIns)
        {
          exportPlugIn.Execute(sisulizerFile);
        }
      }
    }
  }
}
