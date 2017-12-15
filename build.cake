using static Cake.Common.Tools.DotNetCore.MSBuild.DotNetCoreMSBuildSettingsExtensions;
using System.Text.RegularExpressions;

var target = Argument("target", "Default");
var outputDir = Argument("outputDir", "./artifacts/");
var configuration = Argument("configuration", "Release");
var projectsPattern = Argument("projectsPattern", "**/*.csproj");
var testsPattern = Argument("testsPattern", "**/*Tests/*.csproj");
var versionSuffix = Argument("versionSuffix", "local");
var isReleaseBuild = Argument<bool>("isReleaseBuild", false);
var buildName = Argument("buildName", "local-0");
var buildNumberPattern = Argument("buildNumberPattern", @"^.*-(\d+)$");

var buildNumber = Regex.Match(buildName, buildNumberPattern).Groups[1].Value;
versionSuffix = $"{versionSuffix}{buildNumber}";

var binOutputDir = System.IO.Path.Combine(outputDir, "bin");
var testResultsDir = DirectoryPath.FromString(System.IO.Path.Combine(outputDir, "testresults"));
var packOutputDir = System.IO.Path.Combine(outputDir, "pkgs");

var verbosity = DotNetCoreVerbosity.Normal;

var projectFiles = GetFiles(projectsPattern);
var testProjectFiles = GetFiles(testsPattern);
var packProjectFiles = projectFiles.Where(p => testProjectFiles.All(t => t.FullPath != p.FullPath));

var msBuildSettings = new DotNetCoreMSBuildSettings()
	.WithProperty("BuildNumber", buildNumber)
	.WithProperty("IsReleaseBuild", isReleaseBuild.ToString().ToLowerInvariant()); 

var restoreSettings = new DotNetCoreRestoreSettings
{
	Verbosity = verbosity
};

var buildSettings = new DotNetCoreBuildSettings
{
	Verbosity = verbosity,
	Configuration = configuration,
	OutputDirectory = binOutputDir,
	VersionSuffix = versionSuffix,
	MSBuildSettings = msBuildSettings
};

var testSettings = new DotNetCoreTestSettings
{
	Verbosity = verbosity,
	Configuration = configuration,
	OutputDirectory = binOutputDir,
	NoBuild = true,
	Logger = "trx",
	ArgumentCustomization = args => args.Append($"-r {MakeAbsolute(testResultsDir)}")
};

var packSettings = new DotNetCorePackSettings
{
	Verbosity = verbosity,
	Configuration = configuration,
	OutputDirectory = packOutputDir,
	VersionSuffix = versionSuffix,
	MSBuildSettings = msBuildSettings
};

Task("Clean")
	.Does(
		() =>
		{
			if(DirectoryExists(outputDir))
			{
				DeleteDirectory(outputDir, recursive:true);
			}
		});

Task("Restore")
	.DoesForEach(
		projectFiles, 
		(file) => 
		{
			DotNetCoreRestore(file.FullPath, restoreSettings);
		});

Task("Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")	
	.DoesForEach(
		projectFiles, 
		(file) => 
		{
			DotNetCoreBuild(file.FullPath, buildSettings);
		});

Task("Test")
	.DoesForEach(
		testProjectFiles,
		(file) =>
		{
			DotNetCoreTest(file.FullPath, testSettings);
		});

Task("Pack")
	.DoesForEach(
		packProjectFiles,
		(file) =>
		{
			DotNetCorePack(file.FullPath, packSettings);
		});

Task("Compress")
	.Does(
		() =>
		{
			Zip(outputDir, System.IO.Path.Combine(outputDir, "artifacts.zip"));
		});

Task("Default")
	.IsDependentOn("Build")
	.IsDependentOn("Test")
	.IsDependentOn("Pack")
	.IsDependentOn("Compress");

Information($"Build #{buildNumber}");

RunTarget(target);