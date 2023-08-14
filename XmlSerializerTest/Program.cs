// See https://aka.ms/new-console-template for more information

using XmlSerializerTest.Models;
using XmlSerializerTest.Services;

// create xml file
var person = new Person { Name = "Evan" };
SerializationService.Write(person, "person.xml");

// verify xml file
Console.WriteLine(SerializationService.Verify("person.xml") ? "true" : "false");

Console.WriteLine("Successfully.");
Console.ReadLine();