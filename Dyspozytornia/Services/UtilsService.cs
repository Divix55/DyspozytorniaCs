using System;
using Dyspozytornia.Models;
using Microsoft.Extensions.Logging;

namespace Dyspozytornia.Services
{
    public class UtilsService
    {
        private static readonly double SPEED_FACTOR = 1.2;

        public static double calculateDuration(double distance) {
            return distance*SPEED_FACTOR * 1000d / 1000d;
        }

        public static String tryNextHour(String date, String h) {
            String[] hour = h.Split(":");
            int hourInt = Int32.Parse(hour[0]);
            int minInt = Int32.Parse(hour[1]);
            if (hourInt >= 8 && hourInt < 16){
                if (minInt >= 0 && minInt < 60){
                    if(hourInt == 15 && minInt == 59){
                        h = addDigitToTime(hourInt, minInt, 'H');
                    }
                    h = addDigitToTime(hourInt, minInt, 'M');
                }
                else{
                    h = addDigitToTime(hourInt, minInt, 'H');
                }
            }
            else{
                hourInt = 8;
                minInt = 0;
                date = setDateToNextDay(date);
                h = hourInt + ":" + minInt;
            }

            return date + " " + h;
        }

        public static double calculateDistanceInStraightLine(NewMapPointer point1, NewMapPointer point2) {
            if (point1.equals(point2)) {
                return 0;
            }
            else {
                double lon1 = point1.getPointLongitude();
                double lon2 = point2.getPointLongitude();
                double lat1 = point1.getPointLatitude();
                double lat2 = point2.getPointLatitude();
                double theta = lon1 - lon2;

                double dist = Math.Sin(toRadians(lat1)) * Math.Sin(toRadians(lat2)) + Math.Cos(toRadians(lat1)) * Math.Cos(toRadians(lat2)) * Math.Cos(toRadians(theta));
                dist = Math.Acos(dist);
                dist = toDegrees(dist);
                dist = dist * 60 * 1.1515;
                return (dist);
            }
        }

        public static String addDigitToTime(int timeHour, int timeMin, char type){
            //minutes
            if (type == 'M'){
                if (timeMin == 59){
                    timeHour += 1;
                    timeMin = 0;
                }
                else{
                    timeMin += 1;
                }
            }
            //hours
            else if(type == 'H'){
                timeHour += 1;
                timeMin = 0;
            }

            return timeHour + ":" + timeMin;
        }

        public static String setDateToNextDay(String d){
            String[] date = d.Split("-");
            int yearInt = Int32.Parse(date[0]);
            int monthInt = Int32.Parse(date[1]);
            int dayInt = Int32.Parse(date[2]);

            if (dayInt < 31){
                dayInt += 1;
            }
            else if(monthInt < 12){
                dayInt = 1;
                monthInt += 1;
            }
            else{
                dayInt = 1;
                monthInt = 1;
                yearInt += 1;
            }
            return yearInt + "-" + monthInt + "-" + dayInt;
        }
        public static double toRadians(double x)
        {
            return Math.PI * x / 180.0;
        }
        public static double toDegrees(double x)
        {
            return x * (180.0 / Math.PI);
        }
    }
}