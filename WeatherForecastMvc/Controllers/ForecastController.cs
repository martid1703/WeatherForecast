using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherForecastMvc.Models;

namespace WeatherForecastMvc.Controllers
{
    public class ForecastController : Controller
    {
        private readonly ILogger<ForecastController> _logger;
        private readonly ForecastContext _context;
        private readonly IConfiguration Configuration;
        private readonly uint _displayForecastDays;

        public ForecastController(ILogger<ForecastController> logger, ForecastContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            Configuration = configuration;

            if (!uint.TryParse(Configuration["WeatherForecastMvcConfig:DisplayForecastDays"], out _displayForecastDays))
            {
                _displayForecastDays = 10;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Forecast
        public async Task<IActionResult> Index()
        {
            ViewData["SelectedDate"] = DateTime.Now;
            return await IndexWithDate(DateTime.Now);
        }

        [HttpGet()]
        public async Task<IActionResult> IndexWithDate(DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return base.Problem("Model is incorrect.");
            }

            using (_context)
            {
                return _context.DayForecast != null ?
                                        await CreateIndexView(date) :
                                        base.Problem("Entity set 'ForecastContext.DayForecast'  is null.");
            }
        }

        [HttpGet()]
        public async Task<IActionResult> IndexWithDateJS(string jsDate)
        {
            DateTime date = ConvertJsDate(jsDate);

            if (!ModelState.IsValid)
            {
                return base.Problem("Model is incorrect.");
            }

            return _context.DayForecast != null ?
                        await CreateIndexView(date) :
                        base.Problem("Entity set 'ForecastContext.DayForecast'  is null.");
        }

        private static DateTime ConvertJsDate(string jsDate)
        {
            DateTime date = DateTime.Now;
            if (!String.IsNullOrEmpty(jsDate))
            {
                date = DateTime.ParseExact(jsDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            return date;
        }

        private async Task<ViewResult> CreateIndexView(DateTime from)
        {
            _logger.LogInformation("Getting index view.");

            var forecast = await _context.DayForecast
            .Where(f => f.Date >= from.Date && f.Date <= from.AddDays(_displayForecastDays).Date)
            .OrderBy(f => f.Date)
            .ToListAsync();

            var indexForecast = new IndexForecastDTO(from, _displayForecastDays, forecast.Select(f => f.Convert()).ToArray());
            return View("Index", indexForecast);
        }

        // GET: DayForecast/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            _logger.LogInformation($"Getting details for id {id}.");

            if (_context.DayForecast == null)
            {
                return NotFound();
            }

            var dayForecast = await _context.DayForecast.FirstOrDefaultAsync(m => m.Id == id);
            if (dayForecast == null)
            {
                return NotFound();
            }

            return View(dayForecast.Convert());
        }

        [HttpGet]
        public async Task<IActionResult> CheckDetails(string jsDate)
        {
            if (_context.DayForecast == null)
            {
                return NotFound();
            }

            DateTime date = ConvertJsDate(jsDate);

            var dayForecast = await _context.DayForecast.FirstOrDefaultAsync(m => m.Date == date);
            if (dayForecast == null)
            {
                return Ok();
            }

            return Problem("Such date is already added.", statusCode: 500);
        }

        public IActionResult Create(DateTime date)
        {
            ViewData["SelectedDate"] = date.ToShortDateString();

            if (!uint.TryParse(Configuration["WeatherForecastMvcConfig:CalendarSpanDays"], out uint calendarSpanLimit))
            {
                calendarSpanLimit = 10;
            }
            ViewData["CalendarSpanLimit"] = calendarSpanLimit;
            var dto = new DayForecastCreateDTO { Date = date };
            return View(dto);
        }

        // POST: DayForecast/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DayForecastCreateDTO dayForecast)
        {
            _logger.LogInformation($"Creating forecast for date: {dayForecast.Date}.");

            var forecast = dayForecast.Convert();
            if (ModelState.IsValid)
            {
                _context.Add(forecast);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexWithDate), dayForecast.Date);
            }
            return View(dayForecast);
        }

        // GET: Forecast/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (_context.DayForecast == null)
            {
                return NotFound();
            }

            var dayForecast = await _context.DayForecast.FindAsync(id);
            if (dayForecast == null)
            {
                return NotFound();
            }
            return View(dayForecast.Convert());
        }

        // POST: Forecast/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, DayForecastDTO dayForecast)
        {
            if (id != dayForecast.Id)
            {
                return NotFound();
            }

            var forecast = dayForecast.Convert();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(forecast);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayForecastExists(dayForecast.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(forecast);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Deleting forecast with id: {id}.");

            if (_context.DayForecast == null)
            {
                return Problem("Entity set 'ForecastContext.DayForecast'  is null.");
            }
            var dayForecast = await _context.DayForecast.FindAsync(id);
            if (dayForecast != null)
            {
                _context.DayForecast.Remove(dayForecast);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DayForecastExists(Guid id)
        {
            return (_context.DayForecast?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: DayForecast
        public async Task<IActionResult> Privacy()
        {
            return View();
        }
    }
}
