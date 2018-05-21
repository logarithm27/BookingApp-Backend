using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HiHostelFes.API.Data;
using HiHostelFes.API.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace HiHostelFes.API.Controllers
{

    public class RoomController : Controller
    {
        private readonly IRoomsRepository _repo;
        private readonly IMapper _mapper;
        public RoomController(IRoomsRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;

        }
        [Route("api/[controller]/available")]
        [HttpGet]
        public async Task<IActionResult> getAvRooms()
        {
            var rooms = await _repo.GetRooms();
            var roomsToReturn = _mapper.Map<IEnumerable<RoomsDto>>(rooms);
            return Ok(roomsToReturn);
        }
        [Route("api/[controller]/unavailable")]
        [HttpGet]
        public async Task<IActionResult> getNonAvRooms()
        {
            var rooms = await _repo.GetNonDisponibleRoom();
            var roomsToReturn = _mapper.Map<IEnumerable<RoomsDto>>(rooms);
            return Ok(roomsToReturn);
        }
        [Route("api/[controller]/rommbybed/{numOfBeds}")]
        [HttpGet]
        public IActionResult GetRoomsByNumberOfBeds(int numOfBeds)
        {
            var rooms =  _repo.GetRoomsByNumberOfBeds(numOfBeds);
            var roomsToReturn = _mapper.Map<IEnumerable<RoomsDto>>(rooms);
            return Ok(roomsToReturn);
        }
        [Route("api/[controller]/booksuccess/{firstname}/{lastname}/{email}")]
        [HttpGet]
        public async Task<IActionResult> getReservationNumberIfBookedSuccess(
                                string firstname,
                                string lastname,
                                string email)
        {
            var cutomer= await _repo.getReservationNumberIfBookedSuccess(firstname,
                                 lastname,
                                 email);
            return Ok(cutomer);
        }

        [Route("api/[controller]/manageCalendar")]
        [HttpGet]
        public async Task<IActionResult> getCustomers()
        {
            var customers = await _repo.getCustomers();
            var cstomerToReturn = _mapper.Map<IEnumerable<CustomerForBookingDto>>(customers);
            return Ok(customers);
        }
        [Route("api/[controller]/{dateOfArriving}/{roomId}")]
        [HttpGet]
        public async Task<IActionResult> getRoomStatus(DateTime dateOfArriving,int roomId)
        {
            var customer = await _repo.getRoomStatusByDateAndId(dateOfArriving,roomId);
            return Ok(customer);
        }
    }
}