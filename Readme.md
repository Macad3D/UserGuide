# UserGuide to Macad\|3D

This repository contains the user manual for Macad\|3D. 

The generation is based on [docfx](https://dotnet.github.io/docfx/), which is included as a Nuget package in a custom tool. 

The handwritten articles are stored as [markdown](https://dotnet.github.io/docfx/docs/markdown.html?tabs=linux%2Cdotnet) documents in the `docs` directory. Images in the articles are placed directly next to the articles as `.png` or `.apng`. The user guide uses some custom extensions to the standard docfx markdown syntax.

To enhance the auto-generated API documentation, further documents can be entered in the `apidoc` directory, which supplement or overwrite the API documentation in the source code. 
For the general syntax of the configuration, markdown files or yaml headers, please consult the [docfx](https://dotnet.github.io/docfx/) documentation.

The generated user guide will be published on https://macad3d.net/userguide/.

## Development requirements

- Visual Studio 2022 Community Edition 
  - .Net Desktop workload
  - .Net 8 support component

## Building from source

1. Place assemblies from Macad\|3D into the `assemblies` directory.
2. Start `Build.cmd`.
   