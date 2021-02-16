using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FoodOrder.Persistence
{
    public class FoodOrderDbContext : DbContext
    {
        public FoodOrderDbContext( DbContextOptions<FoodOrderDbContext> options )
            : base( options )
        {
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<DrinkOrDish> DrinksOrDishes { get; set; }

        public DbSet<Order> Orders { get; set; }

        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
