// See https://aka.ms/new-console-template for more information
using Enum.Bug.Npgsql;

Console.WriteLine("Hello, World!");

using var dbcontext = new BloggingContext();

var conditions = new System.Enum[] { PostType.Warning, PostType.Info };

dbcontext.Database.EnsureCreated();
var result = dbcontext.Posts.Where(x => conditions.Contains(x.PostType)).ToArray();

foreach (var item in result)
{
    Console.WriteLine($"{item.PostType}");
}

Console.ReadLine();
