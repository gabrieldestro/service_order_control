using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceOrder.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public string? Description { get; set; }

        // Timeline Dates
        public DateTime? ReceivedDate { get; set; }
        public DateTime? DocumentSentDate { get; set; }
        public DateTime? DocumentReceivedDate { get; set; }
        public DateTime? ProjectRegistrationDate { get; set; }
        public DateTime? ProjectSubmissionDate { get; set; }
        public DateTime? ProjectApprovalDate { get; set; }
        public DateTime? InspectionRequestDate { get; set; }
        public DateTime? FinalizationDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }

        // Financial
        public decimal? ProjectValue { get; set; }
        public bool Enabled { get; set; }

        // Navigation Properties
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int FinalClientId { get; set; }
        public Client? FinalClient { get; set; }

        public int ElectricCompanyId { get; set; }
        public ElectricCompany? ElectricCompany { get; set; }
    }
}
