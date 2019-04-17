namespace SlExport.Tests
{
  using System.IO;
  using System.Linq;
  using System.Text;
  using NUnit.Framework;
  using SlExport.Tests.Properties;

  [TestFixture(TestOf = typeof(SisulizerFile))]
  public class SisulizerFileTests
  {
    [Test]
    public void EnsureBasicFunctionsWork()
    {
      using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.SisulizerSmallDemo)))
      {
        var options = new CommonExportOptions { SisulizerProjectFileName = "" };
        var sisulizerFile = new SisulizerFile(memoryStream, options);

        Assert.That(sisulizerFile.Languages.Count(), Is.EqualTo(4)); // native, en, pl, cs
        var languagesStats = sisulizerFile.Languages.ToDictionary(x => x.Language, x => new { x.NativeStringCount, x.CountByStatus});
        var nativeStats = languagesStats["native"];
        Assert.That(nativeStats, Is.Not.Null);
        Assert.That(nativeStats.NativeStringCount, Is.EqualTo(11));
        var nativeStringCountByStatus = nativeStats.CountByStatus.ToDictionary(x => x.Item1, x => (x.StringCount, x.WordCount));
        Assert.That(nativeStringCountByStatus[LangStatus.Completed].StringCount, Is.EqualTo(11));
        Assert.That(nativeStringCountByStatus[LangStatus.Completed].WordCount, Is.EqualTo(11));

        var enStats = languagesStats["en"];
        Assert.That(enStats, Is.Not.Null);
        Assert.That(enStats.NativeStringCount, Is.EqualTo(2));
        var enStringCountByStatus = enStats.CountByStatus.ToDictionary(x => x.Item1, x => x.Item2);
        Assert.That(enStringCountByStatus[LangStatus.AutoTranslated], Is.EqualTo(2));

        Assert.That(sisulizerFile.Projects.Count(), Is.EqualTo(1));
        Assert.That(sisulizerFile.Projects.FirstOrDefault()?.Languages.Count(), Is.EqualTo(4)); // native, en, pl, cs
        Assert.That(sisulizerFile.Projects.FirstOrDefault()?.Name, Is.EqualTo("Magic.dll"));
      }
    }
  }
}
