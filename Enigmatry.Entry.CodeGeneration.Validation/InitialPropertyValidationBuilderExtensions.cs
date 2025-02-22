﻿using System.Text.RegularExpressions;
using Enigmatry.Entry.CodeGeneration.Validation.Helpers;
using Enigmatry.Entry.CodeGeneration.Validation.PropertyValidations;
using Enigmatry.Entry.CodeGeneration.Validation.ValidationRules;

namespace Enigmatry.Entry.CodeGeneration.Validation;

public static class InitialPropertyValidationBuilderExtensions
{
    public static IPropertyValidationBuilder<T, TProperty> IsRequired<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty> builder)
    {
        var response = new PropertyValidationBuilder<T, TProperty>(builder.PropertyRule);
        response.SetValidationRule(new IsRequiredValidationRule(builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    #region STRINGS

    // Disabling nullability for string extension methods below
    // It`s not possible to specify both nullable and non-nullable generic type parameter
    // https://github.com/dotnet/csharplang/blob/main/meetings/2019/LDM-2019-11-25.md

    #nullable disable

    public static IPropertyValidationBuilder<T, string> Match<T>(this IInitialPropertyValidationBuilder<T, string> builder, Regex rule)
    {
        var response = new PropertyValidationBuilder<T, string>(builder.PropertyRule);
        response.SetValidationRule(new PatternValidationRule(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, string> EmailAddress<T>(this IInitialPropertyValidationBuilder<T, string> builder)
    {
        var response = new PropertyValidationBuilder<T, string>(builder.PropertyRule);
        response.SetValidationRule(new EmailAddressValidationRule(builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, string> MinLength<T>(this IInitialPropertyValidationBuilder<T, string> builder, int rule)
    {
        var response = new PropertyValidationBuilder<T, string>(builder.PropertyRule);
        response.SetValidationRule(new MinLengthValidationRule(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, string> MaxLength<T>(this IInitialPropertyValidationBuilder<T, string> builder, int rule)
    {
        var response = new PropertyValidationBuilder<T, string>(builder.PropertyRule);
        response.SetValidationRule(new MaxLengthValidationRule(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, string> Length<T>(this IInitialPropertyValidationBuilder<T, string> builder, int rule)
    {
        var response = MinLength(builder, rule);
        MaxLength(response, rule);
        return response;
    }

    #nullable enable

    #endregion STRINGS

    #region NUMBERS

    public static IPropertyValidationBuilder<T, TProperty> GreaterOrEqualTo<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty>(builder.PropertyRule);
        response.SetValidationRule(new GreaterOrEqualToValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty?> GreaterOrEqualTo<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty?> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty?>(builder.PropertyRule);
        response.SetValidationRule(new GreaterOrEqualToValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty> GreaterThen<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty>(builder.PropertyRule);
        response.SetValidationRule(new GreaterThenValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty?> GreaterThen<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty?> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty?>(builder.PropertyRule);
        response.SetValidationRule(new GreaterThenValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty> LessOrEqualTo<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty>(builder.PropertyRule);
        response.SetValidationRule(new LessOrEqualToValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty?> LessOrEqualTo<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty?> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty?>(builder.PropertyRule);
        response.SetValidationRule(new LessOrEqualToValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty> LessThen<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty>(builder.PropertyRule);
        response.SetValidationRule(new LessThenValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty?> LessThen<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty?> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = new PropertyValidationBuilder<T, TProperty?>(builder.PropertyRule);
        response.SetValidationRule(new LessThenValidationRule<TProperty>(rule, builder.PropertyRule.PropertyInfo, builder.PropertyRule.PropertyExpression));
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty> EqualTo<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = GreaterOrEqualTo(builder, rule);
        LessOrEqualTo(response, rule);
        return response;
    }

    public static IPropertyValidationBuilder<T, TProperty?> EqualTo<T, TProperty>(this IInitialPropertyValidationBuilder<T, TProperty?> builder, TProperty rule)
        where TProperty : struct, IComparable, IComparable<TProperty>, IConvertible, IEquatable<TProperty>, IFormattable
    {
        Check.IfNotNumber(typeof(TProperty));
        var response = GreaterOrEqualTo(builder, rule);
        LessOrEqualTo(response, rule);
        return response;
    }

    #endregion NUMBERS
}
