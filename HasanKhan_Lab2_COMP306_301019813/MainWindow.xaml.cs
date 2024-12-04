using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Color = System.Drawing.Color;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;
using Amazon.S3;
using Amazon.S3.Model;
using HasanKhan_Lab2_COMP306_301019813;

namespace HasanKhan_Lab2_COMP306_301019813
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string tableName = "Bookshelf";
        private DynamoDBContext context;
        private BookshelfTable bookshelfTable;

        static Helper connection = new Helper();
        AmazonS3Client s3Client = connection.Connection(); 
        readonly AmazonDynamoDBClient dbClient = connection.Connect();

        public MainWindow()
        {
            InitializeComponent();
            bookshelfTable = new BookshelfTable();
            this.LoginButton.IsEnabled = true;
            this.SignupButton.IsEnabled = true;
        }

       
        private void TxtUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            bookshelfTable.UserEmail = TxtUserEmail.Text;
        }

        
        private void TxtPassword_TextChanged(object sender, RoutedEventArgs e)
        {
            bookshelfTable.Password = TxtPassword.Password;
        }

        
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadDataAsync(); 
            }
            catch (AmazonS3Exception s3Ex)
            {
                MessageBox.Show($"AWS S3 Error: {s3Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (AmazonDynamoDBException dbEx)
            {
                MessageBox.Show($"DynamoDB Error: {dbEx.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await CreateTableAndUserAsync(); 
            }
            catch (AmazonS3Exception s3Ex)
            {
                MessageBox.Show($"AWS S3 Error: {s3Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (AmazonDynamoDBException dbEx)
            {
                MessageBox.Show($"DynamoDB Error: {dbEx.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task LoadDataAsync()
        {
            Table table = Table.LoadTable(dbClient, tableName);
            string email = TxtUserEmail.Text;
            string password = TxtPassword.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fields can't be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            Document doc = await table.GetItemAsync(email);
            if (doc != null)
            {
                string dbEmail = doc["UserEmail"];
                string dbPassword = doc["Password"];

                if (email == dbEmail && password == dbPassword)
                {
                    MessageBox.Show("Successfully Logged In");
                    Bookshelf booksForm = new Bookshelf(dbEmail);
                    booksForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Incorrect Email or Password entered!");
                }
            }
            else
            {
                MessageBox.Show("User not found!");
            }
        }

        private async Task CreateTableAndUserAsync()
        {
            if (string.IsNullOrEmpty(TxtUserEmail.Text) || string.IsNullOrEmpty(TxtPassword.Password))
            {
                MessageBox.Show("Fields can't be empty!", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            context = new DynamoDBContext(dbClient);
            List<string> currentTables = (await dbClient.ListTablesAsync()).TableNames;

            if (!currentTables.Contains(tableName))
            {
                await CreateBookshelfTableAsync();
            }

            await SaveUserAsync(context);
            LoginButton.IsEnabled = true;
        }

        private async Task CreateBookshelfTableAsync()
        {
            await dbClient.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 5, WriteCapacityUnits = 5 },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement
                {
                    AttributeName = "UserEmail",
                    KeyType = KeyType.HASH
                }
            },
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition
                {
                    AttributeName = "UserEmail",
                    AttributeType = ScalarAttributeType.S
                }
            }
            });
        }

        private async Task SaveUserAsync(DynamoDBContext context)
        {
            Table bookshelfTable = Table.LoadTable(dbClient, tableName);

            string email = TxtUserEmail.Text;
            Document doc = await bookshelfTable.GetItemAsync(email);
            bool userExisted = doc != null;

            if (userExisted)
            {
                MessageBox.Show("Account already exists!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var bookmarkDate1 = DateTime.UtcNow.ToString("o");
                var bookshelfDoc = new Document
                {
                    ["UserEmail"] = email,
                    ["Password"] = TxtPassword.Password,
                    ["BookTitle"] = "Hands-On Machine Learning with - Aurelien Geron",
                    ["Author"] = "Aurelien Geron",
                    ["Bookmark"] = bookmarkDate1,
                    ["BookTitle2"] = "Beginning serverless computing_ developing with Amazon Web Services, Microsoft Azure, and Google Cloud-Apress L.P (2018)",
                    ["Author2"] = "Stigler, Maddie",
                    ["Bookmark2"] = bookmarkDate1,
                    ["S3URI"] = "s3://hasankhancomp306/Hands-On Machine Learning with - Aurelien Geron.pdf",
                    ["S3URI2"] = "s3://hasankhancomp306/Stigler, Maddie - Beginning serverless computing_ developing with Amazon Web Services, Microsoft Azure, and Google Cloud-Apress L.P (2018).pdf",
                    ["LastPage1"] = "1",
                    ["LastPage2"] = "1"
                };

                await bookshelfTable.PutItemAsync(bookshelfDoc);
                MessageBox.Show("Account Created Successfully!", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}