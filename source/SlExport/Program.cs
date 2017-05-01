namespace SlExport
{
  using System;
  using System.Linq;
  using System.IO;
  using CommandLine;

  class Program
  {
    // ToDo: optional mittels Parameter das Herunterbrechen in einzelne Projekte unterdrücken (Ausgabe nur für die gesamte Datei)
    // ToDo: optional mittels Parameter die Ausgabe nur für bestimmte Sprachen beschränken
    static void Main(string[] args)
    {
      var invokedVerb = string.Empty;
      CommonExportOptions invokedVerbOptions = null;
      var options = new Options();
      if (Parser.Default.ParseArguments(
        args,
        options,
        (verb, verbOptions) =>
        {
          invokedVerb = verb;
          invokedVerbOptions = verbOptions as CommonExportOptions;
        }))
      {
      }
      else
      {
        Environment.ExitCode = Parser.DefaultExitCodeFail;
        return;
      }

      var filename = invokedVerbOptions.SisulizerProjectFileName;
      if (!File.Exists(filename))
      {
        Console.WriteLine($"Specified filename [{filename}] not found.");
        Environment.ExitCode = Parser.DefaultExitCodeFail;
        return;
      }

      var sisulizerFile = new SisulizerFile(filename, invokedVerbOptions);
      if (invokedVerbOptions.Verbose)
      {
        Console.WriteLine($"Total projects in file: {sisulizerFile.Projects.Count()}");
      }

      foreach (var exportPlugIn in PlugInManager.Instance.ExportPlugIns.Where(x => string.Equals(x.PlugInId, invokedVerb, StringComparison.OrdinalIgnoreCase)))
      {
        exportPlugIn.Execute(sisulizerFile, invokedVerbOptions);
      }
    }
  }
}