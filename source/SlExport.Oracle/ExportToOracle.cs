﻿namespace SlExport.Oracle
{
  using System.ComponentModel.Composition;
  using global::Oracle.ManagedDataAccess.Client;

  [Export(typeof(ISisulizerStatsExporer))]
  public class ExportToOracle : ISisulizerStatsExporer
  {
    public string PlugInId => "Oracle";

    public string PlugInName => "Export to Oracle database using ODP.Net";

    public void Execute(ISisulizerFile sisulizerFile)
    {
      using (var oracleConnection = new OracleConnection())
      {
      }
    }
  }
}