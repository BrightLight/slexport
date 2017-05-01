namespace SlExport
{
  using CommandLine;
  using CommandLine.Text;

  /// <summary>
  /// Command line options that are available for all export targets.
  /// </summary>
  public class CommonExportOptions
  {
    /// <summary>
    /// Gets or sets the name (and path) of the sisulizer project file that will be analyzed.
    /// </summary>
    [Option('p', "SisulizerProject", Required = true, HelpText = "The Sisulizer project whose stats will be exported")]
    public string SisulizerProjectFileName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether log messages shall be written to the console.
    /// </summary>
    [Option('v', "Verbose")]
    public bool Verbose { get; set; }

    /// <summary>
    /// Gets the description of the available command line options.
    /// </summary>
    /// <returns>The description of the available command line options</returns>
    [HelpOption]
    public string GetUsage()
    {
      return HelpText.AutoBuild(this, x => HelpText.DefaultParsingErrorsHandler(this, x));
    }
  }
}