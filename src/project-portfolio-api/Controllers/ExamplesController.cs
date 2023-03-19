using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.S3.Model;

namespace project_portfolio_api.Controllers;

[Route("[controller]")]
public class ExamplesController : Controller
{
    private string bucketName = Environment.GetEnvironmentVariable("S3_BUCKET")!;
    private string s3Key = Environment.GetEnvironmentVariable("S3_KEY")!;

    [HttpGet]
    public async Task<string> Get()
    {
        using (AmazonS3Client client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1))
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = s3Key,
            };

            try
            {
                GetObjectResponse response = await client.GetObjectAsync(request);
                StreamReader reader = new StreamReader(response.ResponseStream);
                return reader.ReadToEnd();
            }
            catch (AmazonS3Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}