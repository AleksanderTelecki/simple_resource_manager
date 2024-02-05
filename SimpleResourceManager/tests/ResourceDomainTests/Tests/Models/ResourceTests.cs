namespace ResourceDomainTests.Tests.Models
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void TemporarilyReserve_ShouldSetBlockedUntil_WhenResourceIsAvailable()
        {
            // Arrange
            var resource = new ResourceBuilder()
                .WithName("Test Resource")
                .WithDescription("Test Description")
                .Build();

            var reservationHolder = new ReservationHolderBuilder()
                .WithId("holder-id")
                .Build();
            
            var futureDate = DateTime.Now.AddDays(1);

            // Act
            Action act = () => resource.TemporarilyReserve(futureDate, reservationHolder);

            // Assert
            act.Should().NotThrow();
            resource.BlockedUntil.Should().Be(futureDate);
            resource.ReservationHolder.Should().Be(reservationHolder);
        }
        
        [Test]
        public void TemporarilyReserve_ShouldThrow_WhenBlockedUntilIsActive()
        {
            var blockedUntilDate = DateTime.Now.AddDays(1);
            var newReservationDate = DateTime.Now;
            
            // Arrange
            var resource = new ResourceBuilder()
                .WithName("Test Resource")
                .WithDescription("Test Description")
                .BlockedUntil(blockedUntilDate)
                .Build();

            var reservationHolder = new ReservationHolderBuilder()
                .WithId("holder-id")
                .Build();

            // Act
            Action act = () => resource.TemporarilyReserve(newReservationDate, reservationHolder);

            // Assert
            act.Should().Throw<InvalidOperationException>();
            resource.BlockedUntil.Should().NotBe(newReservationDate);
            resource.ReservationHolder.Should().NotBe(reservationHolder);
        }

        [Test]
        public void Reserve_ShouldSetIsBlockedPermanently_WhenResourceIsAvailable()
        {
            // Arrange
            var resource = new ResourceBuilder()
                .WithName("Test Resource")
                .WithDescription("Test Description")
                .Build();
            
            var reservationHolder = new ReservationHolderBuilder()
                .WithId("holder-id")
                .Build();

            // Act
            Action act = () => resource.Reserve(reservationHolder);

            // Assert
            act.Should().NotThrow();
            resource.IsBlockedPermanently.Should().BeTrue();
            resource.ReservationHolder.Should().Be(reservationHolder);
        }

        [Test]
        public void CancelReservation_ShouldClearReservation_WhenCalledByReservationHolder()
        {
            // Arrange
            var reservationHolder = new ReservationHolderBuilder()
                .WithId("holder-id")
                .Build();
            
            var resource = new ResourceBuilder()
                .WithName("Test Resource")
                .WithDescription("Test Description")
                .WithReservationHolder(reservationHolder)
                .Build();
            
            resource.Reserve(reservationHolder);

            // Act
            Action act = () => resource.CancelReservation(reservationHolder);

            // Assert
            act.Should().NotThrow();
            resource.ReservationHolder.Should().BeNull();
            resource.IsBlockedPermanently.Should().BeFalse();
            resource.BlockedUntil.Should().BeNull();
        }

        [Test]
        public void RetireResource_ShouldMakeResourceUnavailable()
        {
            // Arrange
            var resource = new ResourceBuilder()
                .WithName("Test Resource")
                .WithDescription("Test Description")
                .Build();

            // Act
            resource.RetireResource();

            // Assert
            resource.IsAvailable.Should().BeFalse();
        }

        [Test]
        public void UnRetireResource_ShouldMakeResourceAvailable()
        {
            // Arrange
            var resource = new ResourceBuilder()
                .WithName("Test Resource")
                .WithDescription("Test Description")
                .Build();
            
            resource.RetireResource();

            // Act
            resource.UnRetireResource();

            // Assert
            resource.IsAvailable.Should().BeTrue();
        }
    }
}
