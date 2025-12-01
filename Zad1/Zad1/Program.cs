using System;
using System.IO;
string path = "C:\\test\\test_Antoni_Wasik.txt";
int licznik = 0;
if (File.Exists(path))
{
    string tekst = File.ReadAllText(path);
    foreach(char a in tekst)
    {
        if(a == 'a' || a == 'A'){
            licznik++;
        }
    }
    Console.WriteLine(licznik);
}
else
{

    Console.WriteLine("Ten plik nie istnieje");

}