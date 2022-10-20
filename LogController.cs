using Microsoft.EntityFrameworkCore;

namespace Niue;

public class LogController
{
    private readonly DataContext _context;

    public LogController(DataContext context)
    {
        _context = context;
    }

    public async Task<IResult> GetByJobAsync(int jobId)
    {
        var logs = await _context.Logs.Where(l => l.JobId == jobId)
            .Select(l => new { LogId = l.Id, JobId = l.JobId, Date = l.Date, RunMode = l.RunMode.ToString() })
            .ToListAsync();
        return (logs == null) ? Results.NotFound() : Results.Ok(logs);
    }
}