using System;
using Dyspozytornia.Models;
using Microsoft.Extensions.Logging;

namespace Dyspozytornia.Services
{
    public static class UtilsService
    {
        private const double SpeedFactor = 1.2;

        public static double CalculateDuration(double distance) {
            return distance*SpeedFactor * 1000d / 1000d;
        }

        public static string TryNextHour(String date, String h) {
            var hour = h.Split(":");
            var hourInt = Int32.Parse(hour[0]);
            var minInt = Int32.Parse(hour[1]);
            if (hourInt >= 8 && hourInt < 16){
                if (minInt >= 0 && minInt < 60){
                    if(hourInt == 15 && minInt == 59){
                        h = AddDigitToTime(hourInt, minInt, 'H');
                    }
                    h = AddDigitToTime(hourInt, minInt, 'M');
                }
                else{
                    h = AddDigitToTime(hourInt, minInt, 'H');
                }
            }
            else{
                hourInt = 8;
                minInt = 0;
                date = SetDateToNextDay(date);
                h = hourInt + ":" + minInt;
            }

            return date + " " + h;
        }

        public static double CalculateDistanceInStraightLine(NewMapPointer point1, NewMapPointer point2) {
            if (point1.Equals(point2)) {
                return 0;
            }

            var lon1 = point1.PointLongitude;
            var lon2 = point2.PointLongitude;
            var lat1 = point1.PointLatitude;
            var lat2 = point2.PointLatitude;
            var theta = lon1 - lon2;

            var dist = Math.Sin(ToRadians(lat1)) * Math.Sin(ToRadians(lat2)) + Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) * Math.Cos(ToRadians(theta));
            dist = Math.Acos(dist);
            dist = ToDegrees(dist);
            dist = dist * 60 * 1.1515;
            return (dist);
        }

        private static string AddDigitToTime(int timeHour, int timeMin, char type){
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

        private static string SetDateToNextDay(string d){
            var date = d.Split("-");
            var yearInt = int.Parse(date[0]);
            var monthInt = int.Parse(date[1]);
            var dayInt = int.Parse(date[2]);

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

        private static double ToRadians(double x)
        {
            return Math.PI * x / 180.0;
        }

        private static double ToDegrees(double x)
        {
            return x * (180.0 / Math.PI);
        }
    }
}