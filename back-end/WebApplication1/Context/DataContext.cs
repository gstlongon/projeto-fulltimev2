using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.Context
{
    public class DataContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ClientesDB;ConnectRetryCount=0");
        }

        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Motorista> Motorista { get; set; }
    }
}
