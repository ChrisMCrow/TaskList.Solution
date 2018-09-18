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
        private string _dueDate;
        private int _categoryId;

        public Item (string description, string dueDate = "", int categoryId = 0, int id = 0)
        {
            _description = description;
            _id = id;
            _dueDate = dueDate;
            _categoryId = categoryId;
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

        public string GetDueDate()
        {
            return _dueDate;
        }
        public int GetCategoryId()
        {
            return _categoryId;
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
            cmd.CommandText = @"SELECT * FROM tasks ORDER BY due_date ASC;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

            while(rdr.Read())
            {
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                string itemDueDate = rdr.GetString(2);
                int itemCategoryId = rdr.GetInt32(3);
                Item newItem = new Item(itemDescription, itemDueDate, itemCategoryId, itemId);
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
            cmd.CommandText = @"INSERT INTO tasks (description, due_date, category_id) VALUES (@itemDescription, @itemDueDate, @itemCategoryId);";

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@itemDescription";
            description.Value = _description;
            cmd.Parameters.Add(description);

            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@itemDueDate";
            dueDate.Value = _dueDate;
            cmd.Parameters.Add(dueDate);

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@itemCategoryId";
            categoryId.Value = _categoryId;
            cmd.Parameters.Add(categoryId);

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
            string dueDate = "";
            int categoryId = 0;

            while (rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);
                dueDate = rdr.GetString(2);
                categoryId = rdr.GetInt32(3);
            }

            Item foundItem = new Item(itemDescription, dueDate, categoryId, itemId);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return foundItem;
        }
    }
}
