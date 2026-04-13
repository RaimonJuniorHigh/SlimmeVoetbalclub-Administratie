using System;

namespace SlimmeVoetbalclub.Web.Models
{
    // Dit is de blauwdruk (template) voor een clublid
    public class Lid
    {
        // Uniek nummer van het lid (Primary Key in de database)
        public int LidID { get; set; }

        // Tekstvelden voor de naam. 
        // '= string.Empty' haalt groene lijntjes weg: het vakje is nooit écht leeg.
        public string Voornaam { get; set; } = string.Empty;
        public string Achternaam { get; set; } = string.Empty;

        // Datumveld voor de geboortedag
        public DateTime Geboortedatum { get; set; }

        // Contactgegevens
        public string Email { get; set; } = string.Empty;

        // Het ID van het team. Het '?' betekent: mag leeg (null) zijn in de database
        public int? TeamID { get; set; }

        // Is het lid momenteel actief bij de club? (Ja/Nee)
        public bool IsActief { get; set; }

        // Plakt voor- en achternaam aan elkaar voor op het scherm
        public string VolledigeNaam => $"{Voornaam} {Achternaam}";
    }
}