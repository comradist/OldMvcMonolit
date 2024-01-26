using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using Packt.Shared;

namespace Northwind.Mvc.Controllers;

//[Authorize(Roles = "Administrator")]
public class HomeController : Controller
{
    private readonly IHttpClientFactory clientFactory;

    private readonly ILogger<HomeController> _logger;

    private readonly NorthwindContext db;

    public HomeController(ILogger<HomeController> logger, NorthwindContext injectedContext, IHttpClientFactory httpclientFactory)
    {
        _logger = logger;
        db = injectedContext;
        clientFactory = httpclientFactory;
    }

    //[ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        HomeIndexViewModel model = new(
            VisitorCount: Random.Shared.Next(1, 1001),
            Categories: await db.Categories.ToListAsync(),
            Products: await db.Products.ToListAsync()
        );
        //_logger.LogError("This is a serious error");
        //_logger.LogWarning("Warning");
        //logger.LogWarning("Sec warning");
        return View(model);
    }

    public async Task<IActionResult> Customer(string country)
    {
        string uri;

        if(string.IsNullOrEmpty(country))
        {
            ViewData["Title"] = "All Customers Worldwide";
            uri = "api/customer";
        }
        else
        {
            ViewData["Title"] = $"Customer in {country}";
            uri = $"api/customer/?country={country}";
        }

        HttpClient client = clientFactory.CreateClient("Northwind.WebApi");

        HttpRequestMessage request = new(HttpMethod.Get, uri);

        HttpResponseMessage response = await client.SendAsync(request);

        IEnumerable<Customer>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Customer>>();

        return View(model);
    }

    public IActionResult ProductsThatCostMoreThan(decimal? price)
    {
        if(!price.HasValue)
        {
            return BadRequest("You must pass a product price in the query string, for example, /Home/ProductThatCostMoreThan?price=50");
        }

        
        IEnumerable<Product> model = db.Products
        .Include(p => p.Category)
        .Include(p => p.Supplier)
        .Where(p => p.UnitPrice > price);

        if(!model.Any())
        {
            return NotFound($"No products cost more than {price:C}.");
        }
        
        ViewData["MaxPrice"] = price.Value.ToString("C");

        return View(model);
    }

    public IActionResult ModelBinding()
    {
        return View();
    }

    [HttpPost]
    public IActionResult ModelBinding(Thing thing)
    {
        //var kek = ModelState.Values.SelectMany(state => state.Errors);
        HomeModelBindingViewModel model = new(
            Thing: thing, HasErrors: !ModelState.IsValid,
            ValidationErrors: ModelState.Values.SelectMany(state => state.Errors).Select(error => error.ErrorMessage)
        );
        return View(model);
    }

    public async Task<IActionResult> CategoryDetail(int? id)
    {
        if(!id.HasValue)
        {
            return BadRequest("You must pass a category ID in the route");
        }

        Category? model = await db.Categories.Where(c => c.CategoryId == id).Include(p => p.Products).SingleOrDefaultAsync();
        return View(model);
    }

    public async Task<IActionResult> ProductDetail(int? id, string alertstyle = "success")
    {
        ViewData["alertstyle"] = alertstyle;
        if(!id.HasValue)
        {
            return BadRequest("You must pass a product ID in the route, for example, /Home/ProductDetail/21");
        }

        Product? model = await db.Products.Where(p => p.ProductId == id).SingleOrDefaultAsync();

        if(model is null)
        {
            return NotFound($"ProductId {id} not found");
        }

        return View(model);
    }

    [Route("private")]
    [Authorize(Roles ="Admin")]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
