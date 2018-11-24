namespace HiHostelFes.API.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Email { get; set; }
        public string reservationId { get; set; }
        public string dateOfArriving { get; set; }
        public string dateOfLeaving { get; set; }
    }
}
