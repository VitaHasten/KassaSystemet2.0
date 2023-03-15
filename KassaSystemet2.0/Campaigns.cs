using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class Campaigns
    {
        InputControls inputControls = new();
        static List<Campaign> productsOnCampaignList = new List<Campaign>();
        int freeCampaignId = GetFreeCampaignId();
        public void AddCampaign(List<Product> productList)
        {
            while (true)
            {
                Console.Write($"Artikelnummer som kampanjen ska appliceras på (1-{GetHighestProductID()}): ");

                var produktKodCashier = inputControls.CheckInt();

                while (produktKodCashier < 1 || produktKodCashier > app.GetHighestProductID())
                {
                    if (produktKodCashier < 1 || produktKodCashier > app.GetHighestProductID())
                    {
                        Console.Write($"\nFelaktigt produkt-ID. Mata in ett produkt-ID mellan 1 - {app.GetHighestProductID()}: ");
                        produktKodCashier = int.Parse(Console.ReadLine());
                        Console.WriteLine();
                    }
                    Console.Clear();
                }
                DateTime todayDT = app.GetDateNow();
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
                DateOnly startingDate = inputControls.CheckDateInput(dateString);

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
                DateOnly endingDate = inputControls.CheckDateInput(dateString);

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

        public decimal CheckCampaign(int produktKodCashier)
        {

            var today = DateOnly.FromDateTime(DateTime.Now);
            foreach (var c in productsOnCampaignList)
            {
                if (c.GetCampaignProductId() == produktKodCashier && today > c.GetStartDate() && today < c.GetEndDate())
                {
                    return c.GetCampaignPrice();
                }
            }
            return app.GetThePrice(produktKodCashier);
        }
        
        public void ActiveCampaigns()
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

        public void RemoveCampaign()
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
            var remove = inputControls.CheckInt();

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
    }
}
