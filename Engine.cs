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
        await Parallel.ForEachAsync(jobsToRun, new ParallelOptions { MaxDegreeOfParallelism = 20 }, async (job, ct) => await ExecuteJob(job, ct));
    }

    private async Task ExecuteJob(Job job, CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return;
        }

        // run
        Console.WriteLine($"Executing job {job.Id}");
        var result = RunScript(job.Script);

        // log
        using (var c = new DataContext(_configuration))
        {
            // write log
            await c.Logs.AddAsync(new Log { JobId = job.Id, Date = DateTime.UtcNow, RunMode = RunMode.SCHEDULED, Output = result });

            // update due date
            var dbjob = await c.Jobs.FindAsync(job.Id);
            if (dbjob != null)
            {
                dbjob.DueDateUTC = !dbjob.IntervalMinutes.HasValue ? null : DateTime.UtcNow.AddMinutes(dbjob.IntervalMinutes.Value);
                await c.SaveChangesAsync();
            }
        }
    }

    public static string RunScript(string script)
    {
        

        // TODO
        // new instance of the interpreter,
        // execute,
        return script; // for now, just echo back the script
    }
}