using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace StudentCatalog.Models
{
    public class MessagesModel
    {
        public int Id { get; set; }
       
        public int? SenderId {  get; set; }
        public UserModel? Sender { get; set; }
        public int? ReceiverId {  get; set; }
        public UserModel? Receiver { get; set; }
        public string? Message {  get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
