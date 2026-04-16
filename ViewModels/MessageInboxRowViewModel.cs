namespace AD_COURSEWORK_2.ViewModels;

public class MessageInboxRowViewModel
{
    public string OtherUserId { get; set; } = string.Empty;
    public string OtherName { get; set; } = string.Empty;
    public string LastSubject { get; set; } = string.Empty;
    public string LastPreview { get; set; } = string.Empty;
    public DateTime LastAtUtc { get; set; }
    public bool Unread { get; set; }
}
