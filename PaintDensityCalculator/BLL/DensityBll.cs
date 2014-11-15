using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using PaintDensityCalculator.Models;
using PaintDensityCalculator.Utils;
using System.Linq;

namespace PaintDensityCalculator.BLL
{
    public class DensityBll
    {
        private const string DensityFileName = "Density.xml";
        private const string DensityRootElement = "Densities";
        public void AddDensityItem(Density newDensityItem)
        {
            GenericSerializer<Density>.AddItemToFile(DensityFileName, DensityRootElement, newDensityItem);
        }

        public void FirstRun()
        {
            var list = 
                    new Density
                    {
                        Id = 1,
                        DensType = 0,
                        Name = "Буран",
                        DensityValue = 0.92f,
                        AccordingThinner = 0,
                        AccordingHardener = 0,
                        ProportionHardener = 0.2f,
                        ProportionThinner = 0.3f
            };
            GenericSerializer<Density>.WriteFile(DensityFileName, list);
        }

        public List<Density> GetListOfDensities()
        {
            var path = @"C:\Git\PaintDensityCalculator\PaintDensityCalculator\bin\Debug";
//            var file = Path.Combine(Assembly.GetExecutingAssembly().Location, DensityFileName);
            var file = Path.Combine(path, DensityFileName);
            var doc = XDocument.Load(file);
            return doc.Root.Elements("Density").Select(x => 
                GenericSerializer<Density>.Deserialize(x.CreateReader())
                ).ToList();
        }

        public void CsvToXml()
        {
            var lines = File.ReadAllLines(@"C:\csvFile.csv", encoding: Encoding.Default);

            var xml = new XElement("TopElement",
               lines.Select(line => new XElement("Item",
                  line.Split(';')
                      .Select((column, index) => new XElement("Column" + index, column)))));

            xml.Save(@"C:\Git\PaintDensityCalculator\PaintDensityCalculator\bin\Debug\xmlout.xml"); 
        }
    }
}
