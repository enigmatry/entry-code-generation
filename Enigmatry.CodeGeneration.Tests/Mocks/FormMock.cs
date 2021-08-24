using System;

namespace Enigmatry.CodeGeneration.Tests.Mocks
{
    public class FormMock
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DateTimeOffset Date { get; set; }
        public decimal Money { get; set; }
        public int Amount { get; set; }
        public EnumMock Status { get; set; }
        public Guid CategoryId { get; set; }
        public Guid TypeId { get; set; }
        public Guid SubTypeId { get; set; }
    }
}
