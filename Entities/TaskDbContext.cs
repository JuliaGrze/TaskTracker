using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class TaskDbContext : DbContext
    {
        public DbSet<TaskEntity> Tasks { get; set; }

        public TaskDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>().ToTable("Tasks");

            //Seed to Tasks
            string tasks = File.ReadAllText("tasks.json");
            List<TaskEntity> tasksList = JsonSerializer.Deserialize<List<TaskEntity>>(tasks);
            foreach (TaskEntity task in tasksList)
            {
                modelBuilder.Entity<TaskEntity>().HasData(task);
            }
        }
    }
}
