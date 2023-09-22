using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Mime;
using System.Text.Json;
using System.Collections.Generic;
using EntradaAPI.BO;
using AD = Microsoft.ApplicationBlocks.Data.SqlHelper;
using System.Data;

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


        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> Entrada()
        {
            Resultado Resul = new Resultado
            {
                Estado = false
            };

            //1. Consultar información de evergreen
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.datos.gov.co/resource/t2ca-uae5.json");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            List<EvergreenResponse> _EvergreenModel = new List<EvergreenResponse>();
            _EvergreenModel = JsonSerializer.Deserialize<List<EvergreenResponse>>((await response.Content.ReadAsStringAsync()));
            //Console.WriteLine(await response.Content.ReadAsStringAsync());

            //2. Almacenar información en BD Azure
            Resul.ResultadoJson = _EvergreenModel;

            string stringConn = "Server=tcp:srv-evergreen.database.windows.net,1433;Initial Catalog=EvergreenAna;Persist Security Info=False;User ID=azureuser;Password=Q1w2e3r4123.*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            foreach (EvergreenResponse Fila in _EvergreenModel)
            {

                //3. valida información existente
                DataTable Dt =  AD.ExecuteDataset(stringConn, "Sp_Evergreen_Valida_Detalle"
                    , Fila.tipo
                    , Fila.rubro
                    , Fila.subregion
                    , Fila.anio
                    , Fila.municipio
                    , Fila.area_total
                    , Fila.area_produccion
                    , Fila.volumen_produccion).Tables[0];

                //4. Inserta si no existe información
                if (Dt.Rows.Count == 0)
                {
                    AD.ExecuteNonQuery(stringConn, "Sp_Evergreen_Guardar_Detalle"
                        , Fila.tipo
                        , Fila.rubro
                        , Fila.subregion
                        , Fila.anio
                        , Fila.municipio
                        , Fila.area_total
                        , Fila.area_produccion
                        , Fila.volumen_produccion);
                }
            }

            return Ok(Resul);
        }

        //[HttpPost]
        //[Consumes(MediaTypeNames.Application.Json)]
        //public async Task<IActionResult> ConvertCsvToJson([FromBody] ConversionDatos csvRequest)
        //{
        //    try
        //    {
        //        Resultado Resul = new Resultado
        //        {
        //            Estado = false
        //        };
        //        try
        //        {
        //            string csvFilePath = csvRequest.UrlCsv;
        //            StringBuilder sb = new StringBuilder();

        //            using (StreamReader sr = new StreamReader(csvFilePath))
        //            {
        //                string line;
        //                while ((line = sr.ReadLine()) != null)
        //                {
        //                    sb.AppendLine(line);
        //                }
        //            }

        //            string csvString = sb.ToString();


        //            Resul.CsvString = csvString;
        //            Resul.Estado = true;
        //        }
        //        catch (Exception Ex)
        //        {
        //            Resul.Error = Ex.Message;
        //        }

        //        //Console.WriteLine(csvString);
        //        return Ok(Resul);
        //        //return Ok(JsonConvert.SerializeObject(csvString));


        //        //Codigo Bueno
        //        //using var reader = new StreamReader(csvRequest.UrlCsv);
        //        //using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        //        //var records = csv.GetRecords<dynamic>();
        //        //var JSON = JsonConvert.SerializeObject(records);
        //        //return Ok(JSON);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error al convertir el archivo CSV a JSON: {ex.Message}");
        //    }
        //}

        


    }

}

