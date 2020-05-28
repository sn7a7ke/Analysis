namespace Analysis.Common
{
    public class AppSettings
    {
        public string StorageFolder { get; set; }
        public PomCovidSettings PomCovid { get; set; }
    }

    public class PomCovidSettings
    {
        public string Url { get; set; }
        public string JsonFileName { get; set; }
        public string SummaryKey { get; set; }
        public int ResponseCacheDuration { get; set; }
    }
}
