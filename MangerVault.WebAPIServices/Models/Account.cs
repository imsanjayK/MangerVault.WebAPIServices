using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageUsers.Models
{
    //[Collection("account")]
    public class Account
    {
        //[BsonId]
        //public ObjectId _id { get; set; }

        //[Key]
        [BsonId]
        //[JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? id { get; set; }

        [BsonElement("accountName")]
        //[JsonProperty("accountName")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 6)]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Only letters are allowed.")]
        //[RegularExpression(@"^[a-zA-Z0-9!@#$%^&*(),.?'\\s\""]*$", ErrorMessage = "Only letters, numbers, and symbols are allowed.")]
        public string? AccountName { get; set; }

        [BsonElement("type")]
        //[JsonProperty("type")]
        [Required(ErrorMessage = "Type is required")]
        public string? AccountType { get; set; }

        [BsonElement("link")]
        //[JsonProperty("link")]
        [Required(ErrorMessage = "Link is required")]
        [Url]
        public string? link { get; set; }

        [BsonElement("customDatas")]
        //[JsonProperty("customDatas")]
        public IEnumerable<Customdata>? customDatas { get; set; }

        [BsonElement("credentials")]
        //[JsonProperty("credentials")]
        public IEnumerable<Credential>? credentials { get; set; }
    }

    public class Customdata
    {
        //[Display(Name = "id")]
        //public string? Id { get; set; }

        //[Display(Name = "key")]
        [BsonElement("key")]
        //[JsonProperty("key")]
        public string? key { get; set; }

        //[Display(Name = "value")]
        [BsonElement("value")]
        //[JsonProperty("value")]
        public string? value { get; set; }
    }

    public class Credential
    {
        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("passphrase")]
        public string? Passphrase { get; set; }
    }

    public enum AccountType
    {
        Investment,
        Banking,
        SocialMedia,
        Career ,
        JobSeek ,
        Email ,
        Travelling ,
        Finance ,
        Undefined ,
    }
}
