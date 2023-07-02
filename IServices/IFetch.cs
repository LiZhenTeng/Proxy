namespace Proxy.IServices;
public interface IFetch
{
    public Task<object?> FetchTMDB(string key, string path, string query);
    public Task<(byte[]?,string)> FetchImage(string key,string url, string imageName);
}