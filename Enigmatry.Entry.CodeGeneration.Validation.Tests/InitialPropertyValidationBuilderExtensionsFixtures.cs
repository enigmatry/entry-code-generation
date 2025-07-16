using System.Globalization;
using System.Text.RegularExpressions;
using Enigmatry.Entry.CodeGeneration.Validation.PropertyValidations;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;
using Humanizer;
using NUnit.Framework;
using Shouldly;

namespace Enigmatry.Entry.CodeGeneration.Validation.Tests;

[Category("unit")]
public class InitialPropertyValidationBuilderExtensionsFixtures
{
    private const int MinIntField = 1;
    private const int MaxIntField = 100;
    private const double MinDoubleField = 1.5;
    private const double MaxDoubleField = 100.5;
    private const byte MinByteField = 10;
    private const byte MaxByteField = 100;
    private MockModelValidationConfiguration _validationConfiguration = null!;

    [SetUp]
    public void SetUp()
    {
        _validationConfiguration = new MockModelValidationConfiguration();
    }

    [Test]
    public void IsRequired()
    {
        _validationConfiguration
            .RuleFor(x => x.StringField)
            .IsRequired();

        _validationConfiguration.ValidationRules
            .Count().ShouldBe(1);
        _validationConfiguration.ValidationRules.Single().FormlyRuleName
            .ShouldBe("required");
        _validationConfiguration.ValidationRules.Single().CustomMessage
            .ShouldBeEmpty();
        _validationConfiguration.ValidationRules.Single().MessageTranslationId
            .ShouldBe("validators.required");
        _validationConfiguration.ValidationRules.Single().FormlyTemplateOptions
            .ShouldBe(["required: true"]);
        _validationConfiguration.ValidationRules.Single().FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: is required");
    }

    [Test]
    public void Match()
    {
        _validationConfiguration
            .RuleFor(x => x.StringField)
            .Match(new Regex("/^[A-Z]{4}[1-9]{8}$/mu"));

        _validationConfiguration.ValidationRules
            .Count().ShouldBe(1);
        _validationConfiguration.ValidationRules.Single().FormlyRuleName
            .ShouldBe("pattern");
        _validationConfiguration.ValidationRules.Single().CustomMessage
            .ShouldBeEmpty();
        _validationConfiguration.ValidationRules.Single().MessageTranslationId
            .ShouldBe("validators.pattern");
        _validationConfiguration.ValidationRules.Single().FormlyTemplateOptions
            .ShouldBe(["pattern: /^[A-Z]{4}[1-9]{8}$/mu"]);
        _validationConfiguration.ValidationRules.Single().FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: is not in valid format");
    }

    [Test]
    public void EmailAddress()
    {
        _validationConfiguration
            .RuleFor(x => x.StringField)
            .EmailAddress();

        _validationConfiguration.ValidationRules
            .Count().ShouldBe(1);
        _validationConfiguration.ValidationRules.Single().FormlyRuleName
            .ShouldBe("pattern");
        _validationConfiguration.ValidationRules.Single().CustomMessage
            .ShouldBe("Invalid email address format");
        _validationConfiguration.ValidationRules.Single().MessageTranslationId
            .ShouldBe("validators.pattern.emailAddress");
        _validationConfiguration.ValidationRules.Single().FormlyTemplateOptions
            .ShouldBe([@"pattern: /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/"]);
        _validationConfiguration.ValidationRules.Single().FormlyValidationMessage
            .ShouldBe("Invalid email address format");
    }

    [Test]
    public void MinLength()
    {
        _validationConfiguration
            .RuleFor(x => x.StringField)
            .MinLength(0);

        _validationConfiguration.ValidationRules
            .Count().ShouldBe(1);
        _validationConfiguration.ValidationRules.Single().FormlyRuleName
            .ShouldBe("minLength");
        _validationConfiguration.ValidationRules.Single().CustomMessage
            .ShouldBeEmpty();
        _validationConfiguration.ValidationRules.Single().MessageTranslationId
            .ShouldBe("validators.minLength");
        _validationConfiguration.ValidationRules.Single().FormlyTemplateOptions
            .ShouldBe(["minLength: 0"]);
        _validationConfiguration.ValidationRules.Single().FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: should have at least ${field?.templateOptions?.minLength}:min-value: characters");
    }

