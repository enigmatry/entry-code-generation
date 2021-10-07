using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Humanizer;
using JetBrains.Annotations;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    [UsedImplicitly]
    public class FormControlGroupBuilder<T> : BaseControlBuilder<FormControlGroupBuilder<T>>
    {
        private string? _sectionWrapperElement;
        private readonly IList<IControlBuilder> _formControls = new List<IControlBuilder>();

        internal FormControlGroupBuilder(PropertyInfo propertyInfo) : base(propertyInfo)
        {
            _isVisible = true;
        }

        internal FormControlGroupBuilder(string propertyName) : base(propertyName)
        {
            _isVisible = true;
        }

        public FormControlGroupBuilder<T> CreateUiSection(string wrapperElement)
        {
            _sectionWrapperElement = wrapperElement;
            return this;
        }

        public FormControlBuilder FormControl<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            Check.NotNull(propertyExpression, nameof(propertyExpression));

            var propertyInfo = propertyExpression.GetPropertyInfo();
            var formControlBuilder = _formControls.FirstOrDefault(builder => builder.Has(propertyInfo));
            return (FormControlBuilder)GetOrAddBuilder(formControlBuilder, () => new FormControlBuilder(propertyInfo));
        }

        public FormControlBuilder FormControl(string propertyName)
        {
            Check.NotNull(propertyName, nameof(propertyName));

            var formControlBuilder = _formControls.FirstOrDefault(builder => builder.Has(propertyName));
            return (FormControlBuilder)GetOrAddBuilder(formControlBuilder, () => new FormControlBuilder(propertyName));
        }

        public FormControlGroupBuilder<T> FormControlGroup(string groupName)
        {
            Check.NotNull(groupName, nameof(groupName));

            var formControlBuilder = _formControls.FirstOrDefault(builder => builder.Has(groupName));
            return (FormControlGroupBuilder<T>)GetOrAddBuilder(formControlBuilder, () => new FormControlGroupBuilder<T>(groupName));
        }

        public override FormControl Build(ComponentInfo componentInfo)
        {
            var formControls = _formControls.Select(_ => _.Build(componentInfo));

            var translationId = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}.";
            var labelTranslationId = _labelTranslationId ?? $"{translationId}label";
            var placeholderTranslationId = _placeholderTranslationId ?? $"{translationId}placeholder";
            var hintTranslationId = _hintTranslationId ?? $"{translationId}hint";

            return new FormControlGroup
            {
                ComponentInfo = componentInfo,
                PropertyName = _propertyName,
                Label = _label.Humanize(),
                Placeholder = _placeholder.Humanize(),
                Hint = _hint,
                IsVisible = _isVisible,
                IsReadonly = _isReadonly,
                Type = FormControlType.Group,
                ValueType = PropertyInfo?.PropertyType,
                LabelTranslationId = labelTranslationId,
                PlaceholderTranslationId = placeholderTranslationId,
                HintTranslationId = hintTranslationId,
                ClassName = _className,
                SectionWrapperElement = _sectionWrapperElement,
                FormControls = formControls.ToList()
            };
        }

        private IControlBuilder GetOrAddBuilder(IControlBuilder? builder, Func<IControlBuilder> creator)
        {
            if (builder != null)
                return builder;

            var formControlBuilder = creator();
            _formControls.Add(formControlBuilder);

            return formControlBuilder;
        }
    }
}
