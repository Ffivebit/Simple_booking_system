using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class SQLiteDataService
    {
        private static readonly string connectionString = "Data Source=LocalDB.db";

        public void CreateBooking(Booking booking)
        {
            try
            {
                using var dbContext = new MyDbContext();
                
                dbContext.Bookings.AddAsync(booking);
                dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating booking: {ex.Message}");
                throw;
            }
        }

        public List<Resource> GetAllResources()
        {
            try
            {
                using var dbContext = new MyDbContext();
                return dbContext.Resources.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading all resources: {ex.Message}");
                throw;
            }
        }

        public List<Booking> GetTodayBooking()
        {
            try
            {
                using var dbContext = new MyDbContext();
                return dbContext.Bookings.Where(b => b.DateFrom.Date == DateTime.Today).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading all resources: {ex.Message}");
                throw;
            }
        }

        public List<Booking> GetBookings()
        {
            try
            {
                using var dbContext = new MyDbContext();
                return dbContext.Bookings.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading all resources: {ex.Message}");
                throw;
            }
        }


        public Resource GetResourceById(int id)
        {
            try
            {
                using var dbContext = new MyDbContext();
                return dbContext.Resources.FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading resource by ID: {ex.Message}");
                throw;
            }
        }

        public void UpdateResource(Resource resource)
        {
            try
            {
                using var dbContext = new MyDbContext();
                var existingResource = dbContext.Resources.FirstOrDefault(c => c.Id == resource.Id);
                if (existingResource != null)
                {
                    existingResource.Name = resource.Name;
                    existingResource.Quantity = resource.Quantity;
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating resource: {ex.Message}");
                throw;
            }
        }

        public void DeleteResource(int id)
        {
            try
            {
                using var dbContext = new MyDbContext();
                var existingResource = dbContext.Resources.FirstOrDefault(c => c.Id == id);
                if (existingResource != null)
                {
                    dbContext.Resources.Remove(existingResource);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting resource: {ex.Message}");
                throw;
            }
        }

        public List<Booking> GetBookingsForResource(int resourceId)
        {
            try
            {
                using var dbContext = new MyDbContext();
                return dbContext.Bookings.Where(b => b.ResourceId == resourceId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading bookings for resource: {ex.Message}");
                throw;
            }
        }

        private class MyDbContext : DbContext
        {
            public DbSet<Resource> Resources { get; set; }
            public DbSet<Booking> Bookings { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(connectionString);
            }
        }
    }
}
