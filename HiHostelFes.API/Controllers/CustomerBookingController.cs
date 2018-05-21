using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HiHostelFes.API.Data;
using HiHostelFes.API.Dtos;
using HiHostelFes.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HiHostelFes.API.Controllers
{
    [Route("api/[controller]")]
    public class CustomerBookingController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IRoomsRepository _repoForRoom;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CustomerBookingController(IAuthRepository repo, IConfiguration config, IRoomsRepository repoForRoom, IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
            _repoForRoom = repoForRoom;
            _config = config;
            _repo = repo;
        }
        [HttpPost("booking")]
        //FromBody to check informtion from the body 
        public async Task<IActionResult> Booking([FromBody]CustomerForBookingDto customerForBookingDto)
        {
            if (!string.IsNullOrEmpty(customerForBookingDto.FirstName) &&
            !string.IsNullOrEmpty(customerForBookingDto.LastName) &&
            !string.IsNullOrEmpty(customerForBookingDto.Email) &&
            !string.IsNullOrEmpty(customerForBookingDto.DateOfArriving.ToString()) &&
            !string.IsNullOrEmpty(customerForBookingDto.DateOfLeaving.ToString()) &&
            !string.IsNullOrEmpty(customerForBookingDto.CountryOfOrigin))
            {
                customerForBookingDto.FirstName = customerForBookingDto.FirstName.ToLower();
                customerForBookingDto.LastName = customerForBookingDto.LastName.ToLower();
                customerForBookingDto.Email = customerForBookingDto.Email.ToLower();
                customerForBookingDto.DateOfArriving = customerForBookingDto.DateOfArriving;
                customerForBookingDto.DateOfLeaving = customerForBookingDto.DateOfLeaving;
                customerForBookingDto.CountryOfOrigin = customerForBookingDto.CountryOfOrigin.ToLower();
            }
            // validate request
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customerToCreate = new Customer
            {
                FirstName = customerForBookingDto.FirstName.ToUpper(),
                LastName = customerForBookingDto.LastName.ToUpper(),
                Email = customerForBookingDto.Email.ToUpper(),
                DateOfArriving = customerForBookingDto.DateOfArriving.Date,
                DateOfLeaving = customerForBookingDto.DateOfLeaving.Date,
                CountryOfOrigin = customerForBookingDto.CountryOfOrigin,
                PhoneNumber = "HIFES-" + DateTime.Now.Ticks.ToString("x").ToUpper(),
                //If 1 then it's confirmed, else if it's 0 is cancelled
                numberOfChildren = 1,
                numberOfClient = customerForBookingDto.numberOfClient,
                RoomId = customerForBookingDto.RoomId
            };
            var createdCustomer = await _repo.Booking(customerToCreate);
            return StatusCode(201);
        }

        [HttpPost("{firstname}")]
        public async Task<IActionResult> UpdateAdmin(string firstname, [FromBody]CustomerForBlockingDto customerForBookingDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var adminFromRepo = await _repoForRoom.getAdmin(firstname);
            _context.Remove(adminFromRepo);

            if (await _repoForRoom.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {firstname} failed on save");
        }
    }
}