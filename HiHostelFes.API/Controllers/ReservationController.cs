using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HiHostelFes.API.Data;
using HiHostelFes.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HiHostelFes.API.Controllers
{
    [Route("api/[controller]")]

    public class ReservationController : Controller
    {
        private readonly IReservationRepository _repo;
        private readonly IMapper _mapper;
        public ReservationController(IReservationRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // [HttpGet]
        // public async Task<IActionResult> getReservations()
        // {
        //     var reservations = await _repo.getReservation();
        //     var reservationToReturn = _mapper.Map<IEnumerable<ReservationDto>>(reservations);
        //     return Ok(reservationToReturn);
        // }
        //   public IActionResult getCustomersPerReservation()
        // {
        //     var reservation =  _repo.getRoomByReservation();
        //     var reservationToReturn = _mapper.Map<IEnumerable<CustomerForBookingDto>>(reservation);
        //     return Ok(reservationToReturn);
        // }
    }
}