using BillysWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BillysWebsite.Helpers
{
    public static class Functions
    {
        private static string useString = "USE [BillysWebsiteDB] ";
        public static int AddAppointent(string description, DateTime startDate, DateTime endDate,
                                        string firstName, string lastName, DateTime dateOfBirth,
                                        string phoneNumber, string email, string fileName, string fileDescription)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            string query = useString +
                            "INSERT INTO[dbo].[Appointment] " +
                            "([Description] " +
                            ",[StartDate] " +
                            ",[EndDate] " +
                            ",[FirstName] " +
                            ",[LastName] " +
                            ",[DateOfBirth] " +
                            ",[PhoneNumber] " +
                            ",[Email] " +
                            ",[FileName] " +
                            ",[FileDescription]) " +
                            "VALUES " +
                            "('" + description + "', " +
                            "'" + startDate + "', " +
                            "'" + endDate + "', " +
                            "'" + firstName + "', " +
                            "'" + lastName + "', " +
                            "'" + dateOfBirth + "', " +
                            "'" + phoneNumber + "', " +
                            "'" + email + "', " +
                            "'" + fileName + "', " +
                            "'" + fileDescription + "')";
            int success = dbHelper.ExecuteQueries(query);
            dbHelper.CloseConnection();
            return success;
        }

        public static List<AppointmentType> GetAppointmentTypes()
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            List<AppointmentType> appointmentTypes = null;
            string query = "SELECT [Id] " +
                           ",[Name] " +
                           ",[StartTime] " +
                           ",[DaysOfWeek] " +
                           ",[Duration] " +
                           "FROM[dbo].[AppointmentType]";
            using (SqlDataReader dbReader = dbHelper.DataReader(query))
            {
                while (dbReader.Read())
                {
                    if (dbReader.HasRows)
                    {
                        int i = 0;
                        if (appointmentTypes == null)
                            appointmentTypes = new List<AppointmentType>();
                        AppointmentType tempAppointmentType = new AppointmentType();
                        tempAppointmentType.Id = dbReader.GetInt32(i++);
                        tempAppointmentType.Name = dbReader.GetString(i++);
                        tempAppointmentType.StartTime = dbReader.GetTimeSpan(i++);
                        tempAppointmentType.DaysOfWeek = dbReader.GetByte(i++);
                        tempAppointmentType.Duration = dbReader.GetDecimal(i++);
                        appointmentTypes.Add(tempAppointmentType);
                    }
                }
            }
            dbHelper.CloseConnection();
            return appointmentTypes;
        }

        public static List<Appointment> GetAppointments(int appointmentPK = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            List<Appointment> appointments = null;
            string whereString = null;
            string query = useString +
                            "SELECT[AppointmentPK] " +
                            ",[Description] " +
                            ",[StartDate] " +
                            ",[EndDate] " +
                            ",[FirstName] " +
                            ",[LastName] " +
                            ",[DateOfBirth] " +
                            ",[PhoneNumber] " +
                            ",[Email] " +
                            ",[FileName]" +
                            ",[FileDescription] " +
                            "FROM [dbo].[Appointment] ";
            if (appointmentPK != 0)
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
                        int i = 0;
                        if (appointments == null)
                            appointments = new List<Appointment>();
                        Appointment tempAppointment = new Appointment();
                        tempAppointment.AppointmentPK = dbReader.GetInt32(i++);
                        tempAppointment.Description = dbReader.GetString(i++);
                        tempAppointment.StartDate = dbReader.GetDateTime(i++);
                        tempAppointment.EndDate = dbReader.GetDateTime(i++);
                        tempAppointment.FirstName = dbReader.GetString(i++);
                        tempAppointment.LastName = dbReader.GetString(i++);
                        tempAppointment.DateOfBirth = dbReader.GetDateTime(i++);
                        tempAppointment.PhoneNumber = dbReader.GetString(i++);
                        tempAppointment.Email = dbReader.GetString(i++);
                        tempAppointment.FileName = dbReader.GetString(i++);
                        tempAppointment.FileDescription = dbReader.GetString(i++);
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
