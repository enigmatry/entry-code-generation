using System.Reflection;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.Entry.CodeGeneration.Tests.Angular.Mocks;

/// <summary>
/// A rule shaped like a pre-rename consumer implementation: it implements only the original
/// Formly-named interface members, which also pins the published-package source compatibility.
/// It carries no "ruleName: value" template option, so no Angular validator can be generated.
/// </summary>
internal sealed class UnsupportedValidationRuleMock : IFormlyValidationRule
{
    public string CustomMessage => String.Empty;
    public string MessageTranslationId { get; private set; } = String.Empty;
    public bool HasCustomMessage => false;
    public bool HasMessageTranslationId => false;
    public PropertyInfo PropertyInfo { get; } = typeof(FormMock).GetProperty(nameof(FormMock.Name))!;
    public string PropertyName => "name";
    public string FormlyValidationMessage => "Name check failed";
    public string FormlyRuleName => "nameCheck";
    public string[] FormlyTemplateOptions => Array.Empty<string>();

    public void SetMessageTranslationId(string messageTranslationId) => MessageTranslationId = messageTranslationId;
}
