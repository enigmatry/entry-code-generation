using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public class TextareaFormControlBuilder : BaseControlBuilder<TextareaFormControl, TextareaFormControlBuilder>
    {
        private int _rows = 2;
        private int _cols;
        private bool _autoResize;
        private int _autoResizeMinRows;
        private int _autoResizeMaxRows;

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

        public TextareaFormControlBuilder WithAutoResize(int minRows = 0, int maxRows = 0)
        {
            _autoResize = true;
            _autoResizeMinRows = minRows;
            _autoResizeMaxRows = maxRows;
            return this;
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var textareaFormControl = new TextareaFormControl
            {
                Rows = _rows,
                Cols = _cols,
                AutoResize = _autoResize,
                AutoResizeMinRows = _autoResizeMinRows,
                AutoResizeMaxRows = _autoResizeMaxRows
            };
            return Build(componentInfo, textareaFormControl);
        }
    }
}
