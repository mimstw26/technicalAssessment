using GeneralKnowledge.Test.App.Tests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace GeneralKnowledge.Test.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // String manipulations
            var t1 = new StringTests();
            t1.Run();

            // Data retrieval from a XML file
            var t2 = new XmlReadingTest();
            t2.Run();

            //// Image manipulations
            var t3 = new RescaleImageTest();
            t3.Run();

            //// Processing a CSV file
            var t4 = new CsvProcessingTest();
            t4.Run();

            var t5 = new CsvProcessingTest();
            var csvTestDataTable = t5.AccessList();

            Console.WriteLine("Test execution ended.");
            Console.ReadKey();
        }
    }
}
