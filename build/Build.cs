using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

[GitHubActions(
    "Build",
    GitHubActionsImage.UbuntuLatest,
    ImportSecrets = new[] { nameof(GitHubToken) },
    OnPullRequestBranches = new[] { "develop", "main", "master", "release/*" },
    OnPushBranches = new[] { "main", "master", "develop", "release/*" },
    InvokedTargets = new[] { nameof(CI) },
    FetchDepth = 0
    )]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.CI);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter("GitHub Token")] readonly string GitHubToken;

    [Solution] Solution Solution { get; set; }
    Project Project => Solution.GetProject("NukeToolGenerationDemo");

    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            ArtifactsDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetTasks.DotNetRestore(s => s
                .SetProjectFile(Project));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuildTasks.MSBuild(s => s
                .SetProjectFile(Project)
                .SetConfiguration(Configuration));
        });

    Target CI => _ => _
        .DependsOn(Compile)
        .Produces(ArtifactsDirectory)
        .Executes(() =>
        {
            // Copy assembly to artifacts directory
            var assembly = RootDirectory / "bin" / Configuration / "NukeToolGenerationDemo.dll";
            CopyFileToDirectory(assembly, ArtifactsDirectory, FileExistsPolicy.Overwrite);
        }); 
}
