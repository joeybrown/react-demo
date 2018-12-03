using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Npm;
using Nuke.Docker;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;
using static Nuke.Docker.DockerTasks;

//docker build -t orleanschess.host:dev -f ./src/OrleansChess.Host/Dockerfile .

class Build : NukeBuild {
    // AbsolutePath SourceDirectory => RootDirectory / "src/DemoApp.Web";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    // AbsolutePath ClientAppDirectory => SourceDirectory / "ClientApp";
    AbsolutePath DockerDirectory => RootDirectory / "build/DemoApp.Build";

    public static int Main () => Execute<Build> (x => x.DockerBuild);

    // [Parameter ("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    // readonly string Configuration = IsLocalBuild ? "Debug" : "Release";

    Target Clean => _ => _
        .Executes (() => {
            // DeleteDirectories (GlobDirectories (SourceDirectory, "bin", "obj"));
            EnsureCleanDirectory (OutputDirectory);
        });

    Target DockerBuild => _ => _
        .DependsOn(Clean)
        .Executes (() => {
            Docker("build -t demoapp.web:prod .", DockerDirectory);
        });

    // Target Restore => _ => _
    //     .DependsOn (Clean)
    //     .Executes (() => {
    //         Npm ("install", ClientAppDirectory);
    //         DotNetRestore (SourceDirectory);
    //     });

    // Target Compile => _ => _
    //     .DependsOn (Restore)
    //     .Executes (() => {
    //         DotNetBuild (SourceDirectory);
    //         npmBuild ();
    //     });

    // private void npmBuild () {
    //     //todo: adjust for Release
    //     Npm ("run build", ClientAppDirectory);
    // }

    // Target Copy_Assets => _ => _
    //     .DependsOn (Compile)
    //     .Executes (() => {
    //         var binDir = getBinDir ();
    //         var binOutputDir = getBinOutputDir ();
    //         CopyDirectoryRecursively (binDir, binOutputDir);

    //         var clientAppDir = getClientAppDir ();
    //         var clientAppOutputDir = getGlientAppOutputDir ();
    //         CopyDirectoryRecursively (clientAppDir, clientAppOutputDir);
    //     });

    // private AbsolutePath getBinDir () {
    //     //todo: adjust for Release
    //     var binDir = SourceDirectory / "bin/Debug/netcoreapp2.1";
    //     return binDir;
    // }

    // private AbsolutePath getBinOutputDir () {
    //     var binOutputDir = OutputDirectory / "bin";
    //     return binOutputDir;
    // }

    // private AbsolutePath getClientAppDir () {
    //     //todo: adjust for Release
    //     var clientAppDir = ClientAppDirectory / "build";
    //     return clientAppDir;
    // }

    // private AbsolutePath getGlientAppOutputDir () {
    //     var clientAppOutputDir = OutputDirectory / "ClientApp";
    //     return clientAppOutputDir;
    // }
}