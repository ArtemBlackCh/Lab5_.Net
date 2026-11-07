namespace laba5
{
    public interface IFileSaver
    {
        Task SaveAsync<T>(string filePath, List<T> data);
    }
}
