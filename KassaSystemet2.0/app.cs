using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class App
    {
        public void Run()
        {
            AddingStartupProducts();
            MainMenu();
        }
        
        static List<Product> productList = new List<Product>();
        static List<Receipt> receiptList = new List<Receipt>();
        int freeId = GetFreeId();
        int val = 0;
        int receiptNumber = 0;

        void AddingStartupProducts()
        {
            var newProduct = new Product(freeId, "Morötter", 39, enhet.Kilopris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);

            freeId = GetFreeId();
            newProduct = new Product(freeId, "Heroin", 1200, enhet.Kilopris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);
                
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Fnask", 1500, enhet.Styckpris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);
        }

        void MainMenu()
        {
            Console.WriteLine("***KASSASYSTEMET***\n");
            Console.WriteLine("1. Adminmeny");
            Console.WriteLine("2. Ta emot kunder\n");
            Console.WriteLine("9. AVSLUTA\n");

            val = int.Parse(Console.ReadLine());
            switch (val)
            {
                case 1:
                    Console.Clear();
                    AdminMenu();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("*** KASSAN ***");
                    Cashier();
                    break;

                case 3:
                    ShowProductList();
                    break;

                case 9:
                    Environment.Exit(0);
                    break;
            }
        }

        void AdminMenu()
        {
            Console.WriteLine("***KASSASYSTEMET ADMINMENY***\n");
            Console.WriteLine("1. Lägg till ny produkt");
            Console.WriteLine("2. Redigera befintlig produkt\n");
            Console.WriteLine("9. Huvudmeny\n");

            val = int.Parse(Console.ReadLine());
            switch (val)
            {
                case 1:
                    Console.Clear();
                    AddProductByUser();
                    break;
                case 2:
                    EditProduct();
                    break;
                case 3:
                    ShowProductList();
                    break;
                case 9:
                    MainMenu();
                    break;
            }
        }

        void AddProductByUser()
        {
            Console.Write("***LÄGG TILL NY PRODUKT***\n\nProduktnamn: ");
            string productName = Console.ReadLine();
            Console.Write("Pris: ");
            decimal price = Convert.ToDecimal(Console.ReadLine());
            var enheten =GetEnhet();
            var freeId = GetFreeId();
            var newProduct = new Product(freeId, productName, price, enheten);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);
            Console.WriteLine("\nPRODUKTEN SPARAD. ÅTER TILL HUVUDMENYN\n");
            MainMenu();
        }

        static int GetFreeId()
        {
            if (productList.Any(p => p.GetId() > 0)) 
            {
                return productList.Max(p => p.GetId()) + 1; 
            }
            else
            {
                return 1; 
            }
        }

        enhet GetEnhet()
        {
            Console.Write($"Prisenhet? {enhet.Kilopris} eller {enhet.Styckpris}: ");
            string input = Console.ReadLine().ToLower();
            return input.Contains("styck") ? enhet.Styckpris :
                   input.Contains("kilo") ? enhet.Kilopris : enhet.Styckpris;
        }

        void SaveProductToFile(Product product)
        {
            using (StreamWriter writer = new StreamWriter("Produktfil.txt", true))
            {
                writer.WriteLine(product.ToString());
            }
        }

        void ShowProductList()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine(" ID  | Produktnamn       |   Pris   | Enhet     ");
            Console.WriteLine("------------------------------------------------");
            foreach (var showProductList in productList)
            {
                Console.Write(String.Format("{0,-4} | {1,-17} | {2,8} | {3,5}",
                    " " + showProductList.GetId(), showProductList.GetProductName(), showProductList.GetPrice() + " kr", showProductList.GetEnhet() + "\n"));
            }
            Console.WriteLine("------------------------------------------------\n\nTryck ENTER för att återgå till huvudmenyn.");
            Console.ReadLine();
            Console.Clear();
            MainMenu();
        }

        void EditProduct()
        {
            Console.Clear();
            Console.Write("Ange produkt-ID som du vill ändra: ");
            var inputID = int.Parse(Console.ReadLine());
            foreach (var findItem in productList)
            {
                if (inputID == findItem.GetId())
                {
                    Console.WriteLine($"\nID: {findItem.GetId()}\nNAMN: {findItem.GetProductName()}\nPRIS: {findItem.GetPrice()} kr\n");
                }
            }

            Console.WriteLine("Vad vill du ändra? \n1. Namn\n2. Pris\n\n3.Återgå\n");
            var inputChoice = int.Parse(Console.ReadLine());
            if (inputChoice == 1)
            {
                Console.Write("Nytt produktnamn: ");
                var productname = Console.ReadLine();
                foreach (var product in productList)
                {
                    if (inputID == product.GetId())
                    {
                        product.SetProductName(productname);
                        Console.Clear();
                        AdminMenu();
                    }
                }
            }
            else if (inputChoice == 2)
            {
                Console.Write("Nytt pris: ");
                decimal price = decimal.Parse(Console.ReadLine());
                foreach (var product in productList)
                {
                    if (inputID == product.GetId())
                    {
                        product.SetProductPrice(price);
                        Console.Clear();
                        AdminMenu();
                    }
                }
            }
            else if (inputChoice == 3) 
            { 
                Console.Clear();
                AdminMenu(); 
            }
        }
        void Cashier()
        {
            var dateNow = DateTime.Now;
            Console.WriteLine($"KVITTO\t{dateNow}");
            string fullString = "";
            string checkInput = "";
            while (true)
            {
                Console.Write("\nAnge produktkod mellanslag antal: ");
                checkInput = Console.ReadLine();
                if (IsTheInputCorrect(checkInput))
                {
                    fullString = checkInput.ToUpper();
                }

                else
                {
                    Cashier();
                }

                int mellanslagsIndex = fullString.IndexOf(" ");
                string produktKodString = fullString[..mellanslagsIndex];
                string quantityCashierString = fullString[(mellanslagsIndex + 1)..];

                int produktKodCashier = int.Parse(produktKodString);
                int quantityCashier = int.Parse(quantityCashierString);

                var payment = (GetTheSum(produktKodCashier, quantityCashier));
                var name = GetTheName(produktKodCashier);
                int tryProductCode = Convert.ToInt32(produktKodCashier);


                if (!productList.Any(product => product.GetId() == tryProductCode))
                {
                    Console.WriteLine("FELAKTIG INMATNING");
                    Cashier();
                }
                var price = GetThePrice(produktKodCashier);

                var newReciptLine = new Receipt(dateNow, produktKodCashier, name, quantityCashier, payment);
                receiptList.Add(newReciptLine);
                //Console.WriteLine($"\n{name} x {quantityCashier} = {payment.ToString("N2")} kr");
                foreach (var receiptLines in receiptList)
                {
                    Console.WriteLine($"{name} {quantityCashier} * {price} = {payment.ToString("N2")} kr");
                }
            }
        }
        bool IsTheInputCorrect(string checkInput)
        {
            if (checkInput.ToUpper() == "PAY")
            {
                Receipt();
            }
            if (!checkInput.Contains(" "))
            {
                Console.WriteLine("FELAKTIG INMATNING");
                Cashier();
            }

            int mellanslagsIndex = checkInput.IndexOf(" ");
            string inputDigits1 = checkInput[..mellanslagsIndex];
            string inputDigits2 = checkInput[(mellanslagsIndex + 1)..];

            foreach (char c in inputDigits1)
            {
                if (!Char.IsDigit(c))
                {
                    Console.WriteLine("DU SKREV INTE IN EN SIFFRA");
                    return false;
                }
            }

            foreach (char c in inputDigits2)
            {
                if (!Char.IsDigit(c))
                {
                    Console.WriteLine("DU SKREV INTE IN EN SIFFRA");
                    return false;
                }
            }
            return true;
        }
        decimal GetTheSum(int produktKodCashier, int quantity)
        {
            foreach (var price in productList)
            {
                if (produktKodCashier == price.GetId())
                {
                    var prize = price.GetThePrice(price.GetId());
                    decimal sum = prize * quantity;
                    return sum;
                }
            }
            return 0;
        }

        decimal GetThePrice(int produktKodCashier)
        {
            foreach (var price in productList)
            {
                if (produktKodCashier == price.GetId())
                {
                    var priset = price.GetThePrice(price.GetId());

                    return priset;
                }
            }
            return 0;
        }

        string GetTheName(int produktKodCashier)
        {
            foreach (var name in productList)
            {
                if (produktKodCashier == name.GetId())
                {
                    return name.GetTheProductName(produktKodCashier);
                }
            }
            return "fel";
        }
        void Receipt()
        {
            receiptNumber++;
            Console.WriteLine(receiptNumber);
            Console.ReadLine();
            MainMenu();
        }
    }
}