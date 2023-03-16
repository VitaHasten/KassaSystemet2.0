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
        public void Run()
        {
            AddingStartupProducts();
            AddFilesIfNotExisting();
            Load();
            MainMenu();
        }
        
        static DateTime paymentNumber;
        static List<Product> productList = new List<Product>();
        List<Receipt> receiptList = new List<Receipt>();
        List<Receipt> completeReceiptList = new List<Receipt>();
        static List<Campaign> productsOnCampaignList = new List<Campaign>();
        int freeId = GetFreeId();
        int freeCampaignId = GetFreeCampaignId();
        static int receiptNumber = 0;
        static List<CompleteReceipts> ListOfAllReceipts = new List<CompleteReceipts>();
        bool goToMainMenu = false;
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

        void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("***KASSASYSTEMET***\n");
                Console.WriteLine("1. Adminmeny");
                Console.WriteLine("2. Kassan");
                Console.WriteLine("3. Kontrollera kvitton\n");
                Console.WriteLine("9. AVSLUTA\n");
                        
                switch (ReadIntForMainMeny())
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
                        Console.WriteLine(Load());
                        Console.WriteLine("Tryck valfri tangent för att återgå till föregående meny.");
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
            while (goToMainMenu != true)
            {
                Console.Clear();      
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

                switch (ReadIntForAdminMeny())
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
                        AddCampaign();
                        break;
                    case 4:
                        Console.Clear();
                        RemoveCampaign();
                        break;
                    case 5:
                        ActiveCampaigns();
                        Console.WriteLine("Tryck på valfri tangent för att återgå till huvudmenyn.");
                        Console.ReadKey();
                        break;

                    case 9:
                        Console.Clear();
                        goToMainMenu = true;
                        break;
                }
            }
        }

        void AddProductByUser()
        {
            while (true)
            {
                Console.Write("***LÄGG TILL NY PRODUKT***\n\nProduktnamn: ");
                string? productName = Console.ReadLine();
                Console.Write("Pris: ");
                decimal price =CheckDecimalInput();
                var enheten =GetEnhet();
                var freeId = GetFreeId();
                var newProduct = new Product(freeId, productName, price, enheten);
                productList.Add(newProduct);
                Console.WriteLine("\nPRODUKTEN SPARAD. ÅTER TILL HUVUDMENYN\n");
                break;
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
            while (true)
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
                            break;
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
                
        int GetHighestProductID()
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

        DateTime GetDateNow()
        {
            var dateNow = DateTime.Now;
            return dateNow;
        }

        bool Receipt(decimal totalSum)
        {
            while (true)
            {
                receiptNumber++;
                completeReceiptList.AddRange(receiptList);
                var completeReceipt = new CompleteReceipts(receiptNumber, paymentNumber, totalSum, receiptList);
                SaveKvittonummerToFile(completeReceipt, GetDateNow(), totalSum);
                ListOfAllReceipts.Add(completeReceipt);
                receiptList.Clear();
                Console.Clear();
                cashierDone = true;
                return cashierDone;                
            }
        }

        void AddCampaign()
        {
            while (true)
            {
                Console.Write($"Artikelnummer som kampanjen ska appliceras på (1-{GetHighestProductID()}): ");

                var produktKodCashier = CheckInt();

                while (produktKodCashier < 1 || produktKodCashier > GetHighestProductID())
                {
                    if (produktKodCashier < 1 || produktKodCashier > GetHighestProductID())
                    {
                        Console.Write($"\nFelaktigt produkt-ID. Mata in ett produkt-ID mellan 1 - {GetHighestProductID()}: ");
                        produktKodCashier = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                    }
                    Console.Clear();
                }
                DateTime todayDT = GetDateNow();
                DateOnly today = new DateOnly(todayDT.Year, todayDT.Month, todayDT.Day);

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

                string dateString = "";
                DateOnly startingDate = CheckDateInput(dateString);

                while (startingDate < today)
                {
                    if (startingDate < today)
                    {
                        Console.Write("\nStartdatumet du matade in har redan passerat. Mata in dagens datum eller senare: ");
                        dateString = Console.ReadLine();
                        startingDate = DateOnly.Parse(dateString);
                        Console.WriteLine();
                    }
                }

                Console.Write("Kampanjens slutdatum  (YYYY-MM-DD): ");
                DateOnly endingDate = CheckDateInput(dateString);

                while (endingDate <= today)
                {
                    if (endingDate <= today)
                    {
                        Console.Write("\nSlutdatumet du matade in har redan passerat. Mata in ett korrekt slutdatum: ");
                        dateString = Console.ReadLine();
                        endingDate = DateOnly.Parse(dateString);
                        Console.WriteLine();
                    }
                }

                while (endingDate <= startingDate)
                {
                    if (endingDate <= startingDate)
                    {
                        Console.Write($"\nSlutdatumet du matade in inträffar innan kampanjen har startat. Mata in ett slutdatum efter {startingDate}: ");
                        dateString = Console.ReadLine();
                        endingDate = DateOnly.Parse(dateString);
                        Console.WriteLine();
                    }
                }

                Console.Write("Kampanjpris: ");
                decimal campaingPrice = decimal.Parse(Console.ReadLine());

                freeCampaignId = GetFreeCampaignId();

                var campaignProduct = new Campaign(freeCampaignId, produktKodCashier, campaingPrice, startingDate, endingDate);
                productsOnCampaignList.Add(campaignProduct);

                Console.WriteLine($"\nPriset ändrat till {campaingPrice} kr.\n");
                Console.WriteLine("Tryck valfri tangent för att återgå till huvudmenyn.");
                Console.ReadLine();
                Console.Clear();
                break;
            }
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
            while (true)
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
                break;
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
                var remove = CheckInt();

                for (int i = productsOnCampaignList.Count - 1; i >= 0; i--)
                {
                    var p = productsOnCampaignList[i];
                    if (p.CampaignId() == remove)
                    {
                        productsOnCampaignList.RemoveAt(i);
                        Console.WriteLine("\nKampanjen borttagen, tryck valfri tangent för att återgå till huvudmenyn.");
                    }
                    else
                    {
                        Console.WriteLine("\nFelaktig inmatning. Du skickas åter till huvudmenyn.");
                    }
                }
                Console.ReadKey();
        }

        DateOnly CheckDateInput(string dateString)
        {
            DateOnly startingDate= new DateOnly();
            while (true)
            {
                dateString = Console.ReadLine();
                if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
                {
                    startingDate = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
                    return startingDate;
                }
                else
                {
                    Console.Write("\nFelaktig inmatning. Mata in datumet i formatet YYYY-MM-DD: ");
                }
            }
        }

        int ReadIntForMainMeny()
        {
            int val = 0;
            string exit = "";
            while (exit != "exit")
            {
                if (!int.TryParse(Console.ReadLine(), out val))
                {
                    Console.WriteLine("\nDu skrev inte in ett heltal. Försök igen.\n");
                    continue;
                }
                if (val != 1 && val != 2 && val != 3 && val != 4 && val != 9)
                {
                    Console.WriteLine("\nFelaktig inmatning. Ditt val måste vara 1, 2, 3, 4 eller 9.\n");
                    continue;
                }
                exit = "exit";
            }
            return val;
        }

        int ReadIntForAdminMeny()
        {
            int val = 0;
            string exit = "";
            while (exit != "exit")
            {
                if (!int.TryParse(Console.ReadLine(), out val))
                {
                    Console.WriteLine("\nDu skrev inte in ett heltal. Försök igen.\n");
                    continue;
                }
                if (val != 1 && val != 2 && val != 3 && val != 4 && val != 5 && val != 9)
                {
                    Console.WriteLine("\nFelaktig inmatning. Ditt val måste vara 1, 2, 3, 4, 5 eller 9.\n");
                    continue;
                }
                exit = "exit";
            }
            return val;
        }

        decimal CheckDecimalInput()
        {
            decimal result = 0;
            bool inputOK = false;
            while (!inputOK)
            {
                string inputString = Console.ReadLine();
                if (!decimal.TryParse(inputString, out result))
                {
                    Console.Write("Felaktig inmatning. Försök igen: ");
                }
                else if (result < 1)
                {
                    Console.Write("Priset kan aldrig vara lägre än 1 kr. Mata in korrekt pris: ");
                }
                else if (result > 9999)
                {
                    Console.Write("Ledningen har beslutat att inte sälja produkter till priser från 10 000 kr och uppåt. Mata in ett lägre pris: ");
                }
                else
                {
                    inputOK = true;
                }
            }
            return result;
        }

        int CheckInt()
        {
            int heltal = 0;
            while (int.TryParse(Console.ReadLine(), out heltal) == false)
            {
                Console.Write("Du skrev inte in ett heltal. Försök igen: ");
            }
            return heltal;
        }

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

        public void SaveKvittonummerToFile(CompleteReceipts completeReceipts, DateTime dateTime, decimal totalSum)
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
    }
}