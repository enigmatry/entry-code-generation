using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Enigmatry.CodeGeneration.Configuration.Builder;
using Enigmatry.CodeGeneration.Configuration.Form.Controls.Validators;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using Humanizer;

namespace Enigmatry.CodeGeneration.Configuration.Form.Controls
{
    public abstract class BaseControlBuilder<TControl, TBuilder> : IControlBuilder
        where TControl : FormControl
        where TBuilder : BaseControlBuilder<TControl, TBuilder>
    {
        private readonly PropertyAccessor? _propertyAccessor;
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
        protected FormControlAppearance? _appearance;
        protected FormControlFloatLabel? _floatLabel;
        protected CustomValidator? _validator;
        protected List<string> _customWrappers = new List<string>();
        protected string? _tooltipText;
        protected string? _tooltipTranslationId;
        protected IPropertyFormatter? _formatter;
        protected bool _ignore;

        protected BaseControlBuilder(PropertyInfo propertyInfo) : this(propertyInfo.Name)
        {
            _propertyAccessor = new PropertyAccessor(propertyInfo);
            _isVisible = !propertyInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase);
            _formatter = _propertyAccessor.GetDefaultPropertyFormatter();
        }

        protected BaseControlBuilder(string propertyName)
        {
            _propertyName = propertyName.Camelize();
            _isVisible = true;
            _isReadonly = false;
            _hint = String.Empty;
        }

        public PropertyInfo? PropertyInfo => _propertyAccessor?.PropertyInfo;

        /// <summary>
        /// Check if builder has given property info
        /// </summary>
        /// <param name="propertyInfo">property info</param>
        /// <returns></returns>
        public virtual bool Has(PropertyInfo propertyInfo)
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
        /// Configure form control appearance attribute (standard is default)
        /// </summary>
        /// <param name="appearance"></param>
        /// <returns></returns>
        public TBuilder WithAppearance(FormControlAppearance appearance)
        {
            _appearance = appearance;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure form control floatLabel attribute (never is default)
        /// </summary>
        /// <param name="floatLabel"></param>
        /// <returns></returns>
        public TBuilder WithFloatLabel(FormControlFloatLabel floatLabel)
        {
            _floatLabel = floatLabel;
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
        /// Configure custom field wrapper name
        /// </summary>
        /// <param name="wrapperName">Wrapper name to be matched on client side</param>
        /// <returns></returns>
        public TBuilder WithCustomWrapper(string wrapperName)
        {
            return WithCustomWrappers(wrapperName);
        }

        /// <summary>
        /// Configure custom field wrappers names
        /// </summary>
        /// <param name="wrappersNames">Wrappers names to be matched on client side</param>
        /// <returns></returns>
        public TBuilder WithCustomWrappers(params string[] wrappersNames)
        {
            _customWrappers.AddRange(wrappersNames);
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure tooltip text
        /// </summary>
        /// <param name="tooltipText">Tooltip text to be displayed</param>
        /// <returns></returns>
        public TBuilder WithTooltipText(string tooltipText)
        {
            _tooltipText = tooltipText;
            return (TBuilder)this;
        }

        /// <summary>
        /// Configure translationId for tooltip text
        /// </summary>
        /// <param name="translationId"></param>
        /// <returns></returns>
        public TBuilder WithTooltipTranslationId(string translationId)
        {
            _tooltipTranslationId = translationId;
            return (TBuilder)this;
        }
        
        /// <summary>
        /// Set field type formatter
        /// </summary>
        /// <param name="formatter">formatter</param>
        /// <returns></returns>
        public TBuilder WithFormat(IPropertyFormatter formatter)
        {
            if (_propertyAccessor != null)
            {
                formatter.ValidateInputType(_propertyAccessor.PropertyType);
            }
            _formatter = formatter;
            return (TBuilder)this;
        }

        /// <summary>
        /// Set control ignore value to true
        /// </summary>
        public void Ignore()
        {
            _ignore = true;
        }

        /// <summary>
        /// Build form control
        /// </summary>
        /// <param name="componentInfo">Parent componentInfo</param>
        /// <returns></returns>
        public abstract FormControl Build(ComponentInfo componentInfo);

        protected TControl Build(ComponentInfo componentInfo, TControl control)
        {
            var translationId = $"{componentInfo.TranslationId}.{_propertyName.Kebaberize()}.";

            var labelTranslationId = _labelTranslationId ?? $"{translationId}label";
            var placeholderTranslationId = _placeholderTranslationId ?? $"{translationId}placeholder";
            var hintTranslationId = _hintTranslationId ?? $"{translationId}hint";
            var tooltipTranslationId = _tooltipTranslationId ?? $"{translationId}tooltip";
            var label = _label ?? _propertyName.Humanize();
            var placeholder = _placeholder ?? label;
            var tooltip = _tooltipText ?? String.Empty;

            control.ComponentInfo = componentInfo;
            control.PropertyName = _propertyName;
            control.Label = new I18NString(labelTranslationId, label);
            control.Placeholder = new I18NString(placeholderTranslationId, placeholder);
            control.Hint = new I18NString(hintTranslationId, _hint);
            control.Visible = _isVisible;
            control.Readonly = _isReadonly;
            control.Validator = _validator;
            control.ClassName = _className;
            control.Appearance = _appearance;
            control.FloatLabel = _floatLabel;
            control.Tooltip = new I18NString(tooltipTranslationId, tooltip);
            control.Wrappers = new FormControlWrappers(_customWrappers);
            control.Formatter = _formatter;
            control.Ignore = _ignore;

            return control;
        }
    }
}
