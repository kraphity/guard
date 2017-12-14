var target = Argument("target", "Default");
var outputDir = Argument("outputDir", "");
var projectFilesPattern = Argument("projectFilesPattern", "src/**/*.csproj");

var projectFiles = GetFiles(projectFilesPattern);

Task("Clean")
	.Does(
		() =>
		{
		});

Task("Restore")
	.DoesForEach(
		projectFiles, 
		(file) => 
		{
			DotNetCoreRestore(file.FullPath);
		});

Task("Build")
	.IsDependentOn("Restore")
	.Does(
		() =>
		{
			Information("Compile");
		});

Task("Default")
	.IsDependentOn("Build");

RunTarget(target);