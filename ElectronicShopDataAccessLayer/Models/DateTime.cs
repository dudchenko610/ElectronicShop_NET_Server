using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicShopDataAccessLayer.Models
{
    public class DateTime
    {

        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public DateTime()
        {
            Year = System.DateTime.Now.Year;
            Month = System.DateTime.Now.Month;
            Day = System.DateTime.Now.Day;
            Hour = System.DateTime.Now.Hour;
            Minute = System.DateTime.Now.Minute;
            Second = System.DateTime.Now.Second;


        }

        public DateTime(System.DateTime dateTime)
        {
            Year = dateTime.Year;
            Month = dateTime.Month;
            Day = dateTime.Day;
            Hour = dateTime.Hour;
            Minute = dateTime.Minute;
            Second = dateTime.Second;
        }

        public DateTime(int year, int month, int day, int hour, int minute, int second)
        {
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public System.DateTime GetSystemDateTime()
        {
            return new System.DateTime(Year, Month, Day, Hour, Minute, Second);
        }
    }
}
