using DigitalMarketing.Web.Models;
using DigitalMarketing.Web.Utils;
using StackExchange.Redis;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            // Connection refers to a property that returns a ConnectionMultiplexer
            // as shown in the previous example.
            IDatabase cache = Connection.GetDatabase();

            string xml = cache.StringGet(Constants.RedisCacheKey);

            if (string.IsNullOrEmpty(xml))
            {
                var experiences = db.Experiences.OrderByDescending(p => p.Id).Take(3)
                .Select(experience => new TopExperience {
                    Name = experience.Name,
                    Id = experience.Id,
                    Photo = experience.Photo
                }).ToArray();

                xml = ObjectHelper.SerializeObject(experiences);

                cache.StringSet(Constants.RedisCacheKey, xml);
            }

            return View(ObjectHelper.XmlDeserializeFromString<TopExperience[]>(xml));
        }

        // Redis Connection string info
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = Microsoft.Azure.CloudConfigurationManager.GetSetting("CacheConnection").ToString();
            return ConnectionMultiplexer.Connect(cacheConnection);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}