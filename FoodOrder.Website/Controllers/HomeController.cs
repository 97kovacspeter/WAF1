using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FoodOrder.Website.Services;
using FoodOrder.Persistence;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FoodOrder.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodService _foodService;

        public HomeController( FoodService foodService )
        {
            _foodService = foodService;
        }

        public IActionResult Index()
        {
            ViewBag.CategoriesBag = _foodService.GetCategories( );
            ViewBag.FoodBag = _foodService.GetBestTen( );
            return View( );
        }

        public IActionResult CategoryPage( Int32? id, String searchString )
        {
            if (id == null)
            {
                return BadRequest( );
            }

            ViewBag.CategoriesBag = _foodService.GetCategories( );

            Category category = _foodService.GetCategory( (Int32)id );

            List<DrinkOrDish> foods = _foodService.GetDishes( (Int32)id );

            ViewBag.SearchString = searchString;

            ViewBag.CategoryBag = category;

            ViewBag.FoodBag = foods;

            return View( _foodService.GetSearch( searchString ) );
        }

        [HttpGet]
        public IActionResult AddedToCart( Int32 id )
        {
            DrinkOrDish ordered = _foodService.GetDishById( id );

            ViewBag.CategoriesBag = _foodService.GetCategories( );

            string send = "";
            if (Request.Cookies.ContainsKey( "order" ))
            {
                send = Request.Cookies[ "order" ];
            }

            Int32 sum = 0;
            foreach (var item in send.Split( " " ))
            {
                if (item != "")
                {
                    sum += _foodService.GetDishById( Int32.Parse( item ) ).Price;
                }
            }

            if (sum + _foodService.GetDishById( id ).Price <= 20000)
            {
                send += id.ToString( ) + " ";
                Response.Cookies.Append( "order", send, new CookieOptions { Expires = DateTime.Today.AddDays( 2 ) } );
            }
            else
            {
                return View( -1 );
            }

            return View( ordered.CategoryId );
        }

        public IActionResult Cart()
        {

            ViewBag.CategoriesBag = _foodService.GetCategories( );
            string send = "";
            if (Request.Cookies.ContainsKey( "order" ))
            {
                send = Request.Cookies[ "order" ];
            }

            List<DrinkOrDish> orders = new List<DrinkOrDish>( );
            foreach (var item in send.Split( " " ))
            {
                if (item != "")
                {
                    orders.Add( _foodService.GetDishById( Int32.Parse( item ) ) );
                }
            }

            Int32 id = 0;
            if (send == "")
            {
                id = -1;
            }

            ViewBag.OrderBag = orders;

            return View( id );
        }

        public IActionResult RemovedFromCart( Int32 id )
        {

            ViewBag.CategoriesBag = _foodService.GetCategories( );
            if (id != -1)
            {

                DrinkOrDish remove = _foodService.GetDishById( id );

                string send = "";
                if (Request.Cookies.ContainsKey( "order" ))
                {
                    send = Request.Cookies[ "order" ];
                }

                if (send.Contains( id.ToString( ) ))
                {
                    send = send.Remove( send.IndexOf( id.ToString( ) ), 2 );
                }

                Response.Cookies.Append( "order", send, new CookieOptions { Expires = DateTime.Today.AddDays( 2 ) } );

                return View( );
            }
            else
            {
                string send = "";
                Response.Cookies.Append( "order", send, new CookieOptions { Expires = DateTime.Today.AddDays( 2 ) } );
                return View( );
            }

        }

        [HttpPost]
        public IActionResult End( Order model )
        {
            if (model == null)
            {
                return BadRequest( );
            }

            if (String.IsNullOrEmpty( model.Name ))
            {
                ModelState.AddModelError( "Name", "Required!" );
            }
            if (String.IsNullOrEmpty( model.Address ))
            {
                ModelState.AddModelError( "Address", "Required!" );
            }
            if (String.IsNullOrEmpty( model.Phone ))
            {
                ModelState.AddModelError( "Phone", "Required!" );
            }

            if (ModelState.IsValid)
            {
                string send = "";
                if (Request.Cookies.ContainsKey( "order" ))
                {
                    send = Request.Cookies[ "order" ];
                }
                foreach (var item in send.Split( " " ))
                {
                    if (item != "")
                    {
                        _foodService.FameInc( Int32.Parse( item ) );

                    }
                }
                send = "";
                Response.Cookies.Append( "order", send, new CookieOptions { Expires = DateTime.Today.AddDays( 2 ) } );
                return RedirectToAction( "Success" );
            }
            else
            {
                return BadRequest( );
            }
        }

        public IActionResult SendOrder()
        {
            Order model = new Order( );
            return View( model );
        }

        public IActionResult Success()
        {
            return View( );
        }
    }
}
