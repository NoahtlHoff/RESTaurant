using Microsoft.AspNetCore.Mvc;
using RESTaurang.Data;

namespace RESTaurang.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController(AppDbContext db) : Controller
    {
        
    }
}
