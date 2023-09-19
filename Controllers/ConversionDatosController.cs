using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using static EntradaAPI.ConversionDatos;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;
using Newtonsoft.Json;

namespace EntradaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConversionDatosController: ControllerBase
    {
        private readonly ILogger<ConversionDatosController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ConversionDatosController(ILogger<ConversionDatosController> logger
            , IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ConvertCsvToJson([FromBody] ConversionDatos csvRequest)
        {
            try
            {
                using var reader = new StreamReader(@"C:\Users\desarrollo1\Downloads\centros_atencion_metrosalud.csv");
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<dynamic>();
              
                var JSON = JsonConvert.SerializeObject(records);
                return Ok(JSON);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al convertir el archivo CSV a JSON: {ex.Message}");
            }
        }

        


    }

}

