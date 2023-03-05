using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Simple_booking_system_prooffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : Controller
    {
        private readonly ILogger<ResourcesController> _logger;
        private readonly IResource _ResourceService;

        public ResourcesController(ILogger<ResourcesController> logger, IResource resourceService)
        {
            _logger = logger;
            _ResourceService = resourceService;
        }
        // GET: ResourcesController
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Resource> Index()
        {
            return _ResourceService.GetListOfAllResources();
        }
    }
}
