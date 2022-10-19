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
        var jobIds = await _context.Jobs.Select(j => new { Id = j.Id, Name = j.Name, IntervalMinutes = j.IntervalMinutes, DueDate = j.DueDateUTC }).ToListAsync();
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
        return Results.Created($"{Routes.jobs_id}/{job.Id}", job);
    }

    public async Task<IResult> PutAsync(int id, Job incomingJob)
    {
        // note that we use the ID from the query string here to find the existing entity
        var existingJob = await _context.Jobs.FindAsync(id);

        if (existingJob == null) { return Results.NotFound(); }

        // manually update the entity, ignoring the ID of the incoming item
        existingJob.Name = string.IsNullOrWhiteSpace(incomingJob.Name) ? existingJob.Name : incomingJob.Name;
        existingJob.Script = string.IsNullOrWhiteSpace(incomingJob.Script) ? existingJob.Script : incomingJob.Script;
        existingJob.IntervalMinutes = incomingJob.IntervalMinutes == null ? existingJob.IntervalMinutes : incomingJob.IntervalMinutes;
        existingJob.DueDateUTC = incomingJob.DueDateUTC == null ? existingJob.DueDateUTC : incomingJob.DueDateUTC;

        await _context.SaveChangesAsync();
        return Results.Ok(existingJob);
    }

    public async Task<IResult> Delete(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) { return Results.NotFound(); }

        _context.Remove(job);
        await _context.SaveChangesAsync();

        return Results.NoContent();
    }
}