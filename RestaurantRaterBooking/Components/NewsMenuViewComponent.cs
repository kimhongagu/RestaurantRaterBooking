using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterBooking.Components
{
    public class NewsMenuViewComponent : ViewComponent
    {
        private readonly Models.AppContext _context;

        public NewsMenuViewComponent(Models.AppContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.PostCategory.Where(c => c.PostType == Models.PostType.News).ToListAsync();
            return View(categories);
        }
    }
}
