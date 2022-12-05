using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StoredProcedureCall {
    class Program {

        // declares the list of items 
        static List<Item> Items = new List<Item>();

      
        static void Main(string[] args) {
            GetListOfItems(); //calling the function 
            DisplayListOfItems();
        }

        public static void GetListOfItems() {
            using (SqlConnection sqlCon = new SqlConnection("Data Source=SHAJITH; Initial Catalog=WideWorldImporters; Integrated Security=SSPI")) {
                sqlCon.Open();
                SqlCommand sql_cmnd = new SqlCommand("TargetStockLevel", sqlCon);
                sql_cmnd.CommandType = CommandType.StoredProcedure;
                sql_cmnd.Parameters.AddWithValue("@StockLevel", SqlDbType.Int).Value = 100; //same parameter otherwise it will error
                SqlDataReader reader = sql_cmnd.ExecuteReader();

                Items = new List<Item>();
                //this explains while all of the parameters have been read and added to the items list and then closed
                while (reader.Read()) 
                {
                    Item item = new Item();
                    item.StockItemId = int.Parse(reader["StockItemId"].ToString()); // turns into a string initially on c# so it needs ot be converted
                    item.StockItemName = reader["StockItemName"].ToString();
                    item.TargetStockLevel = int.Parse(reader["TargetStockLevel"].ToString());
                    item.LastStockTakeQuantity = int.Parse(reader["LastStockTakeQuantity"].ToString());
                    item.RunningStockValueExcVat = decimal.Parse(reader["RunningStockValueExcVat"].ToString());
                    item.StockLevelWarning = reader["StockLevelWarning"].ToString();
                    Items.Add(item);
                }

                sqlCon.Close();
            }
        }

        public static void DisplayListOfItems()
        {
            foreach(Item item in Items) 
            {
                Console.WriteLine(item.ToString());
            }
        }

    }

    public class Item
    {
        public int StockItemId { get; set; }

        public string StockItemName { get; set; }

        public int TargetStockLevel { get; set; }

        public int LastStockTakeQuantity { get;  set; }

        public decimal RunningStockValueExcVat { get; set; }

        public string StockLevelWarning { get; set; }

        public override string ToString() { // without override it will show the c# ways of implementing it The object ToString will show the defualt name of the class
            return $"Stock Item: {StockItemName} (ID:{StockItemId})";
        }
    }
}

