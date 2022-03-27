using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using dashboard_web.Data;
using dashboard_web.Models;
using Microsoft.AspNetCore.Authorization;
using static dashboard_web.Program;
using Newtonsoft.Json;

namespace dashboard_web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            GetFileName fn = new GetFileName();
            var listOfCredentials = await _context.Credentials.ToListAsync();
            Credentials cd = new Credentials();
            var message = "";

            foreach (var credential in listOfCredentials)
            {
                if (credential.ID == 1)
                {
                    cd = credential;
                    
                }
                if (credential.ID == 2)
                {
                    message = CallWebService("Kolding", credential.Password);
                }
            }

            string file = fn.GetFileNameMethod(cd.UserName, cd.Password);
            string a =  GetFileAndOutPut(cd.UserName, cd.Password, file);

            var result = JsonConvert.DeserializeObject<Weather>(message);

            ViewBag.Temp = result.Temp;
            ViewBag.WindChill = result.Windchill;
            ViewBag.DateAndTime = result.DateAndTime;
            ViewBag.Output = a;

            return View();
        }

        public string GetFileAndOutPut(string un, string pw, string file)
        {
            GetOutput op = new GetOutput();
            int res = op.Getoutput(un, pw, file);
            Output pop = new Output(res);

            return pop.PowerOutput.ToString();
        }

        // GET: Dashboard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var credentials = await _context.Credentials
                .FirstOrDefaultAsync(m => m.ID == id);
            if (credentials == null)
            {
                return NotFound();
            }

            return View(credentials);
        }

        // GET: Dashboard/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserName,Password")] Credentials credentials)
        {
            if (ModelState.IsValid)
            {
                _context.Add(credentials);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(credentials);
        }

        // GET: Dashboard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var credentials = await _context.Credentials.FindAsync(id);
            if (credentials == null)
            {
                return NotFound();
            }
            return View(credentials);
        }

        // POST: Dashboard/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,Password")] Credentials credentials)
        {
            if (id != credentials.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(credentials);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CredentialsExists(credentials.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(credentials);
        }

        // GET: Dashboard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var credentials = await _context.Credentials
                .FirstOrDefaultAsync(m => m.ID == id);
            if (credentials == null)
            {
                return NotFound();
            }

            return View(credentials);
        }

        // POST: Dashboard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var credentials = await _context.Credentials.FindAsync(id);
            _context.Credentials.Remove(credentials);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CredentialsExists(int id)
        {
            return _context.Credentials.Any(e => e.ID == id);
        }
        public class Output
        {
            public int PowerOutput;

            public Output(int pop)
            {
                PowerOutput = pop;
            }
        }
    }
}
