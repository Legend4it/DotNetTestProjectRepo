#addin nuget:?package=Cake.FileHelpers&version=3.1.0
#addin nuget:?package=Cake.Http&version=0.5.0
#addin nuget:?package=Cake.Git&version=0.19.0
#tool  nuget:?package=xunit.runner.console&version=2.2.0
#addin nuget:?package=Cake.Compression&version=0.2.1
#addin nuget:?package=SharpZipLib&version=0.86.0
#addin nuget:?package=Cake.Json&version=3.0.1
#addin nuget:?package=Newtonsoft.Json&version=11.0.1
//#addin nuget:?package=Cake.Powershell&version=0.4.7

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////
var version = GitDescribe(".", true, GitDescribeStrategy.Tags, 7);
version = version.Replace("installation-", "").Replace("addon-", "");
var dotnetVersion = version.Contains('-') ? version.Substring(0, version.LastIndexOf('-')).Replace("-", ".") : version + ".0";
var shortDotnetVersion = dotnetVersion.Substring(0, dotnetVersion.LastIndexOf('.'));

var advancedInstallerPath = EnvironmentVariable("ADVANCED_INSTALLER_PATH") ?? Argument("advancedInstallerPath", @"C:\Program Files (x86)\Caphyon\Advanced Installer 11.6.3\bin\x86\AdvancedInstaller.com");
var vcEssentialsVersion = EnvironmentVariable("VC_VERSION") ?? Argument("vcVersion", "4.1.1.84755");

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
var jenkinsJob = "FLDT%20Testing%20Main%20UI";

var nexusArtifactId = EnvironmentVariable("NEXUS_ARTIFACT_ID") ?? Argument("nexusArtifactId", "installation");
var nexusGroupId = EnvironmentVariable("NEXUS_GROUP_ID") ?? Argument("nexusGroupId", "com.flexlink.fldt");
var nexusAppLink = "http://nexus.flexlink.se/repository/maven-installation-snapshots/" + nexusGroupId.Replace(".", "/") + "/" + nexusArtifactId + "/" + version + "/" + nexusArtifactId + "-" + version + "-win64.msi";

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var outputDir = "./build";
var downloadDir = "./build/downloads";
var solutionFile = "./src/WoodPeckerAddon.sln";

Information("Building version='" + version+"' ('" + dotnetVersion + "')");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
	.Does(() =>
	{
		CleanDirectories(outputDir);
		CleanDirectories("./src/packages");
		CleanDirectories("./src/**/bin");
		CleanDirectories("./src/**/obj");
	});

Task("Init")
	.Does(() =>
	{
		CreateDirectory(outputDir);		
		CreateDirectory(downloadDir);		
		
		Information("Downloading translations");
		CreateDirectory(outputDir + "/translations");
		DownloadFile("http://webapp2.flexlink.se:8080/flqtadmin/export.htm?format=rd&appId=2000&languageCode=en_US", outputDir + "/translations/Translations.en-US.xaml");
		
	});
	
//
// Due to issues with running StartPowershellScript on MSBuild1 server, powershell is run as a separate process.
void RunPowershellInProcess(string command)
{
	//StartPowershellScript(command);
	
	Information("Running powershell command '" + command + "'");
	using(var process = StartAndReturnProcess("powershell", new ProcessSettings{ Arguments = "-Command \"" + command + "\"" }))
	{
		process.WaitForExit();
	}
}

Task("RestoreFlwsFiles")
	.Does(() =>
	{
		var flwsPath = outputDir + "/flws";
		CreateDirectory(flwsPath);
		RunPowershellInProcess("$svc = New-WebServiceProxy -Uri http://ws.flexlink.com/flws/services/FlwsServiceImplPort?wsdl; $links = $svc.getAllProductsUncompressed($null); $xml = Format-List -InputObject $links; Out-File -InputObject $xml -Encoding utf8 " + flwsPath + "/flexlink-products.xml;");
		RunPowershellInProcess("$svc = New-WebServiceProxy -Uri http://ws.flexlink.com/flws/services/FlwsServiceImplPort?wsdl; $links = $svc.getTechnicalLibraryDocuments(); $xml = Format-List -InputObject $links; Out-File -InputObject $xml -Encoding utf8 " + flwsPath + "/flexlink-technicallibrary.xml;");
	});

Task("NuGetRestorePackages")
	.Does(() =>
	{
		NuGetRestore(solutionFile);
	});
	
