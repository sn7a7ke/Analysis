namespace Analysis.Data.Interface
{
    public interface IStorage
    {
        void Save(string data);
        string Load();
    }
}
