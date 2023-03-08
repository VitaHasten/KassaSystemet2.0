using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class Receipt
    {
        public Receipt(DateTime paymentNumber, int produktKodCashier, string productName, decimal quantityCashier, decimal payment)
        {
            paymentNumber = _paymentNumber;
            produktKodCashier = _produktKodCashier;
            productName = _productName;
            quantityCashier = _quantityCashier;
            payment = _payment;
        }

        private DateTime _paymentNumber;
        private int _produktKodCashier;
        private string _productName;
        private decimal _quantityCashier;
        private decimal _payment;

        public DateTime GetRecieptNumber()
        {
            return _paymentNumber;
        }

        public decimal GetProduktKodCashier()
        {
            return _produktKodCashier;
        }

        public string GetProductName()
        {
            return _productName;
        }

        public decimal GetQuantityCashier()
        {
            return _quantityCashier;
        }

        public decimal GetPayment()
        {
            return _payment;
        }
    }
}
