using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterBooking.Components
{
    public class BlogMenuViewComponent : ViewComponent
    {
        private readonly Models.AppContext _context;

        public BlogMenuViewComponent(Models.AppContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.PostCategory.Where(c => c.PostType == Models.PostType.Blog).ToListAsync();
            return View(categories);
        }
    }
}
