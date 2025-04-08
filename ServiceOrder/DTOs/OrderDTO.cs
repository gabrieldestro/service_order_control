using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceOrder.Domain.Entities;

namespace ServiceOrder.DTOs
{
    public class OrderDTO
    {
        public required Order Order { get; set; }
        public OrderDeadline? Deadline { get; set; }

        // 1. Document Sent
        public string DocumentSentStatusIcon => GetStatusIcon(Order.DocumentSentDate, Order.ReceivedDate, Deadline?.DocumentSentDays);
        public string DocumentSentTooltip => GetTooltip(Order.DocumentSentDate, Order.ReceivedDate, Deadline?.DocumentSentDays);

        // 2. Document Received
        public string DocumentReceivedStatusIcon => GetStatusIcon(Order.DocumentReceivedDate, Order.DocumentSentDate, Deadline?.DocumentReceivedDays);
        public string DocumentReceivedTooltip => GetTooltip(Order.DocumentReceivedDate, Order.DocumentSentDate, Deadline?.DocumentReceivedDays);

        // 3. Project Registration
        public string ProjectRegistrationStatusIcon => GetStatusIcon(Order.ProjectRegistrationDate, Order.DocumentReceivedDate, Deadline?.ProjectRegistrationDays);
        public string ProjectRegistrationTooltip => GetTooltip(Order.ProjectRegistrationDate, Order.DocumentReceivedDate, Deadline?.ProjectRegistrationDays);

        // 4. Project Submission
        public string ProjectSubmissionStatusIcon => GetStatusIcon(Order.ProjectSubmissionDate, Order.ProjectRegistrationDate, Deadline?.ProjectSubmissionDays);
        public string ProjectSubmissionTooltip => GetTooltip(Order.ProjectSubmissionDate, Order.ProjectRegistrationDate, Deadline?.ProjectSubmissionDays);

        // 5. Project Approval
        public string ProjectApprovalStatusIcon => GetStatusIcon(Order.ProjectApprovalDate, Order.ProjectSubmissionDate, Deadline?.ProjectApprovalDays);
        public string ProjectApprovalTooltip => GetTooltip(Order.ProjectApprovalDate, Order.ProjectSubmissionDate, Deadline?.ProjectApprovalDays);

        // 6. Inspection Request
        public string InspectionRequestStatusIcon => GetStatusIcon(Order.InspectionRequestDate, Order.ProjectApprovalDate, Deadline?.InspectionRequestDays);
        public string InspectionRequestTooltip => GetTooltip(Order.InspectionRequestDate, Order.ProjectApprovalDate, Deadline?.InspectionRequestDays);

        // 7. Finalization
        public string FinalizationStatusIcon => GetStatusIcon(Order.FinalizationDate, Order.InspectionRequestDate, Deadline?.FinalizationDays);
        public string FinalizationTooltip => GetTooltip(Order.FinalizationDate, Order.InspectionRequestDate, Deadline?.FinalizationDays);

        // 8. Payment
        public string PaymentStatusIcon => GetStatusIcon(Order.PaymentDate, Order.FinalizationDate, Deadline?.PaymentDays);
        public string PaymentTooltip => GetTooltip(Order.PaymentDate, Order.FinalizationDate, Deadline?.PaymentDays);


        // Utilitário para status de ícone
        private string GetStatusIcon(DateTime? current, DateTime? previous, int? days)
        {
            if (Deadline == null || !days.HasValue || !previous.HasValue)
                return "";

            var expected = previous.Value.AddDays(days.Value);

            if (!current.HasValue)
                return DateTime.Today <= expected ? "⚙" : "❌";

            return current.Value <= expected ? "✅" : "⚠";
        }

        // Utilitário para tooltip
        private string GetTooltip(DateTime? current, DateTime? previous, int? days)
        {
            if (Deadline == null || !days.HasValue || !previous.HasValue)
                return "Prazo não configurado";

            var expected = previous.Value.AddDays(days.Value);

            if (!current.HasValue)
            {
                var diff = (expected - DateTime.Today).Days;
                return diff >= 0
                    ? $"Faltam {diff} dias para o prazo."
                    : $"Prazo expirado há {-diff} dias.";
            }

            return current.Value <= expected
                ? "Concluído no prazo."
                : $"Concluído com atraso de {(current.Value - expected).Days} dias.";
        }
    }
}
