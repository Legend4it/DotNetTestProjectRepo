var target = Argument("target", "Default");
var outputDir = "./build";


Task("Init")
	.Does(() =>
	{
        Information("Inside Init Task ...! ");
		CreateDirectory(outputDir);		
	});
Task("Clean")
	.Does(() =>
	{
        Information("Inside Clean Task ...! ");
		CleanDirectories(outputDir);
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
