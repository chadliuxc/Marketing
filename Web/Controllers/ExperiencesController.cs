using DigitalMarketing.Web.Models;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DigitalMarketing.Web.Controllers
{
    public class ExperiencesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Experiences
        public async Task<ActionResult> Index()
        {
            var experiences = db.Experiences;
            return View(await experiences.ToListAsync());
        }

        // GET: Experiences/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = await db.Experiences.FindAsync(id);
            if (experience == null)
            {
                return HttpNotFound();
            }
            return View(experience);
        }

        // GET: Experiences/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Experiences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Photo")] Experience experience)
        {
            if (ModelState.IsValid)
            {
                db.Experiences.Add(experience);
                await db.SaveChangesAsync();
                IDatabase cache = Connection.GetDatabase();
                cache.KeyDelete(Constants.RedisCacheKey);
                return RedirectToAction("Index");
            }

            return View(experience);
        }

        // GET: Experiences/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = await db.Experiences.FindAsync(id);
            if (experience == null)
            {
                return HttpNotFound();
            }

            return View(experience);
        }

        // POST: Experiences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,CategoryId,Name,Photo,Description")] Experience experience)
        {
            if (ModelState.IsValid)
            {
                db.Entry(experience).State = EntityState.Modified;
                await db.SaveChangesAsync();
                IDatabase cache = Connection.GetDatabase();
                cache.KeyDelete(Constants.RedisCacheKey);
                return RedirectToAction("Index");
            }

            return View(experience);
        }

        // GET: Experiences/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Experience experience = await db.Experiences.FindAsync(id);
            if (experience == null)
            {
                return HttpNotFound();
            }
            return View(experience);
        }

        // POST: Experiences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Experience experience = await db.Experiences.FindAsync(id);
            db.Experiences.Remove(experience);
            await db.SaveChangesAsync();
            IDatabase cache = Connection.GetDatabase();
            cache.KeyDelete(Constants.RedisCacheKey);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // Redis Connection string info
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
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
