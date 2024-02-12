using System.Collections.Generic;
using Build.Common.Builder;
using Cake.Common.Build;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;
using Cake.Frosting;

using Path = System.IO.Path;

namespace Build
{
    public sealed class RunTestsAndPublishResults : FrostingTask<Context>
    {
        /// <summary>Runs the task using the specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Run(Context context)
        {
            IDictionary<string, string> testProjects = new Dictionary<string, string>();
            foreach (var testProject in context.Tests.TestProjects)
            {
                testProjects.Add(
                    testProject.Key,
                    Path.Combine(context.Environment.WorkingDirectory.FullPath, testProject.Value));
            }

            string testArtifactsPath = Path.Combine(context.Environment.WorkingDirectory.FullPath,
                $"{context.General.ArtifactsDir}.tests");

            try
            {
                foreach (KeyValuePair<string, string> nameAndPath in testProjects)
                {
                    string coverletArgs = new DotNetTestCoverletParameterBuilder()
                    {
                        CollectCoverage = true,
                        CoverletOutputFormat = "opencover",
                        CoverletOutput = $"{testArtifactsPath}/{nameAndPath.Key}.coverage.xml",
                        Exclude = new List<string> {
                            "[*.Tests?]*" /* test projects */
                        },
                        ExcludeByFile = new List<string> {
                            "**/Data/Migrations/*.cs", /* EF data migrations in CustomerService.Core AND DataMigrator*/
                            "**/*Module.cs", /* module definitions which are ServiceCollection extensions  */
                        }
                    };

                    context.Information($"Coverlet args: {coverletArgs}");

                    context.DotNetTest(
                        nameAndPath.Value,
                        new DotNetTestSettings
                        {
                            VSTestReportPath =
                                Path.Combine(testArtifactsPath, $"{nameAndPath.Key}.TestResult.xml"),
                            Configuration = context.Tests.BuildConfig,
                            ArgumentCustomization = delegate (ProcessArgumentBuilder argument)
                            {
                                argument.Append(new TextArgument(coverletArgs));
                                return argument;
                            }
                        });
                }
            }
            finally
            {
                if (!context.BuildSystem().IsLocalBuild)
                {
                    foreach (KeyValuePair<string, string> nameAndPath in testProjects)
                    {
                        context.AzurePipelines().Commands.PublishTestResults(
                            new AzurePipelinesPublishTestResultsData
                            {
                                TestResultsFiles =
                                    new List<FilePath>
                                    {
                                        Path.Combine(context.Environment.WorkingDirectory.FullPath,
                                            $"{context.General.ArtifactsDir}.tests",
                                            $"{nameAndPath.Key}.TestResult.xml")
                                    },
                                TestRunner = AzurePipelinesTestRunnerType.VSTest
                            });
                    }
                }
            }
        }
    }
}
