namespace Property_Management_System.Models
{
    public class Apartment
    {
        public int ApartmentId { get; set; }
        public int Floor { get; set; }
        public int ApartmentNumber { get; set; }
        public int RentPerMonth { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string status { get; set; }
        public int BuildingId { get; set; }
    }
}