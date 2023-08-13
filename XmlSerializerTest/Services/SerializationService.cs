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
        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add(string.Empty, string.Empty);
        var settings = new XmlWriterSettings
        {
            OmitXmlDeclaration = true,
            Indent = true,
            IndentChars = "\t"
        };

        // serialize object to memory stream.
        var ms = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(ms, settings);
        serializer.Serialize(xmlWriter, person, namespaces);

        // sha512 hash
        var computeHash = SHA512.HashData(ms.ToArray());
        var checksum = string.Join(string.Empty, computeHash.Select(hash => hash.ToString("x2")));

        // add checksum and output to file
        person.CheckSum = checksum;
        var xmlWriterFile = XmlWriter.Create(fileName, settings);
        serializer.Serialize(xmlWriterFile, person, namespaces);
    }
}