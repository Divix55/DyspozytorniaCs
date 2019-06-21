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
        //private readonly DataSource dataSource;
        //private BindingSource dataSource = new BindingSource();

        public SupplyTicketRepository()
        {
            //this.dataSource = dataSource;
        }

        public ArrayList createTicketTable() {
            
            var conn = Connect();
            conn.Open();
            var sql = "select * from Supply";
            
            DbLoggerCategory.Database.Connection connection = null;
            ArrayList listOfTickets = new ArrayList();

            try{
                //connection = dataSource.getConnection();
                //PreparedStatement preparedStatement = connection.prepareStatement(sql);
                
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                //ResultSet resultSet = preparedStatement.executeQuery();
                var resultSet = cmd.ExecuteReader();   
                while (resultSet.Read()) {
                    SupplyTicket newPoint = new SupplyTicket();
                    newPoint.setTicketId(resultSet.GetInt32("SupplyId"));
                    newPoint.setShopId(resultSet.GetInt32("ShopId"));
                    newPoint.setStoreId(resultSet.GetInt32("StoreId"));
                    newPoint.setDriverId(resultSet.GetInt32("DriverId"));
                    newPoint.setDuration(resultSet.GetInt32("DurationTime"));
                    newPoint.setDeliveryDate(resultSet.GetString("DeliveryDate"));
                    newPoint.setTicketStatus(resultSet.GetString("Status"));
                    newPoint.setCompleted(resultSet.GetBoolean("isCompleted"));
                    newPoint.setPath(resultSet.GetInt32("Path"));
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

        //public void createTicketNaive(SupplyTicket ticket)
        //{
        //    throw new NotImplementedException();
        //}

        public void createTicketEntry(SupplyTicket ticket){
            String sql = "Insert into Supply (ShopId, ShopName, DeliveryDate, Status, isCompleted, Path)"
                    + "values(?ShopId, ?ShopName, ?DeliveryDate, ?Status, ?isCompleted, ?Path)";

            String date = ticket.getShopYear() + "-" + ticket.getShopMonth() + "-" + ticket.getShopDay();
            String hour = ticket.getShopHour() + ":" + ticket.getShopMinute();
            int shopId = convertNameToId(ticket.getShopName());

            DbLoggerCategory.Database.Connection connection = null;

            var conn = Connect();
            conn.Open();
            try
            {

                bool completed = false;

                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?ShopId", MySqlDbType.Int32).Value = shopId;
                cmd.Parameters.Add("?ShopName", MySqlDbType.String).Value = ticket.getShopName();
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

        public void createTicketNaive(SupplyTicket ticket) {
            String sql = "update Supply set StoreId = ?StoreId, DriverId = ?DriverId, DeliveryDate = ?DeliveryDate, DurationTime = ?DurationTime, Status = ?Status, Path = ?Path where SupplyId = ?SupplyId";

            int storeId = ticket.getStoreId();
            int driverId = ticket.getDriverId();

            DbLoggerCategory.Database.Connection connection = null;

            var conn = Connect();
            conn.Open();
            try {

                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.Int32).Value = ticket.getDeliveryDate();
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = ticket.getTicketStatus();
                cmd.Parameters.Add("?Status", MySqlDbType.Int32).Value = ticket.getTicketStatus();
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = ticket.getPath();
                cmd.Parameters.Add("?SupplyId", MySqlDbType.Int32).Value = ticket.getTicketId();

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

        //public void createTicketEntry(SupplyTicket ticket)
        //{
        //    throw new NotImplementedException();
        //}

        public String getShopsName(int shopsId) {
            String sql = "Select * from Shops where ShopId = ?ShopId";
            String shopName = "";

            var conn = Connect();
            conn.Open();
            DbLoggerCategory.Database.Connection connection = null;
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

        public float getShopsLon(int shopsId) {
            String sql = "Select * from Shops where ShopId = ?";
            return executeLonSelect(shopsId, sql);
        }

        public float getShopsLat(int shopsId) {
            String sql = "Select * from Shops where ShopId = ?";
            return executeLatSelect(shopsId, sql);
        }

        public float getStoreLon(int storeId){
            String sql = "Select * from Stores where StoreId = ?";
            return executeLonSelect(storeId, sql);
        }

        public float getStoreLat(int storeId){
            String sql = "Select * from Stores where StoreId = ?";
            return executeLatSelect(storeId, sql);
        }

        private float executeLonSelect(int shopsId, String sql) {
            DbLoggerCategory.Database.Connection connection = null;
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

        private float executeLatSelect(int storeId, String sql) {
            DbLoggerCategory.Database.Connection connection = null;
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

        public int[] getDriversByStoreId(int storeId) {
            String sql = "Select * from Drivers where StoreId = ?StoreId";
            int []drivers = new int[15];
            int driverCounter = 0;
            var conn = Connect();
            conn.Open();

            DbLoggerCategory.Database.Connection connection = null;
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

        public ArrayList getTicketsByDrivers(int[] drivers) {
            ArrayList tickets = new ArrayList();
            foreach (int driverId in drivers)
            {
                String sql = "Select * from Supply where DriverId = ?DriverId and IsCompleted = FALSE ";
                var conn = Connect();
                conn.Open();
                DbLoggerCategory.Database.Connection connection = null;

                try
                {
                    var cmd = new MySqlCommand(sql, conn);
                    cmd.Prepare();
                    cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;

                    var resultSet = cmd.ExecuteReader();
                    if (resultSet.Read())
                    {
                        SupplyTicket ticket = new SupplyTicket();
                        ticket.setStoreId(resultSet.GetInt32("StoreId"));
                        ticket.setShopId(resultSet.GetInt32("ShopId"));
                        ticket.setDriverId(driverId);
                        ticket.setDeliveryDate(resultSet.GetString("DeliveryDate"));
                        ticket.setPath(resultSet.GetInt32("Path"));
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

        //public void createTicketNew(SupplyTicket ticket)
        //{
        //    throw new NotImplementedException();
        //}

        public void createTicketNew(SupplyTicket ticket) {
            String sql = "update Supply set StoreId = ?StoreId, DriverId = ?DriverId, DeliveryDate = ?DeliveryDate, DurationTime = ?DurationTime, Status = ?Status, Path = ?Path where SupplyId = ?SupplyId";

            int storeId = ticket.getStoreId();
            int driverId = ticket.getDriverId();
            var conn = Connect();
            conn.Open();
            DbLoggerCategory.Database.Connection connection = null;

            try {
                var cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.Add("?StoreId", MySqlDbType.Int32).Value = storeId;
                cmd.Parameters.Add("?DriverId", MySqlDbType.Int32).Value = driverId;
                cmd.Parameters.Add("?DeliveryDate", MySqlDbType.Int32).Value = ticket.getDeliveryDate();
                cmd.Parameters.Add("?DurationTime", MySqlDbType.Int32).Value = ticket.getDuration();
                cmd.Parameters.Add("?Status", MySqlDbType.Int32).Value = ticket.getTicketStatus();
                cmd.Parameters.Add("?Path", MySqlDbType.Int32).Value = ticket.getPath();
                cmd.Parameters.Add("?SupplyId", MySqlDbType.Int32).Value = ticket.getTicketId();

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

        private int checkSize(String tableName) {
            String sql = "Select * from " + tableName;
            int count = 0;
            var conn = Connect();
            conn.Open();
            DbLoggerCategory.Database.Connection connection = null;
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

        private int convertNameToId(String name) {
            String sql = "Select * from Shops where Name = ?Name";
            int id = -1;

            var conn = Connect();
            conn.Open();
            DbLoggerCategory.Database.Connection connection = null;

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