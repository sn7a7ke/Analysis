using Analysis.Common;

namespace Analysis.Data.Pom
{
    public class PomStorage
    {
        private readonly AppSettings appSettings;
        private readonly FileStorage fileStorage;
        private string json;

        public string Json
        {
            get
            {
                if (string.IsNullOrEmpty(this.json))
                    this.json = this.fileStorage.Load();
                return json;
            }
        }

        public PomStorage(AppSettings appSettings)
        {
            this.appSettings = appSettings;
            var storagePath = $"{appSettings.StorageFolder}/{appSettings.PomCovid.JsonFileName}";
            this.fileStorage = new FileStorage(storagePath);
        }

        public void Save(string data)
        {
            this.json = data;
            this.fileStorage.Save(data);
        }
    }
}
