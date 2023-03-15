using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class Files
    {
        public StringBuilder Load()
        {
            StringBuilder receiptsFromFile = new StringBuilder();

            var test = File.ReadLines("ReceiptFile.txt").ToList();

            if (test.Count() == 0)
            {
                Console.WriteLine("INGA EXISTERANDE KVITTON");
            }

            using (var sr = new StreamReader("ReceiptFile.txt"))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    receiptsFromFile.AppendLine(line);
                }
                return receiptsFromFile;
            }
        }

        public void AddFilesIfNotExisting()
        {
            if (!File.Exists("ReceiptCounter.txt"))
            {
                using (FileStream fs = File.Create("ReceiptCounter.txt"))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("0");
                }
            }

            if (!File.Exists("ReceiptFile.txt"))
            {
                using (FileStream fs = File.Create("ReceiptFile.txt")) ;
            }
        }

        public void SaveKvittonummerToFile(CompleteReceipts completeReceipts, DateTime dateTime, decimal totalSum, List<Receipt> receiptList)
        {
            int receiptNumberFromFile = 0;

            var srRecNr = new StreamReader("ReceiptCounter.txt");
            using (srRecNr)
            {
                receiptNumberFromFile = int.Parse(srRecNr.ReadToEnd()) + 1;
            }


            var swRecNr = new StreamWriter("ReceiptCounter.txt", false);
            using (swRecNr)
            {
                swRecNr.WriteLine(receiptNumberFromFile);
            }

            var sw = new StreamWriter("ReceiptFile.txt", true);
            using (sw)
            {
                sw.WriteLine($"KVITTONUMMER: {receiptNumberFromFile}");
                sw.WriteLine("");
                sw.WriteLine($"{dateTime}");
                sw.WriteLine("");
            }

            foreach (var receiptLine in receiptList)
            {
                SaveReceiptLinesToFile(receiptLine);
            }

            sw = new StreamWriter("ReceiptFile.txt", true);
            using (sw)
            {
                sw.WriteLine("");
                sw.WriteLine($"Total: {totalSum.ToString("N2")} kr");
                sw.WriteLine("-----------------------");
                sw.WriteLine("");
            }
        }

        public void SaveReceiptLinesToFile(Receipt receipt)
        {
            var sw = new StreamWriter("ReceiptFile.txt", true);
            using (sw)
            {
                sw.WriteLine($"{receipt.GetProductName()} {receipt.GetQuantityCashier()} * {receipt.GetEachPrice().ToString("N2")} = {receipt.GetPayment().ToString("N2")}");
            }
        }

        public Files()
        {

        }
    }
}
