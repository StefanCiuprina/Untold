using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace UntoldApp.Models
{
    public class ShowModel
    {
        [Key]
        public int ShowId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ArtistName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Genre { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime DateTime { get; set; }

        [Column()]
        public int MaximumNoOfTickets { get; set; }
    }
}
