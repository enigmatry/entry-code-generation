namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class FormCollisionMock
{
    public Guid AddressesCountry { get; set; }
    public IEnumerable<FormAddressMock> Addresses { get; set; } = Enumerable.Empty<FormAddressMock>();

    // RegionFiltered's generated regionFilteredOptions member collides with the one an autocomplete
    // named Region generates, even though neither member-name prefix contains the other.
    public string Region { get; set; } = String.Empty;
    public string RegionFiltered { get; set; } = String.Empty;
}
