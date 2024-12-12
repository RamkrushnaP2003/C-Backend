using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskExcelMongoDB.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "FullName is required")]
        public string? FullName { get; set; }
        
        [Required(ErrorMessage = "MobileNo is required.")]
        public string? MobileNo { get; set; }

        public string? Address { get; set; }
        public decimal Salary { get; set; }
        [Required(ErrorMessage = "Data of birth is required")]
        public string? DateOfBirth { get; set; }
    }
}
