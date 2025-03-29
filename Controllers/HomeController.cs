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
    #region Ana Sayfa Kýsmý 

   
    public IActionResult Index() // Ana Sayfa
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }
    #endregion


    #region Kategori Notlarý Kýsmý
    public IActionResult CategoryNote(int Id) // Kategoriye ait notlar
    {
        var categoryNote = _context.Notes.Include(n=> n.Category).Where(n => n.CategoryId == Id && n.IsArchive == false && n.IsDeleted == false).ToList();
        ViewBag.CategoryName = _context.Categories.Find(Id).Name;
        return View(categoryNote);
    }
    #endregion

    #region Not Detaylarý Kýsmý
    public IActionResult NoteDetails(int Id)  // Not Detaylarý
    {
         var noteDetails = _context.Notes.Include(n => n.Category).FirstOrDefault(n => n.Id == Id);

        if (noteDetails == null)
        {
            return NotFound();
        }

        return View(noteDetails);
    }
    #endregion

    #region Arþive Yollama Kýsmý

    public IActionResult SendToArchive(int Id) // Notu Arþive Gönder
    {
        var note = _context.Notes.Find(Id);
        note.IsArchive = true;
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    #endregion

    #region Not Silme Kýsmý 
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
    #endregion

    #region Not Güncelleme Ýþlemleri
    [HttpGet]   
    public IActionResult UpdateNote(int Id) // Notu Getir
    {
        var note = _context.Notes.SingleOrDefault(n=> n.Id == Id);
        if (note == null)
        {
            return NotFound();
        }
        ViewBag.Categories = _context.Categories.ToList();
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
    #endregion

    #region Kategori Ekleme Kýsmý
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
        var addCategory = _context.Categories.Add(categoryModel);
        if (addCategory == null)
        {
            return NotFound();
        }
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    #endregion

    #region Not Ekleme Kýsmý
    [HttpGet]   
    public IActionResult AddNote() // Not Ekle sayfasýna yöneltir.
    {
        ViewBag.Categories = _context.Categories.ToList();

        return View();
    }
    [HttpPost]
    public IActionResult AddNote(Note noteModel) // Not Ekle
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(noteModel);
        }
        noteModel.CreatedDate = DateTime.Now;
        noteModel.UpdateDate = DateTime.Now;
        noteModel.IsDeleted = false;
        noteModel.IsArchive = false;

        _context.Notes.Add(noteModel);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    #endregion

    #region Tüm Notlarý Göster Kýsmý 
    public IActionResult AllNotes() 
    {
        var allNotes = _context.Notes.Include(n => n.Category).Where(n => n.IsArchive == false && n.IsDeleted == false).ToList();
        if (allNotes == null)
        {
            return NotFound();
        }
        return View(allNotes);
    }
    #endregion

    #region Arþivlenen Tum Notlarý Göster Kýsmý
    public IActionResult ArchiveNotes() 
    {
        var allArchiveNotes = _context.Notes.Include(n => n.Category).Where(n => n.IsArchive == true && n.IsDeleted == false).ToList();
        if (allArchiveNotes == null) 
        {
            return NotFound();
        }
        return View(allArchiveNotes); 
    }
    #endregion

    #region Silinen Tum Notlarý Göster Kýsmý
    public IActionResult AllDeleteNotes() 
    {
        var allDeleteNotes = _context.Notes.Include(n => n.Category).Where(n => n.IsDeleted == true).ToList();
        if (allDeleteNotes == null)
        {
            return NotFound();
        }
        return View(allDeleteNotes);
    }
    #endregion

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
