using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

namespace MSAccessHttp.Controllers;

[Route("[controller]/[action]")]
[ApiController]
[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class CommandController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> NonQuery(CommandDto queryDto)
    {
        var connectionString = queryDto.ConnectionString;
        using var connection = new OleDbConnection(connectionString);
        connection.Open();
        using var command = InitCommand(queryDto, connection);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        return Ok(rowsAffected);
    }

    [HttpPost]
    public async Task<IActionResult> Reader(CommandDto queryDto)
    {
        List<Dictionary<string, object>> result = [];

        var connectionString = queryDto.ConnectionString;
        //"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = C:\\FilePath.accdb";
        using var connection = new OleDbConnection(connectionString);
        connection.Open();
        using var command = InitCommand(queryDto, connection);

        using var reader = await command.ExecuteReaderAsync();
        var columns = await reader.GetColumnSchemaAsync();

        while (await reader.ReadAsync())
        {
            var resultRow = new Dictionary<string, object>();

            foreach (var column in columns)
            {
                if (column.ColumnOrdinal != null)
                {
                    resultRow.Add(
                        column.ColumnName,
                        reader.GetValue((int)column.ColumnOrdinal));
                }
            }
            result.Add(resultRow);
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Scalar(CommandDto queryDto)
    {
        var connectionString = queryDto.ConnectionString;
        //"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = c:\\FilePath";
        using var connection = new OleDbConnection(connectionString);
        connection.Open();
        using var command = InitCommand(queryDto, connection);

        var result = await command.ExecuteScalarAsync();

        return Ok(result);
    }

    private static OleDbCommand InitCommand(CommandDto queryDto, OleDbConnection connection)
    {
        var command = new OleDbCommand(queryDto.Query, connection);
        if (queryDto.Parameters != null)
        {
            foreach (var parameter in queryDto.Parameters)
            {
                command.Parameters.Add(
                    new OleDbParameter
                    {
                        ParameterName = parameter.ParameterName,
                        DbType = parameter.DbType!.Value,
                        Value = parameter.Value,
                    });
            }
        }

        return command;
    }
}
