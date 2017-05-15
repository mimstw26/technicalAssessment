using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
namespace WebExperience.Test.Models
{
    public class csvDataModel
    {
        public string asset { get; set; }
        public string country { get; set; }
        public string mimeType { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}