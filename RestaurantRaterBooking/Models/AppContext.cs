using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantRaterBooking.Models;
using System.Reflection.Emit;

namespace RestaurantRaterBooking.Models;

public class AppContext : IdentityDbContext<ApplicationUser>
{
    public AppContext(DbContextOptions<AppContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
		base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

		// Cấu hình cho ImageType
		builder.Entity<Image>()
			.Property(i => i.ImageType)
			.HasConversion(
				e => e.ToString(),
				e => (ImageType)Enum.Parse(typeof(ImageType), e)
			);
	}

    public DbSet<RestaurantRaterBooking.Models.Category> Category { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.PostCategory> PostCategory { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.Restaurant> Restaurant { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.Image> Image { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.Tag> Tag { get; set; } = default!;

	public DbSet<RestaurantRaterBooking.Models.RestaurantTag> RestaurantTag { get; set; } = default!;

	public DbSet<RestaurantRaterBooking.Models.News> News { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.NewsTag> NewsTag { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.Blog> Blog { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.BlogTag> BlogTag { get; set; } = default!;

    public DbSet<RestaurantRaterBooking.Models.Review> Review { get; set; }

    public DbSet<RestaurantRaterBooking.Models.Slider> Slider { get; set; } = default!;
}
