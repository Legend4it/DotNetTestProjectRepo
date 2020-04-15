#addin nuget:?package=Cake.FileHelpers&version=3.0.0
#addin nuget:?package=Cake.Http&version=0.5.0
#addin nuget:?package=Cake.Git&version=0.17.0
#tool  nuget:?package=xunit.runner.console&version=2.2.0
#addin nuget:?package=Cake.Compression&version=0.1.6
#addin nuget:?package=SharpZipLib&version=0.86.0
#addin nuget:?package=Cake.Json&version=3.0.1
#addin nuget:?package=Newtonsoft.Json&version=9.0.1

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var regexMatchGroups = System.Text.RegularExpressions.Regex.Match(GitDescribe(".", true, GitDescribeStrategy.Tags), "(.*)-([\\d]*)-([\\w\\d]+$)").Groups;
var version = regexMatchGroups[1].Value + (regexMatchGroups[2].Value == "0" ? "" : "." + (System.Int32.Parse(regexMatchGroups[2].Value) + 1) + "-" + regexMatchGroups[3].Value);;

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Which versions of client starter should be built
var variants = new []{"FLDT", "FLCT", "FLQT", "FL"};

var nugetKey = EnvironmentVariable("NUGET_API_KEY") ?? Argument("nugetKey", "");
var jenkinsUserToken = EnvironmentVariable("JENKINS_USER_TOKEN") ?? Argument("jenkinsUserToken", "");
var gitlabJobId = EnvironmentVariable("CI_JOB_ID") ?? Argument("gitlabJobId", "");
var isOnBuildServer = gitlabJobId != "";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var outputDir = "./build";
var solutionFile = "./src/FlqtStarter.sln";

Information("Building version='" + version+"'");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
	{
		CleanDirectories(outputDir);
		CleanDirectories("./src/**/bin");
		CleanDirectories("./src/**/obj");
	});

Task("Init")
	.Does(() =>
	{
		CreateDirectory(outputDir);		
	});
	
Task("SetVersion")
	.WithCriteria(() => isOnBuildServer)
	.Does(() => {
		var dotnetVersion = version.LastIndexOf('-') != -1 ? version.Substring(0, version.LastIndexOf('-')).Replace("-", ".") : version + ".0";
		Information("Assembly version='" + dotnetVersion + "'");
        ReplaceRegexInFiles("./src/**/AssemblyInfo.cs", 
            "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", 
            dotnetVersion);
        ReplaceRegexInFiles("./src/**/AssemblyInfo.cs", 
            "(?<=AssemblyFileVersion\\(\")(.+?)(?=\"\\))", 
            dotnetVersion);
   });

Task("Build")
	.IsDependentOn("Init")
	.IsDependentOn("NuGetRestorePackages")
	.IsDependentOn("SetVersion")
	.Does(() =>
	{
		foreach (var app in variants) 
		{
			Information("Building client starter for " + app);
			CleanDirectories("./src/**/obj");
			CopyFile("./src/" + app + ".ico", "./src/flqt32.ico");
			MSBuild(solutionFile, settings => settings.SetConfiguration(configuration));
			MoveFile("./src/bin/" + configuration + "/ClientStarter.exe", "./build/ClientStarter-" + app + ".exe");
		}
	});
	
Task("SignBinaries")
	.WithCriteria(() => isOnBuildServer)
	.Does(() => {
        var files = GetFiles("./build/ClientStarter*.exe");
        Sign(files, new SignToolSignSettings {
            CertSubjectName = "FlexLink AB",
            DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
            Description = "FlexLink Client Starter Client v" + version,
            DescriptionUri = new Uri("http://www.flexlink.com/"),
            TimeStampUri = new Uri("http://timestamp.verisign.com/scripts/timstamp.dll")
            });
   });

Task("NuGetRestorePackages")
	.Does(() =>
	{
		NuGetRestore(solutionFile);
	});

Task("NugetPack")
	.IsDependentOn("Build")
	.IsDependentOn("SignBinaries")
	.Does(() =>
	{
		NuGetPack("./src/FlqtStarter.nuspec", new NuGetPackSettings {
			Version = version,
			OutputDirectory = outputDir,
			Properties = new Dictionary<string, string>
			{
				{ "Configuration", configuration }
			}
		});
	});
	
Task("ZipPack")
	.IsDependentOn("Build")
	.Does(() =>
	{
		var zipDestination = "./build/zippkg";
		CreateDirectory(zipDestination);
		CopyFiles("./build/ClientStarter-*", zipDestination);		
		CopyFiles("./src/**/bin/" + configuration + "/ClientStarter.exe.config", zipDestination);		
		ZipCompress(zipDestination, "build/FlexLink.ClientStarter-v"+version+".zip");
	});
	
//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("DefaultCi")
	.IsDependentOn("Clean")
	.IsDependentOn("NugetPack")
	.IsDependentOn("ZipPack")
	;
	
Task("Default")
	.IsDependentOn("Clean")
	.IsDependentOn("NugetPack")
	.IsDependentOn("ZipPack")
	;


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
