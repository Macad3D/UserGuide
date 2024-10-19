using System.Reflection;
using Macad.UserGuide.Markdown;
using Markdig;
using Docfx;
using Docfx.Dotnet;

namespace Macad.UserGuide;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Macad|3D UserGuide Generator");

        // Find Docs folder
        string? pathToDocfxProject = null;
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        while (path != null)
        {
            if (File.Exists(Path.Combine(path, "docfx.json")))
            {
                pathToDocfxProject = path;
                break;
            }
            path = Path.GetDirectoryName(path);
        }

        if (pathToDocfxProject == null)
        {
            Console.WriteLine("Error: Cannot find Docfx project file.");
            Environment.Exit(-1);
        }
        Environment.CurrentDirectory = pathToDocfxProject;

        // Command line switches
        bool runMetadata = true;
        bool runBuild = true;
        if (args.Length > 0)
        {
            runMetadata = args.Any(arg => arg.ToLower() == "metadata");
            runBuild = args.Any(arg => arg.ToLower() == "build");
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
            _CreateXrefMap(pathToDocfxProject);
        }
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
            await Docset.Build("docfx.json", options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Environment.Exit(-1);
        }
    }

    //--------------------------------------------------------------------------------------------------

    static void _CreateXrefMap(string pathToDocfxProject)
    {
        var pathToSource = Path.Combine(pathToDocfxProject, "_site", "xrefmap.yml");
        if (!File.Exists(pathToSource))
        {
            Console.WriteLine("Error: Cannot find generated xrefmap.yml.");
            Environment.Exit(-1);
        }
        var lines = File.ReadAllLines(pathToSource);

        var pathToTarget = Path.Combine(pathToDocfxProject, "_site", "go", "xrefmap.php");
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
                    break;
                }
            }
            else if (line.StartsWith("  href: "))
            {
                string href = line.Substring(8);
                target.WriteLine($"'{guid}'=>'{href}',");
            }
        }

        target.WriteLine(");");
        target.WriteLine("?>");
        target.Close();
    }
}