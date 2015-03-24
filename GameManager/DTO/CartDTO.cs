using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameManager.Models
{
    public class ViewCart
    {
        public string CustomerId { get; set; }
        public ICollection<Game> GamesInCart { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class GamesInViewCart
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public decimal Price { get; set; }
    }

    public class AddNewCartDTO
    {
        public string Customer { get; set; }
        public string Games { get; set; }
    }

    public class AddGameToCartDTO
    {
        public int Game { get; set; }
    }

    public class AddGamesToCartDTO
    {
        public string Games { get; set; }
    }
}