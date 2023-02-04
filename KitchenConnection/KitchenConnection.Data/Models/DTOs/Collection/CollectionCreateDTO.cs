namespace KitchenConnection.DataLayer.Models.DTOs.Collection;

public class CollectionCreateDTO:CollectionCreateRequestDTO
{
    public Guid UserId { get; set; }

    public CollectionCreateDTO(CollectionCreateRequestDTO requestDTO, Guid userId)
    {
        UserId=userId;
        Name=requestDTO.Name;
        Description=requestDTO.Description;
        Recipes=requestDTO.Recipes;
    }
}
public class CollectionCreateRequestDTO
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<Guid> Recipes { get; set; }
}
