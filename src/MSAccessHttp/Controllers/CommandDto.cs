using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;

namespace MSAccessHttp.Controllers;

public record CommandDto(
    [Required]
    string ConnectionString,
    [Required]
    string Query,
    List<ParameterDto>? Parameters
);

public record ParameterDto(
    [Required]
    string ParameterName,
    [property:JsonConverter(typeof(JsonStringEnumConverter<DbType>))]
    [Required]
    DbType? DbType,
    [Required]
    string? Value
);
