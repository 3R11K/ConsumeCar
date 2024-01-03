using Google.Cloud.BigQuery.V2;
using Google.Apis.Auth.OAuth2;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/[controller]")]
public class ConsumeController : ControllerBase
{
    private string credentials = "permissions/valid-perigee-386815-117c454d0389.json";

    [HttpGet("{eficiency}/{kilometers}/{state}")]
    public async Task<IActionResult> Get (double eficiency, string state, double kilometers)
    {
        try
        {
            var credential = GoogleCredential.FromFile(credentials);

            var client = await BigQueryClient.CreateAsync("valid-perigee-386815", credential);

            var sql = $@"SELECT preco_venda FROM `basedosdados.br_anp_precos_combustiveis.microdados` WHERE produto = 'Gasolina' AND sigla_uf = '{state}' ORDER BY ANO DESC LIMIT 100";

            var result = await client.ExecuteQueryAsync(sql, parameters: null);
            var prices = new List<double>();

            foreach(var row in result)
            {
                var price = Convert.ToDouble(row["preco_venda"]);
                prices.Add(price);
            }
            var average = prices.Average();
            var consume = (kilometers / eficiency) * average;
            return Ok(consume);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
