using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResortApplication.Domain.Entities;

namespace ResortApplication.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Villa> Villas { get; set; }
    public DbSet<VillaNumber> VillaNumbers { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Amenity>()
            .HasOne(a => a.Villa)
            .WithMany(v => v.Amenities)
            .HasForeignKey(a => a.VillaId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<VillaNumber>()
            .HasOne(a => a.Villa)
            .WithMany(v => v.VillaNumbers)
            .HasForeignKey(a => a.VillaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Villa)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VillaId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion(
                v => v.ToString(),
                v => (BookingStatus)Enum.Parse(typeof(BookingStatus), v));
        
        modelBuilder.Entity<Villa>().HasData(
            new Villa
            {
                Id = 1,
                Name = "Royal Villa",
                Description =
                    "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://placehold.co/600x400",
                Occupancy = 4,
                Price = 200,
                Sqft = 550
            },
            new Villa
            {
                Id = 2,
                Name = "Premium Pool Villa",
                Description =
                    "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://placehold.co/600x401",
                Occupancy = 4,
                Price = 300,
                Sqft = 550
            },
            new Villa
            {
                Id = 3,
                Name = "Luxury Pool Villa",
                Description =
                    "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                ImageUrl = "https://placehold.co/600x402",
                Occupancy = 4,
                Price = 400,
                Sqft = 750
            });

        modelBuilder.Entity<VillaNumber>().HasData(
            new VillaNumber
            {
                VillaNumberId = 101,
                VillaId = 1
            },
            new VillaNumber
            {
                VillaNumberId = 102,
                VillaId = 1
            },
            new VillaNumber
            {
                VillaNumberId = 103,
                VillaId = 1
            },
            new VillaNumber
            {
                VillaNumberId = 104,
                VillaId = 1
            },
            new VillaNumber
            {
                VillaNumberId = 201,
                VillaId = 2
            },
            new VillaNumber
            {
                VillaNumberId = 202,
                VillaId = 2
            },
            new VillaNumber
            {
                VillaNumberId = 203,
                VillaId = 2
            },
            new VillaNumber
            {
                VillaNumberId = 301,
                VillaId = 3
            },
            new VillaNumber
            {
                VillaNumberId = 302,
                VillaId = 3
            }
        );
    }
}