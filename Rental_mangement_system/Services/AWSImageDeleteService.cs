using Amazon.S3;
using Rental_mangement_system.Pages;
using static System.Net.Mime.MediaTypeNames;
using Amazon.S3.Model;
using Amazon;

namespace Rental_mangement_system.ImageDeleteService
{
    public class AWSImageDeleteService : Rental_mangement_system.ImageDeleteService.ImageDeleteService
    {
        private readonly IWebHostEnvironment environment;

        public AWSImageDeleteService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public async Task<bool> DeleteImageAsync(string imgName)
        {
            var accesskey = "AKIAZZTOW6JHZUG7SRUP";
            var secretkey = "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";
            RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
            var bucketName = "rmsbucket";
            try
            {
                var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = imgName
                };
                await s3Client.DeleteObjectAsync(deleteObjectRequest);

            }
            catch (AmazonS3Exception e)
            {
                    Console.WriteLine(e.Message);
            }
            return true;
        }
    }
}
