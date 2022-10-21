using Microsoft.EntityFrameworkCore;

namespace Niue;

public class LogController
{
    private readonly DataContext _context;

    public LogController(DataContext context)
    {
        _context = context;
    }

    public async Task<IResult> GetAsync(int id)
    {
        var log = await _context.Logs.FindAsync(id);
        return (log == null) ? Results.NotFound() : Results.Ok(new { 
            log.Id, 
            log.JobId, 
            log.Date, 
            RunMode = log.RunMode.ToString(),
            log.Output });
    }

    public async Task<IResult> GetByJobAsync(int jobId, int? limit)
    {
        var take = limit ?? 1000;

        var logs = await _context.Logs.Where(l => l.JobId == jobId)
            .Select(l => new { 
                LogId = l.Id, 
                JobId = l.JobId, 
                Date = l.Date, 
                RunMode = l.RunMode.ToString(),
                l.Output })
            .Take(take)
            .OrderByDescending(l => l.Date)
            .ToListAsync();
        return (logs == null) ? Results.NotFound() : Results.Ok(logs);
    }

}