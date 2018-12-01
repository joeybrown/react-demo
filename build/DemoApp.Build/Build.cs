using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

class Build : NukeBuild
{
    AbsolutePath SourceDirectory => RootDirectory / "src/DemoApp.Web";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    public static int Main () => Execute<Build>(x => x.Copy_Assets);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    Target Clean => _ => _
        .Executes(() =>
        {
            DeleteDirectories(GlobDirectories(SourceDirectory, "bin", "obj"));
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(SourceDirectory);
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(SourceDirectory);
        });

    Target Copy_Assets => _ => _
        .DependsOn(Compile)
        .Executes(() => {
            var binDir = SourceDirectory / "bin/Debug/netcoreapp2.1";
            CopyDirectoryRecursively(binDir, OutputDirectory);
        });

}
