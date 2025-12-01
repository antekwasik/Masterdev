using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Zadanie5_test.Data;
using Zadanie5_test.Models;

namespace Zadanie5_test.Controllers
{
    public class Test : Controller
    {
        private readonly Zadanie5_testContext _Zadanie5_testContext;

        public Test(Zadanie5_testContext zadanie5_testContext)
        {
            _Zadanie5_testContext = zadanie5_testContext;
        }

        public async Task<IActionResult> Index()
        {
            var klient = await _Zadanie5_testContext.klienci.ToListAsync();
            return View(klient);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create([Bind("id,name,surname,pesel,birthyear,płeć")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                _Zadanie5_testContext.klienci.Add(klient);
                await _Zadanie5_testContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(klient);
        }
        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var klient = await _Zadanie5_testContext.klienci.FindAsync(id);
            if (klient != null)
            {
                _Zadanie5_testContext.klienci.Remove(klient);
                await _Zadanie5_testContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
