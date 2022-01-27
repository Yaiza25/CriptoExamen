using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cripto.Models
{
    public class Cartera
    {
        //Clave Principal NO AUTONUMERICA
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CarteraId { get; set; }
        public string Nombre { get; set; }
        public string Exchange { get; set; }

        //Escribe las propiedades de navegación a otras Entidades
        public List<Contrato> Contratos { get; } = new List<Contrato>();

        // A implementar
        public override string ToString() => $"{CarteraId}: {Nombre} - {Exchange}";
    }
    public class Moneda
    {
        //Clave Principal String
        [Key]
        public string MonedaId { get; set; }
        public decimal Actual { get; set; }
        public decimal Maximo { get; set; }

        //Escribe las propiedades de navegación a otras Entidades
        public List<Contrato> Contratos { get; } = new List<Contrato>();

        // A implementar
        public override string ToString() => $"{MonedaId}: actual = {Actual} / maximo = {Maximo}";
    }
    public class Contrato
    {
        //Decide cómo vas a implementar la clave principal

        //Escribe las propiedades de relación 1:N entre Moneda y Cartera
        [Key]
        public int ContratoId { get; set; }
        public int CarteraId { get; set; }
        public string MonedaId { get; set; }
        public int Cantidad { get; set; }

        //Escribe las propiedades de navegación a otras Entidades
        public Cartera Cartera { get; set; }
        public Moneda Moneda { get; set; }

        // A implementar
        public override string ToString() => $"La cartera {CarteraId} de la moneda {MonedaId} tiene {Cantidad}";
    }

}