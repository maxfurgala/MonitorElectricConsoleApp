using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MonitorElectricConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitorElectricConsoleApp
{
    public class AppContext : DbContext
    {
        /// <summary>
        /// коллекция результатов
        /// </summary>
        public DbSet<Result> Results { get; set; }

        public AppContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=local.db");
            //optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
        }
    }
}
