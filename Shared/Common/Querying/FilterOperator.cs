using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Common.Querying;

/// <summary>
/// Mendefinisikan operator filter yang didukung.
/// [JsonConverter] memberi tahu Swagger (dan serializer)
/// untuk menggunakan nama string ("Contains") alih-alih angka (0).
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FilterOperator
{
    Contains,
    Equals,
    NotEquals,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    StartsWith,
    EndsWith
}