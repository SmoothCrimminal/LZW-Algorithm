using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LZW_Algorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {   
            var exampleWord = Console.ReadLine().Trim();
            var wordDict = GetStartDictionary(exampleWord);
            var encodedMessage = GetEncodedMessage(ref wordDict, exampleWord);
            var decodedMessage = GetDecodedMessage(GetStartDictionary(exampleWord), encodedMessage);
            Console.WriteLine(decodedMessage);
        }

        static Dictionary<string, int> GetStartDictionary(string word)
        {
            var startingDict = new Dictionary<string, int>();
            var distinctChars = word.Distinct().ToArray();
            Array.Sort(distinctChars);

            for (int i = 0; i < distinctChars.Length; i++)
            {
                startingDict.Add(distinctChars[i].ToString(), i);
            }

            return startingDict;
        }

        static List<int> GetEncodedMessage(ref Dictionary<string, int> startDict, string word)
        {
            Console.WriteLine("Kodowanie:");

            // Inicjalizacjia zmiennych
            var w = string.Empty;
            var compressedMessage = new List<int>();

            foreach (var c in word)
            {
                var checkWord = w + c;
                
                // Sprawdzamy czy nasz ciąg stworzony z poprzedniego ciągu i następnej litery w słowie znajduje się w naszym słowniku


                if (startDict.ContainsKey(checkWord))
                {
                    // Jeżeli ciąg znajduje się już w słowniku, podmieniamy nasze poprzednie słowo na to, które mamy w słowniku, aby zawsze rozpatrywać nowy ciąg
                    w = checkWord;
                }

                else
                {
                    // W przeciwnym wypadku dodajemy do naszego zakodowanego ciągu znaków indeks ciągu znaków z jakiego skorzystaliśmy do zbudowania nowego podciągu (checkWord)
                    Console.WriteLine($"{startDict[w]} {startDict.Count} = {checkWord}");
                    compressedMessage.Add(startDict[w]);
                    // Dodajemy nasz nowy ciąg znaków do słownika
                    startDict.Add(checkWord, startDict.Count);
                    // Zamieniamy rozpatrywaną literę na ostatnio iterowaną
                    w = c.ToString();
                }
            }

            // Jeśli zostanie jakiś ciąg znaków != pustego to dodajemy go do naszego ciągu
            if (!string.IsNullOrEmpty(w))
                compressedMessage.Add(startDict[w]);

            return compressedMessage;
        }

        static string GetDecodedMessage(Dictionary<string, int> startDict, List<int> encodedMessage)
        {
            Console.WriteLine("Dekodowanie:");

            // Zamiana kluczy i wartości w słowniku, aby łatwiej się na nim operowało
            var swappedDict = startDict.ToDictionary(x => x.Value, x => x.Key);
            var w = swappedDict[encodedMessage.FirstOrDefault()];
            encodedMessage.RemoveAt(0);

            // inicjalizacja naszej wiadomości która będzie dekodowana jako pierwsza litera z ciągu
            var decodedMessage = new StringBuilder(w);

            foreach (var c in encodedMessage)
            {
                var entry = string.Empty;
                // jeśli numer zakodowanego wyrazu znajduje się w słowniku zapisz go do zmiennej
                if (swappedDict.ContainsKey(c))
                    entry = swappedDict[c];
                else if (c == swappedDict.Count) // Zabezpieczenie na wypadek przekroczenia zakresu słownika
                    entry = w + w[0];

                // dopisujemy do naszej zdekodowanej wiadomości ciąg który wyciągnęliśmy uprzednio ze słownika
                decodedMessage.Append(entry);
                Console.WriteLine($"{swappedDict.Count} = {w}?");
                Console.WriteLine($"{swappedDict.Count} = {w + entry[0]}");
                // dodajemy do naszego słownika nowy wyraz, o wartości rozpatrywanego podciągu i pierwszego wyrazu dodawanego do całego wyrazu
                swappedDict.Add(swappedDict.Count, w + entry[0]);
                // 
                w = entry;
            }

            return decodedMessage.ToString();
        }
    }
}
