using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HiHostelFes.API.Models;

namespace HiHostelFes.API.Data
{
    public interface IReservationRepository
    {
         Task<IEnumerable<Reservation>> getReservation();
         IEnumerable<Customer> getRoomByReservation();
         IEnumerable<Customer> getReservationsOfToday();
         
         
    }
}