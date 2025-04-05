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
        public string? Name { get; set; }
        public string? Cnpj { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public bool Enabled { get; set; }

    }
}
