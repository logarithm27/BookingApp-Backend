using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HiHostelFes.API.Models;

namespace HiHostelFes.API.Dtos
{
    public class CustomerForBookingDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountryOfOrigin { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfArriving { get; set; }

        public DateTime DateOfLeaving { get; set; }
        public int numberOfClient { get; set; }
        public int numberOfMale { get; set; }
        public int numberOfFemale { get; set; }
        public int numberOfNights { get; set; }
        public int numberOfChildren { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }

    }
}