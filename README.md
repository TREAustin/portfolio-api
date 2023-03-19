## AWS Lambda Function supporting my project portfolio.
### This serverless web api is in place to proivde a backend for my project portfolio.  It was a little tricky to get started at first but with a few small changes, it is able to be used with REST API Gateway routes.  The original documentation I followed can be found here, https://www.twilio.com/blog/respond-to-twilio-webhooks-using-aws-lambda-and-dotnet.  The first change that was required as of March 2023, is in the .csproj file.
```
//This must be updated to this version.
<PackageReference Include="Amazon.Lambda.AspNetCoreServer" Version="8.0.0" />
```
### The next change I had to make was in the LambdaEntryPoint.cs file.  It will be setup by default with HttpAPI setting, this is easy to change though.  The LambdaEntryPoint needs to inherit from APIGatewayHttpApiV2ProxyFunction not APIGatewayProxyFunction, the example below shows this below.
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
### The final change needed is in startup, the documentation from Twilio will show using LambdaEventSource.HttpApi.  However, this will not work with the RestAPI.  Changing this to LambdaEventSource.RestApi is all that is needed.
```
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    ///Add this line here, 
    services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
}
```
### This web API is still under development.  A few things to come, I am looking to use AWS DynamoDB for my database solution.  Currently, I have a JSON file stored into an S3 bucket.  This works fine for this application since it is a low traffic website at this time.  However, I am looking to expand the functionality and add to the controllers.
