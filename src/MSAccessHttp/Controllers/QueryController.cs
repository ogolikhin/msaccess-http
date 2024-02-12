using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSAccessHttp.Controllers;

[Route("")]
[ApiController]
[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class QueryController : ControllerBase
{
    // GET: api/<QueryController>
    [HttpPost("[action]")]
    public async Task<IActionResult> Select(QueryDto queryDto)
    {
        List<Dictionary<string, object>> result = [];

        var connectionString = queryDto.ConnectionString;
        //"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = c:\\FilePath";
        using var connection = new OleDbConnection(connectionString);
        connection.Open();
        using var command = new OleDbCommand(queryDto.Query, connection);
        if (queryDto.Parameters != null)
        {
            foreach (var parameter in queryDto.Parameters)
            {
                command.Parameters.Add(
                    new OleDbParameter
                    {
                        ParameterName = parameter.ParameterName,
                        DbType = parameter.DbType,
                        Value = parameter.Value,
                    });
            }
        }

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

    // POST api/<QueryController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<QueryController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<QueryController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
