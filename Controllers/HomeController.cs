using LocFarma.Models;
using LocFarma.Repositories.ADO.SQLServer;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Diagnostics;

namespace LocFarma.Controllers
{
    public class HomeController : Controller
    {
        #region [ Repositório ]

        private readonly FarmaciaDAO farmamciaDAO;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.farmamciaDAO = new FarmaciaDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
        }

        #endregion [ Repositório ]

        public IActionResult Index()
        {
            List<Produto> produtos = this.farmamciaDAO.GetProdutosDestaque();
            return View(produtos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}