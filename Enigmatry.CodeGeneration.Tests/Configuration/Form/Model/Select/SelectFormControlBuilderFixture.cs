﻿using System;
using System.Collections.Generic;
using System.Linq;
using Enigmatry.CodeGeneration.Configuration.Form.Model;
using Enigmatry.CodeGeneration.Configuration.Form.Model.Select;
using Enigmatry.CodeGeneration.Configuration.Services;
using FluentAssertions;
using NUnit.Framework;

namespace Enigmatry.CodeGeneration.Tests.Configuration.Form.Model.Select
{
    [Category("unit")]
    public class SelectFormControlBuilderFixture
    {
        [Test]
        public void Select_WithFixedValues_EnumValues()
        {
            var formControlBuilder = typeof(TestModel)
                .GetProperties()
                .Where(prop => prop.Name == nameof(TestModel.EnumProperty))
                .Select(propertyInfo => new FormControlBuilder(propertyInfo))
                .Single();

            var selectFormControlModel = formControlBuilder
                .IsDropDownListControl()
                .WithFixedValues<TestEnum>()
                .Build();

            selectFormControlModel.LookupMethod.GetType().Should().Be(typeof(FixedValuesLookupMethod));

            var fixedLookupMethod = (FixedValuesLookupMethod)selectFormControlModel.LookupMethod;

            fixedLookupMethod.Name.Should().Be($"get{formControlBuilder.PropertyInfo.Name}");
            fixedLookupMethod
                .FixedValues.Select(x => x.Value)
                .Should().BeEquivalentTo(Enum.GetValues(typeof(TestEnum)));
            fixedLookupMethod
                .FixedValues.Select(x => x.DisplayName)
                .Should().BeEquivalentTo(Enum.GetNames(typeof(TestEnum)));
        }

        [Test]
        public void Select_WithFixedValues_CustomValues()
        {
            var formControlBuilder = typeof(TestModel)
                .GetProperties()
                .Where(prop => prop.Name == nameof(TestModel.GuidProperty))
                .Select(propertyInfo => new FormControlBuilder(propertyInfo))
                .Single();

            var customOptions = new List<SelectOption>
            {
                new SelectOption(Guid.NewGuid(), "Value 1"),
                new SelectOption(Guid.NewGuid(), "Value 2"),
            };

            var selectFormControlModel = formControlBuilder
                .IsDropDownListControl()
                .WithFixedValues(customOptions)
                .Build();

            selectFormControlModel.LookupMethod.GetType().Should().Be(typeof(FixedValuesLookupMethod));

            var fixedLookupMethod = (FixedValuesLookupMethod)selectFormControlModel.LookupMethod;

            fixedLookupMethod.Name.Should().Be($"get{formControlBuilder.PropertyInfo.Name}");
            fixedLookupMethod
                .FixedValues.Select(x => x.Value)
                .Should().BeEquivalentTo(customOptions.Select(x => x.Value));
            fixedLookupMethod
                .FixedValues.Select(x => x.DisplayName)
                .Should().BeEquivalentTo(customOptions.Select(x => x.DisplayName));
        }
    }

    internal class TestModel
    {
        public TestEnum EnumProperty { get; set; } = TestEnum.First;
        public Guid GuidProperty { get; set; }
    }

    internal enum TestEnum
    {
        First,
        Second,
        Third
    }
}
