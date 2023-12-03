public class Path
{
    string searchScore { get; set; }

    public string content { get; set; }

    public string title { get; set; }

    // Complex fields are included automatically in an index if not ignored.
    public string url { get; set; }

    public string chunkId { get; set; }

    public string last_updated { get; set; }
}