Task("RestoreMavenDependencies")
	.IsDependentOn("Init")
	.Does(() =>
	{			
		foreach (var dep in new []{
				new {Name="flexcadimport", OutputPath="./src/packages", OutputFilename="", Url="http://nexus.flexlink.se/repository/thirdparty/com/cad-q/flexcadimport/api/2.0.0.1/api-2.0.0.1.zip", Exclusions=new string[]{}},
				new {Name="initvclic", OutputPath=downloadDir, OutputFilename="initvclic.exe", Url="http://nexus.flexlink.se/repository/thirdparty/com/visualcomponents/initvclic/1.0.0/initvclic-1.0.0..exe", Exclusions=new string[]{}},
				new {Name="clientstarter", OutputPath=downloadDir, OutputFilename="clientstarter.exe", Url="http://nexus.flexlink.se/repository/releases/com/flexlink/flqt/flqtstarter/1.4.1/flqtstarter-1.4.1.exe", Exclusions=new string[]{}},
				new {Name="CSharpAnalytics", OutputPath="./src/packages", OutputFilename="CSharpAnalytics.Net45.dll", Url="http://nexus.flexlink.se/repository/thirdparty/com/github/CSharpAnalytics/api/1.2.0-userid-multi/api-1.2.0-userid-multi-net45.dll", Exclusions=new string[]{}},
				new {Name="3dengine-installation-x64", OutputPath="./src/packages", OutputFilename="", Url="http://nexus.flexlink.se/repository/thirdparty/com/visualcomponents/essentials/installation/4.1.1.84755/installation-4.1.1.84755-win64.zip", Exclusions=new string[]{"Uninstall", "uninstall.exe", "Uninstall", "UX.Reporting.dll", "MsiKeyFile", "VcActivatorCmd.Exe", "VcActivatorCmd.Exe.config", "VisualComponents.Engine.Launcher.exe", "VisualComponents.Engine.Launcher.exe.config"}},
			})
		{
			if (string.IsNullOrEmpty(dep.OutputFilename))
			{
				var depZip = dep.OutputPath + "/" + dep.Name + ".zip";
				var depPath = dep.OutputPath + "/" + dep.Name;
				if (!FileExists(depZip))
				{
					if (DirectoryExists(depPath))
					{
						Information("Removing previous installation");
						CleanDirectories(depPath);
					}
					CreateDirectory(depPath);
					Information("Downloading " + dep.Name);
					DownloadFile(dep.Url, depZip);
					Information("Unpacking " + dep.Name + " into " + depPath);
					Unzip(depZip, depPath);
					foreach (var exclude in dep.Exclusions)
					{
						DeleteFiles(depPath + "/" + exclude);
						if (DirectoryExists(depPath + "/" + exclude))
						{
							DeleteDirectory(depPath + "/" + exclude, new DeleteDirectorySettings {
								Recursive = true,
								Force = true
							});
						}
					}
				}
			}
			else 
			{
				var depPath = dep.OutputPath + "/" + dep.Name + "/" ;
				var depFile = depPath + dep.OutputFilename;
				if (!FileExists(depFile))
				{
					CreateDirectory(depPath);
					Information("Downloading " + depFile);
					DownloadFile(dep.Url, depFile);
				}
			}
		}
		
	});

Task("RestoreTactonModelDependencies")
	.IsDependentOn("Init")
	.Does(() =>
	{	
		var tcModelZip = downloadDir + "/TactonModels.zip";
		if (!FileExists(tcModelZip))
		{
			var tcPath = outputDir + "/tacton-models";
			if (DirectoryExists(tcPath))
			{
				Information("Removing previous models");
				CleanDirectories(tcPath);
			}
			
			CreateDirectory(tcPath);
			Information("Downloading Tacton Models into " + tcModelZip);
			DownloadFile("http://files.flexlink.com/tactonmodels/designtool/all.zip", tcModelZip);
			Information("Unpacking Tacton Model sources into " + tcPath);
			Unzip(tcModelZip, tcPath);
		}
	});
	
Task("RestoreEcat")
	.IsDependentOn("Init")
	.Does(() =>
	{
		var ecatZipPath = downloadDir + "/ecat.zip";
		if (!FileExists(ecatZipPath))
		{
			var ecatPath = outputDir + "/ecat";
			if (DirectoryExists(ecatPath))
			{
				Information("Removing previous ecat files");
				CleanDirectories(ecatPath);
			}
			
			CreateDirectory(ecatPath);
			FileWriteText(downloadDir + "/ecat-maven-metadata.xml", HttpGet("http://nexus.flexlink.se/repository/maven-public/com/flexlink/woodpecker/ecat/production/maven-metadata.xml"));
			var ecatVersion = XmlPeek(downloadDir + "/ecat-maven-metadata.xml", "//release/text()");			
			var link = "http://nexus.flexlink.se/repository/maven-public/com/flexlink/woodpecker/ecat/production/" + ecatVersion + "/production-" + ecatVersion + ".zip";
			Information("Downloading ecat from '" + link + "'");
			DownloadFile(link, ecatZipPath);
			Information("Unpacking Ecat files from '" + ecatZipPath + "'");			
			Unzip(ecatZipPath, ecatPath);
		}
	});
	
	
