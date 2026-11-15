using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;
using Macad.UserGuide.Markdown;
using Markdig;
using Docfx;
using Docfx.Dotnet;

namespace Macad.UserGuide;

internal class Program
{
    static List<string> _TempFiles = [];
    static string _PathToDocfxProject = "";

    //--------------------------------------------------------------------------------------------------

    static async Task Main(string[] args)
    {
        Console.WriteLine("Macad|3D UserGuide Generator");

        // Find Docs folder
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        while (path != null)
        {
            if (File.Exists(Path.Combine(path, "docfx.json")))
            {
                _PathToDocfxProject = path;
                break;
            }
            path = Path.GetDirectoryName(path);
        }

        if (string.IsNullOrEmpty(_PathToDocfxProject))
        {
            Console.WriteLine("Error: Cannot find Docfx project file.");
            Environment.Exit(-1);
        }
        Environment.CurrentDirectory = _PathToDocfxProject;

        // Command line switches
        var switches = args.Select(arg => arg.ToLowerInvariant()).ToList();
        bool preliminary = switches.Remove("preliminary");

        bool runMetadata = true;
        bool runBuild = true;
        if (switches.Count > 0)
        {
            runMetadata = switches.Contains("metadata");
            runBuild = switches.Contains("build");
        }

        if (runMetadata)
        {
            // Run metadata stage
            await _RunMetadataStage();
        }

        if (runBuild)
        {
            // Run build stage
            await _RunBuildStage();
            _CreateXrefMap();
            if (preliminary)
            {
                _AddPreliminaryBanner();
            }
        }

        // Cleanup
        _TempFiles.ForEach(File.Delete);
    }

    //--------------------------------------------------------------------------------------------------

    static async Task _RunMetadataStage()
    {
        ApiHelper.LoadFilterList("apiFilter.list");

        var options = new DotnetApiOptions()
        {
            IncludeApi = ApiHelper.GetApiIncludeState,
            IncludeAttribute = ApiHelper.GetAttributeIncludeState
        };

        try
        {
            await DotnetApiCatalog.GenerateManagedReferenceYamlFiles("docfx.json", options);
            ApiHelper.PostProcessMetadata("_api");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(-1);
        }
    }

    //--------------------------------------------------------------------------------------------------

    static async Task _RunBuildStage()
    {
        var options = new BuildOptions
        {
            // Enable custom markdown extensions here
            ConfigureMarkdig = pipeline => pipeline.UseDefinitionLists()
                .UseFigures()
                .Use<FiguredLinkExtension>()
        };

        try
        {
            _CreatePreviewImagesForAnimations("docs");
            await Docset.Build("docfx.json", options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(-1);
        }
    }

    //--------------------------------------------------------------------------------------------------

    const string _PreliminaryBanner =
        "<div class=\"alert alert-warning preliminary-banner d-print-none\" role=\"alert\">" +
        "<strong>Preliminary documentation:</strong> This page has been built from the development branch. " +
        "It may describe features that have not been released yet and is subject to change." +
        "</div>";

    static readonly Regex _ArticleStartRegex = new("<article\\b[^>]*>", RegexOptions.Compiled);

    // Inserts a warning banner at the top of the article of every generated page.
    static void _AddPreliminaryBanner()
    {
        Console.WriteLine("Adding preliminary banner to all pages...");

        var sitePath = Path.Combine(_PathToDocfxProject, "_site");
        int count = 0;
        foreach (var fileName in Directory.EnumerateFiles(sitePath, "*.html", SearchOption.AllDirectories))
        {
            var content = File.ReadAllText(fileName);
            if (content.Contains("preliminary-banner"))
                continue;

            var match = _ArticleStartRegex.Match(content);
            if (!match.Success)
                continue; // e.g. redirect pages

            content = content.Insert(match.Index + match.Length, _PreliminaryBanner);
            File.WriteAllText(fileName, content);
            count++;
        }

        Console.WriteLine($"Preliminary banner added to {count} pages.");
    }

    //--------------------------------------------------------------------------------------------------

    static void _CreateXrefMap()
    {
        var pathToSource = Path.Combine(_PathToDocfxProject, "_site", "xrefmap.yml");
        if (!File.Exists(pathToSource))
        {
            Console.WriteLine("Error: Cannot find generated xrefmap.yml.");
            Environment.Exit(-1);
        }
        var lines = File.ReadAllLines(pathToSource);

        var pathToTarget = Path.Combine(_PathToDocfxProject, "_site", "go", "xrefmap.php");
        var target = File.CreateText(pathToTarget);
        target.WriteLine("<?php");
        target.WriteLine("return array(");

        string guid = "";
        foreach (var line in lines)
        {
            if (line.StartsWith("- uid: "))
            {
                guid = line.Substring(7).ToLower();
                if (!Guid.TryParse(guid, out Guid _))
                {
                    guid = "";
                }
            }
            else if (line.StartsWith("  href: ") && !string.IsNullOrEmpty(guid))
            {
                string href = line.Substring(8);
                target.WriteLine($"'{guid}'=>'{href}',");
            }
        }

        target.WriteLine(");");
        target.WriteLine("?>");
        target.Close();
    }

    //--------------------------------------------------------------------------------------------------

    static void _CreatePreviewImagesForAnimations(string sourcePath)
    {
        Console.WriteLine("Creating preview images for animations...");

        var pathToOverlay = Path.Combine(_PathToDocfxProject, "images", "playbtn_overlay.png");
        using var overlayImage = Image.FromFile(pathToOverlay);

        foreach (var filePath in Directory.EnumerateFiles(sourcePath, "*.apng", SearchOption.AllDirectories))
        {
            string targetPath = Path.ChangeExtension(filePath, ".png");
            if (File.Exists(targetPath))
            {
                continue;
            }

            // Read in APNG, it will only read in the first frame, which we use for preview
            using var image = Image.FromFile(filePath);
            using var graphics = Graphics.FromImage(image);
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphics.DrawImage(overlayImage, (float)(image.Width-overlayImage.Width)/2, (float)(image.Height-overlayImage.Height)/2 );
            image.Save(targetPath);
            _TempFiles.Add(targetPath);
        }
    }
}