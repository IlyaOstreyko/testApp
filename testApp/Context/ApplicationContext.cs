using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using testApp.DataModels;

namespace testApp.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<TestQuestionDataModel> TestQuestionDataModels { get; set; }

        //public ApplicationContext(DbContextOptions<ApplicationContext> options)
        //    : base(options)
        //{
        //    Database.EnsureCreated();
        //}

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
            //optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NewTestQuestion1;Username=postgres;Password=7906");
        }
    }
}
