using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using App.Data;

namespace App.Controllers;

[Authorize]
public class TerritoriosController : Controller
{
    private readonly Contexto _db;

    public TerritoriosController(Contexto db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var lista = await _db.Territorios.OrderBy(o => o.Nombre).ToListAsync();
        return View(lista);
    }

    public IActionResult Anadir() => View();

    [HttpPost]
    public async Task<IActionResult> Anadir(Territorio model)
    {
        if (ModelState.IsValid)
        {
            _db.Add(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public async Task<IActionResult> Editar(int id)
    {
        var model = await _db.Territorios.FindAsync(id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Editar(Territorio model)
    {
        if (ModelState.IsValid)
        {
            _db.Update(model);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Deletar(int id)
    {
        var model = await _db.Territorios.FindAsync(id);

        _db.Territorios.Remove(model);

        await _db.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
