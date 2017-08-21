namespace SlExport
{
  using CommandLine;
  using CommandLine.Text;

  /// <summary>
  /// Defines and stores the command line options.
  /// </summary>
  /// <remarks>
  /// Every export plugin can define a "verb" that must be specified as the first command line argument. This verb (basically the desired target format)
  /// the determines which other command line options are available (different exporters might need different command line arguments).
  /// TODO right now, this class needs to know all exporter plugins. THat kind of defeats the idea of "plugins" and open/closed principal.
  /// Therefore, in the future, each exporter should be able to provide its own options class and there needs to be some way to tell the command line
  /// parser about it. IT seems at the moment, that this command line parser strictly relies on scanning the Options class for attributes.
  /// </remarks>
  public class Options
  {
    /// <summary>
    /// Gets or sets the command line options available for target type "XML".
    /// </summary>
    [VerbOption("xml", HelpText = "Export to XML file")]
    public XmlExportOptions XmlExportOptions { get; set; }

    /// <summary>
    /// Gets or sets the command line options available for target type "CSV".
    /// </summary>
    [VerbOption("csv", HelpText = "Export to CSV file")]
    public CsvExportOptions CsvExportOptions { get; set; }

    /// <summary>
    /// Gets or sets the command line options available for target type "Oracle".
    /// </summary>
    [VerbOption("oracle", HelpText = "Export to Oracle database using ODP.Net")]
    public OracleExportOptions OracleExportOptions { get; set; }

    /// <summary>
    /// Gets the "usage" text that will be written to the console if parsing the command line fails (e.g. due to missing arguments).
    /// </summary>
    /// <param name="verb">The verb (here: target format) that was specified. <c>Null</c> if no verb (target type) was specified at all.</param>
    /// <returns>A description that explains what arguments can be used in the command line.</returns>
    [HelpVerbOption]
    public string GetUsage(string verb)
    {
      return HelpText.AutoBuild(this, verb);
    }
  }
}