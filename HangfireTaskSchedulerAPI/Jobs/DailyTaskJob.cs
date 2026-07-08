using HangfireTaskSchedulerAPI.Data;
using HangfireTaskSchedulerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using HangfireTaskSchedulerAPI.Services;

namespace HangfireTaskSchedulerAPI.Jobs
{
    public class DailyTaskJob
    {
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;

        public DailyTaskJob(
            EmailService emailService,
            ApplicationDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        public void SendDailyTask()
        {
            var tasks = _context.TaskMasters.ToList();

            StringBuilder html = new StringBuilder();


            html.Append("<html>");
            html.Append("<body style='font-family:Arial;'>");

            html.Append("<h2 style='color:blue;'>Today's Pending Tasks</h2>");

            html.Append("<p>Hello Team,</p>");

            html.Append("<p>This is the daily email for today's pending tasks.</p>");

            html.Append("<p>Please find the task details below:</p>");

            html.Append("<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse;'>");

            html.Append("<tr style='background-color:#4CAF50;color:white;'>");
            html.Append("<th>Employee Name</th>");
            html.Append("<th>Task Name</th>");
            html.Append("<th>Status</th>");
            html.Append("</tr>");

            foreach (var task in tasks)
            {
                html.Append("<tr>");
                html.Append($"<td>{task.EmployeeName}</td>");
                html.Append($"<td>{task.TaskName}</td>");
                html.Append($"<td>{task.Status}</td>");
                html.Append("</tr>");
            }

            html.Append("</table>");

            html.Append("<br/>");

            html.Append("<p>Kindly complete the pending tasks on time.</p>");

            html.Append("<br/>");

            html.Append("<b>Thanks & Regards,</b><br/>");
            html.Append("Hangfire Task Scheduler Team");

            html.Append("</body>");
            html.Append("</html>");

            _emailService.SendEmail(
            "your_email@gmail.com",
            "Today's Pending Tasks",
             html.ToString()
   
            );
        }
    }
}