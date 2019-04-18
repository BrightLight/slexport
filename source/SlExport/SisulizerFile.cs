namespace SlExport
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Xml;

  public class SisulizerFile : SisulizerStatBase, ISisulizerFile
  {
    private readonly List<SisulizerProject> projects = new List<SisulizerProject>();

    private readonly CommonExportOptions options;

    private static readonly List<string> excludedRowIds = new List<string>
    {
      "Size.Height",
      "Size.Width",
      "BackColor",
      "ForeColor",
      "Location.X",
      "Location.Y",
      "ClientSize.Height",
      "ClientSize.Width",
      "RightToLeft",
      "Font",
      "FontCategory",
      "FontFamily",
      "FontSize",
      "FontStyle",
      "ImeMode",
      "Height",
      "Width",
      "helpProvider.TrayLocation",
      "Size",
      "ZOrder",
    };

    ////private static readonly HashSet<string> foo = new HashSet<string>();

    private enum SisulizerNodeType
    {
      Undefined,
      Row,
      Native,
      Lang,
    }

    public SisulizerFile(string fileName, CommonExportOptions options)
    {
      this.FileName = fileName;
      this.options = options;
      this.ParseFile();
    }

    public SisulizerFile(Stream fileStream, CommonExportOptions options)
    {
      this.options = options;
      this.ParseStream(fileStream);
    }

    public string FileName { get; }

    public IEnumerable<ISisulizerProject> Projects => this.projects;

    private SisulizerProject AddProject(string projectName)
    {
      var sisulizerProject = new SisulizerProject(this, projectName);
      this.projects.Add(sisulizerProject);
      return sisulizerProject;
    }

    private void ParseFile()
    {
      using (var fileStream = new FileStream(this.FileName, FileMode.Open))
      {
        this.ParseStream(fileStream);

        var totalNativeStringCount = this.Languages.FirstOrDefault(x => x.Language == "native")?.NativeStringCount ?? 0;
        var totalNativeWordCount = this.Languages.FirstOrDefault(x => x.Language == "native")?.NativeWordCount ?? 0;
        this.CalculateNotTranslated(totalNativeStringCount, totalNativeWordCount);
      }
    }

    private void ParseStream(Stream fileStream)
    {
      using (var xr = XmlReader.Create(fileStream))
      {
        SisulizerProject sisulizerProject = null;
        var mostRecentNativeText = string.Empty;
        var mostRecentLanguage = string.Empty;
        var mostRecentStatus = LangStatus.Undefined;
        var mostRecentIsValid = false;
        var sisulizerNodeType = SisulizerNodeType.Undefined;

        // we are forward-only, so we need to store what we encounter and only act on it when the end-element is reached
        while (xr.Read())
        {
          switch (xr.NodeType)
          {
            case XmlNodeType.Element:
              switch (xr.Name)
              {
                case "source":
                  var projectName = xr.GetAttribute("name");
                  if (this.options.Verbose)
                  {
                    Console.WriteLine("Encountered new project " + projectName);
                  }

                  sisulizerProject = this.AddProject(projectName);
                  break;

                case "node":
                  if (xr.GetAttribute("excluded") == "1")
                  {
                    xr.ReadInnerXml(); // we don't need the inner xml, we just want to skip over it and move the reader forward
                  }

                  break;

                case "row":
                  if (SkipRow(xr.GetAttribute("id")) || xr.GetAttribute("hidden") == "1")
                  {
                    xr.ReadInnerXml(); // we don't need the inner xml, we just want to skip over it and move the reader forward
                  }
                  else
                  {
                    ////foo.Add(xr.GetAttribute("id"));
                    sisulizerNodeType = SisulizerNodeType.Row;
                  }

                  break;

                case "native":
                  sisulizerNodeType = SisulizerNodeType.Native;
                  break;

                case "lang":
                  sisulizerNodeType = SisulizerNodeType.Lang;
                  mostRecentLanguage = xr.GetAttribute("id");
                  mostRecentStatus = GetStatusFromInt(xr.GetAttribute("status"));
                  mostRecentIsValid = xr.GetAttribute("invalidated") != "1";

                  break;
                default:
                  sisulizerNodeType = SisulizerNodeType.Undefined;
                  break;
              }

              break;
            case XmlNodeType.Text:
              switch (sisulizerNodeType)
              {
                case SisulizerNodeType.Row:
                case SisulizerNodeType.Native:
                  mostRecentNativeText = xr.Value;
                  sisulizerProject?.IncNative(mostRecentNativeText, string.Empty);
                  break;
                case SisulizerNodeType.Lang:
                  var translatedText = xr.Value;
                  sisulizerProject?.IncLanguage(mostRecentLanguage, mostRecentStatus, mostRecentIsValid, mostRecentNativeText, translatedText);
                  break;
              }
              break;
            case XmlNodeType.EndElement:
            {
              sisulizerNodeType = SisulizerNodeType.Undefined;
              break;
            }
          }
        }
      }

      ////var sb = new StringBuilder();
      ////foreach (var text in foo.OrderBy(x => x))
      ////{
      ////  sb.AppendLine(text);
      ////}

      ////File.WriteAllText("rowids.txt", sb.ToString());
    }

    public override void CalculateNotTranslated(int totalNativeStringCount, int totalNativeWordCount)
    {
      base.CalculateNotTranslated(totalNativeStringCount, totalNativeWordCount);
      foreach (var sisulizerProject in this.projects)
      {
        sisulizerProject.CalculateNotTranslated(totalNativeStringCount, totalNativeWordCount);
      }
    }

    private static bool SkipRow(string rowId)
    {
      return excludedRowIds.Contains(rowId);
    }

    private static LangStatus GetStatusFromInt(string statusAsString)
    {
      if (string.IsNullOrEmpty(statusAsString))
      {
        return LangStatus.Completed;
      }

      if (Enum.TryParse(statusAsString, out LangStatus status))
      {
        return status;
      }

      return LangStatus.Undefined;
    }
  }
}