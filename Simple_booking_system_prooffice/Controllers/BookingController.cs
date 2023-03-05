using Core.Interfaces;
using Core.Models;
using Core.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Simple_booking_system_prooffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly ILogger<ResourcesController> _logger;
        private readonly IBooking _BookingService;

        public BookingController(ILogger<ResourcesController> logger, IBooking bookingService)
        {
            _logger = logger;
            _BookingService = bookingService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public List<Booking> Index()
        {
            return _BookingService.GetAllBookings();
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BookResource(BookResourceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Create a Booking object from the request data
                var booking = new Booking
                {
                    ResourceId = request.ResourceId,
                    DateFrom = request.DateFrom,
                    DateTo = request.DateTo,
                    BookedQuantity = request.BookedQuantity
                };

                // Attempt to book the resource using the booking service
                string errorMessage;
                bool success = _BookingService.BookResource(booking.ResourceId,booking.DateFrom,booking.DateTo,booking.BookedQuantity, out errorMessage);

                if (!success)
                {
                    return BadRequest(errorMessage);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error and return a 500 response
                _logger.LogError(ex, "An error occurred while attempting to book a resource.");
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("GetTodayBookings")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public List<Booking> GetTodayBookings()
        {
            return _BookingService.GetTodaysBooking();
        }
    }
}
