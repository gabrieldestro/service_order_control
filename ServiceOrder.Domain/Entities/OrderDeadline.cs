using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Domain.Entities
{
    public class OrderDeadline
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string OrderIdentifier { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        public int? DocumentSentDays { get; set; }
        public int? DocumentReceivedDays { get; set; }
        public int? ProjectRegistrationDays { get; set; }
        public int? ProjectSubmissionDays { get; set; }
        public int? ProjectApprovalDays { get; set; }
        public int? InspectionRequestDays { get; set; }
        public int? FinalizationDays { get; set; }
        public int? PaymentDays { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }

        public bool Enabled { get; set; } = true;
    }
}
