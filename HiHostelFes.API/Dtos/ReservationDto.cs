using HiHostelFes.API.Models;

namespace HiHostelFes.API.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public Room Room { get; set; }
        public Customer Customer { get; set; }
        public int RoomId { get; set; }        
        public int CustomerId { get; set; }
    }
}