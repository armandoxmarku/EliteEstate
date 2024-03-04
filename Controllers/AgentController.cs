using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using EliteEstate.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace EliteEstate.Controllers;


public class SessionCheckAgentAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? agentId = context.HttpContext.Session.GetInt32("AgentId");
        // Check to see if we got back null
        if (agentId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Agent", null);
        }
    }
}

public class AgentController : Controller
{
    private readonly ILogger<AgentController> _logger;
    private readonly MyContext _context;

    public AgentController(ILogger<AgentController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("RegisterAgent")]
    public IActionResult RegisterAgent()
    {
        return View();
    }

    [HttpPost("RegisterAgentForm")]
    public async Task<IActionResult> RegisterAgentForm(Agent agent, List<IFormFile> image)
    {
        if (ModelState.IsValid)
        {
            foreach (var file in image)
            {
                if (file != null && file.Length > 0)
                {
                    // Save the file and update agent's profile picture property
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    agent.ProfilePicture = "/images/" + fileName;
                }
            }

            if (_context.Agents.Any(a => a.Email == agent.Email))
            {
                ModelState.AddModelError("Email", "Email is already in use!");
                return View("RegisterAgent");
            }

            PasswordHasher<Agent> hasher = new PasswordHasher<Agent>();
            agent.Password = hasher.HashPassword(agent, agent.Password);
            _context.Agents.Add(agent);
            await _context.SaveChangesAsync(); // Use async save changes

            HttpContext.Session.SetInt32("AgentId", agent.AgentId);
            return RedirectToAction("Index");
        }

        return View("RegisterAgent");
    }

    [HttpGet("LoginAgent")]
    public IActionResult LoginAgent()
    {
        return View();
    }

    [HttpPost("LoginAgentForm")]
    public IActionResult LoginAgentForm(LoginAgent loginAgent)
    {
        if (ModelState.IsValid)
        {
            Agent agent = _context.Agents.FirstOrDefault(a => a.Email == loginAgent.LoginEmail);
            if (agent == null)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("LoginAgent");
            }
            PasswordHasher<LoginAgent> hasher = new PasswordHasher<LoginAgent>();
            var result = hasher.VerifyHashedPassword(loginAgent, agent.Password, loginAgent.LoginPassword);
            if (result == 0)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("LoginAgent");
            }
            HttpContext.Session.SetInt32("AgentId", agent.AgentId);
            return RedirectToAction("Index");
        }
        return View("LoginAgent");
    }

    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    [SessionCheckAgent]
    [HttpGet("Agent/CreateProperty")]
    public IActionResult CreateProperty()
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("AgentId");
        ViewBag.PropertyType = _context.PropertyTypes.ToList();
        ViewBag.Categories = _context.Categories.ToList();

        return View("CreateProperty");
    }
    [HttpPost]
    public async Task<IActionResult> AddProperty(Property property, List<IFormFile> images)
    {
        if (ModelState.IsValid)
        {
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            foreach (var image in images)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    var imageEntity = new Image { PropertyId = property.PropertyId, ImagePath = "/images/" + fileName };
                    _context.Images.Add(imageEntity);
                }
            }

            // Update related entities
            if (property.CategoryId.HasValue)
            {
                property.Category = _context.Categories.Find(property.CategoryId);
            }

            if (property.PropertyTypeId.HasValue)
            {
                property.PropertyType = _context.PropertyTypes.Find(property.PropertyTypeId);
            }

            property.AgentId = (int)HttpContext.Session.GetInt32("AgentId");

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View("CreateProperty");
    }
    [HttpGet("DeleteProperty/{id}")]
    public IActionResult DeleteProperty(int id)
    {
        Property property = _context.Properties.Include(u=>u.Images).FirstOrDefault(p => p.PropertyId == id);
        if (property == null)
        {
            return NotFound();
        }
        _context.Properties.Remove(property);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    [HttpGet("EditProperty/{id}")]
    public IActionResult EditProperty(int id)
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("AgentId");
        Property property = _context.Properties.Include(i=>i.Images).FirstOrDefault(p => p.PropertyId == id);
        if (property == null)
        {
            return NotFound();
        }
        ViewBag.PropertyType = _context.PropertyTypes.ToList();
        ViewBag.Categories = _context.Categories.ToList();
        return View(property);
    }
