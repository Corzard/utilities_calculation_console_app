using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserConsumptionData
    {
        /// <summary>
        /// Тестовый айди
        /// </summary>
        public int ID { get; set; }
        //public string Period => $"{DateTime.Now.Month}/{DateTime.Now.Year}"; 
        public string Period { get; set; }
        public int Persons { get; set; }
        public double CWSConsumption { get; set; }
        public double HWSCConsumption { get; set; }
        public double HWSTEConsumption { get; set; }
        public double E { get; set; }
        public double ED { get; set; }
        public double EN { get; set; }
    }
}
