using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ToDoList.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {

            // Проверка за валидност на модела
            if (ModelState.IsValid)
            {
                return View(user); // Връщане на формата с грешките
            }
            try
            {
                // Добавяне на потребителя към контекста
                _context.Users.Add(user);
                _context.SaveChanges();

                // Пренасочване към Index при успех
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Логване на грешката (ако е нужно)
                Console.WriteLine(ex.Message);
                ModelState.AddModelError("", "An error occurred while saving the user.");
            }

            // Ако възникне грешка, връща формата с въведените данни
            return View(user);
        }


        // GET: Users/Edit/5
        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.UserId)
            {
                return NotFound(); // Ако ID не съвпада, връща 404
            }

            if (ModelState.IsValid)
            {
                return View(user); // Връщане на формата с грешките
            }

            try
            {
                // Обновяване на записа в базата данни
                _context.Users.Update(user);
                await _context.SaveChangesAsync(); // Асинхронно запазване

                // Пренасочване към Index след успешно обновяване
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Проверка дали записът все още съществува
                if (!_context.Users.Any(e => e.UserId == id))
                {
                    return NotFound(); // Ако потребителят не съществува
                }
                else
                {
                    throw; // Ако е друга грешка, хвърля отново изключението
                }
            }
            catch (Exception ex)
            {
                // Логване на други потенциални грешки
                Console.WriteLine($"Error while updating user: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the user.");
            }

            // Връща изгледа с въведените данни в случай на грешка
            return View(user);
        }


        // GET: Users/Delete/5
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        


    }
}
