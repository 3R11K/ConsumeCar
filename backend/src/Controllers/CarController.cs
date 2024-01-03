// backend/src/Controllers/CarController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CarController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("{model}/{year}/{make}")]
    public async Task<IActionResult> Get(string model, int year, string make)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            var url = $"https://www.fueleconomy.gov/ws/rest/vehicle/menu/options?year={year}&make={make}&model={model}";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var resultStream = await response.Content.ReadAsStreamAsync();
                var xDocument = XDocument.Load(resultStream);

                var id = xDocument.Root
                    .Descendants("menuItem")
                    .Elements("value")
                    .FirstOrDefault();

            if (id != null)
            {
                var urlCarInfo = $"https://www.fueleconomy.gov/ws/rest/vehicle/{id.Value}";
                var responseCarInfo = await client.GetAsync(urlCarInfo);

                if(responseCarInfo.IsSuccessStatusCode)
                {
                    var resultStreamCarInfo = await responseCarInfo.Content.ReadAsStreamAsync();

                    var xDocumentCarInfo = XDocument.Load(resultStreamCarInfo);

                    var eficiency = xDocumentCarInfo.Root
                        .Descendants("comb08")
                        .FirstOrDefault();

                    double eficiencyValue = double.Parse(eficiency.Value);

                    double eficiencyKM = eficiencyValue * 1.60934/3.78541;
                    
                    if(eficiency != null)
                    {
                        return Ok(new Car
                        {
                            Make = make,
                            Model = model,
                            Year = year,
                            FuelEfficiency = eficiencyKM
                        });
                    }else
                    {
                        return StatusCode(500, "Falha ao obter informações do veículo");
                    }
                }else
                {
                    return StatusCode((int)responseCarInfo.StatusCode, "Falha ao obter informações do veículo");
                }
            }
            else
            {
                // Trate o caso em que não há valor disponível
                return StatusCode(500, "Veículo não encontrado");
            }
            }
            else
            {
                return StatusCode((int)response.StatusCode, "Falha ao obter informações do veículo");
            }
        }
        catch (Exception ex)
        {
            // Log ou manipulação de erro
            return StatusCode(500, "Erro interno do servidor");
        }
    }
}
