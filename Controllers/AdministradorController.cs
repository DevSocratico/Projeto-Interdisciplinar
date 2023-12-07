using LocFarma.Models;
using LocFarma.Repositories.ADO.SQLServer;
using Microsoft.AspNetCore.Mvc;

namespace LocFarma.Controllers
{
    public class AdministradorController : Controller
    {
        #region [ Repositório ]

        private readonly UsuarioDAO usuarioDAO;
        private readonly FarmaciaDAO farmaciaDAO;
        private readonly LogContatoDAO logContatoDAO;

        public AdministradorController(IConfiguration configuration)
        {
            this.usuarioDAO = new UsuarioDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
            this.farmaciaDAO = new FarmaciaDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
            this.logContatoDAO = new LogContatoDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
        }

        #endregion [ Repositório ]

        #region [ Listagem Usuários ]

        [HttpGet]
        public IActionResult Usuarios()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            List<Usuario> usuarios = this.usuarioDAO.GetUsuariosContaPessoal();

            return View(usuarios);
        }

        #endregion [ Listagem Usuários ]

        #region [ Listagem Farmácias ]

        [HttpGet]
        public IActionResult Farmacias()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            List<Farmacia> farmacias = this.farmaciaDAO.GetFarmacias();

            return View(farmacias);
        }

        #endregion [ Listagem Farmácias ]

        #region [ Listagem Produtos ]

        [HttpGet]
        public IActionResult Produtos()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            List<Farmacia> farmacias = this.farmaciaDAO.GetFarmacias();

            return View(farmacias);
        }

        #endregion [ Listagem Produtos ]

        #region [ Ativar / Desativar Usuário ]

        [HttpGet]
        public IActionResult AtivarUsuario(int id)
        {
            this.usuarioDAO.AtivarUsuario(id);

            int idTipoUsuario = this.usuarioDAO.GetUsuario(id).Tipo.Id;

            if (idTipoUsuario == 3)
                return RedirectToAction(nameof(Farmacias));

            return RedirectToAction(nameof(Usuarios));
        }

        [HttpGet]
        public IActionResult DesativarUsuario(int id)
        {
            this.usuarioDAO.DesativarUsuario(id);

            int idTipoUsuario = this.usuarioDAO.GetUsuario(id).Tipo.Id;

            if (idTipoUsuario == 3)
                return RedirectToAction(nameof(Farmacias));

            return RedirectToAction(nameof(Usuarios));
        }

        #endregion [ Ativar / Desativar Usuário ]

        #region [ Detalhes Usuário ]

        [HttpGet]
        public IActionResult DetalhesUsuario(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Usuario usuario = this.usuarioDAO.GetUsuario(id);

            return View(usuario);
        }

        #endregion [ Detalhes Usuário ]

        #region [ Detalhes Farmácia ]

        [HttpGet]
        public IActionResult DetalhesFarmacia(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Farmacia farmacia = this.farmaciaDAO.GetFarmacia(id);

            return View(farmacia);
        }

        #endregion [ Detalhes Farmácia ]

        #region [ Log Contato ]

        public IActionResult LogsContato()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            List<LogContato> logsContato = this.logContatoDAO.GetLogContatos();

            return View(logsContato);
        }

        [HttpPost]
        public IActionResult? NovoLogContato(string nome, string email, string mensagem, string caminho)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(mensagem))
                {
                    LogContato logContato = new LogContato();

                    logContato.Nome = nome;
                    logContato.Email = email;
                    logContato.Mensagem = mensagem;

                    this.logContatoDAO.SalvarLogContato(logContato);
                }
            }
            else
            {
                HttpContext.Session.SetString("LogContatoErro", "Dados inválido!");
            }

            return Redirect(caminho);
        }

        #endregion [ Log Contato ]
    }
}
