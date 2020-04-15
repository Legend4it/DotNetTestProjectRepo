#addin nuget:?package=Cake.FileHelpers&version=2.0.0
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

var nugetKey = EnvironmentVariable("NUGET_API_KEY") ?? Argument("nugetKey", "");
var jenkinsUserToken = EnvironmentVariable("JENKINS_USER_TOKEN") ?? Argument("jenkinsUserToken", "");
var gitlabJobId = EnvironmentVariable("CI_JOB_ID") ?? Argument("gitlabJobId", "");
var isOnBuildServer = gitlabJobId != "";

//////////////////////////////////////////////////////////////////////
// JENKINS ARGUMENTS
//////////////////////////////////////////////////////////////////////
var jenkinsUrl = "jenkins.flexlink.se:9090";
var jenkinsJob = "FLDT%20Testing%20Automation%20UI";
var gitlabTestLink = "https://gitlab.com/api/v4/projects/fsi-lab%2fengineeringtools%2ffldt%2fautomation/jobs/"+gitlabJobId+"/artifacts/build/FlexLink.Automation-latest.zip";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var outputDir = "./build";
var solutionFile = "./src/FldtAutomation.sln";

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
		MSBuild(solutionFile, new MSBuildSettings 
		{
			Verbosity = Verbosity.Minimal,
			Configuration = configuration,
		});
	});

Task("SignBinaries")
	.WithCriteria(() => isOnBuildServer)
	.Does(() => {
        var files = GetFiles("./src/**/bin/" + configuration + "/**/FlexLink.DesignTool*.dll");
        Sign(files, new SignToolSignSettings {
            CertSubjectName = "FlexLink AB",
            DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
            Description = "FlexLink Design Tool Automation framework v" + version,
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
	//.IsDependentOn("Test")
	.Does(() =>
	{
		var settings = new NuGetPackSettings {
			Version = version,
			OutputDirectory = outputDir,
			Properties = new Dictionary<string, string>
			{
				{ "Configuration", configuration }
			}
		};
		NuGetPack("./src/Automation/Automation.csproj", settings);
		NuGetPack("./src/FunctionalTesting/FunctionalTesting.csproj", settings);
	});

Task("ZipPackTests")
	.IsDependentOn("Build")
	//.IsDependentOn("Test")
	.Does(() =>
	{
		var zipDestination = "./build/zippkg-tests";
		CreateDirectory(zipDestination);
		CopyDirectory("./src/FunctionalTestingTests/bin/" + configuration, zipDestination + "/uitests");
		CopyDirectory("./src/FunctionalTestingTests/Layouts", zipDestination + "/layouts");
		ZipCompress(zipDestination, "build/FlexLink.Automation-latest.zip");
	});
	
Task("StartJenkinsBuild")
	.WithCriteria(() => gitlabJobId != "")
	.WithCriteria(() => jenkinsUserToken != "")
	.Does(() =>
	{
		Information("Starting Jenkins job '" + jenkinsJob + "' with artifacts from job '" + gitlabJobId + "'");
        var crumb = HttpGet("http://" + jenkinsUserToken + "@" + jenkinsUrl + "/crumbIssuer/api/xml?xpath=concat(//crumbRequestField,%22:%22,//crumb)").Split(':');
		
		var settings = new HttpSettings
        {
           Headers = new Dictionary<string, string>
            {
				{ crumb[0], crumb[1] },
                { "Cache-Control", "no-store" },
				{ "Authorization", "Basic " + System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(jenkinsUserToken))},
            },
            EnsureSuccessStatusCode = true,
        };
		settings.SetContentType("application/x-www-form-urlencoded");		
		settings.SetRequestBody("json=" + SerializeJson(new {parameter=new []{
			new {name="TEST_LINK", value=gitlabTestLink}}}));
		
		var link = "http://" + jenkinsUrl + "/job/" + jenkinsJob + "/buildWithParameters?TEST_LINK=" + System.Uri.EscapeDataString(gitlabTestLink) + "&delay=60sec";
		Information("Kicking of Jenkins build '" + link + "'");
		HttpPost(link, settings);
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("DefaultCi")
	.IsDependentOn("Clean")
	.IsDependentOn("Build")
	.IsDependentOn("NuGetPack")
	.IsDependentOn("ZipPackTests")
	.IsDependentOn("StartJenkinsBuild")
	;
	
Task("Default")
	.IsDependentOn("NugetPack")
	;


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
