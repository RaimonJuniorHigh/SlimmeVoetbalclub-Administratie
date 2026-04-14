using Microsoft.Data.SqlClient;
using SlimmeVoetbalclub.Web.Models;
using System.Data;

namespace SlimmeVoetbalclub.Web.Repositories
{
    /*
       Deze Repository is de enige plek die met de database praat. 
       Ik gebruik ADO.NET (SqlClient) in plaats van een ORM, 
       waardoor ik zelf de controle heb over de SQL-queries en de mapping.
    */
    public class LedenRepository
    {
        private readonly string _connectionString;

        public LedenRepository(IConfiguration configuration)
        {
            // Haal het adres van de database op (uit appsettings.json)
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Lid> GetAllLeden()
        {
            var ledenLijst = new List<Lid>();

            // STAP 1: Doe de deur naar de database open
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // STAP 2: Stel de vraag aan de database (Hé, geef me alles uit de tabel!)
                string sql = "SELECT * FROM Lidmaatschap";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();

                // STAP 3: Loop door alle rijen die de database teruggeeft
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // STAP 4: Stop de gegevens uit de database in ons C# 'bakje' (Lid.cs)
                        var lid = new Lid
                        {
                            // Pak de kolom uit SQL en stop het in het juiste vakje in C#
                            LidID = Convert.ToInt32(reader["LidID"]),
                            Voornaam = reader["Voornaam"].ToString(),
                            Achternaam = reader["Achternaam"].ToString(),
                            Email = reader["Email"].ToString(),
                            Geboortedatum = Convert.ToDateTime(reader["Geboortedatum"]),
                            IsActief = Convert.ToBoolean(reader["IsActief"]),

                            // Als het team-vakje in SQL leeg is, laat het dan ook leeg in C#
                            TeamID = reader["TeamID"] != DBNull.Value ? Convert.ToInt32(reader["TeamID"]) : null
                        };

                        // Voeg dit lid toe aan onze lijst met alle leden
                        ledenLijst.Add(lid);
                    }
                }
            }

            // Geef de hele lijst met leden terug aan de website
            return ledenLijst;
        }

        public void AddLid(Lid lid)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                // De SQL vraag om een nieuwe rij toe te voegen
                string sql = "INSERT INTO Lidmaatschap (Voornaam, Achternaam, Email, Geboortedatum, IsActief) " +
                             "VALUES (@Voornaam, @Achternaam, @Email, @Geboortedatum, @IsActief)";

                SqlCommand command = new SqlCommand(sql, connection);

                // BEVEILIGING: We gebruiken parameters om SQL-injectie te voorkomen
                command.Parameters.AddWithValue("@Voornaam", lid.Voornaam);
                command.Parameters.AddWithValue("@Achternaam", lid.Achternaam);
                command.Parameters.AddWithValue("@Email", lid.Email);
                command.Parameters.AddWithValue("@Geboortedatum", lid.Geboortedatum);
                command.Parameters.AddWithValue("@IsActief", lid.IsActief);

                connection.Open();
                command.ExecuteNonQuery(); // Voer de opdracht uit
            }
        }

        public void GenerateDummyLeden(int aantal)
        {
            // Lijst met namen om willekeurig uit te kiezen
            string[] voornamen = { "Sven", "Lars", "Tessa", "Milan", "Fleur", "Bram", "Anouk", "Daan" };
            string[] achternamen = { "de Jong", "Scheffers", "Bakker", "Smit", "Visser", "De derde", "Mulder" };
            Random random = new Random();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                for (int i = 0; i < aantal; i++)
                {
                    // Kies een willekeurige voor en achternaam
                    string vNaam = voornamen[random.Next(voornamen.Length)];
                    string aNaam = achternamen[random.Next(achternamen.Length)];
                    string email = vNaam.ToLower() + "." + aNaam.Replace(" ", "").ToLower() + "@gmail.com";

                    string sql = "INSERT INTO Lidmaatschap (Voornaam, Achternaam, Email, Geboortedatum, IsActief) " +
                         "VALUES (@v, @a, @e, @g, 1)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@v", vNaam);
                        command.Parameters.AddWithValue("@a", aNaam);
                        command.Parameters.AddWithValue("@e", email);
                        command.Parameters.AddWithValue("@g", new DateTime(random.Next(1990, 2015), random.Next(1, 12), random.Next(1, 28)));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
