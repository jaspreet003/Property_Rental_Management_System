namespace Property_Management_System.Models;
public class Building
{
    public int BuildingId { get; set; }
    public string OwnerId { get; set; }
    public int NbFloor { get; set; }
    public int NbApartment { get; set; }
    public string ManagerId { get; set; }
    public string OwnerName { get; set; }
    public string ManagerName { get; set; }
    public string Address { get; set; }
    // Other relevant properties
}
