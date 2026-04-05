using Microsoft.AspNetCore.Mvc;
using StudentTodoApi.Models;
using StudentTodoApi.Data;
using System.Collections.Generic;
using System.Linq;

namespace StudentTodoApi.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        // GET /api/tasks -> get all tasks
        [HttpGet]
        public IActionResult GetAllTasks([FromQuery] int? userId)
        {
            if (userId.HasValue)
            {
                // If userId is provided, return tasks for that user (Student view)
                return Ok(DataStore.Tasks.Where(t => t.UserId == userId.Value).ToList());
            }
            // Otherwise return all tasks with student info (Admin view)
            var tasksWithStudentInfo = DataStore.Tasks.Select(t => 
            {
                var student = DataStore.Users.FirstOrDefault(u => u.Id == t.UserId);
                return new
                {
                    t.Id,
                    t.Title,
                    t.IsCompleted,
                    t.UserId,
                    t.Timestamp,
                    StudentName = student?.Name ?? "Unknown",
                    StudentSection = student?.Section ?? "N/A"
                };
            }).ToList();
            
            return Ok(tasksWithStudentInfo);
        }

        // POST /api/tasks/add -> add task
        [HttpPost("add")]
        public IActionResult AddTask([FromBody] TodoTask task)
        {
            if (string.IsNullOrEmpty(task.Title))
            {
                return BadRequest(new { message = "Task title is required" });
            }

            task.Id = DataStore.GetNextTaskId();
            task.IsCompleted = false;
            task.Timestamp = System.DateTime.UtcNow;
            DataStore.Tasks.Add(task);
            return Ok(new { message = "Task added successfully", taskId = task.Id });
        }

        // PUT /api/tasks/review/{id} -> mark as completed
        [HttpPut("review/{id}")]
        public IActionResult ReviewTask(int id)
        {
            var task = DataStore.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });
            }

            task.IsCompleted = true;
            return Ok(new { message = "Task marked as completed" });
        }

        // DELETE /api/tasks/delete/{id} -> delete task
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = DataStore.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound(new { message = "Task not found" });
            }

            DataStore.Tasks.Remove(task);
            return Ok(new { message = "Task deleted successfully" });
        }

        // GET /api/tasks/students -> helper for Admin to see all students
        [HttpGet("students")]
        public IActionResult GetStudents()
        {
            var students = DataStore.Users
                .Where(u => u.Role == "Student")
                .Select(u => new { u.Id, u.Username, u.Name, u.Section })
                .ToList();
            return Ok(students);
        }
    }
}
