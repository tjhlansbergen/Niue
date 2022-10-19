namespace Niue;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<DataContext>();
        builder.Services.AddTransient<JobController>();
        builder.Services.AddTransient<Engine>();

        var app = builder.Build();
        //app.UseHttpsRedirection();

        // jobs
        app.MapGet(Routes.jobs, async (JobController c) => await c.GetAsync());
        app.MapGet(Routes.jobs_id, async (int id, JobController c) => await c.GetAsync(id));
        app.MapPost(Routes.jobs, async (Job job, JobController c) => await c.PostAsync(job));
        app.MapPut(Routes.jobs_id, async (int id, Job job, JobController c) => await c.PutAsync(id, job));
        app.MapDelete(Routes.jobs_id, async (int id, JobController c) => await c.Delete(id));

        // logs

        // kick
        app.MapGet(Routes.kick, async (Engine e) => await e.Kick());

        app.Run();
    }
}