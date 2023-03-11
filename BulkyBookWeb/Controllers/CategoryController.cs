using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BulkyBookWeb.Controllers;
public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _db.Categories.ToList();
        return View(objCategoryList);
    }


    // GET
    public IActionResult Create()
    {
        return View();
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (ModelState.IsValid && IsCategoryValid(category))
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            TempData["success"] = "Category created succesfully";
            return RedirectToAction("Index");
        }
        return View();
    }



    // GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0) 
            return NotFound();

        var categoryFromDb = _db.Categories.Find(id);
        if (categoryFromDb == null)
            return NotFound();

        return View(categoryFromDb);
    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (ModelState.IsValid && IsCategoryValid(category))
        {
            _db.Categories.Update(category);
            _db.SaveChanges();
            TempData["success"] = "Category updated succesfully";
            return RedirectToAction("Index");
        }
        return View();
    }


    // POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        _db.Categories.Remove(new Category() { Id = id });
        _db.SaveChanges();
        TempData["success"] = "Category deleted succesfully";
        return RedirectToAction("Index");
    }


    private bool IsCategoryValid(Category category)
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("DisplayOrder", "The Display Order cannot exactly match the Name.");
            return false;
        }
        if (category.CreatedDateTime == null)
        {
            category.CreatedDateTime = DateTime.Now;
        }
        return true;
    }
}
