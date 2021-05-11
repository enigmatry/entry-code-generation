﻿namespace Enigmatry.CodeGeneration.Configuration.List
{
    public interface IListComponentConfiguration<TSource> : IComponentConfiguration<ListComponentBuilder<TSource>> where TSource : class
    {
    }
}