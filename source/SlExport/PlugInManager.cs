namespace SlExport
{
  using System.Collections.Generic;
  using System.ComponentModel.Composition.Hosting;
  using System.Reflection;

  public class PlugInManager
  {
    private static PlugInManager instance;
    private readonly List<ISisulizerExportPlugin> exportPlugIns = new List<ISisulizerExportPlugin>();

    /// <summary>
    /// Initializes a new instance of the <see cref="PlugInManager"/> class.
    /// </summary>
    public PlugInManager()
    {
      this.RegisterPlugIns();
    }

    public static PlugInManager Instance => instance ?? (instance = new PlugInManager());

    public IEnumerable<ISisulizerExportPlugin> ExportPlugIns => this.exportPlugIns;

    private void RegisterPlugIns()
    {
      var catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
      var container = new CompositionContainer(catalog);
      this.RegisterExportPlugIns(container);
    }

    private void RegisterExportPlugIns(ExportProvider container)
    {
      this.exportPlugIns.AddRange(container.GetExportedValues<ISisulizerExportPlugin>());
    }
  }
}
