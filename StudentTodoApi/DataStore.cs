using StudentTodoApi.Models;
using System.Collections.Generic;
using System;

namespace StudentTodoApi.Data
{
    public static class DataStore
    {
        public static List<User> Users = new List<User>();
        public static List<TodoTask> Tasks = new List<TodoTask>();
        private static int _nextUserId = 1;
        private static int _nextTaskId = 1;

        public static int GetNextUserId() => _nextUserId++;
        public static int GetNextTaskId() => _nextTaskId++;

        static DataStore()
        {
            // Optional: Seed an admin user for testing
            Users.Add(new User { Id = GetNextUserId(), Username = "admin", Password = "password", Name = "Admin User", Section = "N/A", Role = "Admin" });
        }
    }
}
