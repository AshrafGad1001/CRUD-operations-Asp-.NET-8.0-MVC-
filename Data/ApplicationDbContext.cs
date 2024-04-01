using GameZone.Models;

namespace GameZone.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Game> Games { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<GameDevice> GameDevices { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category{ Id = 1,Name="Sports"},
                new Category{ Id = 2,Name="Action"},
                new Category{ Id = 3,Name="Adventrue"},
                new Category{ Id = 4,Name="Racing"},
                new Category{ Id = 5,Name="Fighting"},
                new Category{ Id = 6,Name="Filim"}
            });
            modelBuilder.Entity<Device>().HasData(new Device[]
            {
                new Device{Id =1,Name="playStation",Icon="bi bi-playstation"},
                new Device{Id =2,Name="Xbox",Icon="bi bi-xbox"},
                new Device{Id =3,Name="Nintento Switch",Icon="bi bi-nintendo"},
                new Device{Id =4,Name="PC",Icon="bi bi-pc-display"}
            });

            modelBuilder.Entity<GameDevice>()
                .HasKey(e => new { e.GameId, e.DeviceId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
