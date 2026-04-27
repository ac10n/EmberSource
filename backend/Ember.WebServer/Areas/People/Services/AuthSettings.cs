namespace Ember.WebServer.Areas.People.Services;

public class AuthSettings
{
    public required string JwtKey { get; set; }
    public required string JwtIssuer { get; set; }
    public required string JwtAudience { get; set; }

    public SmtpSettings Smtp { get; set; } = new SmtpSettings { Host = "", Username = "", Password = "" };
    public FacebookSettings Facebook { get; set; } = new FacebookSettings { AppId = "", AppSecret = "" };
    public GoogleSettings Google { get; set; } = new GoogleSettings { ClientId = "", ClientSecret = "" };

    public class SmtpSettings
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 587;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool EnableSsl { get; set; } = true;
    }

    public class FacebookSettings
    {
        public string AppId { get; set; } = "";
        public string AppSecret { get; set; } = "";
    }

    public class GoogleSettings
    {
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
    }
}