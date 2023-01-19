using KitchenConnection.Models.Entities;

namespace KitchenConnection.DataLayer.Models.Entities;
public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public List<Recipe> Recipes { get; set; }
}
