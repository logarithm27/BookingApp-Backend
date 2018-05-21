using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HiHostelFes.API.Models;

namespace HiHostelFes.API.Data
{
    public interface IRoomsRepository
    {
        Task<IEnumerable<Room>> GetRooms();
        IEnumerable<Room> GetRoomsByNumberOfBeds(int numberOfBeds);
        Task<IEnumerable<Room>> GetNonDisponibleRoom();

        Task<Customer> getReservationNumberIfBookedSuccess(
            string firstname,
            string lastname,
            string email);
        Task<IEnumerable<Customer>> getCustomers();
        Task<Customer> getRoomStatusByDateAndId(DateTime date,int RoomId);

        Task<Customer> getAdmin(string firstname);
        Task<bool> SaveAll();

    }
}