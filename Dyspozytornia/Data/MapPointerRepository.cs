using System;
using System.Collections;
using System.Reflection;
using Dyspozytornia.Models;
using MySql.Data.MySqlClient;

namespace Dyspozytornia.Data
{
    public class MapPointerRepository : IMapPointerRepository
    {
        public ArrayList CreateStoreTable() 
        {
            var conn = Connect();
            conn.Open();
            const string sql = "select StoreId, Name, NIP, City, Street, HomeNumber, Longitude, Latitude from Stores";
            
            var listOfPointers = new ArrayList();
            
            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    var newPoint = new NewMapPointer
                    {
                        PointId = resultSet.GetInt32("StoreId"),
                        PointName = resultSet.GetString("Name"),
                        Nip = resultSet.GetString("NIP"),
                        PointCity = resultSet.GetString("City"),
                        PointAddress = resultSet.GetString("Street"),
                        PointAddressBlockNumber = resultSet.GetString("HomeNumber"),
                        PointLongitude = resultSet.GetFloat("Longitude"),
                        PointLatitude = resultSet.GetFloat("Latitude")
                    };
                    listOfPointers.Add(newPoint);
                }
                conn.Close();

            } catch (Exception e) {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            return listOfPointers;
        }
        
        public void CreateMapPointer(NewMapPointer mapPointer, string typeOfPoint) 
        {
            var sql = "";

            switch (typeOfPoint)
            {
                case "Stores":
                    sql =
                        "Insert into Stores (Name, NIP, City, Street, HomeNumber, Longitude, Latitude) values(?name, ?nip, ?city, ?street, ?homeNumber, ?longitude, ?latitude)";
                    break;
                case "Shops":
                    sql =
                        "Insert into Shops (Name, NIP, City, Street, HomeNumber, Longitude, Latitude) values(?name, ?nip, ?city, ?street, ?homeNumber, ?longitude, ?latitude)";
                    break;
            }

            var conn = Connect();
            conn.Open();
            var cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = mapPointer.PointName;
            cmd.Parameters.Add("?nip", MySqlDbType.VarChar).Value = mapPointer.Nip;
            cmd.Parameters.Add("?city", MySqlDbType.VarChar).Value = mapPointer.PointCity;
            cmd.Parameters.Add("?street", MySqlDbType.VarChar).Value = mapPointer.PointAddress;
            cmd.Parameters.Add("?homeNumber", MySqlDbType.VarChar).Value = mapPointer.PointAddressBlockNumber;
            cmd.Parameters.Add("?longitude", MySqlDbType.Float).Value = mapPointer.PointLongitude;
            cmd.Parameters.Add("?latitude", MySqlDbType.Float).Value = mapPointer.PointLatitude;
            
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("MapPointer " + mapPointer.PointName + " created successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
        }
        
        public NewMapPointer GetPointerByName(string shopName) 
        {
            var conn = Connect();
            conn.Open();
            const string sql = "Select * from Shops where Name = @Name";
            
            var newPoint = new NewMapPointer();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Name", shopName);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader(); 
                
                if (resultSet.Read()) {
                    newPoint.PointId = resultSet.GetInt32("ShopId");
                    newPoint.PointName = resultSet.GetString("Name");
                    newPoint.PointLongitude = resultSet.GetFloat("Longitude");
                    newPoint.PointLatitude = resultSet.GetFloat("Latitude");
                }
                conn.Close();

            } catch (Exception e) {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }

            return newPoint;
        }
        
        public ArrayList CreateShopTable() 
        {
            
            var conn = Connect();
            conn.Open();
            const string sql = "select ShopId, Name, NIP, City, Street, HomeNumber, Longitude, Latitude from Shops";
            
            var listOfPointers = new ArrayList();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();

                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    var newPoint = new NewMapPointer
                    {
                        PointId = resultSet.GetInt32("ShopId"),
                        PointName = resultSet.GetString("Name"),
                        Nip = resultSet.GetString("NIP"),
                        PointCity = resultSet.GetString("City"),
                        PointAddress = resultSet.GetString("Street"),
                        PointAddressBlockNumber = resultSet.GetString("HomeNumber"),
                        PointLongitude = resultSet.GetFloat("Longitude"),
                        PointLatitude = resultSet.GetFloat("Latitude")
                    };
                    listOfPointers.Add(newPoint);
                }
                conn.Close();

            } catch (Exception e) {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            return listOfPointers;
        }

        private static MySqlConnection Connect()
        {
            var connection = new MySqlConnection();

            const string server = "localhost";
            const string database = "sipdb";
            const string user = "sipAccessUser";
            const string password = "sip";
            const string port = "3306";
            const string sslM = "none";

            connection.ConnectionString =
                $"server={server};port={port};user id={user}; password={password}; database={database}; SslMode={sslM}";

            return connection;
        }
    }
}