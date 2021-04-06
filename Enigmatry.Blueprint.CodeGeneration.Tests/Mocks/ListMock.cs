using System;
using System.Collections.Generic;

namespace Enigmatry.Blueprint.CodeGeneration.Tests.Mocks
{
    public class ListMock
    {
        public IEnumerable<Item> Items { get; set; } = new List<Item>();

        public class Item
        {
            public Guid Id { get; set; }
            public String Name { get; set; } = String.Empty;
            public DateTimeOffset Date { get; set; }
        }
    }
}
