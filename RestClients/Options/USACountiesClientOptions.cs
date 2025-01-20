namespace RestClients.Options
{
    public class USACountiesClientOptions
    {
        public required string BaseUrl { get; set; }
        public required string Get { get; set; }
        public required string Where { get; set; }
        public required string[] OutFields { get; set; }
        public required bool ReturnGeometry { get; set; }
        public required string F { get; set; }
    }
}
