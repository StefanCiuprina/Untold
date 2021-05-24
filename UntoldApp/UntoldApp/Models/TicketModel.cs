using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UntoldApp.Models
{
    public class TicketModel
    {
        [Key]
        public int TicketId { get; set; }

        [Column()]
        public int ShowId { get; set; }

        [Column()]
        public int NumberOfPlaces { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Names { get; set; }

    }
}
