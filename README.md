## AWS Lambda Function supporting my project portfolio.
### This serverless web api is in place to proivde a backend for my project portfolio.
### It was a little tricky to get started at first but with a few small changes, it is able to be used with REST API Gateway routes.  
### The original documentation I followed can be found here, https://www.twilio.com/blog/respond-to-twilio-webhooks-using-aws-lambda-and-dotnet.
### The first change that was required as of March 2023, is in the .csproj file.
```
//This must be updated to this version.
<PackageReference Include="Amazon.Lambda.AspNetCoreServer" Version="8.0.0" />
```
### The next change I had to make was in the LambdaEntryPoint.cs file.  It will be setup by default with HttpAPI setting, this is easy to change though.
### The LambdaEntryPoint needs to inherit from Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction not Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction.
```
//Default settings.
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayProxyFunction
{
    ... Methods here ...
}
//Needs to be changed to this.
public class LambdaEntryPoint : Amazon.Lambda.AspNetCoreServer.APIGatewayHttpApiV2ProxyFunction
{
    ... Methods here ...
}
```
