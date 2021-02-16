using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoodOrder.Persistence;

namespace FoodOrder.Website.Services
{
    public class FoodService
    {
        private readonly FoodOrderDbContext _context;

        public FoodService( FoodOrderDbContext context )
        {
            _context = context;
        }

        public List<DrinkOrDish> GetBestTen()
        {
            return _context.DrinksOrDishes
                .OrderBy( l => l.Name )
                .OrderByDescending( l => l.Fame )
                .Take( 10 )
                .ToList( );
        }

        public List<DrinkOrDish> GetSearch( string searchString = null )
        {
            return _context.DrinksOrDishes
                .Where( l => l.Name.Contains( searchString ?? "" ) )
                .OrderBy( l => l.Name )
                .ToList( );
        }

        public DrinkOrDish GetDishById( Int32 id )
        {
            return _context.DrinksOrDishes.Find( id );
        }

        public void FameInc( Int32 id )
        {
            _context.DrinksOrDishes.Find( id ).Fame++;
            _context.SaveChanges( );
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList( );
        }

        public Category GetCategory( Int32 id )
        {
            return _context.Categories.Find( id );
        }

        public List<DrinkOrDish> GetDishes( Int32 id )
        {
            return _context.DrinksOrDishes
                .Where( l => l.CategoryId.Equals( id ) )
                .ToList( );
        }

    }
}
