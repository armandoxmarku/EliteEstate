using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EliteEstate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
namespace EliteEstate.Controllers;
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        // Check to see if we got back null
        if (userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("Register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("RegisterUser")]
    public IActionResult RegisterUser(User useriNgaForma)
    {

        if (ModelState.IsValid)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            useriNgaForma.Password = Hasher.HashPassword(useriNgaForma, useriNgaForma.Password);
            _context.Add(useriNgaForma);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View("Register");

    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("LoginUserForm")]
    public IActionResult LoginUserForm(LoginUser useriNgaForma)
    {
        if (ModelState.IsValid)
        {
            User useriNgaDB = _context.Users
            .FirstOrDefault(e => e.Email == useriNgaForma.LoginEmail);
            if (useriNgaDB == null)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email");
                return View("Login");
            }

            PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(useriNgaForma, useriNgaDB.Password, useriNgaForma.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginPassword", "Invalid Password");
                return View("Login");
            }

            HttpContext.Session.SetInt32("UserId", useriNgaDB.UserId);
            return RedirectToAction("Index");
        }
        return View("Login");
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
public IActionResult ExploreProperties(int? categoryId, int? propertyTypeId, string location, int? minPrice, int? maxPrice)
{
    IQueryable<Property> propertiesQuery = _context.Properties
        .Include(p => p.Images)
        .Include(p => p.Category);

    // Apply filters if they are provided
    if (categoryId.HasValue)
    {
        propertiesQuery = propertiesQuery.Where(p => p.CategoryId == categoryId);
    }

    if (propertyTypeId.HasValue)
    {
        propertiesQuery = propertiesQuery.Where(p => p.PropertyTypeId == propertyTypeId);
    }

    if (!string.IsNullOrEmpty(location))
    {
        propertiesQuery = propertiesQuery.Where(p => p.Location == location);
    }

    // Apply price range filter
    if (minPrice.HasValue)
    {
        propertiesQuery = propertiesQuery.Where(p => p.Price >= minPrice);
    }

    if (maxPrice.HasValue)
    {
        propertiesQuery = propertiesQuery.Where(p => p.Price <= maxPrice);
    }

    // Order properties by date created in descending order (latest first)
    propertiesQuery = propertiesQuery.OrderByDescending(p => p.CreatedAt);

    List<Property> properties = propertiesQuery.ToList();

    // Populate categories for the select dropdown
    ViewBag.Categories = _context.Categories.ToList();

    // Populate locations for the select dropdown
    ViewBag.Locations = _context.Properties.Select(p => p.Location).Distinct().ToList();

    // Populate property types for the select dropdown
    ViewBag.PropertyTypes = _context.PropertyTypes.ToList();

    return View(properties);
}




    [HttpGet("Details/{id}")]
    public IActionResult Details(int id)
    {
        ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
        Property property = _context.Properties
            .Include(p => p.Images)
            .Include(p => p.Category)
            .Include(e=>e.PropertyType)
            .Include(a => a.Agent)
            .FirstOrDefault(p => p.PropertyId == id);


        return View("Details", property); 
    }
    [HttpGet("AddToFavorite/{id}")]
    public IActionResult AddToFavorite(int id)
    {
        // Retrieve the property with the given ID from the database
        Property property = _context.Properties.FirstOrDefault(p => p.PropertyId == id);
        ViewBag.PropertyId = property;
        // If the property is not found, return a 404 Not Found response
        if (property == null)
        {
            return NotFound();
        }

        // Check if the user is authenticated
        // Retrieve the current user's ID from the session
        int? userId = HttpContext.Session.GetInt32("UserId");
        ViewBag.UserId = userId;

        // If user ID is null, it means the user is not properly authenticated

        // Check if the property is already in the user's favorites
        // bool propertyAlreadyInFavorites = _context.Favorites.Any(f => f.UserId == userId && f.PropertyId == id);
        // Favorite Existingfavorite = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.PropertyId == id);
        bool isFavorite = _context.Favorites.Any(f => f.UserId == userId && f.PropertyId == id);

        if (!isFavorite)
        {
            // If the property is not in favorites, add it to favorites
            Favorite favorite = new Favorite
            {
                UserId = userId.Value,
                PropertyId = id,
                User = _context.Users.FirstOrDefault(l => l.UserId == userId),
                Property = _context.Properties.FirstOrDefault(l => l.PropertyId == id)
            };
            _context.Add(favorite);
        }
        else
        {
            // If the property is in favorites, remove it from favorites
            Favorite existingFavorite = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.PropertyId == id);
            _context.Remove(existingFavorite);
        }

        _context.SaveChanges();

        return RedirectToAction("ExploreProperties");
    }

    [HttpGet("MyFavorites")]
    public IActionResult Favorites()
    {
        // Retrieve the current user's ID from the session
        int? userId = HttpContext.Session.GetInt32("UserId");

        // If user ID is null, it means the user is not properly authenticated


        // Retrieve all favorite properties for the current user
        var favorites = _context.Favorites
            .Where(f => f.UserId == userId)
            .ToList();

        List<Property> favoriteProperties = new List<Property>();

        // Explicitly load the related properties for each favorite
        foreach (var favorite in favorites)
        {
            var property = _context.Properties
                .Include(p => p.Images)
                .FirstOrDefault(p => p.PropertyId == favorite.PropertyId);

            if (property != null)
            {
                favoriteProperties.Add(property);
            }
        }

        return View("Favorites", favoriteProperties);
    }

 

    public IActionResult Index()
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("UserId");
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
