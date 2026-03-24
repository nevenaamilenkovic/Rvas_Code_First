using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RvasApp.Data;
using RvasApp.Konstante;
using RvasApp.Models;
using System.Security.Claims;

namespace RvasApp.Controllers
{
    [Authorize]//samo ulogovani useri mogu da pristupe
    public class KursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KursController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var kursevi = await _context.Kursevi
                .Include(k=>k.Instruktor)
                .OrderBy(k=>k.Naziv)
                .ToListAsync();

            return View(kursevi);
        }

        [Authorize(Roles =nameof(Roles.Instruktor))]
        //sredi formu na create viewu!
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = nameof(Roles.Instruktor))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Opis,DatumPocetka,DatumZavrsetka,MaksimalanBrojPolaznika")] Kurs kurs)
        {
            if(!ModelState.IsValid)
                return View(kurs);

            var instruktorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            kurs.InstruktorId=instruktorId;

            _context.Kursevi.Add(kurs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return NotFound();

            var kurs = await _context.Kursevi
                .Include(k => k.Instruktor)
                .FirstOrDefaultAsync(k=>k.Id==id.Value);
            if(kurs == null)
                return NotFound();

            return View(kurs);

        }



    }
}
