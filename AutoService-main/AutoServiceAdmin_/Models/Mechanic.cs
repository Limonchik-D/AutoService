using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoServiceAdmin_.Models
{
    public enum Specialization { Motorist, Bodyworker, Electrician }

    public class Mechanic
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public Specialization Specialization { get; set; }
        public List<int> CompletedOrders { get; set; } = new List<int>();
    }
}
