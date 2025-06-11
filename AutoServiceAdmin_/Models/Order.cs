using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceAdmin_.Models
{
    public enum OrderStatus { Pending, InProgress, Completed }

    public class Order
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int ClientId { get; set; }
        public string ProblemDescription { get; set; }
        public int AssignedMechanicId { get; set; }
        public decimal Cost { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
