using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace CrosswordsPuzzleGenerator.Models
{
    public class WordCollection
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Words { get; set; } = new();

        public WordCollection() 
        { 
            Words = new List<string>(); 
        }

        public WordCollection(string name, List<string> words)
        {
            Name = name;
            Words = words;
        }

        public WordCollection(string name)
        {
            Name = name;
        }
    }

    class WordCollectionService
    {
        // private const string _metaFilePath = "Data/collections.json";

        private readonly string _directory = Path.Combine("Data", "Collections");

        public List<WordCollection> LoadCollections()
        {
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            var collections = new List<WordCollection>();

            foreach (var file in Directory.GetFiles(_directory, "*.json"))
            {
                string json = File.ReadAllText(file);
                var collection = JsonSerializer.Deserialize<WordCollection>(json);
                if (collection != null)
                    collections.Add(collection);
            }

            return collections;
        }

        public void SaveCollection(WordCollection collection)
        {
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            string filePath = Path.Combine(_directory, $"{collection.Name}.json");
            string json = JsonSerializer.Serialize(collection, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void DeleteCollection(string name)
        {
            string filePath = Path.Combine(_directory, $"{name}.json");
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
