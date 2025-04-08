using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string? Name { get; set; }

        [MaxLength(18)] // "00.000.000/0000-00" + margem
        public string? Cnpj { get; set; }

        [MaxLength(14)] // "000.000.000-00"
        public string? Cpf { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }

        public bool Enabled { get; set; }
    }
}
