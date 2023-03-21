## AWS Lambda Function supporting my project portfolio.

### ExampleController ###

This serverless web api is in place to proivde a backend for my project portfolio.  It was a little tricky to get started at first but with a few small changes, it is able to be used with REST API Gateway routes.  

### Setup and Notes ###

The original documentation I followed can be found here, https://www.twilio.com/blog/respond-to-twilio-webhooks-using-aws-lambda-and-dotnet.  I did have to make some changes to the steps though since I was using a Rest API Gateway in AWS.

1. Install necessary templates in the command line, this can be done from a terminal inside VSCode.

```
dotnet new -i Amazon.Lambda.Templates
```

2. This will give you a list of options available.  We want to use the serverless.AspNetCoreWebAPI

```
dotnet new serverless.AspNetCoreWebAPI --name <name of your project> --output .
```

3. This project will create a test and src folder.  You will need to move into the subdirectory to continue adding dependencies

```
cd src/<name of your project>
dotnet add package Amazon.Lambda.AspNetCoreServer.Hosting
```

4. You need to install Amazon.Lambda.Tools once on your system globally.  If this has already been done, this step can be skipped.

```
dotnet tool install -g Amazon.Lambda.Tools
```

5. The next step is to update the aws-lambda-tools-defaults.json, if this step is skipped you will see extra prompts after you starting deploying the function.  The function-url isn't necessary since we will be using an API Gateway.  However, you can enable it and it won't have any effect on the API Gateway.

```
{
  "profile": "<If already set through AWS CLI, this not necessary>",
  "region": "<If already set through AWS CLI, this not necessary>",
  "configuration": "Release",
  "function-runtime": "dotnet6",
  "function-memory-size": 512,
  "function-timeout": 60,
  "function-handler": "<name of your project>",
  "function-name": "<name of your project>",
  "function-url-enable": false
}
```

6. A small change in LambdaEntryPoint.cs needs to be made since we are using a RestAPI.  This template will default to the HttpAPI setup.

```
// The APIGateWayProxyFunction is to be used for HttpApi's
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{}

// APIGatewayHttpApiV2ProxyFunction is used with RestApi's and what we need for this project to work correctly.
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction
{}
```
7. In the Startup file, we need to add LambdaEventSource.RestApi into our project.

```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
}
```
8.  The last change needed before deploying the function is to update the version for Amazon.Lambda.AspNetCoreServer.  It is currently creating the template with an older version and needs to be a 8.0.0 or higher.

```
ace-ecommerce-incoming.csproj
<PackageReference Include="Amazon.Lambda.AspNetCoreServer" Version="8.0.0" />
```
9. Finally, the function is ready to deploy.  You will be prompted for any remaining function parameters needed.  This will include creating a IAM role. You can either select an existing or new one.  If you select a new role, you will have to name it as well.  Then, you will be able to attach IAM policies.  Selecting AWSLambdaExecute gives the full access to S3 and CloudWatch Logs, and was used when starting this lambda function.

```
dotnet lambda deploy-function
```

The lambda function is now ready to be setup using a Rest API Gateway and be tested.

### Comments, Questions, or Suggestions? ###

Please feel free to reach out to me at tausti0065@gmail.com.
