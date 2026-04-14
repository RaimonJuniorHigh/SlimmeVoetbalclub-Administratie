using Microsoft.AspNetCore.Mvc;
using SlimmeVoetbalclub.Web.Models;
using SlimmeVoetbalclub.Web.Repositories;

namespace SlimmeVoetbalclub.Web.Controllers
{
    // De Controller regelt alles wat je op het scherm ziet over de Leden
    public class LedenController : Controller
    {
        private readonly LedenRepository _ledenRepository;

        // Hier halen we de Repository op die we in Program.cs hebben klaargezet
        public LedenController(LedenRepository ledenRepository)
        {
            _ledenRepository = ledenRepository;
        }

        // Dit is de 'Index' pagina (de hoofdpagina van Leden)
        public IActionResult Index()
        {
            // 1. We vragen de Repository om de lijst met alle leden uit de database
            var leden = _ledenRepository.GetAllLeden();

            // 2. We geven die lijst door aan de webpagina (de View)
            return View(leden);
        }

        // 1. Dit opent het lege formulier als je op de knop klikt
        public IActionResult Create()
        {
            return View();
        }
        // 2. Dit vangt de data op ALS je op opslaan klikt
        [HttpPost]
        public IActionResult Create(Lid nieuwLid)
        {
            // Hier zeggen we tegen de Repository: "Breng dit nieuwe lid naar de database"
            _ledenRepository.AddLid(nieuwLid);

            // Als het klaar is, sturen we de gebruiker terug naar de lijst
            return RedirectToAction("Index");
        }
        public IActionResult GenereerDummyData()
        {
            // Roepen de dummy functie op
            _ledenRepository.GenerateDummyLeden(10);

            // Na het genereren terug naar de index pagina
            return RedirectToAction("Index");
        }

    }
}