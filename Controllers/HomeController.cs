using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTaking.Data;
using NoteTaking.Models;

namespace NoteTaking.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly AppDbContext _context;
    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index() // Ana Sayfa
    {
        var categories = _context.Categorys.ToList();
        return View(categories);
    }

    public IActionResult CategoryNote(int Id) // Kategoriye ait notlar
    {
        var categoryNote = _context.Notes.Include(n=> n.Category).Where(n => n.CategoryId == Id && n.IsArchive == false && n.IsDeleted == false).ToList();
        ViewBag.CategoryName = _context.Categorys.Find(Id).Name;
        return View(categoryNote);
    }

    public IActionResult NoteDetails(int Id)  // Not Detaylarý
    {
         var noteDetails = _context.Notes.Include(n => n.Category).FirstOrDefault(n => n.Id == Id);

        return View(noteDetails);
    }

    public IActionResult SendToArchive(int Id) // Notu Arþive Gönder
    {
        var note = _context.Notes.Find(Id);
        note.IsArchive = true;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult DeleteNote(int Id) // Notu Sil
    {
        var note = _context.Notes.SingleOrDefault(n => n.Id == Id);
        if (note == null)
        {
            return NotFound();
        }
        note.IsDeleted = true;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpGet]   
    public IActionResult UpdateNote(int Id) // Notu Getir
    {
        var note = _context.Notes.SingleOrDefault(n=> n.Id == Id);
        if (note == null)
        {
            return NotFound();
        }
        ViewBag.Categories = _context.Categorys.ToList();
        return View(note);
    }

    [HttpPost]
    public IActionResult UpdateNote(Note noteModel)
    {
       var note = _context.Notes.SingleOrDefault(n => n.Id == noteModel.Id);
        if (note == null)
        {
            return NotFound();
        }
        note.Title = noteModel.Title;
        note.Description = noteModel.Description;
        note.CategoryId = noteModel.CategoryId;
        note.UpdateDate = DateTime.Now;
        _context.SaveChanges();
        return RedirectToAction("Index");
        
    }
    [HttpGet]
    public IActionResult AddCategory()  // Kategori Ekle sayfasýna yöneltir.
    {
       
        return View();
    }
    [HttpPost]
    public IActionResult AddCategory(Category categoryModel)  // Kategori Ekle
    {
        if (!ModelState.IsValid)
        {
            return View(categoryModel);
        }
        var addCategory = _context.Categorys.Add(categoryModel);
        if (addCategory == null)
        {
            return NotFound();
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    [HttpGet]   
    public IActionResult AddNote() // Not Ekle sayfasýna yöneltir.
    {
        ViewBag.Categories = _context.Categorys.ToList();

        return View();
    }
    [HttpPost]
    public IActionResult AddNote(Note noteModel) // Not Ekle
    {
       

        
        noteModel.CreatedDate = DateTime.Now;
        noteModel.UpdateDate = DateTime.Now;
        noteModel.IsDeleted = false;
        noteModel.IsArchive = false;

        _context.Notes.Add(noteModel);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult AllNotes() 
    {
        var allNotes = _context.Notes.Include(n => n.Category).Where(n => n.IsArchive == false && n.IsDeleted == false).ToList();
        return View(allNotes);
    }
    public IActionResult ArchiveNotes() 
    {
        var allArchiveNotes = _context.Notes.Include(n => n.Category).Where(n => n.IsArchive == true && n.IsDeleted == false).ToList();
        return View(allArchiveNotes); 
    }
    public IActionResult AllDeleteNotes() 
    {
        var allDeleteNotes = _context.Notes.Include(n => n.Category).Where(n => n.IsDeleted == true).ToList();
        return View(allDeleteNotes);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