[HttpPost("UpdateProperty/{id}")]
public async Task<IActionResult> UpdateProperty(int id, Property property, List<IFormFile> images)
{
    if (ModelState.IsValid)
    {
        var propertyToUpdate = await _context.Properties
            .Include(p => p.Images) // Include images for updating
            .FirstOrDefaultAsync(p => p.PropertyId == id);

        if (propertyToUpdate == null)
        {
            return NotFound();
        }

        // Update properties
        propertyToUpdate.PropertyName = property.PropertyName;
        propertyToUpdate.Description = property.Description;
        propertyToUpdate.Location = property.Location;
        propertyToUpdate.Price = property.Price;
        propertyToUpdate.Bedrooms = property.Bedrooms;
        propertyToUpdate.Bathrooms = property.Bathrooms;
        propertyToUpdate.Squaremetres = property.Squaremetres;
        propertyToUpdate.Rooms = property.Rooms;
        propertyToUpdate.Garages = property.Garages;
        propertyToUpdate.Pool = property.Pool;
        propertyToUpdate.Garden = property.Garden;
        propertyToUpdate.CategoryId = property.CategoryId;
        propertyToUpdate.PropertyTypeId = property.PropertyTypeId;

        // Update images
        foreach (var image in images)
        {
            if (image != null && image.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                var imageEntity = new Image { ImagePath = "/images/" + fileName };
                propertyToUpdate.Images.Add(imageEntity); // Add new image to property
            }
        }

       
        await _context.SaveChangesAsync();
        return RedirectToAction("Details", new { id });
    }

    return View("EditProperty", property);
}


    [HttpGet("EditAgentProfile/{id}")]
    public IActionResult EditAgentProfile(int id)
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("AgentId");
        Agent agent = _context.Agents.FirstOrDefault(p => p.AgentId == id);
        return View(agent);
    }

    [HttpPost("UpdateAgentProfile/{id}")]
    public IActionResult UpdateAgentProfile(int id, Agent agent)
    {
        Agent agentToUpdate = _context.Agents.FirstOrDefault(p => p.AgentId == id);
        if (agentToUpdate != null)
        {

            PasswordHasher<Agent> hasher = new PasswordHasher<Agent>();
            agentToUpdate.Name = agent.Name;
            agentToUpdate.LastName = agent.LastName;
            agentToUpdate.Email = agent.Email;
            agentToUpdate.Phone = agent.Phone;
            agentToUpdate.Address = agent.Address;
            agentToUpdate.Bio = agent.Bio;
            if (!string.IsNullOrEmpty(agent.Password))
            {
                agentToUpdate.Password = hasher.HashPassword(agentToUpdate, agent.Password);
            }

            _context.SaveChanges();
            return RedirectToAction("AgentProfile", new { id = agentToUpdate.AgentId });
        }
        return View("EditAgentProfile", agent);
    }

[HttpPost("UploadProfilePicture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile images)
    {

        int? agentId = HttpContext.Session.GetInt32("AgentId");

        if (agentId != null && images != null && images.Length > 0)
        {
            var agent = _context.Agents.FirstOrDefault(u => u.AgentId == agentId);

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(images.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await images.CopyToAsync(stream);
            }

            // Update agent's profile picture property
            agent.ProfilePicture = "/images/" + fileName;

            _context.SaveChanges();

            return RedirectToAction("AgentProfile", new { id = agentId });
        }
        return View("RegisterAgent");
    }

    [HttpGet("PropertyType")]
    public IActionResult PropertyType()
    {
        return View("PropertyType");
    }
    [HttpPost("CreatePropertyType")]
    public IActionResult CreatePropertyType(PropertyType propertyType)
    {
        if (ModelState.IsValid)
        {
            _context.PropertyTypes.Add(propertyType);
            _context.SaveChanges();
            return RedirectToAction("PropertyType");
        }
        return View("PropertyType");
    }
    [HttpGet("Categorie")]
    public IActionResult Categorie()
    {
        return View();
    }
    [HttpPost("CategorieType")]
    public IActionResult CategorieType(Category propertyType)
    {
        if (ModelState.IsValid)
        {
            _context.Add(propertyType);
            _context.SaveChanges();
            return RedirectToAction("Categorie");
        }
        return View("Categorie");
    }


    [HttpGet("ExploreProperties")]
    public IActionResult ExploreProperties(int? categoryId, int? propertyTypeId, string location, int? minPrice, int? maxPrice)
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("AgentId");
        IQueryable<Property> propertiesQuery = _context.Properties 
            .Include(p => p.Images)
            .Include(p => p.Category);

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

        if (minPrice.HasValue)
        {
            propertiesQuery = propertiesQuery.Where(p => p.Price >= minPrice);
        }

        if (maxPrice.HasValue)
        {
            propertiesQuery = propertiesQuery.Where(p => p.Price <= maxPrice);
        }

        propertiesQuery = propertiesQuery.OrderByDescending(p => p.CreatedAt);

        List<Property> properties = propertiesQuery.ToList();

        ViewBag.Categories = _context.Categories.ToList();

        ViewBag.Locations = _context.Properties.Select(p => p.Location).Distinct().ToList();

        ViewBag.PropertyTypes = _context.PropertyTypes.ToList();

        return View(properties);
    }
    [HttpGet("AgentProfile/{id}")]
    public IActionResult AgentProfile(int id)
    {
        int? agentId = HttpContext.Session.GetInt32("AgentId");
        Agent agent = _context.Agents.Include(a => a.Properties).ThenInclude(c => c.Category).Include(r => r.Properties)
            .ThenInclude(p => p.Images).FirstOrDefault(s => s.AgentId == id);
        return View(agent);
    }



    // [SessionCheckAgent]
    // public IActionResult Home()
    // {
    //     return View();
    // }


    [HttpGet("Agent/Details/{id}")]
    public IActionResult Details(int id, double interestRate, int loanTerm)
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("AgentId");
        Property property = _context.Properties
            .Include(p => p.Images)
            .Include(d => d.Category)
            .Include(e => e.PropertyType)
            .Include(a => a.Agent)
            .FirstOrDefault(t => t.PropertyId == id);

        if (property == null)
        {
            return NotFound();
        }

        return View("Details", property);
    }

    public IActionResult Index()
    {
        ViewBag.AgentId = HttpContext.Session.GetInt32("AgentId");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}