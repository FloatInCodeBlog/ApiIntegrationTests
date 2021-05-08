using System;
using System.Collections.Generic;

#nullable disable

namespace WebHostProject.DataAccess.Weather
{
    public partial class SchemaMigration
    {
        public int InstalledRank { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Script { get; set; }
        public int? Checksum { get; set; }
        public string InstalledBy { get; set; }
        public DateTime InstalledOn { get; set; }
        public int ExecutionTime { get; set; }
        public bool Success { get; set; }
    }
}
