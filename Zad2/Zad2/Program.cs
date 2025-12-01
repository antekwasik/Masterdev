using System;
using System.IO;

Console.WriteLine("Poda Nazwe pliku");
string plik = Console.ReadLine();
string path = "C:\\test\\" + plik ;
if (File.Exists(path + ".txt"))
{
    string tekst = File.ReadAllText(path + ".txt");
    string[] tab = tekst.Split("praca");
    if (tab.Length-1 == 5)
    {
        tekst = tekst.Replace("praca", "job");
        string data = DateTime.Now.ToString("dd.MM.yyyy");
        data = data.Replace(".", "");
        File.WriteAllText(path + "_changed-" + data + ".txt", tekst);
    }
    else
    {
        Console.WriteLine("W pliku nie ma wystarczającej liczby słowa praca");
    }
}