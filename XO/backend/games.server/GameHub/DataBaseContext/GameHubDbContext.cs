using GameHub.Models;
using Microsoft.EntityFrameworkCore;

namespace GameHub.DataBaseContext
{
    public class GameHubDbContext : DbContext
    {
        public GameHubDbContext(DbContextOptions<GameHubDbContext> options) : base(options) { }

        public DbSet<GameHubs> Games { get; set; }
    }
}
