using Rental_mangement_system.Pages;
using System.Reflection.Emit;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.S3.Model;
using System.Configuration;
using ThirdParty.BouncyCastle.Asn1;
using static System.Net.WebRequestMethods;
using System.IO;

namespace Rental_mangement_system.ImageUploadService
{
    public class LocalImageUploadService : Rental_mangement_system.ImageUploadService.ImageUploadService
    {
        private readonly IWebHostEnvironment environment;

        public LocalImageUploadService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }
        public async Task<string> UploadImageAsync(IFormFile image)
        {

            string img_key = "";
            /*
            string img_dir = Path.Combine(environment.WebRootPath, "img", LoginModel.current_user_id);
            if (!Directory.Exists(img_dir))
            {
                Directory.CreateDirectory(img_dir);
            }
            var imgPath = Path.Combine(environment.WebRootPath, "img", LoginModel.current_user_id, image.FileName);
            using var fileStream = new FileStream(imgPath, FileMode.Create);
            await image.CopyToAsync(fileStream);
            */
            //var fs = new FileStream(image.FileName, FileMode.OpenOrCreate);
            //await image.CopyToAsync(fs);

            /*using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
            }*/

            var fs = image.OpenReadStream();
            //FileStream fs = File.OpenRead(image, FileMode.Create);
            var accesskey = "access key";
            var secretkey = "secret key";
            RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
            var bucketName = "rmsbucket";
            img_key = "property_images/" + LoginModel.current_user_id + "/" + image.FileName;
            var imgPath = "https://rmsbucket.s3.us-west-2.amazonaws.com/" + img_key;
            var s3Client = new AmazonS3Client(accesskey, secretkey, bucketRegion);
            var fileTransferUtility = new TransferUtility(s3Client);

            try
            {
                /*while (fs == null)
                {
                    Console.WriteLine("waiting for stream");
                }*/

                var fileTransferRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6300000,
                    Key = img_key,
                    CannedACL = S3CannedACL.PublicRead,
                    InputStream = fs,



                };
                fileTransferUtility.UploadAsync(fileTransferRequest).GetAwaiter().GetResult();
                Console.WriteLine("File uploaded successfully");

            }
            catch (AmazonS3Exception e)
            {
                if (e.ErrorCode != null && (e.ErrorCode.Equals("InvalidAccessKeyId") || e.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("AWS credentials error");
                }
                else
                {
                    Console.WriteLine(e.Message);
                }

            }
            return imgPath;
        }
    }
}
