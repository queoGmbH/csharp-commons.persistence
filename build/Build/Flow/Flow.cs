using Cake.Frosting;

namespace Build {
    [IsDependentOn(typeof(FormatCheck))]
    [IsDependentOn(typeof(NugetRestore))]
    [IsDependentOn(typeof(GenerateVersion))]
    [IsDependentOn(typeof(Build))]
    [IsDependentOn(typeof(RunTestsAndPublishResults))]
    [IsDependentOn(typeof(GetNuGetPackagesFromArtifacts))]
    [IsDependentOn(typeof(UploadArtifactsToPipeline))]
    //[Dependency(typeof(PushNuGetPackagesToQueo))]
    public partial class Default { }
}
