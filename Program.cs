namespace ComarchTools
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using System.Xml.Serialization;
    using Schemas;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            var xsd = File.ReadAllText(args[0], Encoding.UTF8);
            var xml = File.ReadAllText(args[1], Encoding.UTF8);

            var xmlSchemaSet = new XmlSchemaSet();
            using (var reader = XmlReader.Create(new StringReader(xsd)))
            {
                xmlSchemaSet.Add(null, reader);
            }

            var xDocument = XDocument.Load(new StringReader(xml));

            xDocument.Validate(xmlSchemaSet, (sender, e) =>
             {
                 Console.WriteLine(e.Message);
             }, true);

            foreach (var element in xDocument.Root.Elements())
            {
                //Console.WriteLine(element.Name);
                if (element.HasElements)
                {
                    foreach (var subElement in element.Elements())
                    {
                        //Console.WriteLine(subElement.Name);
                    }
                }
            }

            var xmlSerializer = new XmlSerializer(typeof(DocumentOrderResponse));
            var @object = (DocumentOrderResponse)xmlSerializer.Deserialize(new StringReader(xml));

            Console.WriteLine($"{@object.OrderResponseHeader.OrderResponseNumber}");
            Console.WriteLine($"{@object.OrderResponseHeader.OrderResponseDate}");
        }
    }
}
