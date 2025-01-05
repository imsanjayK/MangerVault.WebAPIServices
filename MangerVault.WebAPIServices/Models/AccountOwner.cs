using ManageUsers.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangerVault.WebAPIServices.Models
{
    public class AccountOwner
    {
        [BsonId]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string? Id { get; set; }

        //[BsonElement("passcode")]
        //[Required(ErrorMessage = "Link is required")]
        //public Customdata? Passcode { get; set; }

        [BsonElement("credential")]
        //[Required(ErrorMessage = "Link is required")]
        public Credential? Credential { get; set; }
    }
}
