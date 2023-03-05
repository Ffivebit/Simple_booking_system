using Core;
using Core.Interfaces;
using Core.Models;

public class BookingService: IBooking
{
    private readonly SQLiteDataService _dataService;

    public BookingService()
    {
        _dataService = new SQLiteDataService();
    }

    public bool IsBookingAvailable(int resourceId, DateTime dateFrom, DateTime dateTo, int bookedQuantity)
    {
        var existingBookings = _dataService.GetBookingsForResource(resourceId);

        foreach (var booking in existingBookings)
        {
            if (booking.DateFrom > booking.DateTo)
            {
                return false;
            }
            else if (booking.DateFrom <= dateTo && dateFrom <= booking.DateTo)
            {
                // There is a conflict, so the booking is not available
                return false;
            }
        }

        // Check if the requested booking quantity is available
        var availableQuantity = _dataService.GetResourceById(resourceId).Quantity - existingBookings.Sum(b => b.BookedQuantity);
        if (bookedQuantity > availableQuantity)
        {
            // The requested booking quantity is not available
            return false;
        }

        // The booking is available
        return true;
    }

    public bool BookResource(int resourceId, DateTime dateFrom, DateTime dateTo, int bookedQuantity, out string errorMessage)
    {
        errorMessage = null;

        if (!IsBookingAvailable(resourceId, dateFrom, dateTo, bookedQuantity))
        {
            errorMessage = "The requested resource is not available at the specified time or quantity.";
            return false;
        }

        try
        {
            var booking = new Booking
            {
                DateFrom = dateFrom,
                DateTo = dateTo,
                BookedQuantity = bookedQuantity,
                ResourceId = resourceId
            };

            _dataService.CreateBooking(booking);
        }
        catch (Exception ex)
        {
            errorMessage = $"An error occurred while booking the resource: {ex.Message}";
            return false;
        }

        //Call service void method to send a email to admin@admin.com and just write to the console
        SendEmail(resourceId);
        return true;
    }

    public void SendEmail(int resourceId)
    {
        Console.WriteLine($"EMAIL SENT TO admin@admin.com FOR CREATED BOOKING WITH ID: {resourceId}");
    }

    public List<Booking> GetTodaysBooking()
    {
        return _dataService.GetTodayBooking();
    }
    public List<Booking> GetAllBookings()
    {
        return _dataService.GetBookings();
    }
}
