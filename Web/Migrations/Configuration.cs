namespace DigitalMarketing.Web.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Models.ApplicationDbContext context)
        {
            context.Experiences.AddOrUpdate(o => o.Name,
                new Models.Experience
                {
                    Name = "Explore the exotics",
                    Photo = "/Content/exotic 2.jpg"
                },
                new Models.Experience
                {
                    Name = "Capture a lifetime moment",
                    Photo = "/Content/lifetime.jpg"
                },
                new Models.Experience
                {
                    Name = "Escape from the crowd",
                    Photo = "/Content/escape.jpg"
                }
            );
        }
    }
}
