using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Dyspozytornia.Models;
using MySql.Data.MySqlClient;

namespace Dyspozytornia.Data
{
    public class SupplyTicketRepository: ISupplyTicketRepository
    {
        public ArrayList CreateTicketTable() {
            
            var conn = Connect();
            conn.Open();
            const string sql = "select * from Supply";
            
            var listOfTickets = new ArrayList();

            try{
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    SupplyTicket newPoint = new SupplyTicket();
                    newPoint.TicketId = resultSet.GetInt32("SupplyId");
                    newPoint.ShopId = resultSet.GetInt32("ShopId");
                    newPoint.StoreId = resultSet.GetInt32("StoreId");
                    newPoint.DriverId = resultSet.GetInt32("DriverId");
                    newPoint.Duration = resultSet.GetInt32("DurationTime");
                    newPoint.DeliveryDate = resultSet.GetString("DeliveryDate");
                    newPoint.TicketStatus = resultSet.GetString("Status");
                    newPoint.IsCompleted = resultSet.GetBoolean("isCompleted");
                    newPoint.Path = resultSet.GetInt32("Path");
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
            const string sql = "Insert into Supply (StoreId, ShopId, ShopName, DriverId, DeliveryDate, DurationTime, Status, isCompleted, Path) values(?StoreId, ?ShopId, ?ShopName, ?DriverId, ?DeliveryDate, ?DurationTime, ?Status, ?isCompleted, ?Path)";

            var date = ticket.ShopYear + "-" + ticket.ShopMonth + "-" + ticket.ShopDay;
            var hour = ticket.ShopHour + ":" + ticket.ShopMinute;
            var shopId = ConvertNameToId(ticket.ShopName);
            
            var conn = Connect();
            conn.Open();
            try
            {
                const bool completed = false;
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = 1;
                cmd.Parameters.Add("?ShopId", MySqlDbType.Int32).Value = shopId;
                cmd.Parameters.Add("?ShopName", MySqlDbType.String).Value = ticket.ShopName;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = 1;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.String).Value = date + " " + hour;
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = 1;
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
            const string sql = "update Supply set StoreId = ?StoreId, DriverId = ?DriverId, DeliveryDate = ?DeliveryDate, DurationTime = ?DurationTime, Status = ?Status, Path = ?Path where SupplyId = ?SupplyId";

            var storeId = ticket.StoreId;
            var driverId = ticket.DriverId;

            var conn = Connect();
            conn.Open();
            try {

                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.Int32).Value = ticket.DeliveryDate;
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = ticket.Duration;
                cmd.Parameters.Add("?Status", MySqlDbType.Int32).Value = ticket.TicketStatus;
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = ticket.Path;
                cmd.Parameters.Add("?SupplyId", MySqlDbType.Int32).Value = ticket.TicketId;

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
            const string sql = "Select * from Shops where ShopId = @ShopId";
            var shopName = "";

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

        public float GetShopsLon(int shopsId)
        {
            const string sql = "Select * from Shops where ShopId = ?";
            return ExecuteLonSelect(shopsId, sql);
        }

        public float GetShopsLat(int shopsId)
        {
            const string sql = "Select * from Shops where ShopId = ?";
            return ExecuteLatSelect(shopsId, sql);
        }

        public float GetStoreLon(int storeId)
        {
            const string sql = "Select * from Stores where StoreId = ?";
            return ExecuteLonSelect(storeId, sql);
        }

        public float GetStoreLat(int storeId)
        {
            const string sql = "Select * from Stores where StoreId = ?";
            return ExecuteLatSelect(storeId, sql);
        }

        private float ExecuteLonSelect(int shopsId, string sql) {
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

        private float ExecuteLatSelect(int storeId, string sql) {
            float lat=0;
            var conn = Connect();
            conn.Open();
            try {
                
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?", MySqlDbType.Int32).Value = storeId;

                var resultSet = cmd.ExecuteReader();
                if (resultSet.Read()) {
                    lat = resultSet.GetFloat("Latitude");
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
            const string sql = "Select * from Drivers where StoreId = ?StoreId";
            var drivers = new int[15];
            var driverCounter = 0;
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

            var d = new int[driverCounter];
            for (var i = 0; i<driverCounter; i++){
                d[i] = drivers[i];
            }

            return d;
        }

        public ArrayList GetTicketsByDrivers(IEnumerable<int> drivers) {
            var tickets = new ArrayList();
            foreach (var driverId in drivers)
            {
                const string sql = "Select * from Supply where DriverId = ?DriverId and IsCompleted = FALSE ";
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
                        var ticket = new SupplyTicket
                        {
                            StoreId = resultSet.GetInt32("StoreId"),
                            ShopId = resultSet.GetInt32("ShopId"),
                            DriverId = driverId,
                            DeliveryDate = resultSet.GetString("DeliveryDate"),
                            Path = resultSet.GetInt32("Path")
                        };
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
            const string sql = "update Supply set StoreId = ?StoreId, DriverId = ?DriverId, DeliveryDate = ?DeliveryDate, DurationTime = ?DurationTime, Status = ?Status, Path = ?Path where SupplyId = ?SupplyId";

            var storeId = ticket.StoreId;
            var driverId = ticket.DriverId;
            var conn = Connect();
            conn.Open();

            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.Int32).Value = ticket.DeliveryDate;
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = ticket.Duration;
                cmd.Parameters.Add("?Status", MySqlDbType.Int32).Value = ticket.TicketStatus;
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = ticket.Path;
                cmd.Parameters.Add("?SupplyId", MySqlDbType.Int32).Value = ticket.TicketId;

                cmd.ExecuteReader();
                
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

        public ArrayList GetAllPendingTickets()
        {
            var tickets = new ArrayList();
            
            const string sql = "Select * from Supply where Status = ?status and IsCompleted = FALSE ";
            var conn = Connect();
            conn.Open();

            try
            {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?status", MySqlDbType.VarChar).Value = "oczekujace";

                var resultSet = cmd.ExecuteReader();

                while (resultSet.Read())
                {
                    var ticket = new SupplyTicket
                    {
                        TicketId = resultSet.GetInt32("SupplyId"),
                        StoreId = resultSet.GetInt32("StoreId"),
                        ShopId = resultSet.GetInt32("ShopId"),
                        ShopName = resultSet.GetString("ShopName"),
                        DriverId = resultSet.GetInt32("DriverId"),
                        DeliveryDate = resultSet.GetString("DeliveryDate"),
                        IsCompleted = resultSet.GetBoolean("isCompleted"),
                        TicketStatus = resultSet.GetString("Status")
                    };
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

            return tickets;
        }

        private int CheckSize(string tableName) {
            var sql = "Select * from " + tableName;
            var count = 0;
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
            const string sql = "Select * from Shops where Name = ?Name";
            var id = -1;

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