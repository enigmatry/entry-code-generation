using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Formatters;

// ReSharper disable once CheckNamespace
namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularFormattingHtmlHelperExtensions
    {
        public static IHtmlContent Pipe(this IHtmlHelper html, IPropertyFromatter formatter) =>
            formatter switch
            {
                { } when formatter is DatePropertyFormatter => DatePipe(html, formatter),
                { } when formatter is CurrencyPropertyFormatter => CurrencyPipe(html, formatter),
                { } when formatter is DecimalPropertyFormatter => DecimalPipe(html, formatter),
                { } when formatter is PercentPropertyFormatter => PercentPipe(html, formatter),
                { } when formatter is NoFormattingPropertyFormatter => html.NoPipe(),
                _ => throw new NotImplementedException("Formatter type not supported")
            };

        private static IHtmlContent DatePipe(IHtmlHelper html, IPropertyFromatter formatter)
        {
            var dateFormatter = (DatePropertyFormatter)formatter;
            var arguments = new List<string>
                {
                    dateFormatter.Format,
                    dateFormatter.TimeZone,
                    dateFormatter.Locale
                }
                .TakeWhile(arg => !String.IsNullOrWhiteSpace(arg))
                .ToList();
            return html.Raw(CreatePipeAsString("date", arguments));
        }

        private static IHtmlContent CurrencyPipe(IHtmlHelper html, IPropertyFromatter formatter)
        {
            var currencyFormatter = (CurrencyPropertyFormatter)formatter;
            var arguments = new List<string>
                {
                    currencyFormatter.CurrencyCode,
                    currencyFormatter.Display,
                    currencyFormatter.DigitsInfo,
                    currencyFormatter.Locale
                }
                .TakeWhile(arg => !String.IsNullOrWhiteSpace(arg))
                .ToList();
            return html.Raw(CreatePipeAsString("currency", arguments));
        }

        private static IHtmlContent DecimalPipe(IHtmlHelper html, IPropertyFromatter formatter)
        {
            var decimalFormatter = (DecimalPropertyFormatter)formatter;
            var arguments = new List<string>
                {
                    decimalFormatter.DigitsInfo,
                    decimalFormatter.Locale
                }
                .TakeWhile(arg => !String.IsNullOrWhiteSpace(arg))
                .ToList();
            return html.Raw(CreatePipeAsString("number", arguments));
        }

        private static IHtmlContent PercentPipe(IHtmlHelper html, IPropertyFromatter formatter)
        {
            var percentFormatter = (PercentPropertyFormatter)formatter;
            var arguments = new List<string>
                {
                    percentFormatter.DigitsInfo,
                    percentFormatter.Locale
                }
                .TakeWhile(arg => !String.IsNullOrWhiteSpace(arg))
                .ToList();
            return html.Raw(CreatePipeAsString("percent", arguments));
        }

        private static IHtmlContent NoPipe(this IHtmlHelper html) => html.Raw(String.Empty);

        private static string CreatePipeAsString(string pipeName, IEnumerable<string> pipeArguments)
        {
            var pipe = $" | {pipeName}";

            if (pipeArguments.Any())
            {
                pipe += $" : {String.Join(" : ", pipeArguments.Select(arg => $"'{arg}'"))}";
            }

            return pipe;
        }
    }
}
