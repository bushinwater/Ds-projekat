using System;
using System.Data;
using Ds_projekat;

namespace Ds_projekat
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== POKRETANJE TESTA KONEKCIJE ===");

            try
            {
                // 1. Provera da li uopšte vidi Connector
                var connector = Connector.Instance;
                Console.WriteLine("[1/3] Singleton instanca: OK");
                Console.WriteLine("[2/3] Brend iz fajla: " + connector.BrandName);

                // 2. Kreiranje i otvaranje konekcije
                using (IDbConnection veza = connector.CreateConnection())
                {
                    Console.WriteLine("[3/3] Otvaram konekciju ka bazi... (sačekaj)");
                    veza.Open();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n>>> USPEŠNO POVEZANO SA BAZOM! <<<");
                    Console.ResetColor();

                    veza.Close();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n>>> GREŠKA PRILIKOM TESTA <<<");
                Console.WriteLine("Poruka: " + ex.Message);
                if (ex.InnerException != null)
                    Console.WriteLine("Detalji: " + ex.InnerException.Message);
                Console.ResetColor();
            }

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine("Test završen. Pritisni ENTER za izlaz...");
            Console.ReadLine(); // Ovo će držati prozor otvorenim!
        }
    }
}