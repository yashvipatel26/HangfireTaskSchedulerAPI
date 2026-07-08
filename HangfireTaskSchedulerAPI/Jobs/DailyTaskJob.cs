using HangfireTaskSchedulerAPI.Data;
using HangfireTaskSchedulerAPI.Services;
using System.Text;

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

        // ------------------- Daily -------------------
        public void SendDailyTask()
        {
            SendTaskEmail(
                "Daily Task Reminder",
                "Daily Pending Tasks",
                "This is the daily email for today's pending tasks.");
        }

        // ------------------- Weekly -------------------
        public void SendWeeklyTask()
        {
            SendTaskEmail(
                "Weekly Task Reminder",
                "Weekly Pending Tasks",
                "This is the weekly email for pending tasks.");
        }

        // ------------------- Monthly -------------------
        public void SendMonthlyTask()
        {
            SendTaskEmail(
                "Monthly Task Reminder",
                "Monthly Pending Tasks",
                "This is the monthly email for pending tasks.");
        }

        // ------------------- Hourly -------------------
        public void SendHourlyTask()
        {
            SendTaskEmail(
                "Hourly Task Reminder",
                "Hourly Pending Tasks",
                "This is the hourly email for pending tasks.");
        }

        // ------------------- Common Email Method -------------------
        private void SendTaskEmail(string subject, string heading, string message)
        {
            var tasks = _context.TaskMasters.ToList();

            StringBuilder html = new StringBuilder();

            html.Append("<html>");
            html.Append("<body style='font-family:Arial;'>");

            html.Append($"<h2 style='color:blue;'>{heading}</h2>");

            html.Append("<p>Hello Team,</p>");

            html.Append($"<p>{message}</p>");

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
                "projecthangfire@gmail.com",
                subject,
                html.ToString()
            );
        }
    }
}
