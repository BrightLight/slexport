namespace SlExport
{
  using CommandLine;

  /// <summary>
  /// Command line options that are available with "XML" as the export target.
  /// </summary>
  /// <seealso cref="SlExport.CommonExportOptions" />
  public class XmlExportOptions : CommonExportOptions
  {
    /// <summary>
    /// Gets or sets the name (and path) of the XML output file.
    /// </summary>
    [Option('o', "OutputFilename", Required = true, HelpText = "The name (and path) of the XML output file")]
    public string OutputFilename { get; set; }
  }
}