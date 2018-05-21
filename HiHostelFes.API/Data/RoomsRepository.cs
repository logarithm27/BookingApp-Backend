using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiHostelFes.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HiHostelFes.API.Data
{
    public class RoomsRepository : IRoomsRepository
    {
        private readonly DataContext _context;
        public RoomsRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<Room>> GetRooms()
        {
            var rooms = await _context.Room.ToListAsync();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].RoomStatus.Equals(false))
                    rooms.RemoveAt(i);
            }
            return rooms;
        }

        public async Task<IEnumerable<Room>> GetNonDisponibleRoom()
        {
            var rooms = await _context.Room.ToListAsync();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].RoomStatus.Equals(true))
                    rooms.Remove(rooms[i]);
            }
            return rooms;
        }

        public IEnumerable<Room> GetRoomsByNumberOfBeds(int numberOfBeds)
        {
            var rooms = _context.Room.Where(a => a.numberOfBeds >= numberOfBeds).ToList();
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].Id == 9)
                {
                    switch (numberOfBeds)
                    {
                        case 5:
                            rooms[i].Price += 82;
                            break;
                        case 6:
                            rooms[i].Price += (82 * 2);
                            break;
                        case 7:
                            rooms[i].Price += (82 * 3);
                            break;
                    }
                }
                if (rooms[i].Id == 5 || rooms[i].Id == 6)
                {
                    switch (numberOfBeds)
                    {
                        case 3:
                            rooms[i].Price += 82;
                            break;
                        case 4:
                            rooms[i].Price += (82 * 2);
                            break;
                    }
                }
                if (rooms[i].Id == 10 || rooms[i].Id == 8)
                {
                    rooms[i].Price *= numberOfBeds;
                }
                if (rooms[i].Id == 7)
                {
                    switch (numberOfBeds)
                    {
                        case 4:
                            rooms[i].Price += 82;
                            break;
                        case 5:
                            rooms[i].Price += (82 * 2);
                            break;
                    }
                }
                if (rooms[i].Id == 1 || rooms[i].Id == 2 || rooms[i].Id == 3)
                {
                    switch (numberOfBeds)
                    {
                        case 2:
                            rooms[i].Price = 185;
                            break;
                    }
                }

            }
            return rooms;
        }
        public async Task<Customer> getReservationNumberIfBookedSuccess(
                                string firstname,
                                string lastname,
                                string email)
        {
            var customer = await _context.Customers.LastOrDefaultAsync
            (a => a.FirstName.ToUpper() == firstname.ToUpper() &&
            a.LastName.ToUpper() == lastname.ToUpper() && a.Email.ToUpper() == email.ToUpper());
            return customer;

        }

        public async Task<Customer> getAdmin(string firsname)
        {
            var admin = await _context.Customers.FirstOrDefaultAsync(a => a.FirstName == firsname);
            return admin;
        }
        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<Customer>> getCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].numberOfChildren == 0)
                    customers.Remove(customers[i]);
            }
            return customers;
        }

        public async Task<Customer> getRoomStatusByDateAndId(DateTime arrivingDate, int roomId)
        {
            //    var customer =await  _context.Customers.FirstOrDefaultAsync(a=>a.DateOfArriving.Date==arrivingDate.Date && a.RoomId==roomId && a.numberOfChildren==1);
            //    return customer;
            var customer = await _context.Customers.ToListAsync();
            var custum = new Customer();
            for (int i = 0; i < customer.Count; i++)
            {
                if (customer[i].DateOfArriving.Date == arrivingDate.Date && customer[i].RoomId == roomId)
                {
                    custum = customer[i];
                    break;
                }
            }
            return custum;
            //return await _context.Customers.FirstOrDefaultAsync(a=>a.DateOfArriving.Date==arrivingDate.Date && a.RoomId==roomId);
        }
    }
}