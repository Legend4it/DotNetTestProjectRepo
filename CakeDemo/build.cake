var target = Argument("target", "Default");
var outputDir = "./build";
var downloadDir = "./build/download";


Task("Init")
	.Does(() =>
	{
        Information("Inside Init Task ...! ");
		// CreateDirectory(outputDir);	
		CreateDirectory(downloadDir);	
        DownloadFile("http://files.flexlink.com/tacton/all.zip", downloadDir+"/FlexlinkTactonSources.zip");	
        DownloadFile("http://nexus.flexlink.se/repository/thirdparty/com/adobe/acrobatreader/installer/11.0.8.4/installer-11.0.8.4.exe", downloadDir+"/adobe.exe");	

	});
Task("Clean")
	.Does(() =>
	{
        Information("Inside Clean Task ...! ");
		CreateDirectory(downloadDir);	
		CleanDirectories(outputDir);
        DeleteDirectory(outputDir);
		CleanDirectories("./src/**/bin");
		CleanDirectories("./src/**/obj");
	});
Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Init")
    .Does(()=>{
        Information("Inside Build Task ...! ");
        MSBuild("./src/CakeDemo.sln");
    });

Task("Default")
.IsDependentOn("Build")
.Does(()=>{
    Information("Inside Build Task ...! ");
    CopyFiles("./src/CakeDemo/bin/Debug/*.exe", "./build");
});

RunTarget(target);
