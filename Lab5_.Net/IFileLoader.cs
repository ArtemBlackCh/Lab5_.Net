namespace laba5
{
    public interface IFileLoader
    {
        Task<List<T>?> LoadAsync<T>(string filePath);
    }
}
