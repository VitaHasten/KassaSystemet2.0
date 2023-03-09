using System;
using System.Collections;
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
        
        static DateTime paymentNumber;
        static List<Product> productList = new List<Product>();
        List<Receipt> receiptList = new List<Receipt>();
        List<Receipt> completeReceiptList = new List<Receipt>();
        static List<Campaign> productsOnCampaignList = new List<Campaign>();
        int freeId = GetFreeId();
        int freeCampaignId = GetFreeCampaignId();
        int val = 0;
        static int receiptNumber = 0;
        static List<CompleteReceipts> ListOfAllReceipts = new List<CompleteReceipts>();

        void AddingStartupProducts()
        {
            var newProduct = new Product(freeId, "Morötter", 19, Enhet.Kilopris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);

            freeId = GetFreeId();
            newProduct = new Product(freeId, "Hundmat", 499, Enhet.Kilopris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);
                
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Plastfolie", 35, Enhet.Styckpris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);

            freeId = GetFreeId();
            newProduct = new Product(freeId, "Kexchoklad", 10, Enhet.Styckpris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);

            freeId = GetFreeId();
            newProduct = new Product(freeId, "Bilbatteri", 899, Enhet.Styckpris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);

            freeId = GetFreeId();
            newProduct = new Product(freeId, "Klädnypor", 29, Enhet.Styckpris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);

            freeId = GetFreeId();
            newProduct = new Product(freeId, "Apelsiner", 33, Enhet.Kilopris);
            productList.Add(newProduct);
            SaveProductToFile(newProduct);
        }

        void MainMenu()
        {
            Console.WriteLine("***KASSASYSTEMET***\n");
            Console.WriteLine("1. Adminmeny");
            Console.WriteLine("2. Ta emot kunder");
            Console.WriteLine("3. Se produktlista");
            Console.WriteLine("4. Kontrollera kvitton\n");
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
                    Cashier();
                    break;

                case 3:
                    ShowProductList();
                    break;
                case 4:
                    Console.Clear();
                    Console.Write("Kvittonummer: ");
                    var nummer = int.Parse(Console.ReadLine());
                    Showcompletereceipts(nummer);
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
            Console.WriteLine("2. Redigera befintlig produkt");
            Console.WriteLine("3. Lägg till ny kampanj");
            Console.WriteLine("4. Ta bort kampanj");
            Console.WriteLine("5. Visa aktiva kampanjer");
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
                    Console.Clear();
                    Console.Write("Artikelnummer som kampanjen ska appliceras på: ");
                    var artnr = int.Parse(Console.ReadLine());
                    AddCampaign(artnr);
                    Console.Clear();
                    break;
                case 4:
                    Console.Clear();
                    RemoveCampaign();
                    Console.Clear();
                    MainMenu();
                    break;
                case 5:
                    ActiveCampaigns();
                    Console.WriteLine("Tryck på valfri tangent för att återgå till huvudmenyn.");
                    Console.ReadKey();
                    Console.Clear();
                    MainMenu();
                    break;

                case 9:
                    Console.Clear();
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

        static int GetFreeCampaignId()
        {
            if (productsOnCampaignList.Any(p => p.CampaignId() > 0))
            {
                return productsOnCampaignList.Max(p => p.CampaignId()) + 1;
            }
            else
            {
                return 1;
            }
        }

        Enhet GetEnhet()
        {
            Console.Write($"Prisenhet? {Enhet.Kilopris} eller {Enhet.Styckpris}: ");
            string input = Console.ReadLine().ToLower();
            return input.Contains("styck") ? Enhet.Styckpris :
                   input.Contains("kilo") ? Enhet.Kilopris : Enhet.Styckpris;
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
            Console.WriteLine("KASSA");
            Console.WriteLine($"KVITTO\t{GetDateNow()}");
            string fullString = "";
            string checkInput = "";
            decimal totalSum = 0;
            paymentNumber = GetDateNow();
            while (true)
            {
                Console.Write("\nAnge produktkod mellanslag antal: ");
                checkInput = Console.ReadLine();
                if (IsTheInputCorrect(checkInput, totalSum))
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

                var name = GetTheName(produktKodCashier);
                int tryProductCode = Convert.ToInt32(produktKodCashier);
                var eachPrice = CheckCampaign(produktKodCashier);
                var payment = (GetTheSum(produktKodCashier, quantityCashier));
                totalSum +=payment;


                if (!productList.Any(product => product.GetId() == tryProductCode))
                {
                    Console.WriteLine("FELAKTIG INMATNING");
                    Cashier();
                }
                
                var newReceiptLine = new Receipt(paymentNumber, produktKodCashier, name, quantityCashier, eachPrice, payment, totalSum);
                receiptList.Add(newReceiptLine);
                Console.Clear();
                Console.WriteLine("KASSA");
                Console.WriteLine($"KVITTO\t{GetDateNow()}");

                foreach (var receiptLines in receiptList)
                {
                    Console.WriteLine($"{receiptLines.GetProductName()} {receiptLines.GetQuantityCashier()} * {receiptLines.GetEachPrice().ToString("N2")} = {receiptLines.GetPayment().ToString("N2")}");
                }
                Console.Write($"\nTotal: {totalSum.ToString("N2")} kr\n");
            }
            
        }

        bool IsTheInputCorrect(string checkInput, decimal totalSum)
        {
            if (checkInput.ToUpper() == "PAY")
            {
                Receipt(totalSum);
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
            var theSum = CheckCampaign(produktKodCashier) * quantity;
            return theSum;
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

        void Showcompletereceipts(int receiptnumber)
        {
            Console.WriteLine();
            //Console.WriteLine("------------------------------------------------");
            //Console.WriteLine(" Kvittonr |    Datum      |   Pris   | Enhet     ");
            //Console.WriteLine("------------------------------------------------");
            foreach (var showList in ListOfAllReceipts)
            {
                if (showList.GetKvittoNummer() == receiptnumber)
                {
                    Console.WriteLine($"{showList.GetKvittoNummer()} {showList.GetRecieptNumber()} {showList.GetTotalSum()} kr");
                    foreach (var lines in showList.GetList())
                    {
                        Console.WriteLine($"{lines.GetProductName()} {lines.GetPayment()} {lines.GetTotalSum()}");
                    }
                }
            }
            Console.Write("\nTryck valfri tangent för att återgå till huvudmenyn.");
            Console.ReadLine();
        }

        DateTime GetDateNow()
        {
            var dateNow = DateTime.Now;
            return dateNow;
        }

        void Receipt(decimal totalSum)
        {
            receiptNumber++;
            completeReceiptList.AddRange(receiptList);
            
            var completeReceipt = new CompleteReceipts(receiptNumber, paymentNumber, totalSum, receiptList);

            ListOfAllReceipts.Add(completeReceipt);
            receiptList.Clear();
            Console.Clear();
            MainMenu();
        }

        void AddCampaign(int produktKodCashier)
        {
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine(" ID  | Produktnamn    | Ordinariepris  | Enhet          ");
            Console.WriteLine("--------------------------------------------------------");
            foreach (var showProductList in productList)
            {
                if (produktKodCashier == showProductList.GetId())
                {
                    Console.Write(String.Format("{0,-4} | {1,-14} | {2,-14} | {3,5}",
                                        " " + showProductList.GetId(), showProductList.GetProductName(), showProductList.GetPrice() + " kr", showProductList.GetEnhet() + "\n"));
                }
            }
            Console.WriteLine("--------------------------------------------------------\n");
            Console.Write("Kampanjens startdatum (YYYY-MM-DD): ");
            string dateString = Console.ReadLine();
            DateOnly startingDate = DateOnly.Parse(dateString);

            Console.Write("Kampanjens slutdatum  (YYYY-MM-DD): ");
            dateString = Console.ReadLine();
            DateOnly endingDate = DateOnly.Parse(dateString);

            Console.Write("Kampanjpris: ");
            decimal campaingPrice = decimal.Parse(Console.ReadLine());

            freeCampaignId= GetFreeCampaignId();

            var campaignProduct = new Campaign(freeCampaignId, produktKodCashier, campaingPrice, startingDate, endingDate);
            productsOnCampaignList.Add(campaignProduct);

            Console.WriteLine($"\nPriset ändrat till {campaingPrice} kr.\n");
            Console.WriteLine("Tryck valfri tangent för att återgå till huvudmenyn.");
            Console.ReadLine();
            Console.Clear();
            MainMenu();
        }

        decimal CheckCampaign(int produktKodCashier)
        {

            var today = DateOnly.FromDateTime(DateTime.Now);
            foreach (var c in productsOnCampaignList)
            {
                if (c.GetCampaignProductId() == produktKodCashier && today > c.GetStartDate() && today < c.GetEndDate())
                {
                    return c.GetCampaignPrice(); 
                }
            }
            return GetThePrice(produktKodCashier);
        }
        void ActiveCampaigns()
        {
            Console.Clear();
            Console.WriteLine("AKTIVA KAMPANJER:\n");
            foreach (var items in productsOnCampaignList)
            {
                Console.WriteLine($"KampanjID:   {items.CampaignId()}\nProduktkod:  {items.GetCampaignProductId()}\nKampanjpris: {items.GetCampaignPrice()}\nStartdatum:  {items.GetStartDate()}\nSlutdatum:   {items.GetEndDate()}\n");
            }
            if (productsOnCampaignList.Count() == 0)
            {
                Console.WriteLine("INGA AKTIVA KAMPANJER!\n");
            }
        }

        void RemoveCampaign()
        {
            if (productsOnCampaignList.Count() == 0)
            {
                Console.WriteLine("INGA AKTIVA KAMPANJER!\n");
                Console.WriteLine("Tryck valfri tangent för att återgå till huvudmenyn.");
                Console.ReadKey();
                return;
            }
            ActiveCampaigns();
            Console.Write("Mata in kampanj-ID på den kampanj som ska tas bort: ");
            var remove = int.Parse(Console.ReadLine());
            for (int i = productsOnCampaignList.Count - 1; i >= 0; i--)
            {
                var p = productsOnCampaignList[i];
                if (p.CampaignId() == remove)
                {
                    productsOnCampaignList.RemoveAt(i);
                }
            }
            ActiveCampaigns();
            Console.WriteLine("Tryck valfri tangent för att återgå till huvudmenyn.");
            Console.ReadKey();
        }
    }
}