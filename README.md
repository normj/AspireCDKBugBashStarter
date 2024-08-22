# Aspire CDK Bug Bash playground

This repository is for testing the user experience of using CDK with Aspire. You can provide feedback by opening issues or discussions on this repository.

## Global.json issues

If you have .NET 9 preview installed it can cause issues with getting the Aspire workload setup correctly. In the repository there is a `global.json` to lock the .NET SDK version to `8.0.400`. If you have a different .NET SDK version installed update this file to that version. Or if you don't have .NET 9 preview installed you can remove the `global.json` file.

## Aspire setup

Make sure your Visual Studio 2022 is updated to the latest version. You also need to make sure you have the Aspire workload installed at version 8.2.0.

To confirm Aspire worload installation is correct:
* Open command line and navigate to the repository.
* Run `dotnet workload list` to see what workloads are installed.
* If you see `aspire 8.2.0/8.0.100` you are good.
* If you don't see Aspire in the list run `dotnet workload install`
* If you see an Aspire version older then 8.2.0 run `dotnet workload update`
* If you see a 9.X Aspire version then run `dotnet workload uninstall` and then `dotnet workload install`. 

## Repository Solutions

The repository has prebuilt Aspire preview NuGet packages that include the CDK PR. https://github.com/dotnet/aspire/pull/2225. It also contains 2 .NET solutions preconfigured with references to the preview packages.

### Solution CustomerFeedback

This solution contains a simple .NET application with a frontend application that allows entering feedback and then a backend processor for doing semantic analysis on the feedback. The semantic analysis can be viewed when looking at the logs of the backend processor.

The AppHost demonstrates the different approaches of creating the required AWS resources needed for the application. In this case a SNS topic and a SQS queue with the queue subscribed to the topic.

### Solution AspireStarterApplication

This is basically the Aspire starter project template in Visual Studio but has the preview NuGet packages configured for it. Use this solution as a clean slate to get started from scratch but without having to figure out how to configure the preview packages.

