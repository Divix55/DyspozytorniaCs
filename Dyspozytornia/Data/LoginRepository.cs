using System;
using System.Reflection;
using Dyspozytornia.Models;
using MySql.Data.MySqlClient;

namespace Dyspozytornia.Data
{
    public class LoginRepository : ILoginRepository
    {
        public bool CreateUser(User user)
        {
            var conn = Connect();
            conn.Open();
            bool isCreated;

            var sql =
                "Insert into Users (StoreId, UserName, FirstName, LastName, Email, UserPassword) values(?storeId, ?userName, ?firstName, ?lastName, ?email, ?password)";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.Add("?storeId", MySqlDbType.Int32).Value = user.StoreId;
            cmd.Parameters.Add("?userName", MySqlDbType.VarChar).Value = user.UserName;
            cmd.Parameters.Add("?firstName", MySqlDbType.VarChar).Value = user.FirstName;
            cmd.Parameters.Add("?lastName", MySqlDbType.VarChar).Value = user.LastName;
            cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = user.Email;
            cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = user.UserPassword;

            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("User " + user.UserName + " created successful!");
                isCreated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
                isCreated = false;
            }

            conn.Close();
            return isCreated;
        }

        public bool LoginUser(string userName, string hashedPassword)
        {
            var conn = Connect();
            conn.Open();
            var userExist = false;

            var sql =
                "select u.UserName, u.UserPassword from Users u where UserName = @userName and UserPassword = @userPassword";

            var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("userName", userName);
            cmd.Parameters.AddWithValue("userPassword", hashedPassword);

            try
            {
                var output = cmd.ExecuteScalar();
                if (output != null) userExist = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }

            conn.Close();
            return userExist;
        }

        public MySqlConnection Connect()
        {
            var connection = new MySqlConnection();

            var server = "localhost";
            var database = "sipdb";
            var user = "sipAccessUser";
            var password = "sip";
            var port = "3306";
            var sslM = "none";

            connection.ConnectionString =
                $"server={server};port={port};user id={user}; password={password}; database={database}; SslMode={sslM}";

            return connection;
        }
    }
}