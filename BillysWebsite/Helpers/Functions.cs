using BillysWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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
                            "INSERT INTO [dbo].[Appointment] " +
                            "([Title] " +
                            ",[Description] " +
                            ",[StartDate] " +
                            ",[EndDate]) " +
                            "VALUES " +
                            "('" + name + "' " +
                            ",'" + description + "' " +
                            ",'" + startDate + "' " +
                            ",'" + endDate  + "')";
            int success = dbHelper.ExecuteQueries(query);
            dbHelper.CloseConnection();
            return success;
        }

        public static List<Appointment> GetAppointments(int appointmentPK = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            List<Appointment> appointments = null;
            string whereString = null;
            string query = useString +
                           "SELECT[AppointmentPK] " +
                           ",[Title] " +
                           ",[Description] " +
                           ",[StartDate] " +
                           ",[EndDate] " +
                           "FROM[dbo].[Appointment] ";
            if(appointmentPK != 0)
            {
                if (whereString == null)
                {
                    whereString = "WHERE ";
                }
                else
                {
                    whereString += "AND ";
                }
                whereString += "AppointmentPK = " + appointmentPK + " ";
            }
            if(startDate != null)
            {
                if (whereString == null)
                {
                    whereString = "WHERE ";
                }
                else
                {
                    whereString += "AND ";
                }
                whereString += "startDate = '" + startDate + "' ";
            }
            if (endDate != null)
            {
                if (whereString == null)
                {
                    whereString = "WHERE ";
                }
                else
                {
                    whereString += "AND ";
                }
                whereString += "endDate = '" + endDate + "' ";
            }
            if (whereString != null)
                query += whereString;
            query += " ORDER BY startDate";
            using (SqlDataReader dbReader = dbHelper.DataReader(query))
            {
                while (dbReader.Read())
                {
                    if (dbReader.HasRows)
                    {
                        if (appointments == null)
                            appointments = new List<Appointment>();
                        Appointment tempAppointment = new Appointment();
                        tempAppointment.AppointmentPK = dbReader.GetInt32(0);
                        tempAppointment.Title = dbReader.GetString(1);
                        tempAppointment.Description = dbReader.GetString(2);
                        tempAppointment.StartDate = dbReader.GetDateTime(3);
                        tempAppointment.EndDate = dbReader.GetDateTime(4);
                        appointments.Add(tempAppointment);
                    }
                }
            }
            dbHelper.CloseConnection();
            return appointments;
        }

        internal static bool DeleteOldAppointments()
        {
            throw new NotImplementedException();
        }
    }
}
