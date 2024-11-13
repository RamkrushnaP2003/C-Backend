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

        [Required] // Adds validation for non-nullable fields
        public string FullName { get; set; }
        
        [Required]
        public string MobileNo { get; set; }

        public string Address { get; set; }
        public decimal Salary { get; set; }
        [Required]
        public string DateOfBirth { get; set; }
    }
}
