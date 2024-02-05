using System.ComponentModel.DataAnnotations;

namespace Resource.Domain.Entity
{
    public class Resource
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime? BlockedUntil { get; set; }

        public bool IsBlockedPermanently { get; set; } = false;


        public string ReservationHolderId { get; set; }

        public ReservationHolder ReservationHolder { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Resource()
        {
            
        }
        
        public Resource(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }

        public void TemporarilyReserve(DateTime blockedUntil, ReservationHolder reservationHolder)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Current resource is retired!");
            
            if (BlockedUntil > DateTime.Now || IsBlockedPermanently)
                throw new InvalidOperationException("Current resource is already reserved!");
            
            BlockedUntil = blockedUntil;
            ReservationHolder = reservationHolder;
        }
        
        public void Reserve(ReservationHolder reservationHolder)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Current resource is retired!");
            
            if (BlockedUntil > DateTime.Now || IsBlockedPermanently)
                throw new InvalidOperationException("Current resource is already reserved!");

            IsBlockedPermanently = true;
            ReservationHolder = reservationHolder;
        }

        public void CancelReservation(ReservationHolder reservationHolder)
        {
            if (ReservationHolderId != reservationHolder.Id)
                throw new ArgumentException("Current resource is reserved by another user!");

            IsBlockedPermanently = false;
            BlockedUntil = null;
            ReservationHolder = null;
        }

        public void RetireResource()
        {
            IsAvailable = false;
            IsBlockedPermanently = false;
            BlockedUntil = null;
            ReservationHolder = null;
        }
        
        public void UnRetireResource()
        {
            IsAvailable = true;
        }
    }
}
