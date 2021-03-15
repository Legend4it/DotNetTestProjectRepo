using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class DataContext:DbContext
    {
        public DbSet<PassTimeModel> PassTime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=C:\Dev\Source\Repos\DotNetTestProjectRepo\CollactorBank\DAL\App_Data\MainDb.db");

    }
}
