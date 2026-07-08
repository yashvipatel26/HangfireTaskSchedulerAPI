using System.ComponentModel.DataAnnotations.Schema;
namespace HangfireTaskSchedulerAPI.Models

{
    [Table("TaskMaster")]
    public class TaskMaster
    {
        public int Id { get; set; }

        public string EmployeeName { get; set; }

        public string TaskName { get; set; }

        public string Status { get; set; }
    }
}