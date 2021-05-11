﻿using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Services;
using Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.TypeScript;
using Humanizer;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Enigmatry.CodeGeneration.Templates.HtmlHelperExtensions.Angular
{
    public static class AngularServiceMethodHtmlHelperExtensions
    {
        public static IHtmlContent AsyncMethodDependencies(this IHtmlHelper html, IEnumerable<AsyncLookupMethod> methods, string fromPath)
        {
            return html.ImportStatement(methods.Select(methodInfo => methodInfo.ApiClientName).Distinct(), fromPath);
        }

        public static IHtmlContent AsyncMethodDependencyInjections(this IHtmlHelper html, IEnumerable<AsyncLookupMethod> methods)
        {
            return html.Raw(
                String.Join($",{Environment.NewLine}",
                methods.Select(methodInfo => $"private {methodInfo.ApiClientName.Camelize()}: {methodInfo.ApiClientName}").Distinct())
            );
        }

        public static IHtmlContent AsyncMethodInit(this IHtmlHelper html, AsyncLookupMethod method)
        {
            var argumentNames = method.ArgumentNames.Select(argument => $"response.{argument.Camelize()}");
            return html.Raw($"this.{method.Name}({String.Join(", ", argumentNames)});");
        }

        public static IHtmlContent MethodSubjectStatement(this IHtmlHelper html, LookupMethodBase method)
        {
            return method is AsyncLookupMethod
                ? html.Raw($"private {method.AsSubjectStatement()} = new Subject<any[]>();")
                : html.Raw($"private {method.AsSubjectStatement()} = new BehaviorSubject<any[]>(this.{method.AsFixedValuesProperty()});");
        }

        public static IHtmlContent MethodResponseProperty(this IHtmlHelper html, LookupMethodBase method)
        {
            return html.Raw($"{method.AsObservableProperty()} = this.{method.AsSubjectStatement()}.asObservable();");
        }

        public static IHtmlContent AsyncMethod(this IHtmlHelper html, AsyncLookupMethod method)
        {
            var arguments = String.Join(", ", method.ArgumentNames.Select(x => x.Camelize()));
            var methodBody = 
                $"{method.Name}({arguments}) {{\n" +
                $"\t\tthis.{method.ApiClientName.Camelize()}.{method.Name}({arguments})\n" +
                $"\t\t\t.subscribe(x => this.{method.AsSubjectStatement()}.next(x.items));\n" +
                "\t}\n";
            return html.Raw(methodBody);
        }

        public static IHtmlContent FixedValuesProperty(this IHtmlHelper html, FixedValuesLookupMethod method)
        {
            var fixedValues = String.Join("\n", method.FixedValues.Select(x => $"\t\t{{ value: {x.Value}, displayName: \"{x.DisplayName}\" }},"));
            return html.Raw($"private {method.AsFixedValuesProperty()} = [\n{fixedValues}\n\t];");
        }

        public static IHtmlContent FixedValuesFilterMethod(this IHtmlHelper html, FixedValuesLookupMethod method)
        {
            var methodBody =
                $"{method.Name}(keyword: string) {{\n" +
                $"\t\tthis.{method.AsSubjectStatement()}.next(this.{method.AsFixedValuesProperty()}\n" +
                $"\t\t\t.filter(x => x.displayName.toLowerCase().includes(keyword.toLowerCase())));\n" +
                "\t}\n";
            return html.Raw(methodBody);
        }

        public static IHtmlContent FixedValuesDisplayOptionMethod(this IHtmlHelper html, FixedValuesLookupMethod method)
        {
            var methodBody =
                $"{method.Name}DisplayOption(value) {{\n" +
                $"\t\treturn value !== undefined ? this.{method.AsFixedValuesProperty()}\n" +
                $"\t\t\t.find(x => x.value === value)?.displayName : '';\n" +
                "\t}\n";
            return html.Raw(methodBody);
        }

        public static IHtmlContent LookupServiceChangeEventTrigger(this IHtmlHelper html, LookupMethodBase method)
        {
            var onCheangeEventTrigger = method.TriggersOnChangeEvent
                ? $" (selectionChange) =\"{String.Join("", method.DependentMethods.Select(method => $"lookupService.{method.Name}($event.value);"))}\"" 
                : String.Empty;
            return html.Raw(onCheangeEventTrigger);
        }
    }

    public static class IServiceMethodExtensions
    {
        public static string AsSubjectStatement(this IServiceMethod method) => $"_{method.Name.Camelize()}";
        public static string AsObservableProperty(this IServiceMethod method) => $"{method.Name.Camelize()}$";
        public static string AsFixedValuesProperty(this IServiceMethod method) => $"_{method.Name.Camelize()}FixedValues";
    }
}