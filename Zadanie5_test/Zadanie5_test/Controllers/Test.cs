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
        [HttpPost]
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

        public async Task<IActionResult> Edit(int id)
        {
            var klient = await _Zadanie5_testContext.klienci.FirstOrDefaultAsync(x => x.id == id);
            return View(klient);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("id,name,surname,pesel,birthyear,płeć")] Klient klient)
        {
            if (ModelState.IsValid)
            {
                _Zadanie5_testContext.klienci.Update(klient);
                await _Zadanie5_testContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(klient);
        }

        [HttpGet]
        public IActionResult ImportujKlientow()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportujKlientow(IFormFile plik)
        {
            if (plik == null || plik.Length == 0)
            {
                ModelState.AddModelError("", "Nie wybrano pliku.");
                return View();
            }

            using var reader = new StreamReader(plik.OpenReadStream());
            bool pierwszaLinia = true;

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (pierwszaLinia)
                {
                    pierwszaLinia = false;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var dane = line.Split(';');

                if (dane.Length < 5)
                    continue;

                int birthYear = 0;
                int plec = 0;

                int.TryParse(dane[3], out birthYear);
                int.TryParse(dane[4], out plec);

                var klient = new Klient
                {
                    name = dane[0],
                    surname = dane[1],
                    pesel = dane[2],
                    birthyear = birthYear,
                    płeć = plec
                };

                _Zadanie5_testContext.klienci.Add(klient);
            }

            await _Zadanie5_testContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EksportujKlientowDoCSV()
        {
            var klienci = await _Zadanie5_testContext.klienci.ToListAsync();

            var builder = new System.Text.StringBuilder();
            builder.AppendLine("name;surname;pesel;birthyear;płeć");

            foreach (var k in klienci)
            {
                builder.AppendLine($"{k.name};{k.surname};{k.pesel};{k.birthyear};{k.płeć}");
            }

            return File(
                System.Text.Encoding.UTF8.GetBytes(builder.ToString()),
                "text/csv",
                "klienci.csv"
            );
        }

        [HttpGet]
        public IActionResult ImportCSV()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportCSV(IFormFile plik, string format)
        {
            if (plik == null || plik.Length == 0)
            {
                ModelState.AddModelError("", "Nie wybrano pliku.");
                return View();
            }

            if (format == "csv")
            {
                using var reader = new StreamReader(plik.OpenReadStream());
                bool pierwszaLinia = true;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (pierwszaLinia) { pierwszaLinia = false; continue; }
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var dane = line.Split(';');
                    if (dane.Length < 5) continue;

                    int.TryParse(dane[3], out int birthYear);
                    int.TryParse(dane[4], out int plec);

                    var klient = new Klient
                    {
                        name = dane[0],
                        surname = dane[1],
                        pesel = dane[2],
                        birthyear = birthYear,
                        płeć = plec
                    };

                    _Zadanie5_testContext.klienci.Add(klient);
                }

                await _Zadanie5_testContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else if (format == "xlsx")
            {
                using var stream = new MemoryStream();
                await plik.CopyToAsync(stream);
                using var workbook = new ClosedXML.Excel.XLWorkbook(stream);
                var ws = workbook.Worksheet(1); // pierwsza karta

                bool pierwszaLinia = true;
                foreach (var row in ws.RowsUsed())
                {
                    if (pierwszaLinia) { pierwszaLinia = false; continue; }

                    var klient = new Klient
                    {
                        name = row.Cell(1).GetValue<string>(),
                        surname = row.Cell(2).GetValue<string>(),
                        pesel = row.Cell(3).GetValue<string>(),
                        birthyear = row.Cell(4).GetValue<int>(),
                        płeć = row.Cell(5).GetValue<int>()
                    };

                    _Zadanie5_testContext.klienci.Add(klient);
                }

                await _Zadanie5_testContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return BadRequest("Nieobsługiwany format pliku");
        }

        [HttpGet]
        public async Task<IActionResult> EksportujKlientow(string? format = null)
        {
            if (string.IsNullOrEmpty(format))
            {
                return View(); // pokaż formularz z listą rozwijaną
            }

            var klienci = await _Zadanie5_testContext.klienci.ToListAsync();

            if (format == "csv")
            {
                var builder = new System.Text.StringBuilder();
                builder.AppendLine("name;surname;pesel;birthyear;płeć");

                foreach (var k in klienci)
                {
                    builder.AppendLine($"{k.name};{k.surname};{k.pesel};{k.birthyear};{k.płeć}");
                }

                return File(
                    System.Text.Encoding.UTF8.GetBytes(builder.ToString()),
                    "text/csv",
                    "klienci.csv"
                );
            }
            else if (format == "xlsx")
            {
                using var workbook = new ClosedXML.Excel.XLWorkbook();
                var ws = workbook.Worksheets.Add("Klienci");
                ws.Cell(1, 1).Value = "name";
                ws.Cell(1, 2).Value = "surname";
                ws.Cell(1, 3).Value = "pesel";
                ws.Cell(1, 4).Value = "birthyear";
                ws.Cell(1, 5).Value = "płeć";

                int row = 2;
                foreach (var k in klienci)
                {
                    ws.Cell(row, 1).Value = k.name;
                    ws.Cell(row, 2).Value = k.surname;
                    ws.Cell(row, 3).Value = k.pesel;
                    ws.Cell(row, 4).Value = k.birthyear;
                    ws.Cell(row, 5).Value = k.płeć;
                    row++;
                }

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "klienci.xlsx"
                );
            }

            return BadRequest("Nieobsługiwany format");
        }

    }
}  
