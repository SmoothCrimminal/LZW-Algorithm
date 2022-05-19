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
            var compressedMessage = GetEncodedMessage(ref wordDict, exampleWord);
            var decodedMessage = GetDecodedMessage(GetStartDictionary(exampleWord), compressedMessage);
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
            var w = string.Empty;
            var compressedMessage = new List<int>();

            foreach (var c in word)
            {
                var checkWord = w + c;
                
                if (startDict.ContainsKey(checkWord))
                {
                    w = checkWord;
                }

                else
                {
                    compressedMessage.Add(startDict[w]);
                    startDict.Add(checkWord, startDict.Count);
                    w = c.ToString();
                }
            }

            if (!string.IsNullOrEmpty(w))
                compressedMessage.Add(startDict[w]);

            return compressedMessage;
        }

        static string GetDecodedMessage(Dictionary<string, int> startDict, List<int> encodedMessage)
        {
            var swappedDict = startDict.ToDictionary(x => x.Value, x => x.Key);
            var w = swappedDict[encodedMessage.FirstOrDefault()];
            encodedMessage.RemoveAt(0);


            var decodedMessage = new StringBuilder(w);

            foreach (var c in encodedMessage)
            {
                var entry = string.Empty;
                if (swappedDict.ContainsKey(c))
                    entry = swappedDict[c];
                else if (c == swappedDict.Count)
                    entry = w + w[0];

                decodedMessage.Append(entry);
                swappedDict.Add(swappedDict.Count, w + entry[0]);
                w = entry;
            }

            return decodedMessage.ToString();
        }
    }
}
