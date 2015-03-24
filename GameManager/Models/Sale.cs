using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GameManager.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CartId { get; set; }
        public int EmployeeId { get; set; }
        public decimal TotalAmount { get; set; }
    }
}