using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Table = Amazon.DynamoDBv2.DocumentModel.Table;


namespace HasanKhan_Lab2_COMP306_301019813
{
    public class DynamoDBService
    {
        private readonly AmazonDynamoDBClient dbClient;
        private readonly DynamoDBContext context;
        private readonly string tableName = "Bookshelf";

        public DynamoDBService(AmazonDynamoDBClient dbClient)
        {
            this.dbClient = dbClient;
            this.context = new DynamoDBContext(dbClient);
        }

        public async Task CreateTableAndUserAsync(string email, string password)
        {
            List<string> currentTables = (await dbClient.ListTablesAsync()).TableNames;

            if (!currentTables.Contains(tableName))
            {
                await CreateBookshelfTableAsync();
            }

            await SaveUserAsync(email, password);
        }

        private async Task CreateBookshelfTableAsync()
        {
            await dbClient.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 5, WriteCapacityUnits = 5 },
                KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement { AttributeName = "UserEmail", KeyType = KeyType.HASH }
            },
                AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition { AttributeName = "UserEmail", AttributeType = ScalarAttributeType.S }
            }
            });
        }

        private async Task SaveUserAsync(string email, string password)
        {
            Table bookshelfTable = Table.LoadTable(dbClient, tableName);
            Document doc = await bookshelfTable.GetItemAsync(email);

            if (doc != null)
            {
                throw new Exception("User already exists");
            }

            var newUser = new Document
            {
                ["UserEmail"] = email,
                ["Password"] = password,
                // Add book details or other user info here
            };

            await bookshelfTable.PutItemAsync(newUser);
        }
    }

}
