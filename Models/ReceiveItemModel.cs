using System;

namespace CommissionRefs.Models;

public class ReceiveItemModel
{
    public int ReceiveId { get; set; }
    public int Sequence { get; set; }
    public decimal ReceiveAmount { get; set; }
    public int Installment { get; set; }
    public int? RefReceiveId { get; set; }
    public int? RefReceiveItem { get; set; }
}
