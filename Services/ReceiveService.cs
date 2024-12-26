using System;
using CommissionRefs.Models;

namespace CommissionRefs.Services;

public class ReceiveService
{
    public List<ReceiveItemModel> ReadDeductCommissionReceiveItems() 
    {
        List<ReceiveItemModel> receiveItemModels = new List<ReceiveItemModel>();

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 1,
            Sequence = 1,
            ReceiveAmount = 500,
            Installment = 1
        });

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 2,
            Sequence = 1,
            ReceiveAmount = 1000,
            Installment = 1
        });

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 3,
            Sequence = 1,
            ReceiveAmount = 300,
            Installment = 1
        });

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 3,
            Sequence = 2,
            ReceiveAmount = 300,
            Installment = 1
        });

        return receiveItemModels;
    }

    public List<ReceiveItemModel> ReadCancelDeductCommissionReceiveItems()
    {
        List<ReceiveItemModel> receiveItemModels = new List<ReceiveItemModel>();

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 4,
            Sequence = 1,
            ReceiveAmount = 200,
            Installment = 1,
            RefReceiveId = 1,
            RefReceiveItem = 1 
        });

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 6,
            Sequence = 1,
            ReceiveAmount = 300,
            Installment = 1,
            RefReceiveId = 1,
            RefReceiveItem = 1 
        });

        receiveItemModels.Add(new ReceiveItemModel()
        {
            ReceiveId = 5,
            Sequence = 1,
            ReceiveAmount = 300,
            Installment = 1,
            RefReceiveId = 2,
            RefReceiveItem = 1 
        });

        return receiveItemModels;
    }

    public void RemoveDeductAmountNotEnogh(List<ReceiveItemModel> deductCommissions, List<ReceiveItemModel> cancelDeductCommissions) 
    {
        int[] deductRemoveIndexs = new int[deductCommissions.Count];
        int currentDeductRemoveIndex = 0;

        for(int i = 0; i < deductRemoveIndexs.Length; i++) deductRemoveIndexs[i] = -1;

        for (int deductIndex = 0; deductIndex < deductCommissions.Count; deductIndex++)
        {
            ReceiveItemModel deductCommission = deductCommissions[deductIndex];

            for (int cancelIndex = 0; cancelIndex < cancelDeductCommissions.Count; cancelIndex++)
            {
                ReceiveItemModel cancelDeductCommission = cancelDeductCommissions[cancelIndex];

                bool isMatchReceiveId = this.IsMatch(deductCommission.ReceiveId, cancelDeductCommission.RefReceiveId);
                bool isMatchReceiveItem = this.IsMatch(deductCommission.Sequence, cancelDeductCommission.RefReceiveItem);
                bool isMatchInstallment = this.IsMatch(deductCommission.Installment, cancelDeductCommission.Installment);

                if (isMatchReceiveId && isMatchReceiveItem && isMatchInstallment) deductCommissions[deductIndex].ReceiveAmount -= cancelDeductCommission.ReceiveAmount;

                if (deductCommissions[deductIndex].ReceiveAmount <= 0) 
                {
                    deductRemoveIndexs[currentDeductRemoveIndex++] = deductIndex;
                    break;
                }
            }
        }

        this.RemoveDeductCommissions(deductRemoveIndexs, deductCommissions);
    }

    private void RemoveDeductCommissions(int[] deductRemoveIndexs, List<ReceiveItemModel> deductCommissions) 
    {
        for (int removeIndex = 0; removeIndex < deductRemoveIndexs.Length; removeIndex++)
        {
            int deductCommissionIndex = deductRemoveIndexs[removeIndex];

            if (deductCommissionIndex < 0) break;

            deductCommissions.RemoveAt(deductCommissionIndex);
        }
    }

    private bool IsMatch(int? a, int? b) => a == b;
}
