using CommissionRefs.Models;
using CommissionRefs.Services;

namespace Program {
    public class Program {
        private static List<ReceiveItemModel>? deductCommissions;
        private static List<ReceiveItemModel>? cancelDeductCommissions;
        public static void Main(string[] args) {
            // *** dependencies ***
            ReceiveService receiveService = new ReceiveService(); // create instance of receive service
            CancelDeductCommissionRefService refService = new CancelDeductCommissionRefService();
            // *** dependencies ***

            // *** initialize data ***
            deductCommissions = receiveService.ReadDeductCommissionReceiveItems();
            cancelDeductCommissions = receiveService.ReadCancelDeductCommissionReceiveItems();
            // *** initialize data ***

            Console.WriteLine("\nReceive Deduct Table Original-----------");
            Console.WriteLine("ReceiveId\tSeq\tAmount\tInstallment");
            foreach (ReceiveItemModel deductCommission in deductCommissions!)  Console.WriteLine($"{deductCommission.ReceiveId}\t\t{deductCommission.Sequence}\t{deductCommission.ReceiveAmount}\t{deductCommission.Installment}");
            Console.WriteLine("------------------------------------------");

            Console.WriteLine("\nAfter process with cancel deduct commission\n");
            receiveService.RemoveDeductAmountNotEnogh(deductCommissions, cancelDeductCommissions);
            Console.WriteLine("\nReceive Deduct Table Original-----------");
            Console.WriteLine("ReceiveId\tSeq\tAmount\tInstallment");
            foreach (ReceiveItemModel deductCommission in deductCommissions!)  Console.WriteLine($"{deductCommission.ReceiveId}\t\t{deductCommission.Sequence}\t{deductCommission.ReceiveAmount}\t{deductCommission.Installment}");
            Console.WriteLine("------------------------------------------");
        
            ReceiveMatchModel? receiveMatche = null;
            decimal[] cancelDeductCommissionsTest = [1200, 400, 300];
            foreach (decimal cancelDeductAmount in cancelDeductCommissionsTest)
            {
                if (deductCommissions != null)
                {
                    refService.SetDeductCommissionSources(deductCommissions);
                    receiveMatche = refService.Match(cancelDeductAmount);
                }

                Display(receiveMatche, cancelDeductAmount);
            }
        }

        public static void Display(ReceiveMatchModel? receiveMatche, decimal cancelDeductAmount)
        {
            Console.WriteLine("\nReceive Deduct Table Remaining----------");
            Console.WriteLine("ReceiveId\tSeq\tAmount\tInstallment");
            foreach (ReceiveItemModel deductCommission in deductCommissions!)  Console.WriteLine($"{deductCommission.ReceiveId}\t\t{deductCommission.Sequence}\t{deductCommission.ReceiveAmount}\t{deductCommission.Installment}");
            Console.WriteLine("------------------------------------------");

            if (receiveMatche != null)
            {
                Console.WriteLine($"Request cancel deduct commission: {cancelDeductAmount}");
                Console.WriteLine($"Sum receive deduct matched: {receiveMatche.Amount}");
                Console.WriteLine("Receive Deduct Table Matched--------------");
                foreach (ReceiveItemModel deductCommission in receiveMatche.ReceiveMatchs)  Console.WriteLine($"{deductCommission.ReceiveId}\t\t{deductCommission.Sequence}\t{deductCommission.ReceiveAmount}\t{deductCommission.Installment}");
            }
            else 
            {
                Console.WriteLine("Current cancel deduct commission not match with any deduct commission 😒");
            }
            Console.WriteLine("------------------------------------------\n");
        }
    }
}