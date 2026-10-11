using System;

string[] parkeringsplatser = new string[100];
bool körProgrammet = true;


while (körProgrammet)
{
    Console.WriteLine("================================");
    Console.WriteLine("        PRAGUE PARKING");
    Console.WriteLine("================================");
    Console.WriteLine();

    Console.WriteLine("1. Parkera fordon");
    Console.WriteLine("2. Sök efter fordon");
    Console.WriteLine("3. Flytta fordon");
    Console.WriteLine("4. Hämta ut fordon");
    Console.WriteLine("5. Visa parkeringen");
    Console.WriteLine("6. Avsluta");

    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine();

    Console.Write("Välj ett alternativ: ");
    string val = Console.ReadLine() ?? "";

    while (val != "1" && val != "2" && val != "3" &&
           val != "4" && val != "5" && val != "6")
    {
        Console.WriteLine();
        Console.WriteLine("Felaktigt val. Välj mellan 1 och 6.");
        Console.WriteLine();

        Console.Write("Välj ett alternativ: ");
        val = Console.ReadLine() ?? "";
    }

    Console.Clear();

    switch (val)
    {
        case "1":
            {
                Console.WriteLine("===== PARKERA FORDON =====");
                Console.WriteLine();

                Console.WriteLine("1. Bil");
                Console.WriteLine("2. Motorcykel");
                Console.WriteLine();

                Console.Write("Välj fordonstyp: ");
                val = Console.ReadLine() ?? "";

                while (val != "1" && val != "2")
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "Felaktig fordonstyp. Välj 1 för bil eller 2 för motorcykel."
                    );
                    Console.WriteLine();

                    Console.Write("Välj fordonstyp: ");
                    val = Console.ReadLine() ?? "";
                }

                string registreringsnummer = LäsRegistreringsnummer();

                int befintligPlats =
                    HittaFordon(parkeringsplatser, registreringsnummer);

                while (befintligPlats != -1)
                {
                    Console.WriteLine();
                    Console.WriteLine(
                        "Fordonet finns redan på plats " +
                        (befintligPlats + 1) +
                        ". Ange ett annat registreringsnummer."
                    );

                    registreringsnummer = LäsRegistreringsnummer();

                    befintligPlats =
                        HittaFordon(parkeringsplatser, registreringsnummer);
                }

                Console.WriteLine();

                if (val == "1")
                {
                    int plats =
                        ParkeraBil(parkeringsplatser, registreringsnummer);

                    if (plats == -1)
                    {
                        Console.WriteLine(
                            "Det finns ingen ledig plats för bilen."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            "Bilen har parkerats på plats " + (plats + 1)
                        );
                    }
                }
                else
                {
                    int plats =
                        ParkeraMotorcykel(parkeringsplatser, registreringsnummer);

                    if (plats == -1)
                    {
                        Console.WriteLine(
                            "Det finns ingen ledig plats för motorcykeln."
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            "Motorcykeln har parkerats på plats " + (plats + 1)
                        );
                    }
                }

                Console.WriteLine();
                break;
            }

        case "2":
            {
                Console.WriteLine("===== SÖK EFTER FORDON =====");

                string registreringsnummer = LäsRegistreringsnummer();

                int plats =
                    HittaFordon(parkeringsplatser, registreringsnummer);

                Console.WriteLine();

                if (plats == -1)
                {
                    Console.WriteLine("Fordonet kunde inte hittas.");
                }
                else
                {
                    Console.WriteLine(
                        "Fordonet står på parkeringsplats " + (plats + 1)
                    );
                }

                Console.WriteLine();
                break;
            }

        case "3":
            {
                Console.WriteLine("===== FLYTTA FORDON =====");

                string registreringsnummer = LäsRegistreringsnummer();

                int gammalPlats =
                    HittaFordon(parkeringsplatser, registreringsnummer);

                if (gammalPlats == -1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Fordonet kunde inte hittas.");
                    Console.WriteLine();
                    break;
                }

                Console.WriteLine();
                Console.WriteLine(
                    "Fordonet står på plats " + (gammalPlats + 1)
                );
                Console.WriteLine();

                // Hämta fordonet som ska flyttas
                string[] gamlaFordon =
                    parkeringsplatser[gammalPlats].Split('|');

                string fordonSomFlyttas = "";

                foreach (string fordon in gamlaFordon)
                {
                    if (fordon == "BIL#" + registreringsnummer ||
                        fordon == "MC#" + registreringsnummer)
                    {
                        fordonSomFlyttas = fordon;
                        break;
                    }
                }

                bool ärMotorcykel = fordonSomFlyttas[0] == 'M';

                int nyPlats;
                int nyttIndex;

                while (true)
                {
                    Console.Write("Ange ny parkeringsplats (1-100): ");

                    if (!int.TryParse(Console.ReadLine(), out nyPlats))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Du måste skriva ett heltal.");
                        Console.WriteLine();
                        continue;
                    }

                    if (nyPlats < 1 || nyPlats > 100)
                    {
                        Console.WriteLine();
                        Console.WriteLine(
                            "Platsen måste vara mellan 1 och 100."
                        );
                        Console.WriteLine();
                        continue;
                    }

                    nyttIndex = nyPlats - 1;

                    if (nyttIndex == gammalPlats)
                    {
                        Console.WriteLine();
                        Console.WriteLine(
                            "Fordonet står redan där. Välj en annan plats."
                        );
                        Console.WriteLine();
                        continue;
                    }
                    // Flytta fordonet om den nya platsen är tom.
                    if (parkeringsplatser[nyttIndex] == null)
                    {
                        parkeringsplatser[nyttIndex] = fordonSomFlyttas;
                        break;
                    }

                    string[] fordon =
                        parkeringsplatser[nyttIndex].Split('|');

                    // En motorcykel får dela plats med en ensam motorcykel.
                    if (ärMotorcykel &&
                        fordon[0][0] == 'M' &&
                        fordon.Length == 1)
                    {
                        parkeringsplatser[nyttIndex] =
                            parkeringsplatser[nyttIndex] +
                            "|" + fordonSomFlyttas;

                        break;
                    }

                    Console.WriteLine();
                    Console.WriteLine(
                        "Fordonet kan inte flyttas dit. Välj en annan plats."
                    );
                    Console.WriteLine();
                }


                if (gamlaFordon.Length == 1)
                {
                    parkeringsplatser[gammalPlats] = null;
                }
                else if (gamlaFordon[0] == fordonSomFlyttas)
                {
                    parkeringsplatser[gammalPlats] = gamlaFordon[1];
                }
                else
                {
                    parkeringsplatser[gammalPlats] = gamlaFordon[0];
                }
                // Skriv ut meddelande om att fordonet har flyttats
                Console.WriteLine();
                Console.WriteLine(
                    "Fordonet har flyttats från plats " +
                    (gammalPlats + 1) + " till plats " + nyPlats
                );
                Console.WriteLine();

                break;
            }

        case "4":
            {
                Console.WriteLine("===== HÄMTA UT FORDON =====");

                string registreringsnummer = LäsRegistreringsnummer();

                int plats =
                    HittaFordon(parkeringsplatser, registreringsnummer);

                if (plats == -1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Fordonet kunde inte hittas.");
                    Console.WriteLine();
                    break;
                }

                string[] fordon = parkeringsplatser[plats].Split('|');
                // Töm platsen om fordonet är det enda på platsen, annars ta bort fordonet från platsen
                if (fordon.Length == 1)
                {
                    parkeringsplatser[plats] = null;
                }
                else
                {
                    string fordonSomSkaVaraKvar = "";

                    foreach (string ettFordon in fordon)
                    {
                        if (ettFordon != "MC#" + registreringsnummer)
                        {
                            fordonSomSkaVaraKvar = ettFordon;
                        }
                    }

                    parkeringsplatser[plats] = fordonSomSkaVaraKvar;
                }

                Console.WriteLine();
                Console.WriteLine(
                    "Fordonet " + registreringsnummer +
                    " har hämtats från plats " + (plats + 1)
                );
                Console.WriteLine();

                break;
            }

        case "5":
            {
               
                Console.WriteLine("===== PARKERINGSÖVERSIKT =====");
                Console.WriteLine();

                for (int i = 0; i < 34; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {

                        if (i == 33 && j < 2)
                        {
                            Console.Write("{0,-39}", "");
                            continue;
                        }

                        int plats = i + j * 33;
                        string fordon = "LEDIG";
                        // Visar fordonet på platsen om den inte är tom
                        if (parkeringsplatser[plats] != null)
                        {
                            fordon = parkeringsplatser[plats];



                        }

                        string text =
                            "Plats " + (plats + 1) + ": " + fordon;

                        if (j < 2)
                        {
                            Console.Write("{0,-39}", text);
                        }
                        else
                        {
                            Console.Write(text);
                        }
                    }


                    Console.WriteLine();
                }

                Console.WriteLine();


                
                Console.WriteLine();
                break;
            }



        case "6":
            {
                körProgrammet = false;

                Console.WriteLine("Programmet avslutas.");
                Console.WriteLine();

                break;
            }
    }

}

