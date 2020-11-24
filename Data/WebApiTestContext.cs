using Microsoft.EntityFrameworkCore;
using WebApiTest.Models;

namespace WebApiTest.Data
{
    public class WebApiTestContext : DbContext
    {
        public WebApiTestContext(DbContextOptions<WebApiTestContext> opt) : base(opt)
        {
            
        }

        public DbSet<Command> Commands { get; set; }
        

    }
}