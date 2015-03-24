using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameManager.Models
{

    public class ViewSalesAdmin
    {
        public int Id { get; set; }
        public int CartID { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Game> Games {get; set;}
        public int SaleProcessedBy { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class ViewSalesCustomer
    {
        public int CartId { get; set; }
        public ICollection<Game> Games {get; set;}
        public decimal TotalAmount { get; set; }
    }
}