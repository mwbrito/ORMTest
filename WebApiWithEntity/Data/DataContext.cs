using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using WebApiWithEntity.Models;

namespace WebApiWithEntity.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<MovieModel> Movies { get; set;}
        public DbSet<DirectorModel> Directors { get; set;}

        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        // {
        //     modelBuilder.Entity<MovieModel>()
        //         .HasOne(p => p.Director);
        // }
    }
}