using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    
        public class CompleteReceipts
        {
            public CompleteReceipts(int kvittoNummer, DateTime paymentNumber, decimal totalSum, List<Receipt> receipt)
            {
                _paymentNumber = paymentNumber;
                _totalSum = totalSum;
                _kvittoNummer= kvittoNummer;
                _receiptLines = receipt;
            }
            private List<Receipt> _receiptLines { get; set; }
            private int _kvittoNummer { get; set; }
            private DateTime _paymentNumber { get; set; }
            private decimal _totalSum { get; set; }

            //public int GetKvittoNummer()
            //{
            //    return _kvittoNummer;
            //}

            //public DateTime GetRecieptNumber()
            //{
            //    return _paymentNumber;
            //}
                   
            //public decimal GetTotalSum()
            //{
            //    return _totalSum;
            //}

            //public List<Receipt> GetList()
            //{
            //    return _receiptLines;
            //}   
        }
}
