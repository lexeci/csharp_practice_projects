// See https://aka.ms/new-console-template for more information

using System.Xml.Serialization;
using System.Text.Json;

public class Person
{
    public string UserName { get; set; }
    public int UserAge { get; set; }
}

class Program {
    static void Main() {
        
          try
        {
        Person samplePerson = new Person {UserName = "Alice", UserAge = 24};

        using( FileStream fs = new FileStream("peson.dat", FileMode.Create)){
            BinaryWriter writer = new BinaryWriter(fs);
            writer.Write(samplePerson.UserName);
            writer.Write(samplePerson.UserAge);
        }
        Console.WriteLine("Binary serialization complete.");

        Person deserializedPerson;
        using (var fs = new FileStream("peson.dat", FileMode.Open))
        using (var reader = new BinaryReader(fs))
        {
            deserializedPerson = new Person {
                UserName = reader.ReadString(),
                UserAge = reader.ReadInt32()
            };
        }
        Console.WriteLine($"Binary Deserialization - UserName: {deserializedPerson.UserName}, UserAge: {deserializedPerson.UserAge}");

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Person));
        using(StreamWriter writer = new StreamWriter("person.xml")) {
            xmlSerializer.Serialize(writer, samplePerson);
        }
        Console.WriteLine("XML serialization complete.");

        using( StreamReader readerXml = new StreamReader("person.xml")) {
            var deserializedPersonXml = (Person)xmlSerializer.Deserialize(readerXml);
            Console.WriteLine($"XML Deserialization - UserName: {deserializedPersonXml.UserName}, UserAge: {deserializedPersonXml.UserAge}");
        }

        string jsonString = JsonSerializer.Serialize(samplePerson);
        File.WriteAllText("person.json", jsonString);
        Console.WriteLine("JSON serialization complete.");

        using(StreamReader readerJson = new StreamReader("person.json")) {
         // Читаємо JSON з файлу як рядок і десеріалізуємо
        string readJson = File.ReadAllText("person.json");
        var deserializedPersonJson = JsonSerializer.Deserialize<Person>(readJson);

        Console.WriteLine($"JSON Deserialization - UserName: {deserializedPersonJson.UserName}, UserAge: {deserializedPersonJson.UserAge}");
    }
       }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during deserialization: {ex.Message}");
        }
    }
}