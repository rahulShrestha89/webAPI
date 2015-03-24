using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameManager.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Game> Games { get; set; }
    }
}