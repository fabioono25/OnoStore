namespace OnoStore.Identity.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationHours { get; set; }
        public string Emitter { get; set; }
        public string ValidIn { get; set; }
    }
}
