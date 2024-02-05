namespace ResourceDomainTests.Builders.Models;

public class ResourceBuilder
{
    private Resource.Domain.Entity.Resource _resource = new Resource.Domain.Entity.Resource();

    public ResourceBuilder WithId(Guid id)
    {
        _resource.Id = id;
        return this;
    }

    public ResourceBuilder WithName(string name)
    {
        _resource.Name = name;
        return this;
    }

    public ResourceBuilder WithDescription(string description)
    {
        _resource.Description = description;
        return this;
    }

    public ResourceBuilder IsAvailable(bool isAvailable)
    {
        _resource.IsAvailable = isAvailable;
        return this;
    }

    public ResourceBuilder BlockedUntil(DateTime? blockedUntil)
    {
        _resource.BlockedUntil = blockedUntil;
        return this;
    }

    public ResourceBuilder IsBlockedPermanently(bool isBlockedPermanently)
    {
        _resource.IsBlockedPermanently = isBlockedPermanently;
        return this;
    }

    public ResourceBuilder WithReservationHolder(ReservationHolder reservationHolder)
    {
        _resource.ReservationHolder = reservationHolder;
        _resource.ReservationHolderId = reservationHolder.Id;
        return this;
    }

    public ResourceBuilder WithRowVersion(byte[] rowVersion)
    {
        _resource.RowVersion = rowVersion;
        return this;
    }

    public Resource.Domain.Entity.Resource Build()
    {
        return _resource;
    }

    public ResourceBuilder Reset()
    {
        _resource = new Resource.Domain.Entity.Resource();
        return this;
    }
}
