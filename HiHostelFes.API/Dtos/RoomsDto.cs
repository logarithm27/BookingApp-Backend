using System;

namespace HiHostelFes.API.Dtos
{
    public class RoomsDto
    {
         public int Id { get; set; }
         public string NameOfChambre { get; set; }
         public bool RoomStatus { get; set; }
         public string typeOfChambre { get; set; }
         public string ChambreGender { get; set; }
         public int numberOfBeds { get; set; }     
         public int Price { get; set; }  
         public string PhotoUrl { get; set; }
         
    }
}