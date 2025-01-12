﻿namespace Application.Utils;

public class SmtpSettings
{
    public string SmtpServer { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
}