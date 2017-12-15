using static Cake.Common.Tools.DotNetCore.MSBuild.DotNetCoreMSBuildSettingsExtensions;
using System.Text.RegularExpressions;

var target = Argument("target", "Default");
var outputDir = Argument("outputDir", EnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY") ?? "./artifacts/");
var configuration = Argument("configuration", "Release");
var projectsPattern = Argument("projectsPattern", "**/*.csproj");
var testsPattern = Argument("testsPattern", "**/*Tests/*.csproj");
var versionSuffix = Argument("versionSuffix", EnvironmentVariable("BUILD_SOURCEBRANCHNAME") ?? "local");
var isReleaseBuild = Argument<bool>("isReleaseBuild", false);
var buildName = Argument("buildName", EnvironmentVariable("BUILD_BUILDNUMBER") ?? "local-0");
var buildNumberPattern = Argument("buildNumberPattern", @"^.*-(\d+)$");

var buildNumber = Regex.Match(buildName, buildNumberPattern).Groups[1].Value;
versionSuffix = $"{versionSuffix}{buildNumber}";

var outputDirPath = DirectoryPath.FromString(outputDir);

var dropDir = outputDirPath.Combine("drop");

var binOutputDir = outputDirPath.Combine("bin");
var testResultsDir = outputDirPath.Combine("testresults");
var publishOutputDir = outputDirPath.Combine("publish");
var packOutputDir = dropDir.Combine("pkgs");

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

var publishSettings = new DotNetCorePublishSettings
{
	Verbosity = verbosity,
	Configuration = configuration,
	OutputDirectory = publishOutputDir,
	VersionSuffix = versionSuffix,
	MSBuildSettings = msBuildSettings
};

Task("Clean")
	.Does(
		() =>
		{
			Information($"Delete directory '{outputDir}'");

			if(DirectoryExists(outputDir))
			{
				DeleteDirectory(
					outputDir, 
					new DeleteDirectorySettings
					{
						Recursive = true,
						Force = true
					});
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

Task("Publish")
	.DoesForEach(
		packProjectFiles,
		(file) =>
		{
			DotNetCorePublish(file.FullPath, publishSettings);
		});

Task("Compress")
	.Does(
		() =>
		{
			CreateDirectory(dropDir);
			
			new List<DirectoryPath> 
			{ 
				testResultsDir, 
				publishOutputDir
			}.ForEach(
				dir =>
				{
					if(DirectoryExists(dir))
					{
						Information($"Compress directory '{dir}'");

						Zip(dir, dropDir.GetFilePath(dir.GetDirectoryName()).AppendExtension("zip"));
					}
				});
		});

Task("Default")
	.IsDependentOn("Build")
	.IsDependentOn("Test")	
	.IsDependentOn("Publish")
	.IsDependentOn("Pack")
	.IsDependentOn("Compress");

Information($"Build #{buildNumber}");

RunTarget(target);