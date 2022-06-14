﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using csharp_bibliotecaMvc_due.Data;
using csharp_bibliotecaMvc_due.Models;

namespace csharp_bibliotecaMvc_due.Controllers
{
    public class LibroesController : Controller
    {
        private readonly BibliotecaContext _context;

        public LibroesController(BibliotecaContext context)
        {
            _context = context;
        }

        // GET: Libroes
        public async Task<IActionResult> Index()
        {
            return _context.Libros != null ?
                        View(await _context.Libros.ToListAsync()) :
                        Problem("Entity set 'BibliotecaContext.Libros'  is null.");
        }

        // GET: Libroes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            //var libro = await _context.Libros
            //    .FirstOrDefaultAsync(m => m.LibroID == id);

            var libro = await _context.Libros
               .Include(s => s.Prestito).FirstOrDefaultAsync(m => m.LibroID == id); 

            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        //// GET: Libroes/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // GET: Libroes/Create
        public IActionResult Create()
        {
            //Libro myLibro = new Libro();
            //myLibro.Autori = new List<Autore>();
            //myLibro.Autori.Add(new csharp_bibliotecaMvc_due.Models.Autore { Nome = "Dante", Cognome = "Alighieri", DataNascita = DateTime.Parse("26/04/1340") });
            //myLibro.Autori.Add(new csharp_bibliotecaMvc_due.Models.Autore { Nome = "Giorgio", Cognome = "Bocca", DataNascita = DateTime.Parse("26/04/1933") });

            //return View(myLibro);

            //parte per gestire la lista degli autori
            List<Autore> tuttiautori = new List<Autore>();

            tuttiautori = _context.Autoris.ToList();
            // fine parte per gestire la lista degli autori

            ViewData["listaAutori"] = tuttiautori;

           

            return View();


        }

        //// POST: Libroes/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Titolo,Settore,Scaffale,Stato")] Libro libro)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(libro);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(libro);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro,  string[]? autoris)
        {
            if (ModelState.IsValid)
            {


                if (autoris.Length != 0 )
                {
                    List<Autore> lAutori = new List<Autore>();

                    foreach (var ele in autoris)
                    {
                        Autore dainserire = new Autore();

                        string[] autore = ele.Split("-");

                        dainserire.Cognome = autore[0];
                        dainserire.Nome = autore[1];
                        dainserire.DataNascita = Convert.ToDateTime(autore[2]);


                        var aut = _context.Autoris.Where(m => m.Cognome == dainserire.Cognome).First();

                        lAutori.Add(aut); 
                        

                        
                    }

                    Libro aggiunto = new Libro()
                    {
                        Titolo = libro.Titolo,
                        Scaffale = libro.Scaffale,
                        Stato = libro.Stato,
                        Autori = lAutori
                    };

                    _context.Add(aggiunto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
              
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }




        // GET: Libroes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)



            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libroes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibroID,Titolo,Settore,Scaffale,Stato")] Libro libro)
        {
            if (id != libro.LibroID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.LibroID))
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
            return View(libro);
        }

        // GET: Libroes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Libros == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.LibroID == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Libros == null)
            {
                return Problem("Entity set 'BibliotecaContext.Libros'  is null.");
            }
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return (_context.Libros?.Any(e => e.LibroID == id)).GetValueOrDefault();
        }

        public IActionResult AddAutore(int id)
        {
           

            ViewBag.Id = Convert.ToString(id);

            return View("AddAutore");
        }

        public IActionResult AddAutoreDue(int id)
        {
            //parte per gestire la lista degli autori
            List<Autore> tuttiautori = new List<Autore>();

            tuttiautori = _context.Autoris.ToList();
            // fine parte per gestire la lista degli autori

            ViewData["listaAutori"] = tuttiautori;

            ViewBag.Id = Convert.ToString(id);

            return View("AddAutoreDue");
        }

        // POST: Libroes/AddAutore
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAutore([Bind("IdLibro, Nome, Cognome")] AutoreLibro autoreLibro)
        {
            if (ModelState.IsValid)
            {
                Autore nuovo = new Autore();

                nuovo.Nome = autoreLibro.Nome;
                nuovo.Cognome = autoreLibro.Cognome;

                _context.Autoris.Add(nuovo);

              

                var libro = _context.Libros.FirstOrDefault(m => m.LibroID == autoreLibro.IdLibro);
                if (libro.Autori == null) { libro.Autori = new List<Autore>(); }

                libro.Autori.Add(nuovo);


                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // POST: Libroes/AddAutore
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAutoreDue(int LibroID,  string[] autoris )
        {
            if (ModelState.IsValid)
            {
                var libro = _context.Libros.FirstOrDefault(m => m.LibroID == LibroID);

                foreach (var ele in autoris) 
                {
                    

                    var autore = _context.Autoris.Find(Convert.ToInt32(ele));

                    if (autore != null) 
                    { 

                        if (libro.Autori == null) { libro.Autori = new List<Autore>(); }
                        libro.Autori.Add(autore);

                    }


                }


                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
