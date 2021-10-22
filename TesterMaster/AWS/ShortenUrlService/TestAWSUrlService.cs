namespace TesterMaster.AWS.UrlService
{
    using Amazon;
    using Amazon.S3;
    using Amazon.S3.Model;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    public class TestAWSUrlService
    {
        private const double timeoutDuration = 12;

        public static IAmazonS3 CreateS3Client()
        {
            AmazonS3Config config = new AmazonS3Config();
            config.RegionEndpoint = RegionEndpoint.USEast1;

            string accessKey = "key";
            string secretKey = "secret";
            string sessionToken = "IQoJb3JpZ2luX2VjEJT//////////wEaCXVzLWVhc3QtMSJIMEYCIQC4jwoLs6X585hc48DBAsO2yOjvvR0cEWh2jB6AT7LJxAIhANbZuvRqwplJuEoH7cviVkCc0iqnwdazXYD9gUeMJwklKqMDCI3//////////wEQAhoMNDcwNDI3MzU1NDE4Igz7mHHf+bJX4Vgb2oMq9wJaXR9tkWyleSkxlXeTSS1EifvQRfh0C8FF1JWdywIAcH3E4OzdQBEYLRBWb8KZ+YkCJnJMZcZ+iFwfQYMxT4dM3l9dMVapQgSH2MVMUo05B9/l0RrsOadIijTkf6SLxCxW1ba4eM7Kzko3zFIF37l+vXTcoc4KYNxoTk1xAx4aaFLCBVDN3UcmwfHsljPysq9GEgTOVVSXx9Ox6YfgOV0b+1QjTtJiF8ngE7Nod27/gqYEPhejTn6KwvcFSG/GMfkdzm2hWFvp108PvErcP0m7+qhhMwfHDKb1mWEN5Hj1Hv7QKmVWuUkHZjCq06EB7NrvW4K6NCZMK5xprRv/wqzH6LI+koEKaq3TIzlTRgEYvZ9KASvcjtRJMX+90+ri1yGJb/3GzyAV59dQtttC2iQCjMkQ0c7/F5dDfwolhzuYWljQuu9AR5eweYyluhyWHc7zhc4fnw3cUAR2MR0LC5h05BMF6Sa1Yk+wHb72cv1lFwUD89ZrV1kwyu6JgQY6pQGZ98bASV7UddGok0DCRVolwy2sqc9GUsISGcGiIv6Qz/QfvUVZpyx7VMF3kprOF184u4mBDnEH+LeMu3IOc2YNFtglYjwBHhdG6SIewAxL0tGNRzbkps7y3GMRROVjQh4uwxTAuzFg5t+2viYjW74cugljxmg4UUdCaqwPmgzXF3RYTGC3p5150jHMt0FbVjPNmqH8RRDvJqfmqnQOeT+6OAGRouQ=";

            return new AmazonS3Client(
                accessKey,
                secretKey,
                sessionToken,
                config
            );
        }

        private static async Task<ListBucketsResponse> GetBucketsListAsync(IAmazonS3 client)
        {
            var response = await client.ListBucketsAsync();
            foreach (S3Bucket b in response.Buckets)
            {
                Console.WriteLine("{0}\t{1}", b.BucketName, b.CreationDate);
            }
            return response;
        }

        public static string TestAccess()
        {
            var client = CreateS3Client();

            //var buckets = GetBucketsListAsync(client).GetAwaiter().GetResult();

            string urlString = GeneratePreSignedURL(client, timeoutDuration, "bucket", "objectKey");
            Console.WriteLine(urlString);

            return urlString;
        }

        static string GeneratePreSignedURL(IAmazonS3 client, double duration, string bucketName, string objectKey)
        {
            string urlString = "";
            try
            {
                GetPreSignedUrlRequest request1 = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    Expires = DateTime.UtcNow.AddHours(duration)
                };
                urlString = client.GetPreSignedURL(request1);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            return urlString;
        }

        public static Stream TestFileStreamByUrl(string url)
        {
            //Create a stream for the file
            Stream stream = null;

            //Create a WebRequest to get the file
            HttpWebRequest fileReq = (HttpWebRequest)HttpWebRequest.Create(url);

            //Create a response for this request
            HttpWebResponse fileResp = (HttpWebResponse)fileReq.GetResponse();

            //Get the Stream returned from the response
            stream = fileResp.GetResponseStream();

            return stream;
        }
    }
}
