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
        public ArrayList createStoreTable() 
        {
            Console.WriteLine("Enter:");
            var conn = Connect();
            conn.Open();
            var sql = "select StoreId, Name, NIP, City, Street, HomeNumber, Longitude, Latitude from Stores";
            
            var listOfPointers = new ArrayList();
            
            
            Console.WriteLine("Lista");
            
            try{
                //connection = dataSource.getConnection();
                //PreparedStatement preparedStatement = connection.prepareStatement(sql);
                
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                //ResultSet resultSet = preparedStatement.executeQuery();
                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    NewMapPointer newPoint = new NewMapPointer();
                    newPoint.setPointId(resultSet.GetInt32("StoreId"));
                    newPoint.setPointName(resultSet.GetString("Name"));
                    newPoint.setNip(resultSet.GetString("NIP"));
                    newPoint.setPointCity(resultSet.GetString("City"));
                    newPoint.setPointAddress(resultSet.GetString("Street"));
                    newPoint.setPointAddressBlockNumber(resultSet.GetString("HomeNumber"));
                    newPoint.setPointLongitude(resultSet.GetDouble("Longitude"));
                    newPoint.setPointLatitude(resultSet.GetDouble("Latitude"));
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
        
        public void createMapPointer(NewMapPointer mapPointer, String typeOfPoint) 
        {
            String sql = "";
            
            if (typeOfPoint == "Stores") {
                sql = "Insert into Stores (Name, NIP, City, Street, HomeNumber, Longitude, Latitude)" + "values(?, ?, ?, ?, ?, ?, ?)";
            } else if (typeOfPoint == "Shops"){
                sql = "Insert into Shops (Name, NIP, City, Street, HomeNumber, Longitude, Latitude)" + "values(?, ?, ?, ?, ?, ?, ?)";
            }
            
            var conn = Connect();
            conn.Open();
            bool isCreated;
            var cmd = new MySqlCommand(sql, conn);
            cmd.Prepare();
            cmd.Parameters.Add("?name", MySqlDbType.String).Value = mapPointer.getPointName();
            cmd.Parameters.Add("?nip", MySqlDbType.String).Value = mapPointer.getNip();
            cmd.Parameters.Add("?city", MySqlDbType.String).Value = mapPointer.getPointCity();
            cmd.Parameters.Add("?street", MySqlDbType.String).Value = mapPointer.getPointAddress();
            cmd.Parameters.Add("?homeNumber", MySqlDbType.String).Value = mapPointer.getPointAddressBlockNumber();
            cmd.Parameters.Add("?longitude", MySqlDbType.Double).Value = mapPointer.getPointLongitude();
            cmd.Parameters.Add("?latitude", MySqlDbType.Double).Value = mapPointer.getPointLatitude();
            
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("MapPointer " + mapPointer.getPointName() + " created successful!");
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
        }
        
        public NewMapPointer getPointerByName(String shopName) 
        {
            var conn = Connect();
            conn.Open();
            String sql = "Select * from Shops where Name=?";
            
            NewMapPointer newPoint = new NewMapPointer();
            DbLoggerCategory.Database.Connection connection = null;
            
            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader(); 
                
                if (resultSet.NextResult()) {
                    newPoint.setPointId(resultSet.GetInt32("ShopId"));
                    newPoint.setPointName(resultSet.GetString("Name"));
                    newPoint.setPointLongitude(resultSet.GetDouble("Longitude"));
                    newPoint.setPointLatitude(resultSet.GetDouble("Latitude"));
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
        
        public ArrayList createShopTable() 
        {
            
            var conn = Connect();
            conn.Open();
            var sql = "select ShopId, Name, NIP, City, Street, HomeNumber, Longitude, Latitude from Shops";
            
            DbLoggerCategory.Database.Connection connection = null;
            ArrayList listOfPointers = new ArrayList();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();

                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    NewMapPointer newPoint = new NewMapPointer();
                    newPoint.setPointId(resultSet.GetInt32("ShopId"));
                    newPoint.setPointName(resultSet.GetString("Name"));
                    newPoint.setNip(resultSet.GetString("NIP"));
                    newPoint.setPointCity(resultSet.GetString("City"));
                    newPoint.setPointAddress(resultSet.GetString("Street"));
                    newPoint.setPointAddressBlockNumber(resultSet.GetString("HomeNumber"));
                    newPoint.setPointLongitude(resultSet.GetDouble("Longitude"));
                    newPoint.setPointLatitude(resultSet.GetDouble("Latitude"));
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