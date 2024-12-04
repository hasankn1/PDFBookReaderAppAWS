using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HasanKhan_Lab2_COMP306_301019813
{
    public class BookshelfTable
    {
        [DynamoDBHashKey] // Partition key
        public string UserEmail { get; set; }

        [DynamoDBProperty]
        public string Password { get; set; }

        // Book #1
        [DynamoDBProperty]
        public string BookTitle { get; set; }

        [DynamoDBProperty]
        public string Author { get; set; }

        [DynamoDBProperty]
        public string Bookmark { get; set; }

        [DynamoDBProperty]
        public string S3URI { get; set; }

        [DynamoDBProperty]
        public string LastPage1 { get; set; }

        // Book #2
        [DynamoDBProperty]
        public string BookTitle2 { get; set; }

        [DynamoDBProperty]
        public string Author2 { get; set; }

        [DynamoDBProperty]
        public string Bookmark2 { get; set; }

        [DynamoDBProperty]
        public string S3URI2 { get; set; }

        [DynamoDBProperty]
        public string LastPage2 { get; set; }
    }
}
