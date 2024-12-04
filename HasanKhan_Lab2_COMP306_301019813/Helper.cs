using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;

namespace HasanKhan_Lab2_COMP306_301019813
{
    class Helper
    {
        public readonly static IAmazonS3 s3Client;

        static Helper()
        {
            s3Client = GetS3Client();
        }

        private static IAmazonS3 GetS3Client()
        {
            return new AmazonS3Client(RegionEndpoint.CACentral1);
        }
        public AmazonDynamoDBClient Connect()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            string accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            string secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;
            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, Amazon.RegionEndpoint.CACentral1);
            return client;
        }
        public AmazonS3Client Connection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            string accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            string secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;


            AmazonS3Client client = new AmazonS3Client(accessKeyID, secretKey, RegionEndpoint.CACentral1);
            return client;
        }
    }
}
