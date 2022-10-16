namespace Niue;
using Microsoft.EntityFrameworkCore;

public class JobController
{
    private readonly DataContext _context;

    public JobController(DataContext context)
    {
        _context = context;
    }

    public async Task<IResult> GetAsync()
    {
        var jobIds = await _context.Jobs.Select(j => new { Id = j.Id, Name = j.Name }).ToListAsync();
        return Results.Ok(jobIds);
    }

    public async Task<IResult> GetAsync(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        return (job == null) ? Results.NotFound() : Results.Ok(job);
    }

    public async Task<IResult> PostAsync(Job job)
    {
        if (string.IsNullOrWhiteSpace(job.Script))
        {
            return Results.BadRequest("Please include a script");
        }


        await _context.Jobs.AddAsync(job);
        await _context.SaveChangesAsync();
        return Results.Created($"{Routes.job}/{job.Id}", job);
    }
}