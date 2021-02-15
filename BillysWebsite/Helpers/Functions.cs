using System;

namespace BillysWebsite.Helpers
{
    public static class Functions
    {
        private static string useString = "USE [BillysWebsiteDB] ";
        public static int AddAppointent(string name, string description, DateTime startDate, DateTime endDate)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            string query = useString +
                            "INSERT INTO[dbo].[Appointment] " +
                            "([Title] " +
                            ",[Description] " +
                            ",[StartDate] " +
                            ",[EndDate]) " +
                            "VALUES " +
                            "(" + name + " " +
                            "," + description + " " +
                            "," + startDate + " " +
                            "," + endDate  + ")";
            return dbHelper.ExecuteQueries(query); ;
        }
    }
}
