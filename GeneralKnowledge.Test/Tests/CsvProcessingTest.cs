using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using System.Data;
using System.Reflection;

namespace GeneralKnowledge.Test.App.Tests
{
    /// <summary>
    /// CSV processing test
    /// </summary>
    public class CsvProcessingTest : ITest
    {
        public void Run()
        {
            // TODO: 
            // Create a domain model via POCO classes to store the data available in the CSV file below
            // Objects to be present in the domain model: Asset[0], Mime Type[2], and Country[5]
            // Process the file in the most robust way possible
            // The use of 3rd party plugins is permitted

            var csvFile = Resources.AssetImport;
            readData(csvFile);
        }

        public DataTable accessList()
        {
            var csvFile = Resources.AssetImport;
            List<csvData> csvDataList = readData(csvFile);
            DataTable csvDataTable = ToDataTable<csvData>(csvDataList);
            return csvDataTable;
            //return csvDataList;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof
                    (Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public List<csvData> readData(string csvFile)
        {
            List<csvData> resultList = new List<csvData>();
            // these are for testing
            //using(TextReader fileReader = File.OpenText(@"C:\Documents\dotNET developer\dotNET developer\GeneralKnowledge.Test\Resources\AssetImport.csv")){
            //var csvRead = new CsvReader(fileReader);

            // this works by creating reader that reads through the csvFile (which is now a string)
            // then using the CsvReader from CsvHelper (https://joshclose.github.io/CsvHelper/) to quickly parse and read the data
            using (var reader = new StringReader(csvFile))
            {
                var csvRead = new CsvReader(reader);
                csvRead.Configuration.HasHeaderRecord = true;
                csvRead.Configuration.RegisterClassMap<CSVFileDefinitionMap>();

                while (csvRead.Read())
                {
                    //var record = csvRead.GetRecord<csvData>().; // how to add this to a list or map or something?
                    var results = csvRead.GetRecord<csvData>();
                    resultList.Add(results);
                    //myMap.PropertyMap.
                }
                return resultList;
            }
        }

        // store the data in list of class csvData for easy organization
        public class csvData
        {
            public string asset { get; set; }
            public string country { get; set; }
            public string mimeType { get; set; }
        }

        // using csvHelper
        sealed class CSVFileDefinitionMap : CsvClassMap<csvData>
        {
            public CSVFileDefinitionMap()
            {
                Map(m => m.asset).Name("asset id");
                Map(m => m.country).Name("country");
                Map(m => m.mimeType).Name("mime_type");
            }
        }
    }
}
