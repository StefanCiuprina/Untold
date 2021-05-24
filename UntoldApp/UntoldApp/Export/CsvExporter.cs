using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UntoldApp.Models;

namespace UntoldApp.Export
{
    public class CsvExporter : Exporter
    {
        public void Export(List<TicketModel> tickets)
        {
            var fileName = "tickets.csv";
            var lines = new List<string>();
            IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(TicketModel)).OfType<PropertyDescriptor>();
            var header = string.Join(",", props.ToList().Select(x => x.Name));
            lines.Add(header);
            var valueLines = tickets.Select(row => string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
            lines.AddRange(valueLines);
            File.WriteAllLines(fileName, lines.ToArray());
        }
    }
}
