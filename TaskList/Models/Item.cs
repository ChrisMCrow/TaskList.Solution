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
            _dueDate = dueDate;
            _categoryId = categoryId;
            _id = id;
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
                bool areIdsEqual = (this.GetId() == newItem.GetId());
                bool areDescriptionsEqual = (this.GetDescription() == newItem.GetDescription());
                return (areIdsEqual && areDescriptionsEqual);
            }
        }

        public override int GetHashCode()
        {
            return this.GetDescription().GetHashCode();
        }


        public string GetDescription()
        {
            return _description;
        }

        // public void SetDescription(string newDescription)
        // {
        //     _description = newDescription;
        // }

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

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks WHERE id = @thisId;";

            MySqlParameter thisId = new MySqlParameter();
            thisId.ParameterName = "@thisId";
            thisId.Value = _id;
            cmd.Parameters.Add(thisId);

            cmd.ExecuteNonQuery();

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
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

        public void Edit(string newDescription)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE tasks SET description = @newDescription WHERE id = @searchId;";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);

            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);

            cmd.ExecuteNonQuery();
            _description = newDescription;

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public int FindCategoryId()
        {
            int categoryId = this.GetCategoryId();
            Category foundCategory = Category.Find(categoryId);
            return foundCategory.GetId();
        }

        public string FindCategoryName()
        {
            int categoryId = this.GetCategoryId();
            Category foundCategory = Category.Find(categoryId);
            return foundCategory.GetName();
        }
    }
}
