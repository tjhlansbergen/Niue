namespace Niue;

public class JobController
{
    public async Task<IResult> GetAsync()
    {
        // TODO
        return Results.Ok(Enumerable.Range(1, 5));
    }

    public async Task<IResult> GetAsync(int id)
    {
        // TODO
        var job = new Job { Id = id, Name = "Testjob", IntervalMinutes = 5 };


        return Results.Ok(job);
    }

    public async Task<IResult> PostAsync(Job job)
    {
        // TODO add job
        return Results.Created($"{Routes.job}/{job.Id}", job);
    }
}