using System;
using CommissionRefs.Models;

namespace CommissionRefs.Services;

public class CancelDeductCommissionRefService
{
    private List<ReceiveItemModel>? deductCommissions;

    public void SetDeductCommissionSources(List<ReceiveItemModel> deductCommissions)
    {
        this.deductCommissions = deductCommissions;
    }

    public ReceiveMatchModel? Match(decimal cancelDeductAmount)
    {
        if (this.deductCommissions == null) throw new Exception("The resource of deduct commission should not be null");

        // validate data before process
        if (cancelDeductAmount <= 0 || this.deductCommissions.Count == 0) return null;

        // initialize list instance for result
        ReceiveMatchModel receiveMatched = new ReceiveMatchModel();

        // initialize temp cancel deduct 
        decimal tempCancelDeductAmount = cancelDeductAmount;

        // loop process
        for (int index = 0; index < this.deductCommissions.Count; index++)
        {
            ReceiveItemModel deductCommission = this.deductCommissions[index]; // read receive deduct commission by index
            
            decimal deductReceiveAmountRemaining = 0; // use for store current remaining of deduct amount
            decimal deductReceiveAmountUsed = 0; // use for store current used of deduct amount

            // validate cancel deduct amount is full
            if (tempCancelDeductAmount >= deductCommission.ReceiveAmount)
            {
                receiveMatched.Amount += deductCommission.ReceiveAmount; // added sum of deduct use
                deductReceiveAmountRemaining = 0; // change remaining to zero, cancel deduct can cancel all amount of current deduct
                deductReceiveAmountUsed = deductCommission.ReceiveAmount; // set deduct receive amount used full of receive amount
                tempCancelDeductAmount -= deductCommission.ReceiveAmount; // delete amount from cancel deduct amount calculated
            }
            else if (tempCancelDeductAmount < deductCommission.ReceiveAmount)
            {
                receiveMatched.Amount += tempCancelDeductAmount; // added sum of deduct use
                deductReceiveAmountRemaining = deductCommission.ReceiveAmount - tempCancelDeductAmount; // calculate deduct receive amount remaining
                deductReceiveAmountUsed = tempCancelDeductAmount; // set deduct receive amount used by cancel deduct amount
                tempCancelDeductAmount = 0; // set cancel deduct amount to zero
            }

            // add matched
            receiveMatched.ReceiveMatchs.Add(new ReceiveItemModel()
            {
                ReceiveId = deductCommission.ReceiveId,
                Sequence = deductCommission.Sequence,
                ReceiveAmount = deductReceiveAmountUsed,
                Installment = deductCommission.Installment
            });

            // update deduct commission resource
            this.deductCommissions[index].ReceiveAmount = deductReceiveAmountRemaining;

            if (tempCancelDeductAmount <= 0) break;
        }

        // process before ending
        this.Dispose();

        return receiveMatched;
    }

    private void Dispose() 
    {
        this.RemoveDeductCommissionUsed();
    }

    private void RemoveDeductCommissionUsed() 
    {
        // copy instance from original deduct commission
        List<ReceiveItemModel> tempDeductCommissions = new List<ReceiveItemModel>(this.deductCommissions!);
        
        for (int index = 0; index < tempDeductCommissions.Count; index++) 
        {
            ReceiveItemModel deductCommission = tempDeductCommissions[index];

            // if current deduct commission receive amount not enough
            if (deductCommission.ReceiveAmount <= 0)
            {
                // remove from original
                this.deductCommissions!.Remove(deductCommission);
            }
        }
    }

}
