using System.IO;

using Build.Common.Enums;

using Cake.Common.Tools.DotNet;
using Cake.Frosting;

namespace Build
{
    public sealed class Build : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            string versionSuffix;
            foreach (var projectToBuild in context.Lib)
            {
                if (!context.General.IsLocal && context.General.CurrentBranch == Branches.Main)
                {
                    versionSuffix = string.Empty;
                }
                else
                {
                    versionSuffix = $"{projectToBuild.ArtifactVersion.Prerelease}-{projectToBuild.ArtifactVersion.Build}";
                }

                context.DotNetBuild(Path.Combine(context.Environment.WorkingDirectory.FullPath, projectToBuild.MainProject),
                    new Cake.Common.Tools.DotNet.Build.DotNetBuildSettings()
                    {
                        Configuration = "Release",
                        OutputDirectory = "./.artifacts",
                        Verbosity = DotNetVerbosity.Normal,
                        VersionSuffix = versionSuffix
                    });
            }
        }
    }
}
