using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cripto.Models;

namespace CriptoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly CryptoContext db;

        public QueryController(CryptoContext context)
        {
            db = context;
        }

        [HttpGet("1")]
        public async Task<ActionResult> Query1(int ValorActual = 50)
        {
            var list = await db.Moneda.Where(o => o.Actual > ValorActual)
            .Select(b => new
            {
                b.MonedaId,
                b.Maximo,
                b.Actual
            }).ToListAsync();

            return Ok(new
            {
                ValorActual = 1,
                Descripcion = "Monedas con valor actual superior a 50€ ordenadas alfabéticamente",
                Valores = list,
            });
        }

        [HttpGet("2")]
        public async Task<ActionResult> Query2(int ValorMonedas = 2)
        {
            var list = await db.Cartera.Select(m => new
            {
                CarteraId = m.CarteraId,
                TotalMonedas = m.Contratos.Count()
            }).Where(o => o.TotalMonedas > ValorMonedas)
            .ToListAsync();

            return Ok(new
            {
                ValorActual = 2,
                Descripcion = "Carteras con más de 2 monedas contratadas",
                Valores = list,
            });
        }

        [HttpGet("3")]
        public async Task<ActionResult> Query3()
        {
            var list = await db.Cartera.GroupBy(o => o.Exchange)
            .Select(g => new
            {
                Exchange = g.Key,
                TotalCarteras = g.Count()
            }).OrderByDescending(o => o.TotalCarteras)
            .ToListAsync();

            return Ok(new
            {
                ValorActual = 3,
                Descripcion = "Exchanges ordenados por números de carteras",
                Valores = list,
            });
        }

        [HttpGet("4")] // A medias
        public async Task<ActionResult> Query4()
        {
            var list = await db.Cartera.Select(g => new {
                Exchange = g.Exchange,
                TotalMonedas = g.Contratos.Count()
            }).GroupBy(o => o.Exchange)
            .ToListAsync();

            return Ok(new
            {
                ValorActual = 4,
                Descripcion = "Exchanges ordenados por cantidad de monedas",
                Valores = list,
            });
        }

        [HttpGet("5")] 
        public async Task<ActionResult> Query5()
        {
            var list = await db.Contrato.Join(db.Moneda, c => c.MonedaId, o => o.MonedaId, (c, o) => new {
                Moneda = c.MonedaId,
                Contrato = $"{c.MonedaId}{c.ContratoId}",
                ValorContrato = c.Cantidad * o.Actual
            }).OrderByDescending(o => o.ValorContrato)
            .ToListAsync();

            return Ok(new
            {
                ValorActual = 5,
                Descripcion = "Monedas en contratos ordenadas por valor total actual",
                Valores = list,
            });
        }

        [HttpGet("6")]
        public async Task<ActionResult> Query6()
        {
            var list = await db.Contrato.Join(db.Moneda, c => c.MonedaId, o => o.MonedaId, (c, o) => new {
                Moneda = c.MonedaId,
                ValorContrato = c.Cantidad * o.Actual
            }).GroupBy(o => o.Moneda)
            .Select(g => new {
                Moneda = g.Key,
                ValorTotal = g.Sum(o => o.ValorContrato)
            }).OrderByDescending(o => o.ValorTotal).ToListAsync();

            return Ok(new
            {
                ValorActual = 6,
                Descripcion = "Monedas en contratos ordenadas por valor actual total en todos los contratos",
                Valores = list,
            });
        }

        [HttpGet("7")]
        public async Task<ActionResult> Query7()
        {
            var list = await db.Contrato.Join(db.Moneda, c => c.MonedaId, o => o.MonedaId, (c, o) => new {
                Moneda = c.MonedaId,
                ValorContrato = c.Cantidad * o.Actual
            }).GroupBy(o => o.Moneda)
            .Select(g => new {
                Moneda = g.Key,
                ValorTotal = g.Sum(o => o.ValorContrato),
                Contratos = g.Count()
            }).OrderByDescending(o => o.Contratos).ToListAsync();

            return Ok(new
            {
                ValorActual = 7,
                Descripcion = "Idem contando en cuantos contratos aparecen y ordenado por número de contratos",
                Valores = list,
            });
        }


        [HttpGet("8")] // A medias
        public async Task<ActionResult> Query8()
        {
            var list = await db.Contrato.Join(db.Moneda, c => c.MonedaId, o => o.MonedaId, (c, o) => new {
                Moneda = c.MonedaId,
                ValorContrato = c.Cantidad * o.Actual
            }).GroupBy(o => o.Moneda)
            .Select(g => new {
                Moneda = g.Key,
                ValorTotal = g.Sum(o => o.ValorContrato),
                Contratos = g.Count()
            }).OrderByDescending(o => o.Contratos).ToListAsync();

            return Ok(new
            {
                ValorActual = 8,
                Descripcion = "Idem pero con Exchanges ordenados por valor total",
                Valores = list,
            });
        }


        [HttpGet("9")] // A medias
        public async Task<ActionResult> Query9(int ValorPorcentaje = 90)
        {
            var list = await db.Contrato.Join(db.Moneda, c => c.MonedaId, o => o.MonedaId, (c, o) => new {
                Moneda = c.MonedaId,
                Contrato = $"{c.MonedaId}{c.ContratoId}",
                Máximo = o.Maximo,
                Actual = o.Actual,
                Porcentaje = (o.Actual * 100) / o.Maximo
            }).Where(d => d.Porcentaje > ValorPorcentaje)
            .ToListAsync();

            return Ok(new
            {
                ValorActual = 9,
                Descripcion = "Las Contratos y Monedas de Binance con monedas cuyo valor actual es inferior al 90% del valor máximo",
                Valores = list,
            });
        }
    }
}
