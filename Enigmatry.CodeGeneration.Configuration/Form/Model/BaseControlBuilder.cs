using System;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Validators;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Model
{
    public abstract class BaseControlBuilder<TBuilder> : IControlBuilder where TBuilder : BaseControlBuilder<TBuilder>
    {
        public PropertyInfo? PropertyInfo { get; }
        protected readonly string _propertyName;
        protected string? _label;
        protected bool _isVisible;
        protected bool _isReadonly;
        protected string? _placeholder;
        protected string _hint;
        protected string? _labelTranslationId;
        protected string? _placeholderTranslationId;
        protected string? _hintTranslationId;
        protected string? _className;
        protected FormControlType _formControlType;
        protected CustomValidator? _validator;
        protected string? _customControlType;

        protected BaseControlBuilder(PropertyInfo propertyInfo) : this(propertyInfo.Name)
        {
            PropertyInfo = propertyInfo;
            _isVisible = !propertyInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
        }

        protected BaseControlBuilder(string propertyName)
        {
            _propertyName = propertyName.Camelize();
            _isVisible = true;
            _isReadonly = false;
            _hint = String.Empty;
        }

        /// <summary>
        /// Check if builder has given property info
        /// </summary>
        /// <param name="propertyInfo">property info</param>
        /// <returns></returns>
        public bool Has(PropertyInfo propertyInfo)
        {
            return PropertyInfo != null && PropertyInfo == propertyInfo;
        }

        /// <summary>
        /// Check if builder has given property name
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        public bool Has(string propertyName)
        {
            return _propertyName == propertyName;
        }

        /// <summary>
        /// Configure form control label
        /// </summary>
        /// <param name="label">label</param>
        /// <returns></returns>
        public TBuilder WithLabel(string label)
        {
            _label = label;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure form control visibility
        /// </summary>
        /// <param name="isVisible"></param>
        /// <returns></returns>
        public TBuilder IsVisible(bool isVisible)
        {
            _isVisible = isVisible;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure form control readonly status
        /// </summary>
        /// <param name="isReadonly"></param>
        /// <returns></returns>
        public TBuilder IsReadonly(bool isReadonly)
        {
            _isReadonly = isReadonly;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure form control placeholder
        /// </summary>
        /// <param name="placeholder"></param>
        /// <returns></returns>
        public TBuilder WithPlaceholder(string placeholder)
        {
            _placeholder = placeholder;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure form control hint
        /// </summary>
        /// <param name="hint"></param>
        /// <returns></returns>
        public TBuilder WithHint(string hint)
        {
            _hint = hint;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure label translationId (i18n)
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public TBuilder WithLabelTranslationId(string translationId)
        {
            _labelTranslationId = translationId;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure placeholder translationId (i18n)
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public TBuilder WithPlaceholderTranslationId(string translationId)
        {
            _placeholderTranslationId = translationId;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure hint translationId (i18n)
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public TBuilder WithHintTranslationId(string translationId)
        {
            _hintTranslationId = translationId;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure form control class name
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public TBuilder WithClassName(string className)
        {
            _className = className;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure custom field validator name and trigger event (onBlur is default)
        /// </summary>
        /// <param name="validatorName">Validator name to be matched on client side</param>
        /// <param name="validatorTrigger">Event to trigger validator (default is onBlur)</param>
        /// <returns></returns>
        public TBuilder WithValidator(string validatorName, ValidatorTrigger validatorTrigger = ValidatorTrigger.OnBlur)
        {
            _validator = new CustomValidator(validatorName, validatorTrigger);
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure custom form control type to be used
        /// </summary>
        /// <param name="controlTypeName"></param>
        /// <returns></returns>
        public TBuilder WithCustomControlType(string controlTypeName)
        {
            _customControlType = controlTypeName;
            return (TBuilder)this;
        }

        /// <summary>
        /// Build form control
        /// </summary>
        /// <param name="componentInfo">Parent componentInfo</param>
        /// <returns></returns>
        public abstract FormControl Build(ComponentInfo componentInfo);

        protected TControl Build<TControl>(ComponentInfo componentInfo, Action<TControl>? configureAction = null) where TControl : FormControl, new()
        {
            var translationId = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}.";
            var labelTranslationId = _labelTranslationId ?? $"{translationId}label";
            var placeholderTranslationId = _placeholderTranslationId ?? $"{translationId}placeholder";
            var hintTranslationId = _hintTranslationId ?? $"{translationId}hint";
            var label = _label ?? _propertyName.Humanize();
            var placeholder = _placeholder ?? label;

            var formControl = new TControl
            {
                ComponentInfo = componentInfo,
                PropertyName = _propertyName,
                Label = label,
                Placeholder = placeholder,
                Hint = _hint,
                IsVisible = _isVisible,
                IsReadonly = _isReadonly,
                Type = _formControlType,
                ValueType = PropertyInfo?.PropertyType,
                LabelTranslationId = labelTranslationId,
                PlaceholderTranslationId = placeholderTranslationId,
                HintTranslationId = hintTranslationId,
                Validator = _validator,
                ClassName = _className,
                CustomControlType = _customControlType
            };

            configureAction?.Invoke(formControl);

            return formControl;
        }
    }
}
