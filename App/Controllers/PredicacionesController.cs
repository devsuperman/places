using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Data;

namespace App.Controllers;

[Authorize]
public class PredicacionesController : Controller
{
    private readonly Contexto _db;

    public PredicacionesController(Contexto db)
    {
        _db = db;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(int territorioId = 0, DateTime? mesAno = null)
    {
        mesAno ??= DateTime.Today;

        var listaTerritorios = await _db.Territorios.AsNoTracking().OrderBy(o => o.Nombre).ToListAsync();

        ViewData["selectTerritorios"] = new SelectList(listaTerritorios, "Id", "Nombre", territorioId);
        ViewData["mesAno"] = mesAno.Value.ToString("yyyy-MM");

        var query = _db.Predicaciones
            .AsNoTrackingWithIdentityResolution()
            .Include(a => a.Territorio)
            .Where(w =>
                w.Fecha.Month == mesAno.Value.Month &&
                w.Fecha.Year == mesAno.Value.Year);

        if (territorioId > 0)
            query = query.Where(w => w.TerritorioId == territorioId);

        var lista = await query
            .OrderByDescending(o => o.Fecha)
            .ToListAsync();

        return View(lista);
    }

    public async Task<IActionResult> Anadir()
    {
        var model = new Predicacion
        {
            Fecha = DateTime.Today
        };

        await CarregarViewDatas();

        return View(model);
    }

    private async Task CarregarViewDatas(int territorioId = 0, string dirigente = null)
    {
        var territorios = await _db.Territorios
            .AsNoTracking()
            .OrderBy(o => o.Nombre)
            .Select(s => new
            {
                s.Id,
                s.Nombre
            })
            .ToListAsync();

        var listaDirigentes = new string[] {
                "Denilson",
                "Gustavo",
                "Herminio",
                "Tiago"
            };

        ViewData["selectTerritorios"] = new SelectList(territorios, "Id", "Nombre", territorioId);
        ViewData["selectDirigentes"] = new SelectList(listaDirigentes, dirigente);
    }

    [HttpPost]
    public async Task<IActionResult> Anadir(Predicacion model)
    {
        if (ModelState.IsValid)
        {
            _db.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        await CarregarViewDatas(model.TerritorioId, model.Dirigente);

        return View(model);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var model = await _db.Predicaciones.FindAsync(id);

        await CarregarViewDatas(model.TerritorioId, model.Dirigente);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Predicacion model)
    {
        if (ModelState.IsValid)
        {
            _db.Update(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        await CarregarViewDatas(model.TerritorioId, model.Dirigente);
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Deletar(int id)
    {
        var model = await _db.Predicaciones.FindAsync(id);

        _db.Predicaciones.Remove(model);

        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
