namespace Proxy.Models;

public record TMDB
{
    public int? page { get; set; }
    public string? language { get; set; }
    public string? append_to_response { get; set; }
    public string? include_image_language { get; set; }
    public string? with_genres { get; set; }
    public string? query { get; set; }
    public bool? include_adult { get; set; }
    public string GetKey()
    {
        var fields = new List<object>();
        if (this.page != null)
            fields.Add(this.page);
        if (!string.IsNullOrEmpty(this.language))
            fields.Add(this.language);
        if (!string.IsNullOrEmpty(this.append_to_response))
            fields.Add(this.append_to_response);
        if (!string.IsNullOrEmpty(this.include_image_language))
            fields.Add(this.include_image_language);
        if (!string.IsNullOrEmpty(this.with_genres))
            fields.Add(this.with_genres);
        if (!string.IsNullOrEmpty(this.query))
            fields.Add(this.query);
        if (this.include_adult != null)
            fields.Add(this.include_adult);
        return string.Join("-", fields.ToArray());
    }
}