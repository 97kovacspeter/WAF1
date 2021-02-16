using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FoodOrder.Persistence
{
    public class Category
    {
        [Key] public Int32 Id { get; set; }

        [Required] [MaxLength( 50 )] public String Name { get; set; }

        public ICollection<DrinkOrDish> Items { get; set; }
    }

    public class DrinkOrDish
    {
        [Key] public Int32 Id { get; set; }

        [Required] [MaxLength( 50 )] public String Name { get; set; }

        [DataType( DataType.MultilineText )] public String Description { get; set; }

        [Required] [DisplayName( "Category" )] public Int32 CategoryId { get; set; }

        public Int32 Price { get; set; }

        public Boolean Spicy { get; set; }

        public Boolean Vegetarian { get; set; }

        public Int32 Fame { get; set; } //how many times it was ordered

        public Int32 Status { get; set; }

        public virtual Category Category { get; set; }
    }


    public class Order
    {
        [Key] public Int32 Id { get; set; }

        [Required]
        [MinLength( 1 )]
        [MaxLength( 50 )]
        public String Name { get; set; }

        [Required]
        [MinLength( 1 )]
        [MaxLength( 50 )]
        public String Address { get; set; }

        [Required]
        [MinLength( 1 )]
        [MaxLength( 50 )]
        public String Phone { get; set; }

        [ForeignKey( "DrinkOrDish" )]
        public ICollection<DrinkOrDish> Orders { get; set; }

        //[Required] public Boolean Delivered { get; set; }
    }

    /*
     Regisztrált felhasználók modellje. A legalapvetőbb tulajdonságokat
     (pl.: név jelszó) az IdentityUser már tartalmazza, de ebből leszármaztatva
     plusz mezőkkel egészíthetjük ezt.
     */
    public class ApplicationUser : IdentityUser
    {

    }
}
