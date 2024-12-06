namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class FormMock
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateTimeOffset Date { get; set; }
    public decimal Money { get; set; }
    public int Amount { get; set; }
    public string Email1 { get; set; } = String.Empty;
    public string Email2 { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public bool IsActive { get; set; }
    public EnumMock FormStatus { get; set; }
    public Guid CategoryId { get; set; }
    public IEnumerable<Guid> Types { get; set; } = Enumerable.Empty<Guid>();
    public Guid SubTypeId { get; set; }
    public EnumMock MockRadio { get; set; }
    public bool MockBoolSelect { get; set; }
    public IEnumerable<EnumMock> MockMultiCheckbox { get; set; } = Enumerable.Empty<EnumMock>();
    public IEnumerable<string> MultiCheckboxWithStringIds { get; set; } = Enumerable.Empty<string>();
    public IEnumerable<FormAddressMock> Addresses { get; set; } = Enumerable.Empty<FormAddressMock>();
    public DateTimeOffset DateAndTime { get; set; }
}

public class FormAddressMock
{
    public Guid Id { get; set; }
    public string Street { get; set; } = String.Empty;
    public string HouseNumber { get; set; } = String.Empty;
    public string City { get; set; } = String.Empty;
    public bool Verified { get; set; }
}
