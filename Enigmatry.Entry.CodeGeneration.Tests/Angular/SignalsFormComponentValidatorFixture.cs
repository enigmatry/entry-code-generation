using Enigmatry.Entry.CodeGeneration.Angular;
using Enigmatry.Entry.CodeGeneration.Configuration.Form;
using Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular;

[Category("unit")]
public class SignalsFormComponentValidatorFixture
{
    [Test]
    public void Validate_RichTextControlWithoutImport_Throws()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.RichTextInputFormControl(x => x.Description);

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("description");
        exception.Message.ShouldContain("WithImport");
    }

    [Test]
    public void Validate_CustomControlWithoutImport_Throws()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.CustomFormControl(x => x.FileUpload).WithCustomControlType("entry-file-input");

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("fileUpload");
        exception.Message.ShouldContain("WithImport");
    }

    [Test]
    public void Validate_AutocompleteInsideArrayItem_Throws()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration => itemConfiguration.AutocompleteFormControl(x => x.City));

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("addresses.city");
        exception.Message.ShouldContain("not supported inside array items");
    }

    [Test]
    public void Validate_NestedArrayInsideArrayItem_Throws()
    {
        var builder = new FormComponentBuilder<FormMock>();
        builder.Component().HasName("MockEdit").BelongsToFeature("Test");
        builder.ArrayFormControl(x => x.Addresses)
            .WithItemConfiguration(itemConfiguration =>
                itemConfiguration.ArrayFormControl(x => x.NestedAddresses).WithItemConfiguration(_ => { }));

        var exception = Should.Throw<InvalidOperationException>(() => SignalsFormComponentValidator.Validate(builder.Build()));

        exception.Message.ShouldContain("addresses");
        exception.Message.ShouldContain("Nested arrays are not supported");
    }

    [Test]
    public void Validate_MockConfiguration_Passes()
    {
        var builder = new FormComponentBuilder<FormMock>();
        new FormMockConfiguration().Configure(builder);

        Should.NotThrow(() => SignalsFormComponentValidator.Validate(builder.Build()));
    }
}
