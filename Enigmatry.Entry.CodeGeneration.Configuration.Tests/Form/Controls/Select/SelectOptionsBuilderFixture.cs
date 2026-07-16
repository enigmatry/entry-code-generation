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
    public void WithoutGroupKey_option_group_key_is_empty()
    {
        var builder = new SelectOptionsBuilder();

        var options = builder.Build();

        options.OptionGroupKey.ShouldBeEmpty();
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

    private enum GroupedEnumMock
    {
        [SelectOptionGroup("Perishables")]
        Food = 0,
        [SelectOptionGroup("Perishables")]
        Drink = 1,
        Furniture = 2
    }
}
