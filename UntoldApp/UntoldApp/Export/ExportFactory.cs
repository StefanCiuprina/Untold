using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UntoldApp.Export
{
    public static class ExportFactory
    {
        public static Exporter Create(ExportType type)
        {
            if (type == ExportType.CSV)
            {
                return new CsvExporter();
            }
            else
            {
                return new XmlExporter();
            }
        }
    }
}
