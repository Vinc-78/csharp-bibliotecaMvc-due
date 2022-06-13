//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace csharp_bibliotecaMvc.Models
//{
//    [Table("Autore")]
//    public class Autore
//    {
//        [Key]
//        public int AutoreId { get; set; }

       
//        public string Nome { get; set; }
//        public string Cognome { get; set; }

//        public ICollection<Libro>? Libro { get; set; }

//        //public List<Autori>? ElencoAutori { get; set; } // elenco creato per lista autori
//    }
//}


using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace csharp_bibliotecaMvc_due.Models
{
    public class Autore
    {
        [Column(Order = 1), System.ComponentModel.DataAnnotations.Key]
        public string Cognome { get; set; }

        [Column(Order = 2), System.ComponentModel.DataAnnotations.Key]
        public string Nome { get; set; }

        [Column(Order = 3), System.ComponentModel.DataAnnotations.Key]
        public DateTime DataNascita { get; set; }

        public ICollection<Libro>? Libri { get; set; }

    }
}
