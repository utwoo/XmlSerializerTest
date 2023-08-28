using System.Xml.Serialization;

namespace XmlSerializerTest.Models;

public class Person
{
    [XmlElement]
    public string Name { get; set; }
    public string? CheckSum { get; set; }
}