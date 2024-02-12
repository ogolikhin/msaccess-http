﻿using System.Data;
using System.Text.Json.Serialization;

namespace MSAccessHttp.Controllers;

public record QueryDto(
    string ConnectionString,
    string Query,
    List<ParameterDto>? Parameters
);

public record ParameterDto(
    string ParameterName,
    [property:JsonConverter(typeof(JsonStringEnumConverter))]
    DbType DbType,
    string? Value
);
