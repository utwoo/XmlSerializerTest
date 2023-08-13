// See https://aka.ms/new-console-template for more information

using System.ComponentModel.Design.Serialization;
using XmlSerializerTest.Models;
using XmlSerializerTest.Services;

var person = new Person { Name = "Evan1" };
SerializationService.Write(person, "person.xml");

Console.WriteLine("Successfully.");
Console.ReadLine();