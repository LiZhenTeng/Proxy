namespace Proxy.Models;

public class NeteaseCloudMusic
{
    public string GetKey()
    {
        var fields = new List<object>();
        return string.Join("-", fields.ToArray());
    }

}