using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiHostelFes.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HiHostelFes.API.Data
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DataContext _context;
        public ReservationRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Reservation>> getReservation()
        {
            var reservations = await _context.Reservation.ToListAsync();
            return reservations;
        }
        public IEnumerable<Customer> getReservationsOfToday()
        {
            var reservations = _context.Reservation.Where(a=>a.Customer.DateOfArriving.Day.Equals(DateTime.Today.Day)).ToList();
            var customers = new List<Customer>();
            for(int i=0;i<reservations.Count;i++){
                if(reservations[i].CustomerId!=-1)
                    customers.Add(_context.Customers.First(a=>a.Id.Equals(reservations[i].CustomerId)));
            }
            return customers;
        }
        public IEnumerable<Customer> getRoomByReservation()
        {
            var reservations = _context.Reservation.Where(a=>a.Customer.DateOfLeaving.Day.Equals(DateTime.Today.AddDays(3).Day)).ToList();
            var customers = new List<Customer>();
            for(int i=0;i<reservations.Count;i++){
                if(reservations[i].CustomerId!=-1)
                    customers.Add(_context.Customers.First(a=>a.Id.Equals(reservations[i].CustomerId)));
            }
            return customers;
        }
        
    }
}