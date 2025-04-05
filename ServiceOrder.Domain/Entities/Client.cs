using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string? FinalCustomerName { get; set; }
        public string? ResellerName { get; set; }
    }
}
