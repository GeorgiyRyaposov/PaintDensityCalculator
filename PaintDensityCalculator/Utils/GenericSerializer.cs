using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace PaintDensityCalculator.Utils
{
    public class GenericSerializer<T> where T : class, new()
    {
        private XmlSerializer _xmlSerializer;

        public GenericSerializer()
        {
            _xmlSerializer = new XmlSerializer(typeof(T));
        }

        public T Deserialize(string xml)
        {
            TextReader reader = new StringReader(xml);
            return Deserialize(reader);
        }

        public T Deserialize(XmlDocument doc)
        {
            TextReader reader = new StringReader(doc.OuterXml);
            return Deserialize(reader);
        }

        public T Deserialize(TextReader reader)
        {
            var o = (T)_xmlSerializer.Deserialize(reader);
            reader.Close();
            return o;
        }

        public XmlDocument Serialize(T rootclass)
        {
            var xml = StringSerialize(rootclass);
            var doc = new XmlDocument {PreserveWhitespace = true};
            doc.LoadXml(xml);
            return doc;
        }

        private string StringSerialize(T rootclass)
        {
            TextWriter w = WriterSerialize(rootclass);
            string xml = w.ToString();
            w.Close();
            return xml.Trim();
        }

        private TextWriter WriterSerialize(T rootclass)
        {
            TextWriter w = new StringWriter();
            _xmlSerializer = new XmlSerializer(typeof(T));
            _xmlSerializer.Serialize(w, rootclass);
            w.Flush();
            return w;
        }

        public static T ReadFile(string file)
        {
            var serializer = new GenericSerializer<T>();
            try
            {
                string xml;
                using (var reader = new StreamReader(file))
                {
                    xml = reader.ReadToEnd();
                    reader.Close();
                }
                return serializer.Deserialize(xml);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла при чтении файла", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return new T();
        }

        public static bool WriteFile(string fileName, T config)
        {
            var ok = false;
            var serializer = new GenericSerializer<T>();
            try
            {
                var xml = serializer.Serialize(config).OuterXml;
                var filePath = Assembly.GetExecutingAssembly().Location + "\\" + fileName;
                using (var writer = new StreamWriter(filePath, true))
                {
                    writer.Write(xml.Trim());
                    writer.Flush();
                    writer.Close();
                }
                ok = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла при записи файла", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return ok;
        }
    }
}
