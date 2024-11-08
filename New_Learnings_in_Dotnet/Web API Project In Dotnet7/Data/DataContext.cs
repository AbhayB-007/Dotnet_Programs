using Microsoft.EntityFrameworkCore;
using WebAPIProject_In_Dotnet_7.Models;

namespace WebAPIProject_In_Dotnet_7.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Character> Characters { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Name=DefaultConnection");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>(b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));               

                b.Property<int>("Defense")
                    .HasColumnType("int");

                b.Property<int>("HitPoints")
                    .HasColumnType("int");

                b.Property<int>("Intelligence")
                    .HasColumnType("int");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("Strength")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.ToTable("Characters");
            });
        }
    }
}
