namespace Niue;

public class Engine
{
    private readonly IConfiguration _configuration;

    // note that a dbcontext isn't injected here but because of multi-threading this class
    // instanciates and disposes seperate instances of DataContext for each task
    public Engine(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Kick()
    {
        var jobsToRun = new List<Job>();

        // get work
        using (var c = new DataContext(_configuration))
        {
            jobsToRun = c.Jobs.Where(j => j.IntervalMinutes != null
                                        && j.IntervalMinutes > 0
                                        && j.DueDateUTC != null
                                        && j.DueDateUTC <= DateTime.UtcNow)
                                        .ToList();
        }

        // do work
        await Parallel.ForEachAsync(jobsToRun, new ParallelOptions { MaxDegreeOfParallelism = 20 }, async (job, ct) => await Execute(job, ct));
    }

    private async Task Execute(Job job, CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return;
        }

        Console.WriteLine($"Executing job {job.Id}");

        // TODO
        // new instance of the interpreter,
        // execute,
        // write output to a log record

        // last but not least
        using (var c = new DataContext(_configuration))
        {
            var dbjob = await c.Jobs.FindAsync(job.Id);
            if (dbjob != null)
            {
                dbjob.DueDateUTC = !dbjob.IntervalMinutes.HasValue ? null : DateTime.UtcNow.AddMinutes(dbjob.IntervalMinutes.Value);
                await c.SaveChangesAsync();
            }
        }
    }
}