Task("SetVersion")
	.WithCriteria(() => isOnBuildServer)
	.Does(() => {
		Information("Assembly version='" + dotnetVersion + "'");
        ReplaceRegexInFiles("./src/**/AssemblyInfo.cs", "(?<=AssemblyVersion\\(\")(.+?)(?=\"\\))", dotnetVersion);
        ReplaceRegexInFiles("./src/**/AssemblyInfo.cs", "(?<=AssemblyFileVersion\\(\")(.+?)(?=\"\\))", dotnetVersion);
		Information("Updating changelog.html");
        ReplaceTextInFiles("./src/Installation/changelog.html", "%VERSION%", shortDotnetVersion);
        ReplaceTextInFiles("./src/Installation/changelog.html", "%DATE%", DateTime.Now.ToString("yyyy-MM-dd"));
		Information("Updating Installer version");
		using(var process = StartAndReturnProcess(advancedInstallerPath, new ProcessSettings{ Arguments = "/Edit Fldt.aip /SetVersion " + shortDotnetVersion, WorkingDirectory = "./src/Installation" }))
		{
			process.WaitForExit();
		}
   });

Task("Build")
	.IsDependentOn("Init")
	.IsDependentOn("RestoreMavenDependencies")
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

Task("Test")
	.IsDependentOn("Build")
	.Does(() =>
	{
		XUnit2("./src/TabTests/bin/Release/*Tests.dll",
			new XUnit2Settings {
			HtmlReport = true,
			OutputDirectory = outputDir,
			ArgumentCustomization = args=>args.Append("-Verbose")
			});
	});
	
Task("SignBinaries")
	.IsDependentOn("Build")
	.WithCriteria(() => isOnBuildServer)
	.Does(() => {
		foreach (var file in new []{
			new FilePath("./src/Tab/bin/" + configuration + "/Ux.FlexLink.dll"),
			new FilePath("./src/UX.PDFReport/bin/" + configuration + "/UX.Plugin.FlexLink.Pdf.dll"),
			})
		{
			Sign(file, new SignToolSignSettings {
				CertSubjectName = "FlexLink AB",
				DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
				Description = "FlexLink Design Tool Main v" + version,
				DescriptionUri = new Uri("http://www.flexlink.com/"),
				TimeStampUri = new Uri("http://timestamp.verisign.com/scripts/timstamp.dll")
				});
		}
   });

Task("BuildInstallers")
	.IsDependentOn("RestoreFlwsFiles")
	.IsDependentOn("RestoreMavenDependencies")
	.IsDependentOn("RestoreEcat")
	.IsDependentOn("RestoreTactonModelDependencies")
	.IsDependentOn("SignBinaries")
	.Does(() =>
	{
		using(var process = StartAndReturnProcess(advancedInstallerPath, new ProcessSettings{ Arguments = "/rebuild Fldt.aip", WorkingDirectory = "./src/Installation" }))
		{
			process.WaitForExit();
		}
	});

Task("SignInstallers")
	.IsDependentOn("BuildInstallers")
	.WithCriteria(() => isOnBuildServer)
	.Does(() => {
		Sign(GetFiles(outputDir + "/fldt-installation.*"), new SignToolSignSettings {
			CertSubjectName = "FlexLink AB",
			DigestAlgorithm = SignToolDigestAlgorithm.Sha256,
			Description = "FlexLink Design Tool Main v" + version,
			DescriptionUri = new Uri("http://www.flexlink.com/"),
			TimeStampUri = new Uri("http://timestamp.verisign.com/scripts/timstamp.dll")
			});
   });

Task("StartJenkinsBuild")
	.WithCriteria(() => jenkinsUserToken != "")
	.Does(() =>
	{
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
			new {name="FLDT_LINK", value=nexusAppLink}}}));
        
		Information("Starting Jenkins job '" + jenkinsJob + "' with artifacts stored in nexus with version='" + version + "'");
		HttpPost("http://" + jenkinsUrl + "/job/" + jenkinsJob + "/buildWithParameters?FLDT_LINK=" + System.Uri.EscapeDataString(nexusAppLink) + "&delay=60sec", settings);
	});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("DefaultCi")
	.IsDependentOn("Clean")
	.IsDependentOn("Test")
	.IsDependentOn("SignInstallers")
	.IsDependentOn("StartJenkinsBuild")
	;
	
Task("Default")
	.IsDependentOn("Clean")
	.IsDependentOn("Test")
	.IsDependentOn("SignInstallers")
	;
	
Task("PrepareDev")
	.IsDependentOn("NuGetRestorePackages")
	.IsDependentOn("RestoreMavenDependencies")
	.IsDependentOn("RestoreFlwsFiles")
	.IsDependentOn("RestoreTactonModelDependencies")
	.IsDependentOn("RestoreEcat")	
	;


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

