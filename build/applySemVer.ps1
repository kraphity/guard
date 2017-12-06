##---------------------------------------------------------------------
## <copyright file="ApplySemVer.ps1">(c) 2017 Kraphity GmbH</copyright>
##---------------------------------------------------------------------
# apply semantic versioning Major.Minor.Patch[-prerelease[Revision]]
# versionRegex = Regex to get Major.Minor.Patch.Revision, e.g. (\d+)\.(\d+)\.(\d+)\.(\d+)
# Sets the following versioning elements: 
# FileVersion = Major.Minor.Patch.Revision
# AssemblyVersion = Major.Minor.0.0
# Version = Major.Minor.Patch[-prerelease[Revision]]

[CmdletBinding()]
param
(
	[string]$sourceDir = $Env:BUILD_SOURCESDIRECTORY,
	[string]$buildNumber = $Env:BUILD_BUILDNUMBER,
    [string]$prerelease,
    [string]$versionRegex = "(\d+)\.(\d+)\.(\d+)\.(\d+)",
    [boolean]$includeRevision = $true
)

if(-not $sourceDir)
{
    Write-Error "SourceDir not set"
    exit 1
}

if(-not $buildNumber)
{
    Write-Error "BuildNumber not set"
    exit 1
}

Write-Verbose "SourceDir: $sourceDir"
Write-Verbose "BuildNumber: $buildNumber"
Write-Verbose "PreRelease: $prerelease"
Write-Verbose "IncludeRevision: $includeRevision"

$versionMatches = [regex]::matches($buildNumber, $versionRegex)

if($versionMatches.Count -eq 0)
{
	Write-Error "Version number not found in '$buildNumber'"
	exit 1
}

# build version numbers
$versionNumber = $versionMatches[0]
Write-Verbose "VersionNumber: $versionNumber"

if($versionNumber.Groups.Count -lt 5)
{
    Write-Error "Version number does not contain all required parts (Major.Minor.Patch.Revision)"
}

$assemblyFileVersion = $versionNumber.ToString()
$assemblyVersion = "$($versionNumber.Groups[1].Value).$($versionNumber.Groups[2].Value).0.0"
$version = "$($versionNumber.Groups[1].Value).$($versionNumber.Groups[2].Value).$($versionNumber.Groups[3].Value)"

if($prerelease)
{
    $version = "$version-$prerelease"
    if($includeRevision)
    {
        $version = [string]::Format("{0}{1}", $version, $versionNumber.Groups[4].Value)
    }
}

Write-Verbose "AssemblyFileVersion: $assemblyFileVersion"
Write-Verbose "AssemblyVersion: $assemblyVersion"
Write-Verbose "Version: $version"

# Apply the version to csproj files
Write-Verbose "Find project files (*.csproj)"
$csprojFiles = Get-ChildItem $sourceDir -recurse -include "*.csproj"

if($csprojFiles)
{
    Write-Verbose "Apply version to $($csprojFiles.Count) files"
    
    $utf8Encoding = New-Object System.Text.UTF8Encoding($false)
    $xmlWritersettings = New-Object System.Xml.XmlWriterSettings
    $xmlWritersettings.OmitXmlDeclaration = $true
    $xmlWritersettings.Encoding = $utf8Encoding
    $xmlWritersettings.Indent = $true

    foreach ($file in $csprojFiles)
	{
        [xml]$content = Get-Content $file -Encoding UTF8
        
        if($content.SelectNodes("Project/PropertyGroup").Count -eq 0)
        {
            [void]$content.Project.AppendChild($content.CreateElement("PropertyGroup"))
        }

        if($content.SelectNodes("Project/PropertyGroup/FileVersion").Count -eq 0)
        {
            [void]$content.Project.PropertyGroup.AppendChild($content.CreateElement("FileVersion"))
        }

        if($content.SelectNodes("Project/PropertyGroup/AssemblyVersion").Count -eq 0)
        {
            [void]$content.Project.PropertyGroup.AppendChild($content.CreateElement("AssemblyVersion"))
        }

        if($content.SelectNodes("Project/PropertyGroup/Version").Count -eq 0)
        {
            [void]$content.Project.PropertyGroup.AppendChild($content.CreateElement("Version"))
        }

        $content.SelectNodes("Project/PropertyGroup/FileVersion") | foreach { $_.InnerText = $assemblyFileVersion }
        $content.SelectNodes("Project/PropertyGroup/AssemblyVersion") | foreach { $_.InnerText = $assemblyVersion }
        $content.SelectNodes("Project/PropertyGroup/Version") | foreach { $_.InnerText = $version }

		attrib $file -r

        $content.Save([System.Xml.XmlWriter]::Create($file.FullName, $xmlWritersettings))

		Write-Verbose "$($file.FullName) - updated"
    }
}
else
{
    Write-Warning "No files found"
}