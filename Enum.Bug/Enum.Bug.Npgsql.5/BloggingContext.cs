using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enum.Bug.Npgsql
{
    
    public class BloggingContext : DbContext
    {
        /// <summary>
        /// Fast way to ensure we dont store some secret here.
        /// </summary>
        public static string DataBasePasswordHack = String.Empty;

        public static void SetupFromUser()
        {
            Console.WriteLine("Please enter the password for the database:");
            BloggingContext.DataBasePasswordHack = Console.ReadLine()
                ?? throw new ArgumentNullException("Password");
        }

        public static void RunScenarioWithEnumBug()
        {
            BloggingContext.SetupFromUser();
            using var dbcontext = new BloggingContext();

            var conditions = new System.Enum[] { PostType.Warning, PostType.Info };

            dbcontext.Database.EnsureCreated();
            
            var result = dbcontext.Posts.Where(x => conditions.Contains(x.PostType)).ToArray();

            foreach (var item in result)
                Console.WriteLine($"{item.PostType}");

            Console.WriteLine("Query run successfully.");
            Console.ReadLine();
        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host=localhost;Database=postgres;Username=postgres;Password={DataBasePasswordHack}");
        }            
}

public class Blog
{
    public int BlogId { get; set; }
    public string Url { get; set; }

    public List<Post> Posts { get; set; }
}

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public PostType PostType { get; set; }

    public int BlogId { get; set; }
    public Blog Blog { get; set; }
}

public enum PostType
{
    None = 0,
    Info = 1,
    Warning = 2,
    Hobby = 4,
}
}
