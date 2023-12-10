using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrainPro.Models;
using static System.Net.WebRequestMethods;

namespace TrainPro.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {

        }
        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MenuItem>().HasData(
            new MenuItem
            {
                    Id = 1,
                    Name = "Spring Roll",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/spring roll.jpg",
                    Price = 7.99,
                    Category = "Appetizer",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 2,
                    Name = "Idli",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/idli.jpg",
                    Price = 8.99,
                    Category = "Appetizer",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 3,
                    Name = "Panu Puri",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/pani puri.jpg",
                    Price = 8.99,
                    Category = "Appetizer",
                    SpecialTag = "Best Seller"
                }, new MenuItem
                {
                    Id = 4,
                    Name = "Hakka Noodles",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/hakka noodles.jpg",
                    Price = 10.99,
                    Category = "Entrée",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 5,
                    Name = "Malai Kofta",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/malai kofta.jpg",
                    Price = 12.99,
                    Category = "Entrée",
                    SpecialTag = "Top Rated"
                }, new MenuItem
                {
                    Id = 6,
                    Name = "Paneer Pizza",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/paneer pizza.jpg",
                    Price = 11.99,
                    Category = "Entrée",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 7,
                    Name = "Paneer Tikka",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/paneer tikka.jpg",
                    Price = 13.99,
                    Category = "Entrée",
                    SpecialTag = "Chef's Special"
                }, new MenuItem
                {
                    Id = 8,
                    Name = "Carrot Love",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/carrot love.jpg",
                    Price = 4.99,
                    Category = "Dessert",
                    SpecialTag = ""
                }, new MenuItem
                {
                    Id = 9,
                    Name = "Rasmalai",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/rasmalai.jpg",
                    Price = 4.99,
                    Category = "Dessert",
                    SpecialTag = "Chef's Special"
                }, new MenuItem
                {
                    Id = 10,
                    Name = "Sweet Rolls",
                    Description = "Fusc tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    Image = "https://exampleproject.blob.core.windows.net/res/sweet rolls.jpg",
                    Price = 3.99,
                    Category = "Dessert",
                    SpecialTag = "Top Rated"
                });
        }
    }
}
