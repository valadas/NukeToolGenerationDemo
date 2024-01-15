
using JetBrains.Annotations;
using Newtonsoft.Json;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;


/// <summary>
///   <p>A dotnet tool that can use reflection to generate OpenApi from .NetFramework assemblies.</p>
///   <p>For more details, visit the <a href="https://github.com/valadas/WebApiToOpenApiReflector">official website</a>.</p>
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[NuGetPackageRequirement(WebApiToOpenApiReflectorPackageId)]
public partial class WebApiToOpenApiReflectorTasks
    : IRequireNuGetPackage
{
    public const string WebApiToOpenApiReflectorPackageId = "WebApiToOpenApiReflector";
    /// <summary>
    ///   Path to the WebApiToOpenApiReflector executable.
    /// </summary>
    public static string WebApiToOpenApiReflectorPath =>
        ToolPathResolver.TryGetEnvironmentExecutable("WEBAPITOOPENAPIREFLECTOR_EXE") ??
        NuGetToolPathResolver.GetPackageExecutable("WebApiToOpenApiReflector", "WebApiToOpenApiReflector.dll");
    public static Action<OutputType, string> WebApiToOpenApiReflectorLogger { get; set; } = ProcessTasks.DefaultLogger;
    public static Action<ToolSettings, IProcess> WebApiToOpenApiReflectorExitHandler { get; set; } = ProcessTasks.DefaultExitHandler;
    /// <summary>
    ///   <p>A dotnet tool that can use reflection to generate OpenApi from .NetFramework assemblies.</p>
    ///   <p>For more details, visit the <a href="https://github.com/valadas/WebApiToOpenApiReflector">official website</a>.</p>
    /// </summary>
    public static IReadOnlyCollection<Output> WebApiToOpenApiReflector(ArgumentStringHandler arguments, string workingDirectory = null, IReadOnlyDictionary<string, string> environmentVariables = null, int? timeout = null, bool? logOutput = null, bool? logInvocation = null, Action<OutputType, string> logger = null, Action<IProcess> exitHandler = null)
    {
        using var process = ProcessTasks.StartProcess(WebApiToOpenApiReflectorPath, arguments, workingDirectory, environmentVariables, timeout, logOutput, logInvocation, logger ?? WebApiToOpenApiReflectorLogger);
        (exitHandler ?? (p => WebApiToOpenApiReflectorExitHandler.Invoke(null, p))).Invoke(process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>A dotnet tool that can use reflection to generate OpenApi from .NetFramework assemblies.</p>
    ///   <p>For more details, visit the <a href="https://github.com/valadas/WebApiToOpenApiReflector">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;assemblyPaths&gt;</c> via <see cref="WebApiToOpenApiReflectorSettings.AssemblyPaths"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> WebApiToOpenApiReflector(WebApiToOpenApiReflectorSettings toolSettings = null)
    {
        toolSettings = toolSettings ?? new WebApiToOpenApiReflectorSettings();
        using var process = ProcessTasks.StartProcess(toolSettings);
        toolSettings.ProcessExitHandler.Invoke(toolSettings, process.AssertWaitForExit());
        return process.Output;
    }
    /// <summary>
    ///   <p>A dotnet tool that can use reflection to generate OpenApi from .NetFramework assemblies.</p>
    ///   <p>For more details, visit the <a href="https://github.com/valadas/WebApiToOpenApiReflector">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;assemblyPaths&gt;</c> via <see cref="WebApiToOpenApiReflectorSettings.AssemblyPaths"/></li>
    ///   </ul>
    /// </remarks>
    public static IReadOnlyCollection<Output> WebApiToOpenApiReflector(Configure<WebApiToOpenApiReflectorSettings> configurator)
    {
        return WebApiToOpenApiReflector(configurator(new WebApiToOpenApiReflectorSettings()));
    }
    /// <summary>
    ///   <p>A dotnet tool that can use reflection to generate OpenApi from .NetFramework assemblies.</p>
    ///   <p>For more details, visit the <a href="https://github.com/valadas/WebApiToOpenApiReflector">official website</a>.</p>
    /// </summary>
    /// <remarks>
    ///   <p>This is a <a href="http://www.nuke.build/docs/authoring-builds/cli-tools.html#fluent-apis">CLI wrapper with fluent API</a> that allows to modify the following arguments:</p>
    ///   <ul>
    ///     <li><c>&lt;assemblyPaths&gt;</c> via <see cref="WebApiToOpenApiReflectorSettings.AssemblyPaths"/></li>
    ///   </ul>
    /// </remarks>
    public static IEnumerable<(WebApiToOpenApiReflectorSettings Settings, IReadOnlyCollection<Output> Output)> WebApiToOpenApiReflector(CombinatorialConfigure<WebApiToOpenApiReflectorSettings> configurator, int degreeOfParallelism = 1, bool completeOnFailure = false)
    {
        return configurator.Invoke(WebApiToOpenApiReflector, WebApiToOpenApiReflectorLogger, degreeOfParallelism, completeOnFailure);
    }
}
#region WebApiToOpenApiReflectorSettings
/// <summary>
///   Used within <see cref="WebApiToOpenApiReflectorTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
[Serializable]
public partial class WebApiToOpenApiReflectorSettings : ToolSettings
{
    /// <summary>
    ///   Path to the WebApiToOpenApiReflector executable.
    /// </summary>
    public override string ProcessToolPath => base.ProcessToolPath ?? WebApiToOpenApiReflectorTasks.WebApiToOpenApiReflectorPath;
    public override Action<OutputType, string> ProcessLogger => base.ProcessLogger ?? WebApiToOpenApiReflectorTasks.WebApiToOpenApiReflectorLogger;
    public override Action<ToolSettings, IProcess> ProcessExitHandler => base.ProcessExitHandler ?? WebApiToOpenApiReflectorTasks.WebApiToOpenApiReflectorExitHandler;
    /// <summary>
    ///   The assembly or assemblies to process. (Required)
    /// </summary>
    public virtual string[] AssemblyPaths { get; internal set; }
    protected override Arguments ConfigureProcessArguments(Arguments arguments)
    {
        arguments
          .Add("{value}", AssemblyPaths);
        return base.ConfigureProcessArguments(arguments);
    }
}
#endregion
#region WebApiToOpenApiReflectorSettingsExtensions
/// <summary>
///   Used within <see cref="WebApiToOpenApiReflectorTasks"/>.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static partial class WebApiToOpenApiReflectorSettingsExtensions
{
    #region AssemblyPaths
    /// <summary>
    ///   <p><em>Sets <see cref="WebApiToOpenApiReflectorSettings.AssemblyPaths"/></em></p>
    ///   <p>The assembly or assemblies to process. (Required)</p>
    /// </summary>
    [Pure]
    public static T SetAssemblyPaths<T>(this T toolSettings, string[] assemblyPaths) where T : WebApiToOpenApiReflectorSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.AssemblyPaths = assemblyPaths;
        return toolSettings;
    }
    /// <summary>
    ///   <p><em>Resets <see cref="WebApiToOpenApiReflectorSettings.AssemblyPaths"/></em></p>
    ///   <p>The assembly or assemblies to process. (Required)</p>
    /// </summary>
    [Pure]
    public static T ResetAssemblyPaths<T>(this T toolSettings) where T : WebApiToOpenApiReflectorSettings
    {
        toolSettings = toolSettings.NewInstance();
        toolSettings.AssemblyPaths = null;
        return toolSettings;
    }
    #endregion
}
#endregion
