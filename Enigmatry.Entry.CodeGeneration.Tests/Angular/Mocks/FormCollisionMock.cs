namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class FormCollisionMock
{
    public Guid AddressesCountry { get; set; }
    public IEnumerable<FormAddressMock> Addresses { get; set; } = Enumerable.Empty<FormAddressMock>();
}
