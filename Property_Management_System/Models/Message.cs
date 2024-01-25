namespace Property_Management_System.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Content { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

    }
}