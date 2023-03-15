using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public enum Enhet { Styckpris, Kilopris}
    public class Product
    {
        public Product(int productId, string productName, decimal price, Enhet enhet )
        {
            _productId=productId;
            _productName=productName;
            _price=price;
            _enhet=enhet;
        }

        private int _productId { get; set; }
        private decimal _price { get; set; }
        private Enhet _enhet { get; set; }
        private string _productName { get; set; }

        public int GetId() { return _productId; }
        public decimal GetPrice() { return _price; }
        public decimal GetThePrice(int productId) { return _price; }

        public Enhet GetEnhet() { return _enhet; }
        public string GetProductName() { return _productName;}
        public string GetTheProductName(int productId) { return _productName; }

        public void SetProductName(string newProductName)
        {
            _productName=newProductName;
        }

        public void SetProductPrice(decimal newProductPrice)
        {
            _price = newProductPrice;
        }
    }
}


