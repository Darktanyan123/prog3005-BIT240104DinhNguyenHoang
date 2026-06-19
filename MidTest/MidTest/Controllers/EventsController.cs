using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MidTest.Data;
using MidTest.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MidTest.Controllers
{
    public class EventsController : Controller
    {
        private readonly MyDbContext _context;

        public EventsController(MyDbContext context)
        {
            _context = context;
        }

        // 1. TRANG DANH SÁCH (INDEX)
        public async Task<IActionResult> Index(string searchTerm, int? categoryId, DateTime? fromDate, string sortBy)
        {
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-ddTHH:mm");
            ViewBag.SortBy = sortBy;
            ViewBag.Categories = new SelectList(await _context.EventCategories.ToListAsync(), "Id", "Name");

            var query = _context.Events.Include(e => e.EventCategory).Include(e => e.EventImages).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm) || e.Location.Contains(searchTerm));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(e => e.EventCategoryId == categoryId.Value);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(e => e.StartDate >= fromDate.Value);
            }

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(e => e.Price),
                "price_desc" => query.OrderByDescending(e => e.Price),
                _ => query.OrderByDescending(e => e.StartDate)
            };

            var results = await query.ToListAsync();

            if (!results.Any())
            {
                ViewBag.EmptyMessage = "Không tìm thấy kết quả nào phù hợp với điều kiện tìm kiếm.";
            }

            return View(results);
        }

        // 2. TRANG TẠO MỚI (GET)
        public async Task<IActionResult> Create()
        {
            ViewBag.EventCategoryId = new SelectList(await _context.EventCategories.ToListAsync(), "Id", "Name");
            return View();
        }


        // 3. XỬ LÝ TẠO MỚI (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event_BIT240104 model, string ImageUrl)
        {
            // Bỏ qua kiểm tra Validation cho các thuộc tính liên kết để tránh ModelState bị False vô lý
            ModelState.Remove("EventCategory");
            ModelState.Remove("EventImages");
            ModelState.Remove("Status"); // Bỏ qua nếu Status được tính toán tự động trong Model

            // Kiểm tra ràng buộc thời gian bắt buộc
            if (model.EndDate <= model.StartDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu sự kiện.");
            }

            bool isDuplicate = await _context.Events.AnyAsync(e => e.Name == model.Name && e.StartDate == model.StartDate);
            if (isDuplicate)
            {
                ModelState.AddModelError("", "Không được tạo hai sự kiện trùng tên và cùng thời gian bắt đầu.");
            }

            if (ModelState.IsValid)
            {
                _context.Events.Add(model);
                await _context.SaveChangesAsync();

                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    var img = new EventImage_BIT240104 { EventId = model.Id, ImageUrl = ImageUrl, IsThumbnail = true };
                    _context.EventImages.Add(img);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi, nạp lại dữ liệu cho ô dropdown và hiển thị lại trang
            ViewBag.EventCategoryId = new SelectList(await _context.EventCategories.ToListAsync(), "Id", "Name", model.EventCategoryId);
            return View(model);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();


var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return NotFound();

            ViewBag.EventCategoryId = new SelectList(
                await _context.EventCategories.ToListAsync(),
                "Id",
                "Name",
                ev.EventCategoryId);

            return View(ev);


}

        // 5. XỬ LÝ CHỈNH SỬA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event_BIT240104 model)
        {
            if (id != model.Id) return NotFound();

            // Bỏ qua kiểm tra Validation cho các thuộc tính liên kết
            ModelState.Remove("EventCategory");
            ModelState.Remove("EventImages");
            ModelState.Remove("Status");

            if (model.EndDate <= model.StartDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc phải lớn hơn ngày bắt đầu sự kiện.");
            }

            bool isDuplicate = await _context.Events.AnyAsync(e => e.Id != id && e.Name == model.Name && e.StartDate == model.StartDate);
            if (isDuplicate)
            {
                ModelState.AddModelError("", "Không được sửa thành tên và thời gian bắt đầu trùng với sự kiện khác.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Events.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Events.Any(e => e.Id == model.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.EventCategoryId = new SelectList(await _context.EventCategories.ToListAsync(), "Id", "Name", model.EventCategoryId);
            return View(model);
        }

        // 6. CHI TIẾT SỰ KIỆN
        // CHI TIẾT SỰ KIỆN
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound(); // Trả về trang 404 nếu không truyền Id

            // Phải nạp kèm (Include) cả Category và Images để giao diện hiển thị không bị lỗi null liên kết
            var @event = await _context.Events
                .Include(e => e.EventCategory)
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(m => m.Id == id);

            // QUAN TRỌNG: Nếu không tìm thấy sự kiện nào trùng với ID, phải chặn lại không truyền null sang View
            if (@event == null)
            {
                return NotFound();
            }

            // Chức năng 6: Kiểm tra nếu sự kiện chưa có ảnh thì tạo cảnh báo
            if (@event.EventImages == null || !@event.EventImages.Any())
            {
                ViewBag.ImageWarning = "Sự kiện này hiện chưa được cập nhật hình ảnh.";
            }

            // Gửi đúng thực thể @event vào View
            return View(@event);
        }

        // 7. TRANG XÓA (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var @event = await _context.Events
                .Include(e => e.EventCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null) return NotFound();

            if (@event.Status == "Đang diễn ra")
            {
                ViewBag.ErrorMessage = "Không được phép xóa sự kiện đang trong quá trình diễn ra!";
            }

            return View(@event);
        }

        // 8. XỬ LÝ XÓA (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            if (@event.Status == "Đang diễn ra")
            {
                TempData["Error"] = "Xóa thất bại! Sự kiện đang diễn ra.";
                return RedirectToAction(nameof(Index));
            }

            var images = _context.EventImages.Where(i => i.EventId == id);
            _context.EventImages.RemoveRange(images);

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}