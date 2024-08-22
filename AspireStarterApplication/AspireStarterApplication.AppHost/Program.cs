using Amazon;

var builder = DistributedApplication.CreateBuilder(args);

// Update this to the profile and region you want to test with. 
// This can be removed if you want to rely on default environmental profile region setting.
var awsConfig = builder.AddAWSSDKConfig()
                        .WithProfile("default")
                        .WithRegion(RegionEndpoint.USWest2);

var apiService = builder.AddProject<Projects.AspireStarterApplication_ApiService>("apiservice");

builder.AddProject<Projects.AspireStarterApplication_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
