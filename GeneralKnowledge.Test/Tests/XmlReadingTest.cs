using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GeneralKnowledge.Test.App.Tests
{
    /// <summary>
    /// This test evaluates the 
    /// </summary>
    public class XmlReadingTest : ITest
    {
        public string Name { get { return "XML Reading Test"; } }

        public void Run()
        {
            var xmlData = Resources.SamplePoints;

            // TODO: 
            // Determine for each parameter stored in the variable below, the average value, lowest and highest number.
            // Example output
            // parameter   LOW AVG MAX
            // temperature   x   y   z
            // pH            x   y   z
            // Chloride      x   y   z
            // Phosphate     x   y   z
            // Nitrate       x   y   z

            PrintOverview(xmlData);
        }

        private void PrintOverview(string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            // goes through the nodes of the xml document and looks into each measurement block to view the params.
            // from there, it is able to create the node list by grabbing all params that match the unique given name.
            var temperatures = xmlDoc.SelectNodes("/samples/measurement/param[@name='temperature']");
            var pHs = xmlDoc.SelectNodes("/samples/measurement/param[@name='pH']");
            var phosphates = xmlDoc.SelectNodes("/samples/measurement/param[@name='Phosphate']");
            var chlorides = xmlDoc.SelectNodes("/samples/measurement/param[@name='Chloride']");
            var nitrates = xmlDoc.SelectNodes("/samples/measurement/param[@name='Nitrate']");

            var temperatureMax = getMax(temperatures);
            var phsMax = getMax(pHs);
            var phosphatesMax = getMax(phosphates);
            var chloridesMax = getMax(chlorides);
            var nitratesMax = getMax(nitrates);

            double temperatureMin = getMin(temperatures, temperatureMax);
            double phsMin = getMin(pHs, phsMax);
            double phosphatesMin = getMin(phosphates, phosphatesMax);
            double chloridesMin = getMin(chlorides, chloridesMax);
            double nitratesMin = getMin(nitrates, nitratesMax);

            double temperatureAvg = getAvg(temperatures);
            double phsAvg = getAvg(pHs);
            double phosphateAvg = getAvg(phosphates);
            double chlorideAvg = getAvg(chlorides);
            double nitrateAvg = getAvg(nitrates);

            // this was how I got the best formatting.  I'm sure there are better ways, but this at least looked pretty.
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-10}", "Parameter", "low", "avg", "max");
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-10}", "Temperature", temperatureMin, temperatureAvg, temperatureMax);
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-10}", "Phs", phsMin, phsAvg, phsMax);
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-10}", "Phosphate", phosphatesMin, phosphateAvg, phosphatesMax);
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-10}", "Chloride", chloridesMin, chlorideAvg, chloridesMax);
            Console.WriteLine("{0,-15} {1,-10} {2,-10} {3,-10}", "Nitrate", nitratesMin, nitrateAvg, nitratesMax);
        }

        private double getMax(XmlNodeList nodeList)
        {
            double max = 0;
            int i = 0;

            foreach (XmlNode node in nodeList)
            {
                //  looks at the InnerText, which is the actual value of the node
                double doubleStatsValue = Convert.ToDouble(node.InnerText);

                // this is in case there are negatives. originally I had max = 0 and
                // only did if(doubleStatsValue > max) but realized that if there were only negative values then
                // there would never be a max.
                if (i == 0)
                    max = doubleStatsValue;

                else
                {
                    if (doubleStatsValue > max)
                        max = doubleStatsValue;
                }

                i++;
            }
            return max;
        }

        private double getMin(XmlNodeList nodeList, double max)
        {
            // this just starts at the max value provided from getMax and goes down from there
            double min = max;

            foreach (XmlNode node in nodeList)
            {
                double doubleStatsValue = Convert.ToDouble(node.InnerText);

                if (doubleStatsValue < min)
                    min = doubleStatsValue;
            }
            return min;
        }

        private double getAvg(XmlNodeList nodeList)
        {
            double avg = 0;

            foreach (XmlNode node in nodeList)
            {
                double doubleStatsValue = Convert.ToDouble(node.InnerText);
                avg += doubleStatsValue;

            }
            avg = avg / nodeList.Count;
            return Math.Round(avg, 2); // rounds the decimal to two places for cosmetics
        }
    }
}
