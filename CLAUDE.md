# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FwkSolution1 is an **ASP.NET MVC 5 web application** built on .NET Framework 4.8.1. It's a minimal template-based project with a single controller (HomeController) providing basic web pages (Index, About, Contact). The project includes Bootstrap 5 for styling and jQuery for client-side interaction.

**Key Characteristics:**
- .NET Framework 4.8.1 (not .NET Core)
- Uses `Web.config` for configuration (not `appsettings.json`)
- Single project in solution: FwkWebApplication1
- No test projects or testing infrastructure
- No database layer
- Uses IIS Express development server on HTTPS port 44331

## Build Commands

### Build the solution
```bash
msbuild FwkSolution1.sln
```

### Clean build
```bash
msbuild FwkSolution1.sln /t:Clean
```

### Debug build
```bash
msbuild FwkSolution1.sln /p:Configuration=Debug
```

### Release build
```bash
msbuild FwkSolution1.sln /p:Configuration=Release
```

### Compile and run locally
Open the solution in Visual Studio 2022 and press F5, or use:
```bash
dotnet build FwkSolution1.sln
```

The application runs at `https://localhost:44331/` using IIS Express.

## Development Environment

**Requirements:**
- Visual Studio 2022 (or compatible version with .NET Framework 4.8.1 support)
- .NET Framework 4.8.1 SDK
- IIS Express (included with Visual Studio)

**Restore NuGet packages:**
```bash
nuget restore FwkSolution1.sln
```

## Architecture

### Folder Structure
- **Controllers/** - MVC controllers (currently only HomeController with Index, About, Contact actions)
- **Views/** - Razor templates (.cshtml files)
  - `Shared/_Layout.cshtml` - Master layout page
  - `Home/*.cshtml` - Home page views
- **App_Start/** - Application initialization
  - `RouteConfig.cs` - URL routing configuration
  - `FilterConfig.cs` - Global MVC filter setup (including HandleErrorAttribute)
  - `BundleConfig.cs` - CSS/JS bundling configuration for optimization
- **Content/** - CSS stylesheets (Bootstrap 5, custom Site.css)
- **Scripts/** - JavaScript libraries (jQuery, Bootstrap, validation)
- **Models/** - Empty folder reserved for model classes
- **App_Data/** - Reserved folder for application data
- **Properties/** - Assembly metadata (AssemblyInfo.cs)

### MVC Pattern
The application follows the standard ASP.NET MVC 5 pattern:
- **Models** - Data models (currently unused)
- **Views** - Razor view templates with shared `_Layout.cshtml` master page
- **Controllers** - HomeController handles page navigation

### Key Initialization Files
- **Global.asax.cs** - Registers routes, filters, and bundles during application startup
- **Web.config** - Application configuration (includes assembly binding redirects for NuGet dependencies)
- **Web.Debug.config** and **Web.Release.config** - Environment-specific configuration transforms

## Dependencies

Major NuGet packages:
- **Microsoft.AspNet.Mvc 5.2.9** - MVC framework
- **Microsoft.AspNet.Razor 3.2.9** - Razor view engine
- **Microsoft.AspNet.WebPages 3.2.9** - Web page support
- **System.Web.Optimization 1.1.3** - CSS/JS bundling and minification
- **jQuery 3.7.0** - JavaScript library
- **Bootstrap 5.2.3** - CSS framework
- **Newtonsoft.Json 13.0.3** - JSON serialization
- **Microsoft.CodeDom.Providers.DotNetCompilerPlatform 2.0.1** - Roslyn compiler for newer C# features

See `packages.config` for complete list.

## Configuration

The application uses **Web.config** files for configuration:
- `Web.config` - Main configuration file
- `Web.Debug.config` - Debug-specific transformations
- `Web.Release.config` - Release-specific transformations

### Key Configuration Settings
- **appSettings:**
  - `webpages:Version` - 3.0.0.0
  - `ClientValidationEnabled` - true
  - `UnobtrusiveJavaScriptEnabled` - true
- **Compilation:** TargetFramework 4.8.1, Debug mode enabled by default
- **Assembly Binding Redirects:** Manages version conflicts for multiple NuGet dependencies

## Common Development Tasks

### Add a new page/view
1. Add a new action method to `HomeController` (Controllers/HomeController.cs)
2. Create a new `.cshtml` file in `Views/Home/` folder
3. The `_Layout.cshtml` provides the page structure and navigation

### Modify styling
- Edit `Content/Site.css` for custom styles
- Bootstrap 5 CSS files are in the `Content/` folder
- Bundle configuration is in `App_Start/BundleConfig.cs`

### Add JavaScript functionality
- Add scripts to the `Scripts/` folder
- Reference in views using bundle tags or direct script tags
- jQuery is available globally on all pages

### Add routes
- Edit `App_Start/RouteConfig.cs` to add new routes
- Default route pattern: `{controller}/{action}/{id}`

## Important Notes

- **No testing infrastructure** - No test projects or testing frameworks are present
- **No database layer** - This is a view/presentation-only application
- **Framework version** - Uses .NET Framework 4.8.1, not .NET Core/.NET 5+. Tools and commands reflect this.
- **IIS Express only** - Development server is configured for IIS Express on HTTPS port 44331
- **Bundling/Minification** - Enabled via `System.Web.Optimization` in Release builds
- **Error handling** - Global HandleErrorAttribute configured in FilterConfig.cs shows Error.cshtml on exceptions
- **Unobtrusive validation** - Client-side form validation is implemented using jQuery Unobtrusive Validation
