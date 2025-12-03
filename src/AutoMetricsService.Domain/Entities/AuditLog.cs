using Core.Framework.Domain.Common;
using System;

namespace AutoMetricsService.Domain.Entities
{
    public class AuditLog : BaseEntity
    {
        public string TableName { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Operation { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime OperationDate { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public Guid? TransactionID { get; set; }
    }
}
