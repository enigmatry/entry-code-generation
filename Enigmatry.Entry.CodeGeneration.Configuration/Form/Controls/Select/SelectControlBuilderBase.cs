using System;
using System.Reflection;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public abstract class SelectControlBuilderBase<TControl, TBuilder> : BaseControlBuilder<TControl, TBuilder> where TControl : SelectControlBase
        where TBuilder : SelectControlBuilderBase<TControl, TBuilder>
    {
        protected readonly SelectOptionsBuilder _optionsBuilder = new SelectOptionsBuilder();

        protected SelectControlBuilderBase(PropertyInfo propertyInfo) : base(propertyInfo)
        {
        }

        protected SelectControlBuilderBase(string propertyName) : base(propertyName)
        {
        }

        public TBuilder WithOptions(Action<SelectOptionsBuilder>? options = null)
        {
            options?.Invoke(_optionsBuilder);
            return (TBuilder)this;
        }
    }
}
