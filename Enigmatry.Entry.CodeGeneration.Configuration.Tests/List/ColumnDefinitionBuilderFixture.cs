using Enigmatry.Entry.CodeGeneration.Configuration.Formatters;
using Enigmatry.Entry.CodeGeneration.Configuration.List.Model;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.List;

[Category("unit")]
public class ColumnDefinitionBuilderFixture
{
    [TestCase("DateTime", "DateTimeOffset")]
    public void WithFormat_Date(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new DatePropertyFormatter());
            action.Should().NotThrow();
        }
    }

    [TestCase("Short", "Int", "Long", "Float", "Double", "Decimal", "String")]
    public void WithFormat_Date_InvalidPropertyType(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new DatePropertyFormatter());
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();

        }
    }

    [TestCase("Short", "Int", "Long", "Float", "Double", "Decimal")]
    public void WithFormat_Currency(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new CurrencyPropertyFormatter());
            action.Should().NotThrow();
        }
    }

    [TestCase("DateTime", "DateTimeOffset", "String")]
    public void WithFormat_Currency_InvalidPropertyType(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new CurrencyPropertyFormatter());
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }

    [TestCase("Short", "Int", "Long", "Float", "Double", "Decimal", "String")]
    public void WithFormat_Decimal(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new DecimalPropertyFormatter());
            action.Should().NotThrow();
        }
    }

    [TestCase("DateTime", "DateTimeOffset")]
    public void WithFormat_Decimal_InvalidPropertyType(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new DecimalPropertyFormatter());
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }

    [TestCase("Short", "Int", "Long", "Float", "Double", "Decimal")]
    public void WithFormat_Percent(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new PercentPropertyFormatter());
            action.Should().NotThrow();
        }
    }

    [TestCase("DateTime", "DateTimeOffset", "String")]
    public void WithFormat_Percent_InvalidPropertyType(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new PercentPropertyFormatter());
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }

    [TestCase("Short", "Int", "Long", "Float", "Double", "Decimal", "DateTime", "DateTimeOffset", "String")]
    public void WithFormat_NoFormat(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new NoFormattingPropertyFormatter());
            action.Should().NotThrow();
        }
    }

    [TestCase("Boolean")]
    public void WithFormat_CheckMark(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new BooleanPropertyFormatter());
            action.Should().NotThrow();
        }
    }

    [TestCase("Short", "Int", "Long", "Float", "Double", "Decimal", "DateTime", "DateTimeOffset", "String")]
    public void WithFormat_CheckMark_InvalidPropertyType(params string[] propertyNames)
    {
        foreach (var propertyName in propertyNames)
        {
            Action action = () => CreatePropertyBuilder(propertyName).WithFormat(new BooleanPropertyFormatter());
            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
    private static ColumnDefinitionBuilder CreatePropertyBuilder(string propertyName) =>
        typeof(TestModel)
            .GetProperties()
            .Where(prop => prop.Name == propertyName)
            .Select(propertyInfo => new ColumnDefinitionBuilder(propertyInfo))
            .Single();

    internal class TestModel
    {
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        public short Short { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public float Float { get; set; }
        public double Double { get; set; }
        public decimal Decimal { get; set; }
        public string String { get; set; } = String.Empty;
        public bool Boolean { get; set; }
    }
}
