using Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Tests.Form.Controls.Select;

[Category("unit")]
public class SelectOptionsBuilderFixture
{
    [Test]
    public void WithGroupKey_sets_option_group_key_on_dynamic_options()
    {
        var builder = new SelectOptionsBuilder();

        var options = builder.WithGroupKey("categoryGroup").Build();

        options.OptionGroupKey.ShouldBe("categoryGroup");
    }

    [Test]
    public void WithoutGroupKey_option_group_key_is_null()
    {
        var builder = new SelectOptionsBuilder();

        var options = builder.Build();

        options.OptionGroupKey.ShouldBeNull();
    }

    [Test]
    public void WithFixedValues_of_enum_sets_group_from_attribute()
    {
        var builder = new SelectOptionsBuilder();

        var options = builder.WithFixedValues<GroupedEnumMock>().Build();

        var perishable = options.FixedOptions.Single(o => (int)o.Value! == (int)GroupedEnumMock.Food);
        perishable.Group.ShouldNotBeNull();
        perishable.Group.Value.ShouldBe("Perishables");

        var ungrouped = options.FixedOptions.Single(o => (int)o.Value! == (int)GroupedEnumMock.Furniture);
        ungrouped.Group.ShouldBeNull();
    }

    [Test]
    public void DefaultOptionsAsString_with_fixed_values_and_group_key_includes_groupProperty()
    {
        var options = new SelectOptionsBuilder()
            .WithFixedValues(new[] { new SelectOption(1, "Label 1") })
            .WithGroupKey("categoryGroup")
            .Build();

        options.DefaultOptionsAsString.ShouldBe(
            "{ valueProperty: 'value', labelProperty: 'displayName', sortProperty: '', groupProperty: 'categoryGroup' }");
    }

    [Test]
    public void DefaultOptionsAsString_with_fixed_values_and_no_group_key_omits_groupProperty()
    {
        var options = new SelectOptionsBuilder()
            .WithFixedValues(new[] { new SelectOption(1, "Label 1") })
            .Build();

        options.DefaultOptionsAsString.ShouldBe("{ valueProperty: 'value', labelProperty: 'displayName', sortProperty: '' }");
    }

    [Test]
    public void DefaultOptionsAsString_with_dynamic_values_and_group_key_is_empty_object()
    {
        var options = new SelectOptionsBuilder()
            .WithDynamicValues()
            .WithGroupKey("categoryGroup")
            .Build();

        options.DefaultOptionsAsString.ShouldBe("{}");
    }

    [Test]
    public void DefaultOptionsAsString_with_dynamic_values_and_no_group_key_is_empty_object()
    {
        var options = new SelectOptionsBuilder()
            .WithDynamicValues()
            .Build();

        options.DefaultOptionsAsString.ShouldBe("{}");
    }

    private enum GroupedEnumMock
    {
        [SelectOptionGroup("Perishables")]
        Food = 0,
        [SelectOptionGroup("Perishables")]
        Drink = 1,
        Furniture = 2
    }
}
