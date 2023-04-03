using System;
using System.Collections.Generic;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Formatters;

/// <summary>
/// The IPropertyFormatter interface defines the methods and properties required for creating custom property formatters.
/// Property formatters are used for formatting column data in a table component. Implementing this interface allows you
/// to define custom formatting logic for specific property types and control how the data is displayed in a column.
/// </summary>
public interface IPropertyFormatter
{
    /// <summary>
    /// Validates whether the formatter supports the given input type.
    /// </summary>
    /// <param name="inputType">The input type to validate.</param>
    /// <returns>true if the input type is supported; otherwise, false.</returns>
    public bool ValidateInputType(Type inputType);

    /// <summary>
    /// Returns a list of supported input types for this formatter.
    /// </summary>
    /// <returns>A list of supported input types.</returns>
    IList<Type> SupportedInputTypes();

    /// <summary>
    /// Gets the name of the JavaScript formatter function associated with this formatter.
    /// </summary>
    public string JsFormatterName { get; }

    /// <summary>
    /// Serializes the formatter's configuration into a JavaScript object.
    /// </summary>
    /// <returns>A string representation of the JavaScript object.</returns>
    public string ToJsObject();
}
