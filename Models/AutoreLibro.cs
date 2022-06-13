namespace csharp_bibliotecaMvc_due.Models
{
    public class AutoreLibro
    {
        public int IdLibro { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }

        public List<Autore>? listaAu { get; set; }

    }
}
