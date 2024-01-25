namespace Property_Management_System.Models
{
    public class ApartmentViewModel
    {
        public int ApartmentId { get; set; }
        public int Floor { get; set; }
        public string ApartmentNumber { get; set; }
        public decimal RentPerMonth { get; set; }
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string Status { get; set; }
        public int BuildingId { get; set; }
        public string BuildingAddress { get; set; }

        // Add any additional properties you need from the Apartment model
    }
}
