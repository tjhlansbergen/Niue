var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string jobs = "/jobs";

//app.UseHttpsRedirection();

app.MapGet(jobs, () =>
{
    var jobs =  Enumerable.Range(1, 5).Select(index => new Job()).ToArray();
    return jobs;
})
.WithName("Jobs");


// curl -i -X POST -H "Content-Type: application/json" -d '{"name": "ha"}' localhost:5271/jobs
app.MapPost(jobs, async (Job job) => 
{
    // await db save
    var g = Guid.NewGuid();
    return Results.Created($"{jobs}/{g}", g);
});

app.Run();

record Job
{
    public string Name {get; set;} = "name";
}