namespace Analysis.Data.Interface
{
    public interface IRawStorage
    {
        void Save(string data);
        string Load();
    }
}
