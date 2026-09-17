bool continuer = true;

while (continuer)
{
    Console.WriteLine("GestCredit");
    Console.WriteLine("1. Option 1");
    Console.WriteLine("2. Option 2");
    Console.WriteLine("3. Option 3");
    Console.WriteLine("4. Quitter");
    Console.Write("Votre choix : ");

    string? saisie = Console.ReadLine();

    switch (saisie)
    {
        case "1":
            Console.WriteLine("Vous avez choisi l'option 1");
            break;
        case "2":
            Console.WriteLine("Vous avez choisi l'option 2");
            break;
        case "3":
            Console.WriteLine("Vous avez choisi l'option 3");
            break;
        case "4":
            continuer = false;
            Console.WriteLine("Au revoir !");
            break;
        default:
            Console.WriteLine("Saisie invalide, veuillez réessayer.");
            break;
    }
}