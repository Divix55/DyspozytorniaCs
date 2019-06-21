using System;
using System.Collections;
using System.Reflection;
using Dyspozytornia.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Dyspozytornia.Data
{
    public class MapPointerRepository : IMapPointerRepository
    {
        public ArrayList CreateStoreTable() 
        {
            var conn = Connect();
            conn.Open();
            var sql = "select StoreId, Name, NIP, City, Street, HomeNumber, Longitude, Latitude from Stores";
            
            var listOfPointers = new ArrayList();
            
            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    NewMapPointer newPoint = new NewMapPointer();
                    newPoint.pointId = resultSet.GetInt32("StoreId");
                    newPoint.pointName = resultSet.GetString("Name");
                    newPoint.nip = resultSet.GetString("NIP");
                    newPoint.pointCity = resultSet.GetString("City");
                    newPoint.pointAddress = resultSet.GetString("Street");
                    newPoint.pointAddressBlockNumber = resultSet.GetString("HomeNumber");
                    newPoint.pointLongitude = resultSet.GetDouble("Longitude");
                    newPoint.pointLatitude = resultSet.GetDouble("Latitude");
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
            
            if (typeOfPoint == "Stores") {
                sql = "Insert into Stores (Name, NIP, City, Street, HomeNumber, Longitude, Latitude) values(?name, ?nip, ?city, ?street, ?homeNumber, ?longitude, ?latitude)";
            } else if (typeOfPoint == "Shops"){
                sql = "Insert into Shops (Name, NIP, City, Street, HomeNumber, Longitude, Latitude) values(?name, ?nip, ?city, ?street, ?homeNumber, ?longitude, ?latitude)";
            }
            
            var conn = Connect();
            conn.Open();
            var cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = mapPointer.pointName;
            cmd.Parameters.Add("?nip", MySqlDbType.VarChar).Value = mapPointer.nip;
            cmd.Parameters.Add("?city", MySqlDbType.VarChar).Value = mapPointer.pointCity;
            cmd.Parameters.Add("?street", MySqlDbType.VarChar).Value = mapPointer.pointAddress;
            cmd.Parameters.Add("?homeNumber", MySqlDbType.VarChar).Value = mapPointer.pointAddressBlockNumber;
            cmd.Parameters.Add("?longitude", MySqlDbType.Float).Value = mapPointer.pointLongitude;
            cmd.Parameters.Add("?latitude", MySqlDbType.Float).Value = mapPointer.pointLatitude;
            
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("MapPointer " + mapPointer.pointName + " created successful!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
        }
        
        public NewMapPointer GetPointerByName(String shopName) 
        {
            var conn = Connect();
            conn.Open();
            var sql = "Select * from Shops where Name = @Name";
            
            NewMapPointer newPoint = new NewMapPointer();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader(); 
                
                if (resultSet.NextResult()) {
                    newPoint.pointId = resultSet.GetInt32("ShopId");
                    newPoint.pointName = resultSet.GetString("Name");
                    newPoint.pointLongitude = resultSet.GetDouble("Longitude");
                    newPoint.pointLatitude = resultSet.GetDouble("Latitude");
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
            var sql = "select ShopId, Name, NIP, City, Street, HomeNumber, Longitude, Latitude from Shops";
            
            ArrayList listOfPointers = new ArrayList();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();

                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    NewMapPointer newPoint = new NewMapPointer();
                    newPoint.pointId = resultSet.GetInt32("ShopId");
                    newPoint.pointName = resultSet.GetString("Name");
                    newPoint.nip = resultSet.GetString("NIP");
                    newPoint.pointCity = resultSet.GetString("City");
                    newPoint.pointAddress = resultSet.GetString("Street");
                    newPoint.pointAddressBlockNumber = resultSet.GetString("HomeNumber");
                    newPoint.pointLongitude = resultSet.GetDouble("Longitude");
                    newPoint.pointLatitude = resultSet.GetDouble("Latitude");
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