using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameManager.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public int InventoryCount { get; set; }
        [JsonIgnore]
        public virtual ICollection<Genre> Genre { get; set; }
        [JsonIgnore]
        public virtual ICollection<Tag> Tags { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cart> Carts { get; set; }
    }
}