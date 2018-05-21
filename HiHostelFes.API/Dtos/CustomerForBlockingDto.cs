namespace HiHostelFes.API.Dtos
{
    public class CustomerForBlockingDto
    {
         public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
         public int RoomId { get; set; }
         public int numberOfChildren { get; set; }
    }
}