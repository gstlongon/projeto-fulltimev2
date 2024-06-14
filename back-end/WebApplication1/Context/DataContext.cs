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

        public DbSet<Encomenda> Encomenda { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Motorista> Motorista { get; set; }
        public DbSet<Trajeto> Trajeto { get; set; }
        public DbSet<Loja> Loja { get; set; }

    }
}