// Metod 1: Läser ett registreringsnummer med högst 10 tecken
string LäsRegistreringsnummer()
{
    while (true)
    {
        Console.WriteLine();
        Console.Write("Ange registreringsnummer: ");

        string registreringsnummer = Console.ReadLine() ?? "";

        if (registreringsnummer.Length == 0 ||
            registreringsnummer.Length > 10)
        {
            Console.WriteLine();
            Console.WriteLine(
                "Registreringsnumret måste innehålla 1-10 tecken."
            );
            continue;
        }

        // "|" används för att skilja två MC åt i lagringen
        if (registreringsnummer.Contains("|"))
        {
            Console.WriteLine();
            Console.WriteLine(
                "Registreringsnumret får inte innehålla |."
            );
            continue;
        }

        return registreringsnummer;
    }
}

// Metod 2: Returnerar platsens index eller -1 om fordonet inte hittas
int HittaFordon(string[] parkeringsplatser, string registreringsnummer)
{
    for (int i = 0; i < parkeringsplatser.Length; i++)
    {
        if (parkeringsplatser[i] != null)
        {
            string[] fordon = parkeringsplatser[i].Split('|');

            foreach (string ettFordon in fordon)
            {
                if (ettFordon == "BIL#" + registreringsnummer ||
                    ettFordon == "MC#" + registreringsnummer)
                {
                    return i;
                }
            }
        }
    }

    return -1;
}

// Metod 3:Parkerar en bil på första tomma platsen
int ParkeraBil(string[] parkeringsplatser, string registreringsnummer)
{
    for (int i = 0; i < parkeringsplatser.Length; i++)
    {
        if (parkeringsplatser[i] == null)
        {
            parkeringsplatser[i] = "BIL#" + registreringsnummer;
            return i;
        }
    }

    return -1;
}

// Metod 4:Försöker först dela plats med en ensam motorcykel
int ParkeraMotorcykel(
    string[] parkeringsplatser,
    string registreringsnummer)
{
    for (int i = 0; i < parkeringsplatser.Length; i++)
    {
        if (parkeringsplatser[i] != null)
        {
            string[] fordon = parkeringsplatser[i].Split('|');

            if (fordon[0][0] == 'M' && fordon.Length == 1)
            {
                parkeringsplatser[i] =
                    parkeringsplatser[i] + "|MC#" + registreringsnummer;

                return i;
            }
        }
    }

    // Om inte möjligt, används en tom plats istället
    for (int i = 0; i < parkeringsplatser.Length; i++)
    {
        if (parkeringsplatser[i] == null)
        {
            parkeringsplatser[i] = "MC#" + registreringsnummer;
            return i;
        }
    }

    return -1;
}