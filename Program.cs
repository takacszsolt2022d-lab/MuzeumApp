
using MuzeumApp;

List<Mutargy> lista = new List<Mutargy>();
int hibassorokszama = 0;

foreach (string sor in File.ReadAllLines("mutargyak.txt").Skip(1))
{
    if (string.IsNullOrWhiteSpace(sor))
    {
        hibassorokszama++;
        continue;
    }

    string[] sorok = sor.Split('|');

    if (sorok.Length != 4)
    {
        hibassorokszama++;
        continue;
    }

    string tipus = sorok[0].Trim().ToUpper();
    string azon = sorok[1].Trim();

    if (string.IsNullOrWhiteSpace(azon))
    {
        hibassorokszama++;
        continue;
    }

    if (tipus != "M" && tipus != "F")
    {
        hibassorokszama++;
        continue;
    }

    if (!int.TryParse(sorok[2].Trim(), out int ertek))
    {
        hibassorokszama++;
        continue;
    }

    if (!int.TryParse(sorok[3].Trim(), out int extra))
    {
        hibassorokszama++;
        continue;
    }

    if (tipus == "F")
    {
        lista.Add(new Festmeny(azon, ertek, extra));
    }
    else if (tipus == "M")
    {
        lista.Add(new Mutargy(azon, ertek));
    }
}

Console.WriteLine($"Sikeresen beolvasott sorok száma: {lista.Count} db");
Console.WriteLine($"Hibás sorok száma: {hibassorokszama} db");


foreach (var kiir in lista)
{
    Console.WriteLine(kiir);
}

Console.WriteLine("\nLINQ statisztikák:");

double atlag = lista.Average(m => m.BecsultErtek);
Console.WriteLine($"A mutárgyak átlagos értéke: {atlag} Ft");

Mutargy legertekesebb = lista.MaxBy(m => m.BecsultErtek);
Console.WriteLine($"Legértékesebb műtárgy:   {legertekesebb.Azonosito}  ({legertekesebb.BecsultErtek}) Ft");

int millios  = lista.Count(m => m.BecsultErtek > 1000000);
Console.WriteLine($"1 millió Ft értéket meghaladó műtárgyak száma: {millios } db");


Console.WriteLine("Festményke külön elemzése:\n");

List<int> festemnykor = lista
    .Where(m => m is Festmeny)
    .Select(m => ((Festmeny)m).KorEvekben)
    .ToList();

Console.WriteLine($"Kigyűjtött festény korok:\n {string.Join(" év  ",festemnykor)} év.");


var kimenet = lista
    .OrderByDescending(m => m.BecsultErtek)
    .Select(m => $"{m.Azonosito}; {m.BecsultErtek};{m.BiztositasDij()}");


File.WriteAllLines("riport.txt", kimenet);
Console.WriteLine("\n A riport.txt fájl sikeresen elkészült!");


