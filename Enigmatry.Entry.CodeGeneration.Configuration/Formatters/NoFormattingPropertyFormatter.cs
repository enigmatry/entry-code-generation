using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

/// <summary>
/// The NoFormattingPropertyFormatter is an implementation of the <see cref="IPropertyFormatter"/> interface
/// that does not apply any formatting to the property value in a table column.
/// </summary>
public class NoFormattingPropertyFormatter : BasePropertyFormatter
{
    /// <inheritdoc/>
    public override IList<Type> SupportedInputTypes() => new List<Type>();

    /// <inheritdoc/>
    public override string ToJsObject() => "undefined";

    /// <inheritdoc/>
    public override string JsFormatterName => String.Empty;

    /// <inheritdoc/>
    public override bool ValidateInputType(Type inputType) => true;
}
