using HangfireTaskSchedulerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HangfireTaskSchedulerAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskMaster> TaskMasters { get; set; }
    }
}