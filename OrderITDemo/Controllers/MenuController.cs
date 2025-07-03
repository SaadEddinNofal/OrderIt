using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderITDemo.Data;
using OrderITDemo.Models;
using OrderITDemo.Repository.Base;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace OrderITDemo.Controllers
{

    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Menu> _repository;
        private readonly IHostingEnvironment _host;
        [Obsolete]
        public MenuController(IRepository<Menu> repository, ApplicationDbContext context, IHostingEnvironment host)
        {
            _host = host;
            _context = context;
            _repository = repository;
        }
        public void CreatChoose(int e = 0)
        {
            ViewBag.E = _context.Categories.ToList();
        }
        public IActionResult IndexUser()
        {
            CreatChoose();
            IEnumerable<Category> menu = _context.Categories.Include(m => m.Menu).ToList();

            return View(menu);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            CreatChoose();
            IEnumerable<Category> menu = _context.Categories.Include(m => m.Menu).ToList();
            return View(menu);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add(int id = 0)
        {
            CreatChoose();
            if (id != 0)
            {
                var menu = _repository.FindById(id);
                if (menu == null) { return NotFound(); }
                return View(menu);
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Menu menu, int id)
        {
            if (!ModelState.IsValid) { return View(menu); }
            if (id == 0)
            {
               
                if (menu.MenuFile !=null)
                {
                    MemoryStream stream = new MemoryStream();
                    menu.MenuFile.CopyTo(stream);
                    menu.ItemPicture = stream.ToArray();
                }   
                _repository.Add(menu);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var existingMenu = _repository.FindById(id);
                existingMenu.Name = menu.Name;
                existingMenu.Price = menu.Price;
                existingMenu.CategoryId = menu.CategoryId;
                existingMenu.Description = menu.Description;
                existingMenu.MenuFile = menu.MenuFile;
                existingMenu.IsExist = menu.IsExist;
                /*existingMenu.ItemPicture = menu.ItemPicture;*/
                if (menu.MenuFile != null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        menu.MenuFile.CopyTo(stream);
                        existingMenu.ItemPicture = stream.ToArray();
                    }
                }
                _repository.Update(existingMenu);
                _repository.Save();
                return RedirectToAction(nameof(Index));
            }

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            _repository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
