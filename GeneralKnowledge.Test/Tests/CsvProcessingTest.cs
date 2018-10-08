using System;
using System.Collections.Generic;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using GeneralKnowledge.Test.App.Classes;
using System.Configuration;
using MongoDB.Driver;

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
            var results = ReadData(csvFile);
            //SaveToMongo(results).ConfigureAwait(false);
        }

        public DataTable AccessList()
        {
            var csvFile = Resources.AssetImport;
            var csvDataList = ReadData(csvFile);
            var csvDataTable = ToDataTable(csvDataList);
            return csvDataTable;
            //return CsvDataList;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof
                    (Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                if(type != null)
                    dataTable.Columns.Add(prop.Name, type);
            }
            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public List<CsvData> ReadData(string csvFile)
        {
            // these are for testing
            //using(TextReader fileReader = File.OpenText(@"C:\Documents\dotNET developer\dotNET developer\GeneralKnowledge.Test\Resources\AssetImport.csv")){
            //var csvRead = new CsvReader(fileReader);

            // this works by creating reader that reads through the csvFile (which is now a string)
            // then using the CsvReader from CsvHelper (https://joshclose.github.io/CsvHelper/) to quickly parse and read the data

            var resultList = new List<CsvData>();
            using (var reader = new StringReader(csvFile))
            {
                var csvRead = new CsvReader(reader);
                csvRead.Configuration.HasHeaderRecord = true;
                csvRead.Configuration.RegisterClassMap<CsvFileDefinitionMap>();

                while (csvRead.Read())
                {
                    //var record = csvRead.GetRecord<CsvData>().; // how to add this to a list or map or something?
                    var results = csvRead.GetRecord<CsvData>();
                    resultList.Add(results);
                    //myMap.PropertyMap.
                }
                return resultList;
            }
        }

        // using csvHelper
        private sealed class CsvFileDefinitionMap : CsvClassMap<CsvData>
        {
            public CsvFileDefinitionMap()
            {
                Map(m => m.Asset).Name("asset id");
                Map(m => m.Country).Name("country");
                Map(m => m.MimeType).Name("mime_type");
            }
        }

        public async Task SaveToMongo(List<CsvData> results)
        {
            if (results != null && results.Count != 0)
            {
                var client = new MongoClient(ConfigurationManager.AppSettings["mongoConnection"]);
                var database = client.GetDatabase("TechAssDb");

                var assets = database?.GetCollection<CsvData>("Assets");
                if (assets != null) await assets.InsertManyAsync(results).ConfigureAwait(false);
            }

        }
    }
}
