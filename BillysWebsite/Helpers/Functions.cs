using BillysWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static BillysWebsite.Models.AppointmentType;

namespace BillysWebsite.Helpers
{
    public static class Functions
    {
        private static string useString = "USE [BillysWebsiteDB] ";
        public static int AddAppointent(string description, DateTime startDate, DateTime endDate,
                                        string firstName, string lastName, DateTime dateOfBirth,
                                        string phoneNumber, string email, string fileName,
                                        string fileDescription, int typeId)
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
                            ",[FileDescription]" +
                            ",[TypeId]) " +
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
                            "'" + fileDescription + "'," +
                            "" + typeId + ")";
            int success = dbHelper.ExecuteQueries(query);
            dbHelper.CloseConnection();
            return success;
        }

        public static List<AppointmentType> GetAppointmentTypes(int Id = 0)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            List<AppointmentType> appointmentTypes = null;
            string whereString = null;
            string query = "SELECT [Id] " +
                           ",[Name] " +
                           ",[StartTime] " +
                           ",[DaysOfWeek] " +
                           ",[Color] " +
                           ",[Price] " +
                           ",[DurationType] " +
                           ",[Duration] " +
                           "FROM[dbo].[AppointmentType]";
            if (Id != 0)
            {
                if (whereString == null)
                {
                    whereString = "WHERE ";
                }
                else
                {
                    whereString += "AND ";
                }
                whereString += "Id = " + Id + " ";
            }
            if (whereString != null)
                query += whereString;
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
                        dbReader.GetChars(i++, 0, tempAppointmentType.Color, 0, 6);
                        tempAppointmentType.Price = dbReader.GetDecimal(i++);
                        tempAppointmentType.DurationType = dbReader.GetBoolean(i++) ? DURATION_TYPE.HOURS : DURATION_TYPE.MINUTES;
                        tempAppointmentType.Duration = dbReader.GetInt32(i++);
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
                            "SELECT A.[Id] " +
                            ",[Description] " +
                            ",[StartDate] " +
                            ",[EndDate] " +
                            ",[FirstName] " +
                            ",[LastName] " +
                            ",[DateOfBirth] " +
                            ",[PhoneNumber] " +
                            ",[Email] " +
                            ",[FileName] " +
                            ",[FileDescription] " +
                            ",A.[TypeId] " +
                            ",AType.[Name] " +
                            ",AType.[StartTime] " +
                            ",AType.[DaysOfWeek] " +
                            ",AType.[Color] " +
                            ",AType.[Price] " +
                            ",AType.[DurationType] " +
                            ",AType.[Duration] " +
                            "FROM[BillysWebsiteDB].[dbo].[Appointment] A " +
                            "LEFT JOIN AppointmentType AType ON A.TypeId = AType.Id ";
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
                whereString += "Id = " + appointmentPK + " ";
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
                        tempAppointment.Id = dbReader.GetInt32(i++);
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
                        //Apointment Type
                        tempAppointment.AppointmentType = new AppointmentType();
                        tempAppointment.AppointmentType.Id = dbReader.GetInt32(i++);
                        tempAppointment.AppointmentType.Name = dbReader.GetString(i++);
                        tempAppointment.AppointmentType.StartTime = dbReader.GetTimeSpan(i++);
                        tempAppointment.AppointmentType.DaysOfWeek = dbReader.GetByte(i++);
                        dbReader.GetChars(i++, 0, tempAppointment.AppointmentType.Color, 0, 6);
                        tempAppointment.AppointmentType.Price = dbReader.GetDecimal(i++);
                        tempAppointment.AppointmentType.DurationType = dbReader.GetBoolean(i++) ? DURATION_TYPE.HOURS : DURATION_TYPE.MINUTES;
                        tempAppointment.AppointmentType.Duration = dbReader.GetInt32(i++);
                        appointments.Add(tempAppointment);
                    }
                }
            }
            dbHelper.CloseConnection();
            return appointments;
        }

        public static int AddAppointmentType(string name, TimeSpan startTime, int daysOfWeek,
                                              string color, decimal price,
                                              int durationType, int duration)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            dbHelper.OpenConection();
            string query = useString +
                            "INSERT INTO[dbo].[AppointmentType] " +
                            "([Name] " +
                            ",[StartTime] " +
                            ",[DaysOfWeek] " +
                            ",[Color] " +
                            ",[Price] " +
                            ",[DurationType] " +
                            ",[Duration]) " +
                            "VALUES " +
                            "('" + name + "' " +
                            ",'" + startTime.ToString(@"hh\:mm\:ss") + "' " +
                            "," + daysOfWeek +" " +
                            ",'" + color +"' " +
                            "," + price +" " +
                            "," + durationType +" " +
                            "," + duration +")";
            int success = dbHelper.ExecuteQueries(query);
            dbHelper.CloseConnection();
            return success;
        }

        internal static bool DeleteOldAppointments()
        {
            throw new NotImplementedException();
        }
    }
}
