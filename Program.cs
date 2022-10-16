namespace Niue;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<DataContext>();
        builder.Services.AddTransient<JobController>();

        var app = builder.Build();

        //app.UseHttpsRedirection();

        // jobs
        app.MapGet(Routes.jobs, async (JobController controller) => await controller.GetAsync());
        app.MapGet(Routes.job, async (int id, JobController controller) => await controller.GetAsync(id));
        app.MapPost(Routes.jobs, async (Job job, JobController constroller) => await constroller.PostAsync(job));


        app.Run();
    }
}