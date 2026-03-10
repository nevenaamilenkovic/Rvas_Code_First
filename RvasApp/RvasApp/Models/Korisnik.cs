using Microsoft.AspNetCore.Identity;
namespace RvasApp.Models
{
    public class Korisnik:IdentityUser
    {
        public string? Ime { get; set; }
        public string? Prezime { get; set; }
        //Username kod IdentityUser-a je uvek email!
        public string? KorisnickoIme { get; set; }

        public DateTime DatumRegistracije { get; set; } = DateTime.UtcNow;
    }
}
