using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PaintDensityCalculator.Utils
{
    public static class GenericSerializer<T> where T : new()
    {
        public static T Deserialize(XDocument doc)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = doc.Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        public static T Deserialize(XmlReader reader)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(reader);
        }

        public static XDocument Serialize(object obj)
        {
            var xmlSerializer = new XmlSerializer(obj.GetType());

            var doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, obj);
            }

            return doc;
        }

        public static T ReadFile(string file)
        {
            try
            {
                string xml;
                using (var reader = new StreamReader(file))
                {
                    xml = reader.ReadToEnd();
                    reader.Close();
                }
                return Deserialize(XDocument.Parse(xml));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла при чтении файла", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new T();
        }

        public static bool WriteFile(string fileName, T obj)
        {
            var ok = false;
            try
            {
                var xml = Serialize(obj);
                var xdoc = new XElement("Densities",
                    new XElement(xml.Root));
                var path = @"C:\Git\PaintDensityCalculator\PaintDensityCalculator\bin\Debug";
//                var filePath = Path.Combine(Assembly.GetExecutingAssembly().Location, fileName);
                var filePath = Path.Combine(path, fileName);
                xdoc.Save(filePath);
                
                ok = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Произошла при записи файла", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return ok;
        }

        public static void AddItemToFile(string fileName, string rootElement, T obj)
        {
            var file = Path.Combine(Assembly.GetExecutingAssembly().Location, fileName);
            var document = XDocument.Load(file);
            // Or var characters = document.Root.Element(ns + "Characters")
            var characters = document.Descendants(rootElement).FirstOrDefault();
            if (characters != null)
            {
                characters.Add(new XElement(Serialize(obj).Root));
                document.Save(file);
            }
        }
    }
}
