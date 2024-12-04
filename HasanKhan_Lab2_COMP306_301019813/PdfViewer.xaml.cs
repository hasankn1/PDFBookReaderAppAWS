using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amazon;
using Amazon.Runtime;
using System.Data;
using System.Drawing;
using Color = System.Drawing.Color;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;
using Microsoft.Extensions.Configuration;
using System.IO;
using Syncfusion.Windows.PdfViewer;
using Amazon.S3;
using Amazon.S3.Model;
using HasanKhan_Lab2_COMP306_301019813;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;

namespace HasanKhan_Lab2_COMP306_301019813
{
    /// <summary>
    /// Interaction logic for PdfViewer.xaml
    /// </summary>
    public partial class PdfViewer : Window
    {
        string userEmail;
        string bookTitle;

        private DynamoDBContext context;
        static readonly Helper connection = new Helper();
        readonly AmazonDynamoDBClient dynamoDbClient = connection.Connect();
        readonly AmazonS3Client s3Client = connection.Connection();

        public PdfViewer(string UserEmail, string BookTitle)
        {
            InitializeComponent();

            try
            {
                userEmail = UserEmail;
                bookTitle = BookTitle;
                LoadBookFromS3();
            }
            catch (AmazonDynamoDBException dbEx)
            {
                MessageBox.Show($"Error initializing DynamoDB client: {dbEx.Message}");
            }
            catch (AmazonS3Exception s3Ex)
            {
                MessageBox.Show($"Error initializing S3 client: {s3Ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during initialization: {ex.Message}");
            }
        }

        private async void LoadBookFromS3()
        {
            Table table = Table.LoadTable(dynamoDbClient, "Bookshelf");
            Document doc = await table.GetItemAsync(userEmail);

            // Retrieve S3URI based on bookTitle
            string s3Uri;
            if (bookTitle.Equals(doc["BookTitle"].AsString()))
            {
                s3Uri = doc["S3URI"].AsString();
            }
            else if (bookTitle.Equals(doc["BookTitle2"].AsString()))
            {
                s3Uri = doc["S3URI2"].AsString();
            }
            else
            {
                MessageBox.Show("Book not found in the database.");
                return;
            }

            // Split S3URI to get the bucket name and object key
            Uri uri = new Uri(s3Uri);
            string bucketName = uri.Host;
            string key = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'));

            // Fetch the book from S3 using the bucket name and object key
            MemoryStream docStream = await FetchBookFromS3(bucketName, key);
            PdfViewer1.ItemSource = docStream; // Load into PdfViewer

            // Load the last saved page number
            int lastPageIndex = GetLastPageIndex(doc);
            PdfViewer1.GotoPage(lastPageIndex); // Open the book at the last saved page
            bookmarkLabel.Content = lastPageIndex; // Update the bookmark label to show the last page bookmarked
        }

        private int GetLastPageIndex(Document doc)
        {
            // Initialize last page index to 0 (or any default)
            int lastPageIndex = 0;

            // Retrieve the bookmark based on book title
            if (bookTitle.Equals(doc["BookTitle"].AsString()))
            {
                lastPageIndex = doc["LastPage1"].AsInt();
            }
            else if (bookTitle.Equals(doc["BookTitle2"].AsString()))
            {
                lastPageIndex = doc["LastPage2"].AsInt();
            }

            return lastPageIndex;
        }

        private async Task<MemoryStream> FetchBookFromS3(string bucketName, string key)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            GetObjectResponse response = await s3Client.GetObjectAsync(request);
            MemoryStream docStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(docStream);
            return docStream;
        }

        private async void Bookmark(string bookTitle)
        {
            context = new DynamoDBContext(dynamoDbClient);
            UpdateItemOperationConfig config = new UpdateItemOperationConfig
            {
                ReturnValues = ReturnValues.AllNewAttributes
            };
            Table table = Table.LoadTable(dynamoDbClient, "Bookshelf");
            var user = new Document();
            user["UserEmail"] = userEmail;
            user["DateTime"] = DateTime.Now;

            // Determine which bookmark to update
            if (bookTitle.Equals("Hands-On Machine Learning with - Aurelien Geron"))
            {
                user["LastPage1"] = PdfViewer1.CurrentPageIndex;
            }
            else if (bookTitle.Equals("Beginning serverless computing_ developing with Amazon Web Services, Microsoft Azure, and Google Cloud-Apress L.P (2018)"))
            {
                user["LastPage2"] = PdfViewer1.CurrentPageIndex;
            }

            await table.UpdateItemAsync(user, config);
            MessageBox.Show("Bookmark saved successfully!");
        }

        // Bookmark button click handler
        private void BookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            Bookmark(bookTitle);
            LoadBookFromS3();
        }

        private void Window_ClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Bookmark(bookTitle);
            Bookshelf bookshelfWindow = new Bookshelf(userEmail);
            bookshelfWindow.Show();
            e.Cancel = true;
            this.Hide();
        }
    }
}
