using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Linq;
using System.Windows;
using System.Data;
using Amazon.S3;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
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
using Amazon.S3.Model;
using HasanKhan_Lab2_COMP306_301019813;

namespace HasanKhan_Lab2_COMP306_301019813
{
    /// <summary>
    /// Interaction logic for BooksList.xaml
    /// </summary>
    public partial class Bookshelf : Window
    {
        public string userEmail;
        private DynamoDBContext context;
        static Helper connection = new Helper();
        readonly AmazonDynamoDBClient client = connection.Connect();

        public Bookshelf(string email)
        {
            InitializeComponent();
            userEmail = email;
            txtUserEmail.Text = userEmail; // Display user email on UI
            LoadDataAsync(userEmail); // Load book data for the user
        }

        // Async method to load book data from DynamoDB
        public async Task LoadDataAsync(string email)
        {
            try
            {
                Table table = Table.LoadTable(client, "Bookshelf");
                Document doc = await table.GetItemAsync(email);

                if (doc != null)
                {
                    // Extract book titles and timestamps from the document
                    string title1 = doc["BookTitle"].AsString();
                    string title2 = doc["BookTitle2"].AsString();
                    DateTime time1 = DateTime.Parse(doc["Bookmark"].AsString());
                    DateTime time2 = DateTime.Parse(doc["Bookmark2"].AsString());

                    // Sort the books by their last bookmark (time)
                    if (DateTime.Compare(time1, time2) > 0)
                    {
                        Book1.Content = title1;
                        Book2.Content = title2;

                    }
                    else
                    {
                        Book1.Content = title2;
                        Book2.Content = title1;
                    }
                }
                else
                {
                    MessageBox.Show("No books found for this user.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the first book button
        private void Book1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedBookTitle = Book1.Content.ToString();
                OpenBookViewer(selectedBookTitle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Event handler for the second book button
        private void Book2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selectedBookTitle = Book2.Content.ToString();
                OpenBookViewer(selectedBookTitle);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while opening the book: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // open the PdfViewer window for the selected book
        private void OpenBookViewer(string bookTitle)
        {
            PdfViewer viewForm = new PdfViewer(userEmail, bookTitle); // Pass userEmail and book title to PdfViewer
            viewForm.Show();
            this.Hide(); // Hide the current window
        }

        private void Window_ClosingHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Close();
        }
    }
}

