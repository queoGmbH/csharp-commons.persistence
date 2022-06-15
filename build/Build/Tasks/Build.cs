using System.IO;

using Build.Common.Enums;
using Build.Common.Extensions;

using Cake.Common.Tools.MSBuild;
using Cake.Frosting;

namespace Build
{
    public sealed class Build : FrostingTask<Context>
    {
        public override void Run(Context context)
        {
            string versionPrefix, versionSuffix;
            foreach (var projectToBuild in context.Lib)
            {
                if (!context.General.IsLocal && context.General.CurrentBranch == Branches.Main)
                {
                    versionPrefix =
                        $"{projectToBuild.ArtifactVersion.Major}.{projectToBuild.ArtifactVersion.Minor}.{projectToBuild.ArtifactVersion.Patch}";
                    versionSuffix = string.Empty;
                }
                else
                {
                    versionPrefix =
                        $"{projectToBuild.ArtifactVersion.Major}.{projectToBuild.ArtifactVersion.Minor}.{projectToBuild.ArtifactVersion.Patch}-{projectToBuild.ArtifactVersion.Prerelease}";
                    versionSuffix = $"{projectToBuild.ArtifactVersion.Build}";
                }

                context.MSBuild(
                    Path.Combine(context.Environment.WorkingDirectory.FullPath, projectToBuild.MainProject).AsFilePath(),
                    settings => settings
                        .WithTarget("pack")
                        .WithProperty("IsPackable", "true")
                        .WithProperty("VersionPrefix", versionPrefix)
                        .WithProperty("VersionSuffix", versionSuffix)
                        .WithProperty("PackageOutputPath", "../../.artifacts")
                        .Configuration = "Release");
            }
        }
    }
}
