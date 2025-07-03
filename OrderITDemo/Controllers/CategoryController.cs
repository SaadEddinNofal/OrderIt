using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using NuGet.Protocol;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Repository.Base;

namespace OrderITDemo.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        public CategoryController(IRepository<Category> repository)
        {
            _categoryRepository = repository;
        }
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();   
            return View(categories);
        }
        [HttpGet]
        public IActionResult Add(int id = 0)
        {
            if (id != 0)
            {
                var category = _categoryRepository.FindById(id);
                if(category == null) { return NotFound();}
                return View(category);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category , int id)
        {
            if (!ModelState.IsValid) { return View(category); }
            if (id == 0)
            {
                _categoryRepository.Add(category);
                _categoryRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                category.CategoryId = id;
                _categoryRepository.Update(category);
                _categoryRepository.Save();
                return RedirectToAction(nameof(Index));
            } 
        }
      
        public IActionResult Delete(int id)
        {
           
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
