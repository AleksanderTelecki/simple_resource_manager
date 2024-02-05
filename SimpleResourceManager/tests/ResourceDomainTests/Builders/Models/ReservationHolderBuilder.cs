namespace ResourceDomainTests.Builders.Models;

public class ReservationHolderBuilder
{
    private ReservationHolder _reservationHolder = new ReservationHolder();

    public ReservationHolderBuilder WithId(string id)
    {
        _reservationHolder.Id = id;
        return this;
    }

    public ReservationHolderBuilder WithUserName(string userName)
    {
        _reservationHolder.UserName = userName;
        return this;
    }

    public ReservationHolderBuilder WithEmail(string email)
    {
        _reservationHolder.Email = email;
        return this;
    }

    public ReservationHolderBuilder WithPhoneNumber(string phoneNumber)
    {
        _reservationHolder.PhoneNumber = phoneNumber;
        return this;
    }

    public ReservationHolderBuilder WithResources(ICollection<Resource.Domain.Entity.Resource> resources)
    {
        _reservationHolder.Resources = resources;
        return this;
    }

    public ReservationHolderBuilder AddResource(Resource.Domain.Entity.Resource resource)
    {
        if (_reservationHolder.Resources == null)
        {
            _reservationHolder.Resources = new List<Resource.Domain.Entity.Resource>();
        }

        _reservationHolder.Resources.Add(resource);
        return this;
    }

    public ReservationHolder Build()
    {
        return _reservationHolder;
    }

    public ReservationHolderBuilder Reset()
    {
        _reservationHolder = new ReservationHolder();
        return this;
    }
}
