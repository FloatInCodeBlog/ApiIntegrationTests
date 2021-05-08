using System;
using System.Collections.Generic;

#nullable disable

namespace WebHostProject.DataAccess.Weather
{
    public partial class Prediction
    {
        public DateTime Day { get; set; }
        public decimal Temperature { get; set; }
    }
}
