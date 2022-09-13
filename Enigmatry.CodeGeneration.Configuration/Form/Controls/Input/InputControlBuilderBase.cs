using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public abstract class InputControlBuilderBase<TControl, TBuilder> : BaseControlBuilder<TControl, TBuilder> where TControl : InputControlBase
        where TBuilder : InputControlBuilderBase<TControl, TBuilder>
    {
        protected string? _type;

        protected InputControlBuilderBase(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        protected InputControlBuilderBase(string propertyName) : base(propertyName)
        {
        }

        /// <summary>
        /// Configure form control input type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public TBuilder WithInputType(string type)
        {
            _type = type;
            return (TBuilder)this;
        }
    }
}
