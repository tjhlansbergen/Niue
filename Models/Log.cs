namespace Niue;

public record Log
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Record { get; set; } = string.Empty;
    public RunMode RunMode { get; set; }
}

public enum RunMode
{
    ADHOC = 0,
    SCHEDULED = 1
}