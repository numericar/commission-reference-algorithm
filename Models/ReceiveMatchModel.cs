using System;

namespace CommissionRefs.Models;

public class ReceiveMatchModel
{
    public decimal Amount { get; set; }
    public List<ReceiveItemModel> ReceiveMatchs { get; set; } = new List<ReceiveItemModel>();
}
