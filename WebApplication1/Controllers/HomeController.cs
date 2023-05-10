using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class Ficha
    {
        public int Izquierda { get; set; }
        public int Derecha { get; set; }
    }
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DominoController : ControllerBase
    {

        [HttpPost]
        public IActionResult Ordenar([FromBody] List<Ficha> fichas)
        {
            if (fichas.Count < 2 || fichas.Count > 6)
            {
                return BadRequest("El conjunto de fichas debe tener entre 2 y 6 fichas.");
            }



            var fichasOrdenadas = new List<Ficha> { fichas.First() };
            fichas.RemoveAt(0);

            while (fichas.Any())
            {
                var ultimaFicha = fichasOrdenadas.Last();
                var siguienteFicha = fichas.FirstOrDefault(f =>
                    f.Izquierda == ultimaFicha.Derecha || f.Derecha == ultimaFicha.Derecha);

                if (siguienteFicha == null)
                {
                    return BadRequest("No se pudo construir una cadena correcta de fichas.");
                }

                if (siguienteFicha.Izquierda == ultimaFicha.Derecha)
                {
                    fichasOrdenadas.Add(siguienteFicha);
                }
                else
                {
                    fichasOrdenadas.Add(new Ficha { Izquierda = siguienteFicha.Derecha, Derecha = siguienteFicha.Izquierda });
                }

                fichas.Remove(siguienteFicha);
            }
            if (fichasOrdenadas.First().Izquierda != fichasOrdenadas.Last().Derecha)
            {
                return BadRequest("Los números primero y último no son los mismos.");
            }
            return Ok(fichasOrdenadas);
        }

    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
