// Modal Cadastro
function abrirModalCadastro() {
    const cadastroModal = new bootstrap.Modal(document.getElementById('cadastroModal'));
    cadastroModal.show();
}

function abrirModalCadastroContaPessoal() {
    const cadastroContaPessoalModal = new bootstrap.Modal(document.getElementById('cadastroContaPessoalModal'));
    cadastroContaPessoalModal.show();
}

function abrirModalCadastroFarmacia() {
    const cadastroFarmaciaModal = new bootstrap.Modal(document.getElementById('cadastroFarmaciaModal'));
    cadastroFarmaciaModal.show();
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