using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            // Проверка за валидност на модела
            if (ModelState.IsValid)
            {
                return View(category); // Връщане на формата с грешките
            }

            try
            {
                // Задаване на потребителския ID (ако е необходимо)
                var userId = GetCurrentUserId(); // Метод за извличане на текущия потребител
                category.UserId = userId;

                // Добавяне на категорията към контекста
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(); // Асинхронно запазване в базата

                // Пренасочване към Index при успех
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Логване на грешката (ако възникне)
                Console.WriteLine($"Error while saving category: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the category.");
            }

            // Ако възникне грешка, връща формата с въведените данни
            return View(category);
        }

        // Метод за получаване на текущия потребител (пример)
        private int GetCurrentUserId()
        {
            return 1;
        }



        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return View(category);
            }

            try
            {
                // Задаване на потребителския ID (ако е необходимо)
                var userId = GetCurrentUserId(); // Метод за извличане на текущия потребител
                category.UserId = userId;

                _context.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while updating category: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the category.");
            }

            return View(category);
        }



        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.User) // Include the related User entity
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index), "Categories");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

    }
}
