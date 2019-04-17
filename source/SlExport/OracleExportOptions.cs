namespace SlExport
{
  using CommandLine;

  /// <summary>
  /// Command line options that are available with "Oracle" as the export target.
  /// </summary>
  /// <seealso cref="SlExport.CommonExportOptions" />
  public class OracleExportOptions : CommonExportOptions
  {
    /// <summary>
    /// Gets or sets the Oracle connection string that will be used to connect to the database.
    /// </summary>
    [Option("ConnectionString", Required = true, HelpText = "A connection string to connect to the database where the statistics data will be stored.")]
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the name of the database table that stored the statistics data.
    /// </summary>
    [Option("TableName", Required = true, HelpText = "The name of the database table that stored the statistics data.")]
    public string TableName { get; set; }

    /// <summary>
    /// Gets or sets the name of the product to which this statistics data belongs.
    /// </summary>
    [Option("Product", Required = true, HelpText = "The product to which this statistics data belongs.")]
    public string Product { get; set; }

    /// <summary>
    /// Gets or sets the product version to which this statistics data belongs.
    /// </summary>
    [Option("Version", Required = true, HelpText = "The product version to which this statistics data belongs")]
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets the build number to which this statistics data belongs.
    /// </summary>
    [Option("Build", Required = true, HelpText = "The build number to which this statistics data belongs.")]
    public int Build { get; set; }
  }
}