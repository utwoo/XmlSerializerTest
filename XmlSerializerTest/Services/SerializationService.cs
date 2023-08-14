using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using XmlSerializerTest.Models;

namespace XmlSerializerTest.Services;

public class SerializationService
{
    public static void Write(Person person, string fileName)
    {
        if (File.Exists(fileName))
            File.Delete(fileName);

        var serializer = new XmlSerializer(typeof(Person));
        // serialize object to memory stream.
        var ms = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(ms, GetXmlWriterSettings());
        serializer.Serialize(xmlWriter, person, GetXmlSerializerNamespaces());

        // sha512 hash
        var computeHash = SHA512.HashData(ms.ToArray());
        var checksum = string.Join(string.Empty, computeHash.Select(hash => hash.ToString("x2")));

        // add checksum and output to file
        person.CheckSum = checksum;
        using var xmlWriterFile = XmlWriter.Create(fileName, GetXmlWriterSettings());
        serializer.Serialize(xmlWriterFile, person, GetXmlSerializerNamespaces());
    }

    public static bool Verify(string fileName)
    {
        // read xml file and deserialize to object
        var xmlReader = XmlReader.Create(fileName);
        var serializer = new XmlSerializer(typeof(Person));
        var person = serializer.Deserialize(xmlReader) as Person;

        if (person == null)
            return false;

        // get checksum element in xml file
        var checksum = person.CheckSum;
        
        // calculate checksum for xml file 
        person.CheckSum = null;
        
        var ms = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(ms, GetXmlWriterSettings());
        serializer.Serialize(xmlWriter, person, GetXmlSerializerNamespaces());
        
        var computeHash = SHA512.HashData(ms.ToArray());
        var checksumVerify = string.Join(string.Empty, computeHash.Select(hash => hash.ToString("x2")));

        // verify checksum
        return checksumVerify == checksum;
    }

    private static XmlWriterSettings GetXmlWriterSettings()
    {
        return new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = true,
            IndentChars = "\t"
        };
    }

    private static XmlSerializerNamespaces GetXmlSerializerNamespaces()
    {
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);
        return namespaces;
    }
}