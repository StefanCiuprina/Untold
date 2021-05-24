using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UntoldApp.Models;

namespace UntoldApp.Export
{
    public interface Exporter
    {
        void Export(List<TicketModel> tickets);
    }
}
