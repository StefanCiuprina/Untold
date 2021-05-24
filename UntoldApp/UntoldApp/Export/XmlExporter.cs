using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UntoldApp.Models;

namespace UntoldApp.Export
{
    public class XmlExporter : Exporter
    {
        public void Export(List<TicketModel> tickets)
        {
            System.Xml.Serialization.XmlSerializer writer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<TicketModel>));

            var path = $"tickets.xml";
            System.IO.FileStream file = System.IO.File.Create(path);

            writer.Serialize(file, tickets);
            file.Close();
        }
    }
}
