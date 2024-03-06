using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;
using FluentAssertions;
using Humanizer;
using NUnit.Framework;

namespace Enigmatry.Entry.CodeGeneration.Validation.Tests;

[Category("unit")]
public class ValidationConfigurationFixture
{
    [Test]
    public void ValidationConfiguration()
    {
        var validationConfiguration = new ModelConfiguration();

        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == nameof(ValidationMockModel.IntField).Camelize())
            .Should().HaveCount(3);
        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == nameof(ValidationMockModel.IntField).Camelize())
            .Select(x => x.FormlyRuleName)
            .Should().BeEquivalentTo("required", "min", "max");

        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == nameof(ValidationMockModel.DoubleField).Camelize())
            .Should().HaveCount(3);
        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == nameof(ValidationMockModel.DoubleField).Camelize())
            .Select(x => x.FormlyRuleName)
            .Should().BeEquivalentTo("required", "min", "max");

        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == nameof(ValidationMockModel.StringField).Camelize())
            .Should().HaveCount(3);
        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == nameof(ValidationMockModel.StringField).Camelize())
            .Select(x => x.FormlyRuleName)
            .Should().BeEquivalentTo("required", "minLength", "maxLength");
    }

    [TestCase(nameof(ValidationMockModel.IntField), "required", "", "validators.required")]
    [TestCase(nameof(ValidationMockModel.IntField), "min", ModelConfiguration.CustomMessage, "")]
    [TestCase(nameof(ValidationMockModel.IntField), "max", ModelConfiguration.CustomMessage, ModelConfiguration.CustomMessageTranslationId)]
    [TestCase(nameof(ValidationMockModel.StringField), "required", ModelConfiguration.CustomMessage, ModelConfiguration.CustomMessageTranslationId)]
    [TestCase(nameof(ValidationMockModel.StringField), "minLength", ModelConfiguration.CustomMessage, "")]
    [TestCase(nameof(ValidationMockModel.StringField), "maxLength", "", "validators.maxLength")]
    public void ValidationConfigurationPerValidationRule(
        string propertyName,
        string validationRuleName,
        string validationMessage,
        string validationMessageTranslationId)
    {
        var validationConfiguration = new ModelConfiguration();

        validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == propertyName.Camelize())
            .Should().NotBeNullOrEmpty();
        IFormlyValidationRule? validationRule = validationConfiguration.ValidationRules
            .Where(x => x.PropertyName == propertyName.Camelize())
            .SingleOrDefault(rule => rule.FormlyRuleName == validationRuleName);
        validationRule.Should().NotBeNull();
        validationRule?.CustomMessage.Should().Be(validationMessage);
        validationRule?.MessageTranslationId.Should().Be(validationMessageTranslationId);
    }

    [Test]
    public void ValidationConfigurationForPatterns()
    {
        var validationConfiguration = new PatternsConfiguration();

        validationConfiguration.ValidationRules
            .Select(x => x.PropertyName.Pascalize())
            .Should().BeEquivalentTo(nameof(ValidationMockModel.StringField), nameof(ValidationMockModel.OtherStringField));
        validationConfiguration.ValidationRules
            .Select(x => x.FormlyRuleName)
            .Should().BeEquivalentTo("pattern", "pattern");
        validationConfiguration.ValidationRules
            .Where(x => x.HasCustomMessage)
            .Select(x => $"{x.PropertyName.Pascalize()}: {x.CustomMessage}")
            .Should().BeEquivalentTo($"{nameof(ValidationMockModel.OtherStringField)}: Invalid email address format");
        validationConfiguration.ValidationRules
            .Where(x => x.HasMessageTranslationId)
            .Select(x => $"{x.PropertyName.Pascalize()}: {x.MessageTranslationId}")
            .Should().BeEquivalentTo(
                $"{nameof(ValidationMockModel.StringField)}: validators.pattern",
                $"{nameof(ValidationMockModel.OtherStringField)}: validators.pattern.emailAddress"
            );
        validationConfiguration.ValidationRules
            .Select(x => x.FormlyValidationMessage)
            .Should().BeEquivalentTo(
                "${field?.templateOptions?.label}:property-name: is not in valid format",
                "Invalid email address format"
            );
    }

    [Test]
    public void ValidationConfigurationForNullables()
    {
        var validationConfiguration = new NullablesConfiguration();

        validationConfiguration.ValidationRules
            .Select(x => x.PropertyName.Pascalize()).Distinct()
            .Should().BeEquivalentTo(
                nameof(ValidationMockModel.NullableIntField),
                nameof(ValidationMockModel.NullableDoubleField),
                nameof(ValidationMockModel.NullableByteField),
                nameof(ValidationMockModel.NullableStringField)
            );

        validationConfiguration.ValidationRules
            .Select(x => x.FormlyRuleName).Distinct()
            .Should().BeEquivalentTo("required", "min", "max", "minLength", "maxLength", "pattern");
        validationConfiguration.ValidationRules
            .All(x => x.HasMessageTranslationId).Should().BeTrue();
        validationConfiguration.ValidationRules
            .All(x => x.HasCustomMessage).Should().BeFalse();
        validationConfiguration.ValidationRules
            .Select(x => x.FormlyValidationMessage).Distinct()
            .Should().BeEquivalentTo(
                "${field?.templateOptions?.label}:property-name: is required",
                "${field?.templateOptions?.label}:property-name: value should be more than ${field?.templateOptions?.min}:min-value:",
                "${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.max}:max-value:",
                "${field?.templateOptions?.label}:property-name: should have at least ${field?.templateOptions?.minLength}:min-value: characters",
                "${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.maxLength}:max-value: characters",
                "${field?.templateOptions?.label}:property-name: is not in valid format"
            );
    }
}

#region Mocks

internal class ModelConfiguration : ValidationConfiguration<ValidationMockModel>
{
    public const string CustomMessage = "CUSTOM_VALIDATION_MESSAGE";
    public const string CustomMessageTranslationId = "CUSTOM_VALIDATION_MESSAGE_TRANSLATION_ID";

    public ModelConfiguration()
    {
        RuleFor(x => x.IntField)
            .IsRequired()
            .GreaterThen(0).WithMessage(CustomMessage)
            .LessThen(10).WithMessage(CustomMessage, CustomMessageTranslationId);

        RuleFor(x => x.DoubleField)
            .IsRequired()
            .GreaterThen(0.5).WithMessage(CustomMessage)
            .LessThen(10).WithMessage(CustomMessage, CustomMessageTranslationId);

        RuleFor(x => x.StringField)
            .IsRequired().WithMessage(CustomMessage, CustomMessageTranslationId)
            .MinLength(0).WithMessage(CustomMessage)
            .MaxLength(10);
    }
}

internal class PatternsConfiguration : ValidationConfiguration<ValidationMockModel>
{
    public PatternsConfiguration()
    {
        RuleFor(x => x.OtherStringField).EmailAddress();
        RuleFor(x => x.StringField).Match(new("/[A-Z]/"));
    }
}

internal class NullablesConfiguration : ValidationConfiguration<ValidationMockModel>
{
    public NullablesConfiguration()
    {
        RuleFor(x => x.NullableIntField).IsRequired().GreaterThen(1).LessThen(10);
        RuleFor(x => x.NullableDoubleField).IsRequired().GreaterThen(1.1).LessThen(10.1);
        RuleFor(x => x.NullableByteField).IsRequired().GreaterThen((byte)1).LessThen((byte)10);
        RuleFor(x => x.NullableStringField).MinLength(0).MaxLength(100).Match(new("/[A-Z]/"));
    }
}

#endregion Mocks
