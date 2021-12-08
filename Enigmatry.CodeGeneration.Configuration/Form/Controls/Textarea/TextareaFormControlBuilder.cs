using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class TextareaFormControlBuilder : BaseControlBuilder<TextareaFormControl, TextareaFormControlBuilder>
    {
        private int _rows = 2;
        private int _cols;

        public TextareaFormControlBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        public TextareaFormControlBuilder(string propertyName) : base(propertyName)
        {
        }

        public TextareaFormControlBuilder WithRows(int rows)
        {
            _rows = rows;
            return this;
        }

        public TextareaFormControlBuilder WithCols(int cols)
        {
            _cols = cols;
            return this;
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var textareaFormControl = new TextareaFormControl { Rows = _rows, Cols = _cols };
            return Build(componentInfo, textareaFormControl);
        }
    }
}
