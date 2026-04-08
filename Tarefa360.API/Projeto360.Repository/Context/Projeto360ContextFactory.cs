using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

public class Projeto360ContextFactory 
    : IDesignTimeDbContextFactory<Projeto360Context>
{
    public Projeto360Context CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var dbPath = Path.Combine(
            basePath,
            "..",
            "Projeto360.Repository",
            "projeto360.sqlite"
        );

        var optionsBuilder = new DbContextOptionsBuilder<Projeto360Context>();
        optionsBuilder.UseSqlite($"Data Source={dbPath}");

        return new Projeto360Context(optionsBuilder.Options);
    }
}