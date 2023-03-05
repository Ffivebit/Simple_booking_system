using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBooking
    {
        public bool IsBookingAvailable(int resourceId, DateTime dateFrom, DateTime dateTo, int bookedQuantity);
        public bool BookResource(int resourceId, DateTime dateFrom, DateTime dateTo, int bookedQuantity, out string errorMessage);
        public List<Booking> GetTodaysBooking();
        public List<Booking> GetAllBookings();
    }
}
