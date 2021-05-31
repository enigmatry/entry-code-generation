using System;

namespace Enigmatry.CodeGeneration.Configuration.Builder
{
    public abstract class BaseComponentBuilder<T> : IComponentBuilder<T> where T : IComponentModel
    {
        protected readonly Type _modelType;
        protected readonly ComponentInfoBuilder _componentInfoBuilder;
        protected readonly RoutingInfoBuilder _routingInfoBuilder;
        protected readonly ApiClientInfoBuilder _apiClientInfoBuilder;
        protected readonly FeatureInfoBuilder _featureInfoBuilder;

        protected BaseComponentBuilder(Type modelType)
        {
            _modelType = modelType;
            _componentInfoBuilder = new ComponentInfoBuilder(modelType);
            _routingInfoBuilder = new RoutingInfoBuilder();
            _apiClientInfoBuilder = new ApiClientInfoBuilder();
            _featureInfoBuilder = new FeatureInfoBuilder();
        }

        public ComponentInfoBuilder Component() => _componentInfoBuilder;

        public RoutingInfoBuilder Routing() => _routingInfoBuilder;

        public ApiClientInfoBuilder ApiClient() => _apiClientInfoBuilder;

        public FeatureInfoBuilder Feature() => _featureInfoBuilder;

        public abstract T Build();
    }
}
