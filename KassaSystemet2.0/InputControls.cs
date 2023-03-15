using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KassaSystemet2._0
{
    public class InputControls
    {
        public DateOnly CheckDateInput(string dateString)
        {
            DateOnly startingDate = new DateOnly();
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

        public int ReadIntForMainMeny()
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

        public int ReadIntForAdminMeny()
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

        public decimal CheckDecimalInput()
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

        public int CheckInt()
        {
            int heltal = 0;
            while (int.TryParse(Console.ReadLine(), out heltal) == false)
            {
                Console.Write("Du skrev inte in ett heltal. Försök igen: ");
            }
            return heltal;
        }

        public InputControls()
        {

        }
    }
}
