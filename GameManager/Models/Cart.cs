using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameManager.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool isProcessed { get; set; } 
        public virtual ICollection<Game> Games { get; set; }
    }
}