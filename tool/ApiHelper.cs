using System.Diagnostics;
using Docfx.Dotnet;
using Microsoft.CodeAnalysis;

namespace Macad.UserGuide;

public enum ExtSymbolIncludeState
{
    Default,
    Include,
    Exclude,
    IncludeProtected
}

public static class ApiHelper
{
    static readonly Dictionary<string, ExtSymbolIncludeState> _TypeFilterList = new();

    //--------------------------------------------------------------------------------------------------

    public static void LoadFilterList(string fileName)
    {
        string[] lines = File.ReadAllLines(fileName);
        foreach (string line in lines)
        {
            if(string.IsNullOrWhiteSpace(line))
                continue;

            ExtSymbolIncludeState includeState = line[0] switch
            {
                '+' => ExtSymbolIncludeState.Include,
                '#' => ExtSymbolIncludeState.IncludeProtected,
                '-' => ExtSymbolIncludeState.Exclude,
                _ => ExtSymbolIncludeState.Default
            };
            if (includeState == ExtSymbolIncludeState.Default)
            {
                throw new Exception("Each api filter line must start with + or -.");
            }

            _TypeFilterList.Add(line.Substring(1).Trim(), includeState);
        }
    }

    //--------------------------------------------------------------------------------------------------

    public static SymbolIncludeState GetApiIncludeState(ISymbol arg)
    {
        string name = arg.ToString() ?? string.Empty;
        string spaceName = arg.ContainingNamespace?.ToString() ?? string.Empty;
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(spaceName))
        {
            return SymbolIncludeState.Default;
        }

        // Filter by type list
        if (arg.Kind is not (SymbolKind.NamedType or SymbolKind.Method or SymbolKind.Property))
        {
            return SymbolIncludeState.Default;
        }

        ExtSymbolIncludeState state = ExtSymbolIncludeState.Exclude;

        // Namespaces must exactly match as a base
        state = _TypeFilterList.GetValueOrDefault(spaceName, state);

        // Search for more matching filter
        foreach (var typeFilter in _TypeFilterList)
        {
            var key = typeFilter.Key;
            if (name.Length == key.Length)
            {
                // must exact match
                if (name.Equals(key))
                {
                    state = typeFilter.Value;
                }
            }
            else if (name.StartsWith(key))
            {
                // check if we checked part of the path
                if (name[key.Length] == '.' || name[key.Length] == '(' || name[key.Length] == '<')
                {
                    // must at least match the namespace
                    if (key.StartsWith(spaceName))
                    {
                        state = typeFilter.Value;
                    }
                }
            }
        }

        // Filter by access
        if (arg.DeclaredAccessibility != Accessibility.Public
            && (arg.DeclaredAccessibility != Accessibility.Protected || state != ExtSymbolIncludeState.IncludeProtected))
        {
            return SymbolIncludeState.Exclude;
        }

        return _ToIncludeState(state);
    }

    //--------------------------------------------------------------------------------------------------

    static SymbolIncludeState _ToIncludeState(ExtSymbolIncludeState extState)
    {
        return extState switch
        {
            ExtSymbolIncludeState.Exclude => SymbolIncludeState.Exclude,
            ExtSymbolIncludeState.Include => SymbolIncludeState.Include,
            ExtSymbolIncludeState.IncludeProtected => SymbolIncludeState.Include,
            _ => SymbolIncludeState.Default
        };
    }

    //--------------------------------------------------------------------------------------------------

    public static SymbolIncludeState GetAttributeIncludeState(ISymbol arg)
    {
        return SymbolIncludeState.Exclude;
    }

    //--------------------------------------------------------------------------------------------------

    public static void PostProcessMetadata(string apiDirectory)
    {
        foreach (var apiFileName in Directory.EnumerateFiles(apiDirectory, "*.yml"))
        {
            string[] lines = File.ReadAllLines(apiFileName);
            List<string> newLines = new(lines.Length);
            foreach (var line in lines)
            {
                // Remove HRef, it is wrong for all filtered elements
                // The href link is correctly built by docfx when missing in metadata
                string trimmedLine = line.Trim([' ', '-']);
                if(trimmedLine.StartsWith("href:") && !trimmedLine.StartsWith("href: http"))
                    continue;
                newLines.Add(line);
            }
            File.WriteAllLines(apiFileName, newLines);
        }
    }
}