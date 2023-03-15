using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace KassaSystemet2._0
{
    public class App
    {
        Files files = new();
        InputControls inputControls = new();
        Campaigns campaigns = new();
        public void Run()
        {
            AddingStartupProducts();
            files.AddFilesIfNotExisting();
            files.Load();
            MainMenu();
        }
        
        static DateTime paymentNumber;
        static List<Product> productList = new List<Product>();
        List<Receipt> receiptList = new List<Receipt>();
        List<Receipt> completeReceiptList = new List<Receipt>();
        static List<CompleteReceipts> ListOfAllReceipts = new List<CompleteReceipts>();
        int freeId = GetFreeId();
        static int receiptNumber = 0;
        bool cashierDone = false;

        void AddingStartupProducts()
        {
            var newProduct = new Product(freeId, "Morötter", 19.90M, Enhet.Kilopris);
            productList.Add(newProduct);
            
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Hundmat", 499.99M, Enhet.Kilopris);
            productList.Add(newProduct);
                            
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Plastfolie", 34.90M, Enhet.Styckpris);
            productList.Add(newProduct);
            
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Kexchoklad", 9.90M, Enhet.Styckpris);
            productList.Add(newProduct);
            
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Bilbatteri", 899.00M, Enhet.Styckpris);
            productList.Add(newProduct);
            
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Klädnypor", 29.90M, Enhet.Styckpris);
            productList.Add(newProduct);
            
            freeId = GetFreeId();
            newProduct = new Product(freeId, "Apelsiner", 33.45M, Enhet.Kilopris);
            productList.Add(newProduct);
        }
                
        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("***KASSASYSTEMET***\n");
                Console.WriteLine("1. Adminmeny");
                Console.WriteLine("2. Kassan");
                Console.WriteLine("3. Kontrollera kvitton\n");
                Console.WriteLine("9. AVSLUTA\n");

                switch (inputControls.ReadIntForMainMeny())
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
                        Console.Clear();
                        Console.WriteLine(files.Load());
                        Console.Write("Tryck valfri tangent för att återgå till föregående meny.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 9:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        void AdminMenu()
        {
            bool breakTheLoop = false;
            while (breakTheLoop != true)
            {
                Console.WriteLine("***KASSASYSTEMET ADMINMENY***\n");
                Console.WriteLine("   PRODUKTER");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("1. Lägg till ny produkt");
                Console.WriteLine("2. Redigera befintlig produkt\n");
                Console.WriteLine("   KAMPANJER");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("3. Lägg till ny kampanj");
                Console.WriteLine("4. Ta bort kampanj");
                Console.WriteLine("5. Visa aktiva kampanjer\n");
                Console.WriteLine("   ÅTERGÅ");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("9. Huvudmeny\n");

                switch (inputControls.ReadIntForAdminMeny())
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
                        campaigns.AddCampaign(productList);
                        Console.Clear();
                        break;
                    case 4:
                        Console.Clear();
                        campaigns.RemoveCampaign();
                        Console.Clear();
                        break;
                    case 5:
                        campaigns.ActiveCampaigns();
                        Console.WriteLine("Tryck på valfri tangent för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case 9:
                        Console.Clear();
                        breakTheLoop = true;
                        break;
                }
            }
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
        
        public Enhet GetEnhet()
        {
            bool inputOK = false;
            Enhet enhet = Enhet.Styckpris;

            while (!inputOK)
            {
                Console.Write($"Prisenhet? {Enhet.Kilopris} eller {Enhet.Styckpris}: ");
                string input = Console.ReadLine().ToLower();

                if (input.Contains("kilo") || input.Contains("kg"))
                {
                    enhet = Enhet.Kilopris;
                    inputOK = true;
                }
                else if (input.Contains("st"))
                {
                    enhet = Enhet.Styckpris;
                    inputOK = true;
                }
                else
                {
                    Console.Write("Ogiltig enhet. ");
                }
            }

            return enhet;
        }

        void EditProduct()
        {
            Console.Clear();
            Console.Write($"Ange produkt-ID som du vill ändra (1-{GetHighestProductID()}): ");
            var inputID = CheckInt();
            while (inputID < 1 || inputID > GetHighestProductID())
            {
                if (inputID < 1 || inputID > GetHighestProductID())
                {
                    Console.Write($"Felaktig inmating. Produkt-ID måste vara mellan 1-{GetHighestProductID()}: ");
                    inputID = CheckInt();
                }
            }
                        
            foreach (var findItem in productList)
            {
                if (inputID == findItem.GetId())
                {
                    Console.WriteLine($"\nID: {findItem.GetId()}\nNAMN: {findItem.GetProductName()}\nPRIS: {findItem.GetPrice()} kr\n");
                }
            }

            Console.WriteLine("Vad vill du ändra? \n1. Namn\n2. Pris\n\n3.Återgå\n");
            var inputChoice = CheckInt();

            if (inputChoice < 1 || inputChoice > 3)
            {
                while (inputChoice < 1 || inputChoice > 3)
                {
                    Console.Write("Felaktig inmatning. Välj 1, 2 eller 3: ");
                    inputChoice = CheckInt();
                }
            }

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
                decimal price = CheckDecimalInput();
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
            paymentNumber = GetDateNow();
            Console.WriteLine("KASSA");
            Console.WriteLine($"KVITTO\t{paymentNumber}");
            string? checkInput = "";
            decimal totalSum = 0;
                        
            while (true)
            {
                Console.Write("\nAnge produktkod mellanslag antal: ");
                checkInput = Console.ReadLine();
                (int produktKodCashier, int quantityCashier) = GetTwoIntegers(checkInput, totalSum);
                var name = GetTheName(produktKodCashier);
                var eachPrice = CheckCampaign(produktKodCashier);
                var payment = GetTheSum(produktKodCashier, quantityCashier);
                totalSum += payment;

                var newReceiptLine = new Receipt(paymentNumber, produktKodCashier, name, quantityCashier, eachPrice, payment, totalSum);
                receiptList.Add(newReceiptLine);
                Console.Clear();
                Console.WriteLine("KASSA");
                Console.WriteLine($"KVITTO\t{paymentNumber}");

                foreach (var receiptLines in receiptList)
                {
                    Console.WriteLine($"{receiptLines.GetProductName()} {receiptLines.GetQuantityCashier()} * {receiptLines.GetEachPrice().ToString("N2")} = {receiptLines.GetPayment().ToString("N2")}");
                }
                Console.Write($"\nTotal: {totalSum.ToString("N2")} kr\n");
            }
        }

        void SaveKvittonummerToFile(CompleteReceipts completeReceipts, DateTime dateTime, decimal totalSum)
        {
            int receiptNumberFromFile = 0;

            var srRecNr = new StreamReader("ReceiptCounter.txt");
            using (srRecNr)
            {
                if (srRecNr.Peek() == -1)
                {
                    receiptNumberFromFile = 0;
                }
                else
                {
                    receiptNumberFromFile = int.Parse(srRecNr.ReadToEnd())+1;
                }
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

        void SaveReceiptLinesToFile(Receipt receipt)
        {
            var sw = new StreamWriter("ReceiptFile.txt", true);
            using (sw)
            {
                sw.WriteLine($"{receipt.GetProductName()} {receipt.GetQuantityCashier()} * {receipt.GetEachPrice().ToString("N2")} = {receipt.GetPayment().ToString("N2")}");
            }
        }

        (int, int) GetTwoIntegers(string checkInput, decimal totalSum)
        {
            bool validInput = false;
            int integer1 = 0;
            int integer2 = 0;

            while (!validInput)
            {
                if (checkInput.ToUpper() == "PAY" && receiptList.Count > 0)
                {
                    Receipt(totalSum);
                }
                if (!checkInput.Contains(" "))
                {
                    Console.WriteLine("\nFELAKTIG INMATNING");
                    Console.Write("\nAnge produktkod mellanslag antal: ");
                    checkInput = Console.ReadLine();
                    continue;
                }

                int mellanslagsIndex = checkInput.IndexOf(" ");
                string inputDigits1 = checkInput[..mellanslagsIndex];
                string inputDigits2 = checkInput[(mellanslagsIndex + 1)..];

                if (!int.TryParse(inputDigits1, out integer1) || !int.TryParse(inputDigits2, out integer2))
                {
                    Console.WriteLine("\nFELAKTIG INMATNING");
                    Console.Write("\nAnge produktkod mellanslag antal: ");
                    checkInput = Console.ReadLine();
                    continue;
                }
                
                integer1 = int.Parse(inputDigits1);
                integer2 = int.Parse(inputDigits2);

                if (!productList.Any(product => product.GetId() == integer1))
                {
                    Console.WriteLine("\nFELAKTIG INMATNING");
                    Console.Write("\nAnge produktkod mellanslag antal: ");
                    checkInput = Console.ReadLine();
                    continue; 
                }

                validInput = true;
            }

            return (integer1, integer2);
        }

        public decimal GetTheSum(int produktKodCashier, int quantity)
        {
            var theSum = campaigns.CheckCampaign(produktKodCashier) * quantity;
            return theSum;
        }

        public decimal GetThePrice(int produktKodCashier)
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

        public string GetTheName(int produktKodCashier)
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
                
        public int GetHighestProductID()
        {
            int highestProduktID = 0;
            foreach (var ID in productList)
            {
                int currentProduktID = ID.GetId();

                if (currentProduktID > highestProduktID)
                {
                    highestProduktID = currentProduktID;
                }
            }
            return highestProduktID;
        }

        public DateTime GetDateNow()
        {
            var dateNow = DateTime.Now;
            return dateNow;
        }

        void AddProductByUser()
        {
            while (true)
            {
                Console.Write("***LÄGG TILL NY PRODUKT***\n\nProduktnamn: ");
                string? productName = Console.ReadLine();
                Console.Write("Pris: ");
                decimal price = inputControls.CheckDecimalInput();
                var enheten = GetEnhet();
                var freeId = GetFreeId();
                var newProduct = new Product(freeId, productName, price, enheten);
                productList.Add(newProduct);
                Console.WriteLine("\nPRODUKTEN SPARAD. ÅTER TILL HUVUDMENYN\n");
                break;
            }
        }
        
        void EditProduct()
        {
            while (true)
            {
                Console.Clear();
                Console.Write($"Ange produkt-ID som du vill ändra (1-{GetHighestProductID()}): ");
                var inputID = inputControls.CheckInt();
                while (inputID < 1 || inputID > GetHighestProductID())
                {
                    if (inputID < 1 || inputID > GetHighestProductID())
                    {
                        Console.Write($"Felaktig inmating. Produkt-ID måste vara mellan 1-{GetHighestProductID()}: ");
                        inputID = inputControls.CheckInt();
                    }
                }

                foreach (var findItem in productList)
                {
                    if (inputID == findItem.GetId())
                    {
                        Console.WriteLine($"\nID: {findItem.GetId()}\nNAMN: {findItem.GetProductName()}\nPRIS: {findItem.GetPrice()} kr\n");
                    }
                }

                Console.WriteLine("Vad vill du ändra? \n1. Namn\n2. Pris\n\n3.Återgå\n");
                var inputChoice = inputControls.CheckInt();

                if (inputChoice < 1 || inputChoice > 3)
                {
                    while (inputChoice < 1 || inputChoice > 3)
                    {
                        Console.Write("Felaktig inmatning. Välj 1, 2 eller 3: ");
                        inputChoice = inputControls.CheckInt();
                    }
                }

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
                            break;
                        }
                    }
                }
                else if (inputChoice == 2)
                {
                    Console.Write("Nytt pris: ");
                    decimal price = inputControls.CheckDecimalInput();
                    foreach (var product in productList)
                    {
                        if (inputID == product.GetId())
                        {
                            product.SetProductPrice(price);
                            Console.Clear();
                            break;
                        }
                    }
                }
                else if (inputChoice == 3)
                {
                    Console.Clear();
                    break;
                }
                break;
            }
        }

        void Cashier()
        {
            paymentNumber = GetDateNow();
            Console.WriteLine("KASSA");
            Console.WriteLine($"KVITTO\t{paymentNumber}");
            string? checkInput = "";
            decimal totalSum = 0;
            
            while (cashierDone != true)
            {
                Console.Write("\nAnge produktkod mellanslag antal: ");
                checkInput = Console.ReadLine();
                (int produktKodCashier, int quantityCashier) = GetTwoIntegers(checkInput, totalSum);
                var name = GetTheName(produktKodCashier);
                var eachPrice = campaigns.CheckCampaign(produktKodCashier);
                var payment = GetTheSum(produktKodCashier, quantityCashier);
                totalSum += payment;

                var newReceiptLine = new Receipt(paymentNumber, produktKodCashier, name, quantityCashier, eachPrice, payment, totalSum);
                receiptList.Add(newReceiptLine);
                Console.Clear();
                Console.WriteLine("KASSA");
                Console.WriteLine($"KVITTO\t{paymentNumber}");

                foreach (var receiptLines in receiptList)
                {
                    Console.WriteLine($"{receiptLines.GetProductName()} {receiptLines.GetQuantityCashier()} * {receiptLines.GetEachPrice().ToString("N2")} = {receiptLines.GetPayment().ToString("N2")}");
                }
                Console.Write($"\nTotal: {totalSum.ToString("N2")} kr\n");
            }
        }

        bool Receipt(decimal totalSum)
        {
            while (true)
            {
                receiptNumber++;
                completeReceiptList.AddRange(receiptList);
                var completeReceipt = new CompleteReceipts(receiptNumber, paymentNumber, totalSum, receiptList);
                files.SaveKvittonummerToFile(completeReceipt, GetDateNow(), totalSum, receiptList);
                ListOfAllReceipts.Add(completeReceipt);
                receiptList.Clear();
                Console.Clear();
                return cashierDone = true;                
            }
        }
    }
}