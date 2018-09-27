using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using HiHostelFes.API.Data;
using HiHostelFes.API.Dtos;
using HiHostelFes.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MailKit.Net.Smtp;  
using MimeKit; 

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
         [HttpPost("updateroom/{roomId}")]
        public async Task<IActionResult> UpdateBooking(int roomId, [FromBody]RoomsDto roomDto)
        {
            var roomToUpdate = await _repoForRoom.getRoom(roomId);
            //  if (!ModelState.IsValid)
            //     return BadRequest(ModelState);
            roomToUpdate.initialPrice=roomDto.initialPrice;
            roomToUpdate.Price=roomDto.Price;
            roomToUpdate.NameOfChambre=roomDto.NameOfChambre;
            roomToUpdate.numberOfBeds=roomDto.numberOfBeds;
            roomToUpdate.startingFrom=roomDto.startingFrom;

            // _mapper.Map(customerForUpdateDto,customerForUpdateBooking);

            if (await _repoForRoom.SaveAll())
                return NoContent();

            throw new Exception($"Failed to Update booking");
        }
        [HttpPost("updatebooking/{reservationId}")]
        public async Task<IActionResult> UpdateBooking(string reservationId, [FromBody]CustomerForUpdateDto customerForUpdateDto)
        {
            var customerForUpdateBooking = await _repoForRoom.changeBooking(reservationId);
            //  if (!ModelState.IsValid)
            //     return BadRequest(ModelState);
            customerForUpdateBooking.FirstName=customerForUpdateDto.FirstName.ToUpper();
            customerForUpdateBooking.LastName=customerForUpdateDto.LastName.ToUpper();
            customerForUpdateBooking.DateOfArriving=customerForUpdateDto.DateOfArriving.Date;
            customerForUpdateBooking.DateOfLeaving=customerForUpdateDto.DateOfLeaving.Date;
            customerForUpdateBooking.DateOfReservation=DateTime.Now;
            customerForUpdateBooking.Price=customerForUpdateDto.Price;
            customerForUpdateBooking.numberOfClient=customerForUpdateDto.numberOfClient;
            customerForUpdateBooking.RoomId=customerForUpdateDto.RoomId;
            customerForUpdateBooking.numberOfFemale+=1;

            // _mapper.Map(customerForUpdateDto,customerForUpdateBooking);

            if (await _repoForRoom.SaveAll())
                return NoContent();

            throw new Exception($"Failed to Update booking");
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
                DateOfReservation = DateTime.Now,
                Price = customerForBookingDto.Price,
                CountryOfOrigin = customerForBookingDto.CountryOfOrigin.ToUpper(),
                PhoneNumber = DateTime.Now.Ticks.ToString("x").ToUpper(),
                customerShowedHimSelf = true,
                //If 1 then it's confirmed, else if it's 0 is cancelled
                numberOfChildren = 1,
                numberOfClient = customerForBookingDto.numberOfClient,
                RoomId = customerForBookingDto.RoomId
            };
            // await sendMail(customerToCreate);
            var createdCustomer = await _repo.Booking(customerToCreate);
            if(await sendMail(customerToCreate))
            return StatusCode(201);

            return StatusCode(404);
                
        }
   
    
        public async Task<bool> sendMail(Customer customerForBookingDto)
        {
             string FromAddress = "hostelFez@youthostel.com";  
                string FromAdressTitle = "Reservation Information From Hostelling International Fez";  
                //To Address  
                string ToAddress = customerForBookingDto.Email;  
                string ToAdressTitle = "Reservation Information From Hostelling International Fez";  
                string Subject = "Reservation Information From Hostelling International Fez";  
                string BodyContent =
                 "Hello "+ customerForBookingDto.FirstName+ " "+customerForBookingDto.LastName+",\n"+
                 "Here is informations of your booking : "+"\n"+
                 "Date of your arriving : "+customerForBookingDto.DateOfArriving.Date+"\n"+
                 "Date of leaving : "+customerForBookingDto.DateOfLeaving.Date+"\n"+
                 "Total Price : "+ customerForBookingDto.Price+" MAD \n"+
                 "Your Booking ID (You Must Keep with you : ) "+ customerForBookingDto.PhoneNumber;

   
                //Smtp Server  
                string SmtpServer = "smtp.gmail.com";  
                //Smtp Port Number  
                int SmtpPortNumber = 587;  
   
                var mimeMessage = new MimeMessage();  
                mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));  
                mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));  
                mimeMessage.Subject = Subject;  
                mimeMessage.Body = new TextPart("plain")  
                {  
                    Text = BodyContent  
   
                };  
   
                var  client = new SmtpClient();
                    await client.ConnectAsync(SmtpServer, SmtpPortNumber, false);  
                    // Note: only needed if the SMTP server requires authentication  
                    // Error 5.5.1 Authentication   
                    await client.AuthenticateAsync("omar.maftoulii@gmail.com", "omario15");
                    await client.SendAsync(mimeMessage);  
                    Console.WriteLine("The mail has been sent successfully !!");  
                    // Console.ReadLine();  
                    // await client.DisconnectAsync(true);  
                    return true;
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

         [HttpPost("cancel/{reservationId}")]
        public async Task<IActionResult> cancelReservation(string reservationId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerForCancellation = await _repoForRoom.cancelBooking(reservationId);
            customerForCancellation.numberOfChildren=0;
            customerForCancellation.DateOfCancellation=DateTime.Now;          

            if (await _repoForRoom.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {reservationId} failed on save");
        }

        [HttpPost("notshowed/{reservationId}")]
        public async Task<IActionResult> notShowed(string reservationId, [FromBody]CustomerBookingController customerBookingController)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerForCancellation = await _repoForRoom.cancelBooking(reservationId);
            customerForCancellation.customerShowedHimSelf=false;      

            if (await _repoForRoom.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {reservationId} failed on save");
        }
    }
}
