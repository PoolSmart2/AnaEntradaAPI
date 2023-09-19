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
using System.Collections.Generic;
using System.Text;
using EntradaAPI.BO;

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
                Resultado Resul = new Resultado
                {
                    Estado = false
                };
                try
                {
                    string csvFilePath = csvRequest.UrlCsv;
                    StringBuilder sb = new StringBuilder();

                    using (StreamReader sr = new StreamReader(csvFilePath))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            sb.AppendLine(line);
                        }
                    }

                    string csvString = sb.ToString();


                    Resul.CsvString = csvString;
                    Resul.Estado = true;
                }
                catch (Exception Ex)
                {
                    Resul.Error = Ex.Message;
                }

                //Console.WriteLine(csvString);
                return Ok(Resul);
                //return Ok(JsonConvert.SerializeObject(csvString));


                //Codigo Bueno
                //using var reader = new StreamReader(csvRequest.UrlCsv);
                //using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                //var records = csv.GetRecords<dynamic>();
                //var JSON = JsonConvert.SerializeObject(records);
                //return Ok(JSON);

            }
            catch (Exception ex)
            {
                return BadRequest($"Error al convertir el archivo CSV a JSON: {ex.Message}");
            }
        }

        


    }

}

