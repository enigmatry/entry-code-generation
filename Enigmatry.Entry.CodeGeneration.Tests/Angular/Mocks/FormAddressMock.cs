namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class FormAddressMock
{
    public Guid Id { get; set; }
    public string Street { get; set; } = String.Empty;
    public string HouseNumber { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public string Country { get; set; } = String.Empty;
    public bool Verified { get; set; }
}
