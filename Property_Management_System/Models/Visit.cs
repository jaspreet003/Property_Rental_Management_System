using System.ComponentModel.DataAnnotations;
using Property_Management_System.Enums;
using Property_Management_System.Models;

public class Visit
{
    public int VisitId { get; set; }
    public int BuildingId { get; set; }
    public int ApartmentId { get; set; }
    public string OwnerId { get; set; }
    public string ManagerId { get; set; }
    public string TenantId { get; set; }
    public SlotEnum Slot { get; set; }
    [DataType(DataType.Date)]
    public DateTime VisitDate { get; set; }

    public virtual Building Building { get; set; }
}
