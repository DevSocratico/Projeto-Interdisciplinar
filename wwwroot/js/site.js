$(document).ready(function () {
    var caminho = window.location.pathname;
    destacarMenu(caminho);
    mostrarCarousel(caminho);
});

// Destacar link do menu
function destacarMenu(caminho) {
    if (caminho === '/') {
        $('#inicio').addClass('active');
    } else if (caminho === '/Home/Sobre') {
        $('#sobre').addClass('active');
    }
}

// Mostrar carousel na página inicial
function mostrarCarousel(caminho) {
    if (caminho === '/') {
        $('#carouselIndicators').removeClass("d-none").addClass("d-block");
    } else {
        $('#carouselIndicators').removeClass("d-block").addClass("d-none");
    }
}

// Modal Contato
function abrirModalContato() {
    const cadastroModal = new bootstrap.Modal(document.getElementById('contatoModal'));
    cadastroModal.show();
}

// Modal Login
function abrirModalLogin() {
    const cadastroModal = new bootstrap.Modal(document.getElementById('loginModal'));
    cadastroModal.show();
}

// Modal Cadastro
function abrirModalCadastro() {
    const cadastroModal = new bootstrap.Modal(document.getElementById('cadastroModal'));
    cadastroModal.show();
}

// Modal Cadastro Conta Pessoal
function abrirModalCadastroContaPessoal() {
    const cadastroContaPessoalModal = new bootstrap.Modal(document.getElementById('cadastroContaPessoalModal'));
    cadastroContaPessoalModal.show();
}

// Modal Cadastro Farmácia
function abrirModalCadastroFarmacia() {
    const cadastroFarmaciaModal = new bootstrap.Modal(document.getElementById('cadastroFarmaciaModal'));
    cadastroFarmaciaModal.show();
}

// Modal Alerta
function abrirModalAlerta() {
    const cadastroModal = new bootstrap.Modal(document.getElementById('alertaModal'));
    cadastroModal.show();
}

// Modo Noturno
const botaoDark = document.getElementById('modo-noturno');
let theme = localStorage.getItem('data-theme');

if (theme == 'dark') {
    botaoDark.checked = true;
}

botaoDark.addEventListener('change', () => {
    if (botaoDark.checked) {
        ativarModoNoturno();
    } else {
        ativarModoClaro();
    }
});

const ativarModoNoturno = () => {
    document.documentElement.setAttribute("data-theme", "dark");
    localStorage.setItem("data-theme", "dark");
}

const ativarModoClaro = () => {
    document.documentElement.setAttribute("data-theme", "light");
    localStorage.setItem("data-theme", 'light');
}

if (theme == 'dark') ativarModoNoturno();