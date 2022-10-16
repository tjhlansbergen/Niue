namespace Niue;

public record Job
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Script { get; set; } = string.Empty;
    public int IntervalMinutes { get; set; }
    public DateTime? DueDate { get; set; }
}