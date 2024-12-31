using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManageUsers.Models
{
    public class Account
    {
        [Key]
        [JsonProperty("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? id { get; set; }

        [JsonProperty("accountName")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 6)]
        //[RegularExpression(@"^[A-Z]+[a-zA-Z]*$", ErrorMessage = "Only letters are allowed.")]
        //[RegularExpression(@"^[a-zA-Z0-9!@#$%^&*(),.?'\\s\""]*$", ErrorMessage = "Only letters, numbers, and symbols are allowed.")]
        public string? accountName { get; set; }

        [JsonProperty("type")]
        [Required(ErrorMessage = "Type is required")]
        public string? accountType { get; set; }

        [JsonProperty("link")]
        [Required(ErrorMessage = "Link is required")]
        [Url]
        public string? link { get; set; }

        [JsonProperty("customDatas")]
        public IEnumerable<Customdata>? customDatas { get; set; }

        [JsonProperty("credentials")]
        public IEnumerable<Credential>? credentials { get; set; }
    }

    public class Customdata
    {
        //[Display(Name = "id")]
        //public string? Id { get; set; }

        //[Display(Name = "key")]
        [JsonProperty("key")]
        public string? key { get; set; }

        //[Display(Name = "value")]
        [JsonProperty("value")]
        public string? value { get; set; }
    }

    public class Credential
    {
        //[Display(Name = "id")]
        //public string? Id { get; set; }

        //[Display(Name = "username")]
        [JsonProperty("username")]
        public string? username { get; set; }

        //[Display(Name = "password")]
        [JsonProperty("password")]
        public string? password { get; set; }
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
