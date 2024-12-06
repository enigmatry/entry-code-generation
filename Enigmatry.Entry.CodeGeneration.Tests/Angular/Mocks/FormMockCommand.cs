namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

public class FormMockCommand
{
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public DateTimeOffset Date { get; set; }
    public decimal Money { get; set; }
    public EnumMock Status { get; set; }
    public Guid CategoryId { get; set; }
    public Guid TypeId { get; set; }
    public Guid SubTypeId { get; set; }
    public EnumMock MockRadio { get; set; }
}
