namespace GTBack.Core.DTO;

public class MailServiceOptions
{
    public string SenderEmail { get; set; }
    public string SenderPassword { get; set; }
    public string SenderName { get; set; } = "Default Sender Name";
    public string ReceiverEmail { get; set; }
    public string ReceiverName { get; set; } = "Default Receiver Name";
    public string Subject { get; set; }
    public string Body { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public bool UseSsl { get; set; } = true;
}