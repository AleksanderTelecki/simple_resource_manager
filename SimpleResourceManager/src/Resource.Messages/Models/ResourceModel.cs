namespace Resource.Messages.Models;

public class ResourceModel
{
    public Guid ResourceId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public ResourceModel(Guid resourceId, string name, string description)
    {
        ResourceId = resourceId;
        Name = name;
        Description = description;
    }

    public ResourceModel()
    {
        
    }
    
}