using ContactApp.JsonRepository.Models;
using System.Text.Json;

namespace ContactApp.JsonRepository
{
    public class ContactRepository
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "contacts.json");

        public List<Contact> GetAllContacts()
        {
            if (!File.Exists(_filePath)) return new List<Contact>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
        }

        public void SaveAllContacts(List<Contact> contacts)
        {
            var json = JsonSerializer.Serialize(contacts);
            File.WriteAllText(_filePath, json);
        }

        
    }
}
