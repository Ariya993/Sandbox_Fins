using Microsoft.EntityFrameworkCore;
using Sandbox_Calc.Model;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Tambahkan DbSet untuk tabel di database
    // public DbSet<User> Users { get; set; }
   // public DbSet<RequestLog> RequestLogs { get; set; }
   // public DbSet<Product> Products { get; set; }
    public DbSet<Appuser> APPUSER { get; set; }
    public DbSet<Transaksi> Transaksi { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<APPUSER_FCM> APPUSER_FCM { get; set; }
}