namespace SlExport.Oracle
{
  using System.ComponentModel.Composition;
  using global::Oracle.ManagedDataAccess.Client;

  /// <summary>
  /// Export plugin that writes the Sisulizer statistics into an Oracle database table
  /// </summary>
  /// <remarks>
  /// TODO check if table already exists. If not, create it
  /// TODO insert statistics data into table
  /// TODO option to update existing data instead of inserting new lines
  /// </remarks>
  /// <seealso cref="SlExport.ISisulizerExportPlugin" />
  [Export(typeof(ISisulizerExportPlugin))]
  public class ExportToOraclePlugIn : ISisulizerExportPlugin
  {
    public string PlugInId => "Oracle";

    public string PlugInName => "Export to Oracle database using ODP.Net";

    public void Execute(ISisulizerFile sisulizerFile, CommonExportOptions exportOptions)
    {
      var oracleExportOptions = exportOptions as OracleExportOptions;
      using (var oracleConnection = new OracleConnection(oracleExportOptions.ConnectionString))
      {
      }
    }
  }
}
