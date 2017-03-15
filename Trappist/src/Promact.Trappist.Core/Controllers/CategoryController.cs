using Microsoft.AspNetCore.Mvc;
using Promact.Trappist.DomainModel.DbContext;
using System.Threading.Tasks;
using Promact.Trappist.DomainModel.Models.Category;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Promact.Trappist.Core.Controllers
{

    [Produces("application/json")]
    [Route("api/Category")]
    public class CategoryController :Controller
    {
        private readonly TrappistDbContext _context;
        public CategoryController(TrappistDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetCategory()
        {
            return Json(_context.Category.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }
        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> RemoveCategory([FromRoute]int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = await _context.Category.FirstOrDefaultAsync(m => m.Id == categoryId);
            if (category == null)
            {
                return NotFound();
            }
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }
    }
}
