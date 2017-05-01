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
  }
}