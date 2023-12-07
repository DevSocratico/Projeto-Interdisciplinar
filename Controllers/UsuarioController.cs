using LocFarma.Repositories.ADO.SQLServer;
using Microsoft.AspNetCore.Mvc;
using LocFarma.Biblioteca;
using LocFarma.Models;

namespace LocFarma.Controllers
{
    public class UsuarioController : Controller
    {
        #region [ Repositório ]

        private readonly UsuarioDAO repositorio;
        private readonly IWebHostEnvironment webHostEnvironment;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.repositorio = new UsuarioDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
            this.webHostEnvironment = webHostEnvironment;
        }

        #endregion [ Repositório ]

        #region [ Login / Logout ]

        [HttpPost]
        public IActionResult Login(string caminho, string email, string senha)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
                {
                    Usuario usuario = this.repositorio.GetUsuario(email, Criptografia.CriptografarSenhaSHA256(senha));

                    if (usuario.Id > 0)
                    {
                        this.repositorio.LoginUsuario(usuario.Id);
                        HttpContext.Session.SetInt32("IdUsuario", usuario.Id);
                        HttpContext.Session.SetInt32("IdTipoUsuario", usuario.Tipo.Id);

                        ViewBag.TipoUsuario = usuario.Tipo.Id;

                        if (usuario.Tipo.Id == 3)
                            return RedirectToAction("Perfil", "Farmacia");

                        return View("Perfil", usuario);
                    }
                }
            }

            HttpContext.Session.SetString("LoginErro", "Dados inválido!");

            return Redirect(caminho);
        }

        public IActionResult Logout(int idUsuario)
        {
            this.repositorio.LogoutUsuario(idUsuario);
            HttpContext.Session.SetInt32("IdUsuario", 0);
            HttpContext.Session.SetInt32("IdTipoUsuario", 0);

            return RedirectToAction("Index", "Home");
        }

        #endregion [ Login / Logout ]

        #region [ Cadastro Usuário ]

        [HttpPost]
        public IActionResult CadastrarContaPessoal(string caminho, string nome, string email, string senha)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
                {
                    if (this.repositorio.CheckEmail(email))
                    {
                        TipoUsuario tipoUsuario = new TipoUsuario(2, "Conta Pessoal");

                        Usuario usuario = new Usuario();

                        usuario.Tipo = tipoUsuario;
                        usuario.Nome = nome;
                        usuario.Email = email;
                        usuario.Senha = Criptografia.CriptografarSenhaSHA256(senha);
                        usuario.NomeFoto = "perfil.svg";

                        this.repositorio.AdicionarContaPessoal(usuario);

                        HttpContext.Session.SetInt32("IdUsuario", usuario.Id);

                        this.repositorio.LoginUsuario(usuario.Id);
                        return View("Perfil", usuario);
                    }
                }
            }

            HttpContext.Session.SetString("CadastroContaPessoalErro", "Dados inválido!");

            return Redirect(caminho);
        }

        [HttpPost]
        public IActionResult CadastrarFarmacia(string caminho, string nome, string email, string senha, string cnpj)
        {
            if (ModelState.IsValid)
            {
                if (this.repositorio.CheckEmail(email))
                {
                    if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha) && !string.IsNullOrEmpty(cnpj))
                    {
                        TipoUsuario tipoUsuario = new TipoUsuario(3, "Farmácia");

                        Farmacia farmacia = new Farmacia();

                        farmacia.Tipo = tipoUsuario;
                        farmacia.Nome = nome;
                        farmacia.Email = email;
                        farmacia.Senha = Criptografia.CriptografarSenhaSHA256(senha);
                        farmacia.CNPJ = cnpj;
                        farmacia.NomeFoto = "perfil.svg";

                        this.repositorio.AdicionarFarmacia(farmacia);

                        HttpContext.Session.SetInt32("IdUsuario", farmacia.Id);

                        this.repositorio.LoginUsuario(farmacia.Id);

                        return RedirectToAction("Perfil", "Farmacia", farmacia);
                    }
                }
            }

            HttpContext.Session.SetString("CadastroFarmaciaErro", "Dados inválido!");

            return Redirect(caminho);
        }

        #endregion [ Cadastro Usuário ]

        #region [ Editar Usuário ]

        [HttpGet]
        public IActionResult EditarUsuario(int id, string? origem = null)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;
            ViewBag.Origem = origem;

            Usuario usuario = this.repositorio.GetUsuario(id);

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario.Foto != null)
                {
                    usuario.NomeFoto = $"{Guid.NewGuid()}-{usuario.Foto.FileName}";
                    string pasta = $"imgs/perfil/{usuario.NomeFoto}";
                    string pastaServidor = Path.Combine(this.webHostEnvironment.WebRootPath, pasta);
                    usuario.Foto.CopyTo(new FileStream(pastaServidor, FileMode.Create));
                }
                else
                {
                    usuario.NomeFoto = this.repositorio.GetNomeFoto(usuario.Id);
                }

                this.repositorio.AlterarUsuario(usuario);
                return RedirectToAction(nameof(Perfil));
            }
            catch
            {
                return View();
            }
        }

        #endregion [ Editar Usuário ]

        #region [ Perfil Usuário ]

        [HttpGet]
        public IActionResult Perfil()
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            Usuario usuario = this.repositorio.GetUsuario(idUsuario);

            HttpContext.Session.SetInt32("IdTipoUsuario", usuario.Tipo.Id);

            ViewBag.TipoUsuario = usuario.Tipo.Id;

            return View(usuario);
        }

        #endregion [ Perfil Usuário ]

        #region [ Cadastrar Endereço ]

        public IActionResult CadastrarEndereco()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CadastrarEndereco(Endereco endereco)
        {
            try
            {
                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                this.repositorio.NovoEndereco(idUsuario, endereco);

                return RedirectToAction(nameof(Enderecos));
            }
            catch
            {
                return View();
            }
        }

        #endregion [ Cadastrar Endereço ] 

        #region [ Listagem Endereços ]

        public IActionResult Enderecos()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            List<Endereco> enderecos = this.repositorio.GetEnderecos(idUsuario);

            return View(enderecos);
        }

        #endregion [ Listagem Endereços ]

        #region [ Detalhes Endereço ]

        public IActionResult DetalhesEndereco(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Endereco endereco = this.repositorio.GetEndereco(id);

            return View(endereco);
        }

        #endregion [ Detalhes Endereço ]

        #region [ Editar Endereço ]

        [HttpGet]
        public IActionResult EditarEndereco(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Endereco endereco = this.repositorio.GetEndereco(id);

            return View(endereco);
        }

        [HttpPost]
        public IActionResult EditarEndereco(Endereco endereco)
        {
            try
            {
                this.repositorio.AlterarEndereco(endereco);

                int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

                return RedirectToAction(nameof(Enderecos));
            }
            catch
            {
                return View();
            }
        }

        #endregion [ Editar Produto ]

        #region [ Excluir Produto ]

        public IActionResult ExcluirEndereco(int id)
        {
            this.repositorio.ExcluirEndereco(id);

            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            return RedirectToAction(nameof(Enderecos));
        }

        #endregion [ Excluir Produto ]

        #region [ Alterar Senha ]

        [HttpGet]
        public IActionResult AlterarSenha()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            return View();
        }

        [HttpPost]
        public IActionResult AlterarSenha(Usuario usuario)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            if (!string.IsNullOrEmpty(usuario.SenhaAtual) && !string.IsNullOrEmpty(usuario.NovaSenha))
            {
                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                string senhaAtual = Criptografia.CriptografarSenhaSHA256(usuario.SenhaAtual);

                if (this.repositorio.CheckSenha(idUsuario, senhaAtual))
                {
                    string novaSenha = Criptografia.CriptografarSenhaSHA256(usuario.NovaSenha);
                    this.repositorio.AlterarSenha(idUsuario, novaSenha);

                    ViewBag.MsgAlterarSenha = "sucesso";
                }
                else
                {
                    ViewBag.MsgAlterarSenha = "erro";
                }
            }

            return View(usuario);
        }

        #endregion [ Alterar Senha ]
    }
}
