﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Core2D.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Serializer.Newtonsoft
{
    /// <summary>
    /// Json serializer.
    /// </summary>
    public sealed class NewtonsoftTextSerializer : ITextSerializer
    {
        /// <summary>
        /// Specifies the settings on a <see cref="JsonSerializer"/> object.
        /// </summary>
        private static readonly JsonSerializerSettings Settings;

        /// <summary>
        /// Initializes static data.
        /// </summary>
        static NewtonsoftTextSerializer()
        {
            Settings = new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Objects,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                ContractResolver = new ProjectContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                Converters =
                {
                    new KeyValuePairConverter(),
                    new ArgbColorJsonConverter(),
                    new FontStyleConverter(),
                    new ShapeStateConverter()
                }
            };
        }

        /// <inheritdoc/>
        string ITextSerializer.Serialize<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Settings);
        }

        /// <inheritdoc/>
        T ITextSerializer.Deserialize<T>(string text)
        {
            return JsonConvert.DeserializeObject<T>(text, Settings);
        }
    }
}
