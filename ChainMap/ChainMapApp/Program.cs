using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using ChainMapLib;

namespace ChainMapExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("### Przykład użycia ChainMap ###\n");

            // Tworzenie słowników
            var dict1 = CreateDictionary("Pierwszy");
            var dict2 = CreateDictionary("Drugi");
            var dict3 = CreateDictionary("Trzeci");

            // Tworzenie ChainMap
            var chainMap = new ChainMap<string, string>(dict1, dict2, dict3);

            while (true)
            {
                Console.WriteLine("\n### Menu ###");
                Console.WriteLine("1. Wyświetl wszystkie wartości");
                Console.WriteLine("2. Zmiana wartości dla klucza");
                Console.WriteLine("3. Dodaj nowy słownik z wyborem priorytetu");
                Console.WriteLine("4. Usuń klucz");
                Console.WriteLine("5. Pobierz wszystkie klucze");
                Console.WriteLine("6. Pobierz wszystkie wartości");
                Console.WriteLine("7. Sprawdź, czy chainmap zawiera klucz/wartość");
                Console.WriteLine("8. Zakończ");

                Console.Write("Wybierz operację: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllValues(chainMap);
                        break;
                    case "2":
                        ChangeValueForKey(chainMap);
                        break;
                    case "3":
                        AddNewDictionary(chainMap);
                        break;
                    case "4":
                        RemoveKey(chainMap);
                        break;
                    case "5":
                        GetAllKeys(chainMap);
                        break;
                    case "6":
                        GetAllValues(chainMap);
                        break;
                    case "7":
                        CheckIfContains(chainMap);
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                        break;
                }
            }
        }

        static void ShowAllValues(ChainMap<string, string> chainMap)
        {
            Console.WriteLine("\nWszystkie wartości:");
            foreach (var kvp in chainMap)
            {
                Console.WriteLine($"Klucz: {kvp.Key}, Wartość: {kvp.Value}");
            }
        }

        static void ChangeValueForKey(ChainMap<string, string> chainMap)
        {
            Console.WriteLine("\nDodawanie klucza:");

            Console.Write("Podaj klucz do dodania: ");
            var keyToAdd = Console.ReadLine();
            Console.Write("Podaj wartość: ");
            var value = Console.ReadLine();

            try
            {
                chainMap[keyToAdd] = value;
            }
            catch
            {
                Console.WriteLine($"Klucz '{keyToAdd}' nie istnieje w ChainMap.");
            }
        }

        static void AddNewDictionary(ChainMap<string, string> chainMap)
        {
            Console.WriteLine("\nDodawanie nowego słownika:");

            var newDict = CreateDictionary("Nowy");

            Console.Write("Podaj priorytet nowego słownika: ");
            
            var priorityS = Console.ReadLine();
            int.TryParse(priorityS, out var priority);
            chainMap.AddDictionary(newDict, priority);
            Console.WriteLine("Nowy słownik dodany do ChainMap.");
        }

        static void RemoveKey(ChainMap<string, string> chainMap)
        {
            Console.WriteLine("\nUsuwanie klucza:");

            Console.Write("Podaj klucz do usunięcia: ");
            var keyToRemove = Console.ReadLine();

            if (chainMap.ContainsKey(keyToRemove))
            {
                if (chainMap.Remove(keyToRemove))
                {
                    Console.WriteLine($"Klucz '{keyToRemove}' usunięty.");
                }
                
                else {
                    Console.WriteLine($"Nie udało się usunąć klucza '{keyToRemove}', brakuje go w slowniku głównym");
                }
            }
            else
            {
                Console.WriteLine($"Klucz '{keyToRemove}' nie istnieje w ChainMap.");
            }
        }


        static void GetAllKeys(ChainMap<string, string> chainMap)
        {
            Console.WriteLine("\nPobieranie wszystkich kluczy:");
            var Keys = chainMap.Keys;
            Console.WriteLine(String.Join("\n",Keys)); ;
        }

        static void GetAllValues(ChainMap<string, string> chainMap)
        {
            Console.WriteLine("\nPobieranie wszystkich wartości z ich źródłami:");
            var Values = chainMap.Values;
            Console.WriteLine(String.Join("\n", Values)); ;
        }
        static Dictionary<string, string> CreateDictionary(string name)
        {
            var dictionary = new Dictionary<string, string>();

            Console.WriteLine($"\nTworzenie słownika '{name}':");
            while (true)
            {
                
                Console.Write("Podaj klucz (lub wpisz 'koniec' aby zakończyć): ");
                var key = Console.ReadLine();
                if (key is null || key == "")
                {
                    Console.WriteLine("niepoprawna warość");
                    continue;
                }
                if (key.ToLower() == "koniec")
                    break;

                Console.Write("Podaj wartość: ");
                var value = Console.ReadLine();
                if (value is null || value == "")
                {
                    Console.WriteLine("niepoprawna warość");
                    continue;
                }
                try
                {
                    dictionary.Add(key, value);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Wartość dla podanego klucza już istnieje. Podaj inną wartość.");
                }
            }
            return dictionary;
        }
        public static void CheckIfContains(ChainMap<string,string> chainMap)
        {
            string? key, value;
            Console.WriteLine("Sprawdzamy klucz, czy wartość?");
            Console.WriteLine("1. Klucz");
            Console.WriteLine("2. Wartość");
            Console.WriteLine("3. I jedno I drugie");
            var choice = Console.ReadLine();
            switch (choice) 
            {
                case "1":
                    Console.WriteLine("Podaj klucz:");
                    key = Console.ReadLine();
                    if (key is null || key == "")
                    {
                        Console.WriteLine("niepoprawna wartość");
                        return;
                    }
                    if (chainMap.ContainsKey(key)) { Console.WriteLine("Zawiera"); } else { Console.WriteLine("nie zawiera"); } 
                    break;
                case "2":
                    Console.WriteLine("Podaj Wartość:");
                     value = Console.ReadLine();
                    if ( value is null || value == "")
                    {
                        Console.WriteLine("niepoprawna wartość");
                        return;
                    }
                    if (chainMap.ContainsValue(value)) { Console.WriteLine("Zawiera"); } else { Console.WriteLine("nie zawiera"); }
                    break;
                case "3":
                    Console.WriteLine("Podaj klucz:");
                    key = Console.ReadLine();
                    Console.WriteLine("Podaj wartość:");
                    value = Console.ReadLine();
                    if (key is null || value is null || value== "" || key == "")
                    {
                        Console.WriteLine("niepoprawna wartość");
                        return;
                    }
                    var kvp = new KeyValuePair<string, string>(key, value);
                    if (chainMap.Contains(kvp)) { Console.WriteLine("Zawiera"); } else { Console.WriteLine("nie zawiera"); }
                    break;
            }
        }
    }
}
