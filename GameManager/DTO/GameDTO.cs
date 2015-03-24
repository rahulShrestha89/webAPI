using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameManager.Models
{
    public class GameDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryCount { get; set; }
        public string Genres { get; set; }
        public string Tags { get; set; }
    }

    public class NewGameDTO
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryCount { get; set; }
        public string Genres { get; set; }
        public string Tags { get; set; }
    }

    public class GameGenreDTO
    {
        public string Genres { get; set; }
    }

    public class GameTagDTO
    {
        public string Tags { get; set; }
    }

    public class CategoriesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genres { get; set; }
        public string Tags { get; set; }
    }

    public class GameCategoriesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Genres { get; set; }
    }
}