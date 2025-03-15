using DatingApp.Backend.Attributes;

namespace DatingApp.Backend.Configs.Settings
{
    [Configuration("Token")]
    public class TokenSettings
    {
        public string Secret { get; set; }
    }
}
