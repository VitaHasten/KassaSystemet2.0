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
            List<Product> productList = new List<Product>();
            var freeId = GetFreeId();
            int val = 0;

            AddingStartupProducts();
            MainMenu();



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
                Console.WriteLine("1. Lägg till ny produkt");
                Console.WriteLine("2. Ta emot kunder\n");
                Console.WriteLine("9. AVSLUTA\n");

                val = int.Parse(Console.ReadLine());
                switch (val)
                {
                    case 1:
                        Console.Clear();
                        AddProductByUser();
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("------------------------------------------------");
                        Console.WriteLine(" ID  | Produktnamn       |   Pris   | Enhet     ");
                        Console.WriteLine("------------------------------------------------");
                        foreach (var showProductList in productList)
                        {
                            Console.Write(String.Format("{0,-4} | {1,-17} | {2,8} | {3,5}",
                                " " + showProductList.GetId(), showProductList.GetProductName(), showProductList.GetPrice()+" kr", showProductList.GetEnhet()+"\n"));
                        }
                        Console.WriteLine("------------------------------------------------\n\nTryck ENTER för att återgå till huvudmenyn.");
                        Console.ReadLine();
                        Console.Clear();
                        MainMenu();
                        break;

                    case 9:
                        Environment.Exit(0);
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

            int GetFreeId()
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
                string input = Console.ReadLine();
                return input switch
                {
                    "Styckpris" => enhet.Styckpris,
                    "Kilopris" => enhet.Kilopris,
                };
            }

            void SaveProductToFile(Product product)
            {
                using (StreamWriter writer = new StreamWriter("Produktfil.txt", true))
                {
                    writer.WriteLine(product.ToString());
                }
            }
        }
    }
}
