﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
            var tasks = await _context.ToDoItems
                .Include(t => t.Category)
                .ToListAsync();
            return View(tasks); // Връща изглед с всички задачи
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ToDoItem toDoItem)
        {
            // Проверка за валидност на модела
            if (ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", toDoItem.CategoryId);
                return View(toDoItem); // Връщане на формата с грешките
            }

            try
            {
                

                // Добавяне на задачата към контекста
                _context.ToDoItems.Add(toDoItem);
                await _context.SaveChangesAsync(); // Асинхронно запазване в базата

                // Пренасочване към Index при успех
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Логване на грешката (ако възникне)
                Console.WriteLine($"Error while saving to-do item: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while saving the to-do item.");
            }

            // Ако възникне грешка, връща формата с въведените данни
            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", toDoItem.CategoryId);
            return View(toDoItem);
        }

        // Метод за получаване на текущия потребител (пример)
        private int GetCurrentUserId()
        {
            return 1; // Примерен потребителски ID
        }



        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.ToDoItems.FindAsync(id);
            if (task == null) return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", task.CategoryId);
            return View(task);
        }

        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ToDoItem toDoItem)
        {
            if (id != toDoItem.ToDoItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", toDoItem.CategoryId);
                return View(toDoItem); // Връщане на формата с грешките
            }

            try
            {
                

                _context.Update(toDoItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(toDoItem.ToDoItemId))
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
                Console.WriteLine($"Error while updating to-do item: {ex.Message}");
                ModelState.AddModelError("", "An error occurred while updating the to-do item.");
            }

            ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name", toDoItem.CategoryId);
            return View(toDoItem);
        }

      


        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.ToDoItems
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.ToDoItemId == id);
            if (task == null) return NotFound();

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _context.ToDoItems
                 .Include(t => t.Category) // Включване на свързаната категория
                 .FirstOrDefaultAsync(t => t.ToDoItemId == id);

            if (task != null)
            {
                _context.ToDoItems.Remove(task);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


        private bool TaskExists(int id)
        {
            return _context.ToDoItems.Any(e => e.ToDoItemId == id);
        }
    }
}
