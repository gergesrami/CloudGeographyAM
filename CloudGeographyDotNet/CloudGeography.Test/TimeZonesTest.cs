﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudGeography.Test
{
    [TestClass]
    public class TimeZonesTest
    {
        [TestMethod]
        public void Get_All_TimeZones()
        {
            CloudGeographyClient client = new();
            List<TimeZoneInfo> timeZones = client.TimeZones.Get();

            Assert.IsTrue(timeZones.Any());
        }

        [TestMethod]
        public void Get_List_Of_Specific_TimeZones()
        {
            CloudGeographyClient client = new();
            List<TimeZoneInfo> timeZones = client.TimeZones.Get(new[]{ "Hawaiian Standard Time", "Middle East Standard Time", "Greenland Standard Time" });

            List<string> input =new() { "Hawaiian Standard Time", "Middle East Standard Time","Greenland Standard Time" };
            List<TimeZoneInfo> expectedList = TimeZoneInfo.GetSystemTimeZones().ToList().Where(TZ => input.Any(key => key == TZ.Id)).ToList();

            for (int i = 0; i < timeZones.Count; i++)
                Assert.IsTrue(expectedList[i].Id == timeZones[i].Id);
        }

        [TestMethod]
        public void Get_Current_Time_Of_TimeZoneID()
        {
            CloudGeographyClient client = new();
            DateTime dateTime = client.TimeZones.GetDateTime("Eastern Standard Time");

            DateTime expextedValue = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

            Assert.IsTrue(expextedValue >= dateTime);
            Assert.IsTrue(expextedValue <= dateTime.AddMinutes(0.1));
        }

        [TestMethod]
        public void Convert_UTC_Time_To_TimeZone_By_TimeZone_Id()
        {
            CloudGeographyClient client = new();
            DateTime convertedTime = client.TimeZones.GetDateTime(DateTime.Parse("2022-11-09 10:00:00 AM"), "Middle East Standard Time");

            DateTime expectedTime = DateTime.Parse("2022-11-09 12:00:00 PM");

            Assert.IsTrue(expectedTime >= convertedTime);
            Assert.IsTrue(expectedTime <= convertedTime.AddMinutes(0.1));
        }

        [TestMethod]
        public void Convert_UTC_Time_From_TimeZone_To_TimeZone()
        {
            CloudGeographyClient client = new();
            DateTime dateTime = client.TimeZones.GetDateTime(DateTime.Parse("2022-11-08 12:00:00 PM"), "Middle East Standard Time", "Afghanistan Standard Time");

            DateTime expextedValue = TimeZoneInfo.ConvertTime(DateTime.Parse("2022-11-08 12:00:00 PM"), TimeZoneInfo.FindSystemTimeZoneById("Middle East Standard Time"), TimeZoneInfo.FindSystemTimeZoneById("Afghanistan Standard Time"));

            Assert.AreEqual(expextedValue, dateTime);
        }
    }
}
