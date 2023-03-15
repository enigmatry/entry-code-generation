using System.ComponentModel;

namespace Enigmatry.Entry.CodeGeneration.Configuration.Form.Controls;

public enum FormControlAppearance
{
    [Description("standard")]
    Standard = 0,
    [Description("fill")]
    Fill = 1,
    [Description("outline")]
    Outline = 2,
    [Description("legacy")]
    Legacy = 3,
    [Description("none")]
    None = 4
}