using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public enum enhet { Styckpris, Kilopris}
    public class Product
    {
        public Product(int productId, string productName, decimal price, enhet enhet )
        {
            _productId=productId;
            _productName=productName;
            _price=price;
            _enhet=enhet;
        }

        private int _productId { get; set; }
        private decimal _price { get; set; }
        private enhet _enhet { get; set; }
        private string _productName { get; set; }

        public int GetId() { return _productId; }
        public decimal GetPrice() { return _price; }
        public enhet GetEnhet() { return _enhet; }
        public string GetProductName() { return _productName;}
    }
}


