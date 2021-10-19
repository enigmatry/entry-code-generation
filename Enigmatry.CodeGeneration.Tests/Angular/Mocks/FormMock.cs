﻿using System;

namespace Enigmatry.CodeGeneration.Tests.Angular.Mocks
{
    public class FormMock
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public DateTimeOffset Date { get; set; }
        public decimal Money { get; set; }
        public int Amount { get; set; }
        public string Email1 { get; set; } = String.Empty;
        public string Email2 { get; set; } = String.Empty;
        public EnumMock FormStatus { get; set; }
        public Guid CategoryId { get; set; }
        public Guid TypeId { get; set; }
        public Guid SubTypeId { get; set; }
        public EnumMock MockRadio { get; set; }
    }
}