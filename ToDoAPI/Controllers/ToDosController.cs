﻿//  Step 06. This controller was scaffolded using Entity Framework. When we get all ToDos in Swagger, we
//  notice that the Category object returns null. We need to customize the logic in our controller endpoints
//  below to fetch the Category object when getting a ToDo.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
// Step 10c. Add using statement for Cors below and enable Cors above the Route in the controller.
using Microsoft.AspNetCore.Cors;


namespace ToDoAPI.Controllers {
    // Enable Cors below.
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase {
        private readonly ToDoContext _context;

        public ToDosController(ToDoContext context) {
            _context = context;
        }

        // GET: api/ToDos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDo>>> GetToDos() {
            if (_context.ToDos == null) {
                return NotFound();
            }
            // Step 07. Modify the GET functionality to include categories.
            var toDos = await _context.ToDos.Include("Category").Select(
                x => new ToDo() {
                    // Assign each toDo in our dataset to a new ToDo object for this application.
                    ToDoId = x.ToDoId,
                    Name = x.Name,
                    Done = x.Done,
                    CategoryId = x.CategoryId,
                    Category = x.Category != null ? new Category() {
                        CategoryId = x.Category.CategoryId,
                        CatName = x.Category.CatName,
                        CatDesc = x.Category.CatDesc
                    } : null
                }
            ).ToListAsync();

            return Ok(toDos);
        }

        // GET: api/ToDos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDo(int id) {
            if (_context.ToDos == null) {
                return NotFound();
            }
            // Step 08. Modify the code below to include categories.
            var toDo = await _context.ToDos.Where(
                x => x.ToDoId == id).Select(x => new ToDo() {
                    // Assign each resource in our dataset to a new resource object for
                    // this application.
                    ToDoId = x.ToDoId,
                    Name = x.Name,
                    Done = x.Done,
                    CategoryId = x.CategoryId,
                    Category = x.Category != null ? new Category() {
                        CategoryId = x.Category.CategoryId,
                        CatName = x.Category.CatName,
                        CatDesc = x.Category.CatDesc
                    } : null
                }
            ).FirstOrDefaultAsync();


            if (toDo == null) {
                return NotFound();
            }

            return Ok(toDo);
        }

        // PUT: api/ToDos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDo(int id, ToDo toDo) {
            if (id != toDo.ToDoId) {
                return BadRequest();
            }

            _context.Entry(toDo).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ToDoExists(id)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ToDos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDo>> PostToDo(ToDo toDo) {
            if (_context.ToDos == null) {
                return Problem("Entity set 'ToDoContext.ToDos'  is null.");
            }
            // Step 09. Modify the code below to manage how a ToDo is posted.
            ToDo newToDo = new ToDo() {
                Name = toDo.Name,
                Done = toDo.Done,
                CategoryId = toDo.CategoryId
            };

            _context.ToDos.Add(newToDo);
            await _context.SaveChangesAsync();

            return Ok(newToDo);
        }

        // DELETE: api/ToDos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo(int id) {
            if (_context.ToDos == null) {
                return NotFound();
            }
            var toDo = await _context.ToDos.FindAsync(id);
            if (toDo == null) {
                return NotFound();
            }

            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoExists(int id) {
            return (_context.ToDos?.Any(e => e.ToDoId == id)).GetValueOrDefault();
        }
    }
}
