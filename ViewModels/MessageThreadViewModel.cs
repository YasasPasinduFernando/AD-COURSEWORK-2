namespace AD_COURSEWORK_2.ViewModels;

public class MessageThreadViewModel
{
    public string OtherUserId { get; set; } = string.Empty;
    public string OtherUserName { get; set; } = string.Empty;
    public List<MessageLine> Messages { get; set; } = new();

    public class MessageLine
    {
        public bool IsMine { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAtUtc { get; set; }
        public bool IsRead { get; set; }
    }
}
