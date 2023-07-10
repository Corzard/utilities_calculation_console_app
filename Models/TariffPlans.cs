﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
    //public class EntityAttribute : Attribute
    //{

    //}
    //[Entity]
    public class TariffPlans 
    {
        [Key]
        public string Service { get; set; }
        /// <summary>
        /// RUB per utin
        /// </summary>
        public double Tariff { get; set; }
        public double Standart { get; set; }
        public string Units { get; set; }
    }
}
