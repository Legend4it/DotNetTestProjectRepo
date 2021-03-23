using DAL;
using Microsoft.EntityFrameworkCore;

namespace ConsoleClient
{
    public class PassTimeContext:DbContext
    {
        public DbSet<PassTimeModel>   PassTime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=C:\Dev\Source\Repos\DotNetTestProjectRepo\CollactorBank\Common\DB\db1.db");
    }
}
