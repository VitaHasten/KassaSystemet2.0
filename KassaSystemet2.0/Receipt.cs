using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class Receipt
    {
        public Receipt(DateTime paymentNumber, int produktKodCashier, string productName, int quantityCashier, decimal eachPrice, decimal payment, decimal totalSum)
        {
            _paymentNumber = paymentNumber;
            _produktKodCashier = produktKodCashier;
            _productName = productName;
            _quantityCashier = quantityCashier;
            _payment = payment;
            _eachPrice = eachPrice;
            _totalSum = totalSum;
        }

        private DateTime _paymentNumber { get; set; }
        private int _produktKodCashier { get; set; }
        private string _productName { get; set; }
        private int _quantityCashier { get; set; }
        private decimal _payment { get; set; }
        private decimal _eachPrice { get; set; }
        private decimal _totalSum { get; set; }

        //public DateTime GetRecieptNumber()
        //{
        //    return _paymentNumber;
        //}

        //public decimal GetProduktKodCashier()
        //{
        //    return _produktKodCashier;
        //}

        public string GetProductName()
        {
            return _productName;
        }

        public int GetQuantityCashier()
        {
            return _quantityCashier;
        }

        public decimal GetPayment()
        {
            return _payment;
        }

        //public decimal GetTotalSum()
        //{
        //    return _totalSum;
        //}

        public decimal GetEachPrice()
        {
            return _eachPrice;
        }
    }
}
