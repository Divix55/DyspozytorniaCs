using System;
using System.Collections;
using System.Reflection;
using Dyspozytornia.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Dyspozytornia.Data
{
    public class SupplyTicketRepository: ISupplyTicketRepository
    {
        public ArrayList CreateTicketTable() {
            
            var conn = Connect();
            conn.Open();
            var sql = "select * from Supply";
            
            ArrayList listOfTickets = new ArrayList();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    SupplyTicket newPoint = new SupplyTicket();
                    newPoint.ticketId = resultSet.GetInt32("SupplyId");
                    newPoint.shopId = resultSet.GetInt32("ShopId");
                    newPoint.storeId = resultSet.GetInt32("StoreId");
                    newPoint.driverId = resultSet.GetInt32("DriverId");
                    newPoint.duration = resultSet.GetInt32("DurationTime");
                    newPoint.deliveryDate = resultSet.GetString("DeliveryDate");
                    newPoint.ticketStatus = resultSet.GetString("Status");
                    newPoint.isCompleted = resultSet.GetBoolean("isCompleted");
                    newPoint.path = resultSet.GetInt32("Path");
                    listOfTickets.Add(newPoint);
                }
                conn.Close();

            } catch (Exception e) {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            return listOfTickets;
        }


        public void CreateTicketEntry(SupplyTicket ticket){
            String sql = "Insert into Supply (ShopId, ShopName, DeliveryDate, Status, isCompleted, Path)"
                    + "values(?ShopId, ?ShopName, ?DeliveryDate, ?Status, ?isCompleted, ?Path)";

            String date = ticket.shopYear + "-" + ticket.shopMonth + "-" + ticket.shopDay;
            String hour = ticket.shopHour + ":" + ticket.shopMinute;
            int shopId = ConvertNameToId(ticket.shopName);
            
            var conn = Connect();
            conn.Open();
            try
            {
                bool completed = false;
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?ShopId", MySqlDbType.Int32).Value = shopId;
                cmd.Parameters.Add("?ShopName", MySqlDbType.String).Value = ticket.shopName;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.String).Value = date + " " + hour;
                cmd.Parameters.Add("?Status", MySqlDbType.String).Value = "oczekujace";
                cmd.Parameters.Add("?isCompleted", MySqlDbType.Bool).Value = completed;
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = -1;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
        }

        public void CreateTicketNaive(SupplyTicket ticket) {
            String sql = "update Supply set StoreId = ?StoreId, DriverId = ?DriverId, DeliveryDate = ?DeliveryDate, DurationTime = ?DurationTime, Status = ?Status, Path = ?Path where SupplyId = ?SupplyId";

            int storeId = ticket.storeId;
            int driverId = ticket.driverId;

            var conn = Connect();
            conn.Open();
            try {

                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.Int32).Value = ticket.deliveryDate;
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = ticket.duration;
                cmd.Parameters.Add("?Status", MySqlDbType.Int32).Value = ticket.ticketStatus;
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = ticket.path;
                cmd.Parameters.Add("?SupplyId", MySqlDbType.Int32).Value = ticket.ticketId;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
        }

        public String GetShopsName(int shopsId) {
            String sql = "Select * from Shops where ShopId = @ShopId";
            String shopName = "";

            var conn = Connect();
            conn.Open();
            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?ShopId", MySqlDbType.Int32).Value = shopsId;
                var resultSet = cmd.ExecuteReader();   
                
                if (resultSet.Read()) {
                    shopName = resultSet.GetString("Name");
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            return shopName;
        }

        public float GetShopsLon(int shopsId) {
            String sql = "Select * from Shops where ShopId = ?";
            return ExecuteLonSelect(shopsId, sql);
        }

        public float GetShopsLat(int shopsId) {
            String sql = "Select * from Shops where ShopId = ?";
            return ExecuteLatSelect(shopsId, sql);
        }

        public float GetStoreLon(int storeId){
            String sql = "Select * from Stores where StoreId = ?";
            return ExecuteLonSelect(storeId, sql);
        }

        public float GetStoreLat(int storeId){
            String sql = "Select * from Stores where StoreId = ?";
            return ExecuteLatSelect(storeId, sql);
        }

        private float ExecuteLonSelect(int shopsId, String sql) {
            float lon=0;
            var conn = Connect();
            conn.Open();
            try {
                
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?", MySqlDbType.Int32).Value = shopsId;
                
                var resultSet = cmd.ExecuteReader();
                if (resultSet.Read()) {
                    lon = resultSet.GetFloat("Longitude");
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }

            return lon;
        }

        private float ExecuteLatSelect(int storeId, String sql) {
            float lat=0;
            float lon=0;
            var conn = Connect();
            conn.Open();
            try {
                
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?", MySqlDbType.Int32).Value = storeId;

                var resultSet = cmd.ExecuteReader();
                if (resultSet.Read()) {
                    lon = resultSet.GetFloat("Latitude");
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            
            return lat;
        }

        public int[] GetDriversByStoreId(int storeId) {
            String sql = "Select * from Drivers where StoreId = ?StoreId";
            int []drivers = new int[15];
            int driverCounter = 0;
            var conn = Connect();
            conn.Open();
            
            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                
                var resultSet = cmd.ExecuteReader();
                if (resultSet.Read()) {
                    drivers[driverCounter] = resultSet.GetInt32("DriverId");
                    driverCounter += 1;
                }
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }

            int[] d = new int[driverCounter];
            for (int i = 0; i<driverCounter; i++){
                d[i] = drivers[i];
            }

            return d;
        }

        public ArrayList GetTicketsByDrivers(int[] drivers) {
            ArrayList tickets = new ArrayList();
            foreach (int driverId in drivers)
            {
                String sql = "Select * from Supply where DriverId = ?DriverId and IsCompleted = FALSE ";
                var conn = Connect();
                conn.Open();

                try
                {
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;

                    var resultSet = cmd.ExecuteReader();
                    if (resultSet.Read())
                    {
                        SupplyTicket ticket = new SupplyTicket();
                        ticket.storeId = resultSet.GetInt32("StoreId");
                        ticket.shopId = resultSet.GetInt32("ShopId");
                        ticket.driverId = driverId;
                        ticket.deliveryDate = resultSet.GetString("DeliveryDate");
                        ticket.path = resultSet.GetInt32("Path");
                        tickets.Add(ticket);
                    }

                    conn.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error when executing method " +
                                      MethodBase.GetCurrentMethod().Name +
                                      ". Possible Cause: " + e.Message);
                    conn.Close();
                }
            }

            return tickets;
        }

        public void CreateTicketNew(SupplyTicket ticket) {
            String sql = "update Supply set StoreId = ?StoreId, DriverId = ?DriverId, DeliveryDate = ?DeliveryDate, DurationTime = ?DurationTime, Status = ?Status, Path = ?Path where SupplyId = ?SupplyId";

            int storeId = ticket.storeId;
            int driverId = ticket.driverId;
            var conn = Connect();
            conn.Open();

            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.Int32).Value = ticket.deliveryDate;
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = ticket.duration;
                cmd.Parameters.Add("?Status", MySqlDbType.Int32).Value = ticket.ticketStatus;
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = ticket.path;
                cmd.Parameters.Add("?SupplyId", MySqlDbType.Int32).Value = ticket.ticketId;

                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
        }

        private int CheckSize(String tableName) {
            String sql = "Select * from " + tableName;
            int count = 0;
            var conn = Connect();
            conn.Open();
            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader();
                if (resultSet.Read()){
                    count++;
                }
            
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            return count;
        }

        private int ConvertNameToId(String name) {
            String sql = "Select * from Shops where Name = ?Name";
            int id = -1;

            var conn = Connect();
            conn.Open();

            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?Name", MySqlDbType.Int32).Value = name;
                var resultSet = cmd.ExecuteReader();
                if (resultSet.Read()){
                    id = resultSet.GetInt32("ShopId");
                }
            
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when executing method " +
                                  MethodBase.GetCurrentMethod().Name +
                                  ". Possible Cause: " + e.Message);
                conn.Close();
            }
            return id;
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