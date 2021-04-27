﻿using System;

namespace Enigmatry.Blueprint.CodeGeneration.Configuration.Builder
{
    public class ApiClientInfoBuilder : IBuilder<ApiClientInfo>
    {
        private string _apiClientName = String.Empty;

        public ApiClientInfoBuilder WithName(string apiClientName)
        {
            _apiClientName = apiClientName.Replace("Controller", "");
            return this;
        }

        public ApiClientInfo Build()
        {
            return new ApiClientInfo(_apiClientName);
        }
    }
}
