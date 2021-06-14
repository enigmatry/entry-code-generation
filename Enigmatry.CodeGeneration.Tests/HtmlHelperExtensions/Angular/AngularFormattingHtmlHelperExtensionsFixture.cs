using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular;
using Enigmatry.CodeGeneration.Configuration.Formatters;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.HtmlHelperExtensions.Angular
{
    public class AngularFormattingHtmlHelperExtensionsFixture : CodeGenerationFixtureBase
    {
        [TestCase("", "", "", ExpectedResult = " | date")]
        [TestCase("", "UTC", "", ExpectedResult = " | date")]
        [TestCase("", "", "en", ExpectedResult = " | date")]
        [TestCase("dd-MM-yyyy", "", "", ExpectedResult = " | date : 'dd-MM-yyyy'")]
        [TestCase("dd-MM-yyyy", "", "en", ExpectedResult = " | date : 'dd-MM-yyyy'")]
        [TestCase("dd-MM-yyyy", "UTC", "", ExpectedResult = " | date : 'dd-MM-yyyy' : 'UTC'")]
        [TestCase("dd-MM-yyyy", "UTC", "en", ExpectedResult = " | date : 'dd-MM-yyyy' : 'UTC' : 'en'")]
        public string Pipe_DatePropertyFormatter(string format, string timeZone, string locale)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.Pipe(
                new DatePropertyFormatter()
                    .WithFormat(format)
                    .WithTimeZone(timeZone)
                    .WithLocale(locale)
            );

            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        [TestCase("", "", "", "", ExpectedResult = " | currency")]
        [TestCase("", "symbol", "", "", ExpectedResult = " | currency")]
        [TestCase("", "", "4.2-2", "", ExpectedResult = " | currency")]
        [TestCase("", "", "", "en", ExpectedResult = " | currency")]
        [TestCase("EUR", "", "", "", ExpectedResult = " | currency : 'EUR'")]
        [TestCase("EUR", "", "4.2-2", "", ExpectedResult = " | currency : 'EUR'")]
        [TestCase("EUR", "", "", "en", ExpectedResult = " | currency : 'EUR'")]
        [TestCase("EUR", "symbol", "", "", ExpectedResult = " | currency : 'EUR' : 'symbol'")]
        [TestCase("EUR", "symbol", "", "en", ExpectedResult = " | currency : 'EUR' : 'symbol'")]
        [TestCase("EUR", "symbol", "4.2-2", "", ExpectedResult = " | currency : 'EUR' : 'symbol' : '4.2-2'")]
        [TestCase("EUR", "symbol", "4.2-2", "en", ExpectedResult = " | currency : 'EUR' : 'symbol' : '4.2-2' : 'en'")]
        public string Pipe_CurrencyPropertyFormatter(string currencyCode, string display, string digitsInfo, string locale)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.Pipe(
                new CurrencyPropertyFormatter()
                    .WithCurrencyCode(currencyCode)
                    .WithDisplay(display)
                    .WithDigitsInfo(digitsInfo)
                    .WithLocale(locale)
            );

            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        [TestCase("", "", ExpectedResult = " | number")]
        [TestCase("", "en", ExpectedResult = " | number")]
        [TestCase("4.2-2", "", ExpectedResult = " | number : '4.2-2'")]
        [TestCase("4.2-2", "en", ExpectedResult = " | number : '4.2-2' : 'en'")]
        public string Pipe_DecimalPropertyFormatter(string digitsInfo, string locale)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.Pipe(
                new DecimalPropertyFormatter()
                    .WithDigitsInfo(digitsInfo)
                    .WithLocale(locale)
            );

            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        [TestCase("", "", ExpectedResult = " | percent")]
        [TestCase("", "en", ExpectedResult = " | percent")]
        [TestCase("4.2-2", "", ExpectedResult = " | percent : '4.2-2'")]
        [TestCase("4.2-2", "en", ExpectedResult = " | percent : '4.2-2' : 'en'")]
        public string Pipe_PercentPropertyFormatter(string digitsInfo, string locale)
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.Pipe(
                new PercentPropertyFormatter()
                    .WithDigitsInfo(digitsInfo)
                    .WithLocale(locale)
            );

            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        [TestCase(ExpectedResult = " | checkMark")]
        public string Pipe_CheckMarkPropertyFormatter()
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.Pipe(new BooleanPropertyFormatter());

            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        [TestCase(ExpectedResult = "")]
        public string Pipe_NoFormattingPropertyFormatter()
        {
            var stringWriter = new StringWriter();

            var htmlContent = _htmlHelper.Pipe(new NoFormattingPropertyFormatter());

            htmlContent.WriteTo(stringWriter, HtmlEncoder.Default);
            return stringWriter.ToString();
        }

        [Test]
        public void Pipe_NotSupportedPropertyFormatter()
        {
            Action action = () => _htmlHelper.Pipe(new NotSupportedPropertyFormatter());

            action.Should().ThrowExactly<NotImplementedException>("Formatter type not supported");
        }



        internal class NotSupportedPropertyFormatter : IPropertyFormatter
        {
            public IList<Type> SupportedInputTypes() => throw new NotImplementedException();
        }
    }
}
