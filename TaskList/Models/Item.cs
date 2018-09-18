using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TaskList;

namespace TaskList.Models
{
    public class Item
    {
        private string _description;
        private int _id;

        public Item (string description, int id = 0)
        {
            _description = description;
            _id = id;
        }


        public string GetDescription()
        {
            return _description;
        }

        public void SetDescription(string newDescription)
        {
            _description = newDescription;
        }

        public int GetId()
        {
            return _id;
        }

        public override bool Equals(System.Object otherItem)
        {
            if (!(otherItem is Item))
            {
                return false;
            }
            else
            {
                Item newItem = (Item) otherItem;
                bool idEquality = (this.GetId() == newItem.GetId());
                bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
                return (idEquality && descriptionEquality);
            }
        }

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                Item newItem = new Item(itemDescription, itemId);
                allItems.Add(newItem);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks;";

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO tasks (description) VALUES (@itemDescription);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@itemDescription";
            description.Value = _description;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static Item Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = id;
            cmd.Parameters.Add(thisId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;

            int itemId = 0;
            string itemDescription = "";

            while (rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
            }

            Item foundItem = new Item(itemDescription, itemId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundItem;
        }
    }
}
