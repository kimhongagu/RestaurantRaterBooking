using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterBooking.Components
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly Models.AppContext _context;

        public CategoryMenuViewComponent(Models.AppContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Category.ToListAsync();
            return View(categories);
        }
    }
}