    [Test]
    public void MaxLength()
    {
        _validationConfiguration
            .RuleFor(x => x.StringField)
            .MaxLength(100);

        _validationConfiguration.ValidationRules
            .Count().ShouldBe(1);
        _validationConfiguration.ValidationRules.Single().FormlyRuleName
            .ShouldBe("maxLength");
        _validationConfiguration.ValidationRules.Single().CustomMessage
            .ShouldBeEmpty();
        _validationConfiguration.ValidationRules.Single().MessageTranslationId
            .ShouldBe("validators.maxLength");
        _validationConfiguration.ValidationRules.Single().FormlyTemplateOptions
            .ShouldBe(["maxLength: 100"]);
        _validationConfiguration.ValidationRules.Single().FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.maxLength}:max-value: characters");
    }

    [Test]
    public void Length()
    {
        _validationConfiguration
            .RuleFor(x => x.StringField)
            .Length(10);

        _validationConfiguration.ValidationRules.Count().ShouldBe(2);

        IFormlyValidationRule minRule = _validationConfiguration.ValidationRules.Single(x => x.FormlyRuleName == "minLength");
        minRule.CustomMessage.ShouldBeEmpty();
        minRule.MessageTranslationId.ShouldBe("validators.minLength");
        minRule.FormlyTemplateOptions.ShouldBe(["minLength: 10"]);
        minRule.FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: should have at least ${field?.templateOptions?.minLength}:min-value: characters");

        IFormlyValidationRule maxRule = _validationConfiguration.ValidationRules.Single(x => x.FormlyRuleName == "maxLength");
        maxRule.CustomMessage.ShouldBeEmpty();
        maxRule.MessageTranslationId.ShouldBe("validators.maxLength");
        maxRule.FormlyTemplateOptions.ShouldBe(["maxLength: 10"]);
        maxRule.FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.maxLength}:max-value: characters");
    }

    [Test]
    public void GreaterThen()
    {
        _validationConfiguration
            .RuleFor(x => x.IntField)
            .GreaterThen(MinIntField);
        _validationConfiguration
            .RuleFor(x => x.DoubleField)
            .GreaterThen(MinDoubleField);
        _validationConfiguration
            .RuleFor(x => x.ByteField)
            .GreaterThen(MinByteField);

        _validationConfiguration.ValidationRules.Count().ShouldBe(3);

        AssertNumbercMinValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.IntField)), MinIntField, false);
        AssertNumbercMinValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.DoubleField)), MinDoubleField, false);
        AssertNumbercMinValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.ByteField)), MinByteField, false);
    }

    [Test]
    public void GreaterOrEqualTo()
    {
        _validationConfiguration
            .RuleFor(x => x.IntField)
            .GreaterOrEqualTo(MinIntField);
        _validationConfiguration
            .RuleFor(x => x.DoubleField)
            .GreaterOrEqualTo(MinDoubleField);
        _validationConfiguration
            .RuleFor(x => x.ByteField)
            .GreaterOrEqualTo(MinByteField);

        _validationConfiguration.ValidationRules.Count().ShouldBe(3);

        AssertNumbercMinValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.IntField)), MinIntField, true);
        AssertNumbercMinValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.DoubleField)), MinDoubleField, true);
        AssertNumbercMinValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.ByteField)), MinByteField, true);
    }

    [Test]
    public void LessThen()
    {
        _validationConfiguration
            .RuleFor(x => x.IntField)
            .LessThen(MaxIntField);
        _validationConfiguration
            .RuleFor(x => x.DoubleField)
            .LessThen(MaxDoubleField);
        _validationConfiguration
            .RuleFor(x => x.ByteField)
            .LessThen(MaxByteField);

        _validationConfiguration.ValidationRules.Count().ShouldBe(3);

        AssertNumbercMaxValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.IntField)), MaxIntField, false);
        AssertNumbercMaxValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.DoubleField)), MaxDoubleField, false);
        AssertNumbercMaxValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.ByteField)), MaxByteField, false);
    }

    [Test]
    public void LessOrEqualTo()
    {
        _validationConfiguration
            .RuleFor(x => x.IntField)
            .LessOrEqualTo(MaxIntField);
        _validationConfiguration
            .RuleFor(x => x.DoubleField)
            .LessOrEqualTo(MaxDoubleField);
        _validationConfiguration
            .RuleFor(x => x.ByteField)
            .LessOrEqualTo(MaxByteField);

        _validationConfiguration.ValidationRules.Count().ShouldBe(3);

        AssertNumbercMaxValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.IntField)), MaxIntField, true);
        AssertNumbercMaxValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.DoubleField)), MaxDoubleField, true);
        AssertNumbercMaxValidationRule(GetRuleByPropertyName(nameof(ValidationMockModel.ByteField)), MaxByteField, true);
    }

    [Test]
    public void EqualToInt()
    {
        _validationConfiguration
            .RuleFor(x => x.IntField)
            .EqualTo(MinIntField);

        _validationConfiguration.ValidationRules.Count().ShouldBe(2);

        AssertNumbercMinValidationRule(GetRuleByFormlyRuleName("min"), MinIntField, true);
        AssertNumbercMaxValidationRule(GetRuleByFormlyRuleName("max"), MinIntField, true);
    }

    [Test]
    public void EqualToDouble()
    {
        _validationConfiguration
            .RuleFor(x => x.DoubleField)
            .EqualTo(MinDoubleField);

        _validationConfiguration.ValidationRules.Count().ShouldBe(2);

        AssertNumbercMinValidationRule(GetRuleByFormlyRuleName("min"), MinDoubleField, true);
        AssertNumbercMaxValidationRule(GetRuleByFormlyRuleName("max"), MinDoubleField, true);
    }

    [TestCase("MESSAGE", "")]
    [TestCase("MESSAGE", "MESSAGE_TRANSLATION_ID")]
    public void WithMessage(string message, string messageTranslationId)
    {
        _validationConfiguration
            .RuleFor(x => x.IntField)
            .IsRequired()
            .WithMessage(message, messageTranslationId);

        _validationConfiguration.ValidationRules.Single()
            .CustomMessage.ShouldBe(message);
        _validationConfiguration.ValidationRules.Single()
            .MessageTranslationId.ShouldBe(String.IsNullOrWhiteSpace(messageTranslationId) ? String.Empty : messageTranslationId);
    }

    [Test]
    public void InvalidWithMessage()
    {
        Func<IPropertyValidationBuilder<ValidationMockModel, DateTimeOffset>> lessThenFunc =
            () => (IPropertyValidationBuilder<ValidationMockModel, DateTimeOffset>)
                _validationConfiguration.RuleFor(x => x.IntField).LessThen(100).WithMessage("", "test");

        InvalidOperationException exception = Should.Throw<InvalidOperationException>(lessThenFunc);
        exception.Message.ShouldBe($"{nameof(ValidationMockModel.IntField)} validation message cannot be empty.");
    }

    private static void AssertNumbercMinValidationRule<T>(IFormlyValidationRule rule, T value, bool isEqual)
    {
        rule.FormlyRuleName
            .ShouldBe("min");
        rule.CustomMessage
            .ShouldBeEmpty();
        rule.MessageTranslationId
            .ShouldBe("validators.min");
        rule.FormlyTemplateOptions
            .ShouldBe(["type: 'number'", $"min: {String.Format(CultureInfo.InvariantCulture, "{0}", value)}{(isEqual ? "" : $" + {GetIncrement<T>()}")}"]);
        rule.FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: value should be more than ${field?.templateOptions?.min}:min-value:");
    }

    private static void AssertNumbercMaxValidationRule<T>(IFormlyValidationRule rule, T value, bool isEqual)
    {
        rule.FormlyRuleName
            .ShouldBe("max");
        rule.CustomMessage
            .ShouldBeEmpty();
        rule.MessageTranslationId
            .ShouldBe("validators.max");
        rule.FormlyTemplateOptions
            .ShouldBe(["type: 'number'", $"max: {String.Format(CultureInfo.InvariantCulture, "{0}", value)}{(isEqual ? "" : $" - {GetIncrement<T>()}")}"]);
        rule.FormlyValidationMessage
            .ShouldBe("${field?.templateOptions?.label}:property-name: value should be less than ${field?.templateOptions?.max}:max-value:");
    }

    private IFormlyValidationRule GetRuleByPropertyName(string propertyName)
    {
        return _validationConfiguration.ValidationRules.Single(x => x.PropertyName == propertyName.Camelize());
    }

    private IFormlyValidationRule GetRuleByFormlyRuleName(string formlyRuleName)
    {
        return _validationConfiguration.ValidationRules.Single(x => x.FormlyRuleName == formlyRuleName);
    }

    private static string GetIncrement<T>()
    {
        return new[] { typeof(float), typeof(decimal), typeof(double) }.Contains(typeof(T)) ? "0.1" : "1";
    }
}

internal class MockModelValidationConfiguration : ValidationConfiguration<ValidationMockModel> { }
