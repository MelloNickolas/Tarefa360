import style from "./_login.module.css";
import Button from "react-bootstrap/Button";
import { useState, useRef, useCallback } from "react";
import { useNavigate, Link } from "react-router-dom";
import ReCAPTCHA from "react-google-recaptcha";
import { iniciarLogin, confirmarCodigo } from "../../services/ServicoAutenticacao";
import logo from "../../assets/LogoAzul.png";

// ATENÇÃO: substitua pela sua Site Key pública do reCAPTCHA v2.
// Obtenha em: https://www.google.com/recaptcha/admin
// A Site Key é PÚBLICA — pode ficar no código do frontend.
// A Secret Key fica SOMENTE no appsettings.json da API.
const RECAPTCHA_SITE_KEY = "6LeDD6osAAAAAIUNXdiR1uEiK80gmmlWjDo5oPIt";

export function Login() {
  const [email, setEmail] = useState("");
  const [senha, setSenha] = useState("");
  const [captchaToken, setCaptchaToken] = useState(null);
  const [carregando, setCarregando] = useState(false);
  const [erro, setErro] = useState("");

  // Estados do modal de verificação 2FA
  const [modalAberto, setModalAberto] = useState(false);
  const [codigo, setCodigo] = useState("");
  const [erroModal, setErroModal] = useState("");
  const [carregandoModal, setCarregandoModal] = useState(false);

  const captchaRef = useRef(null);
  const navigate = useNavigate();

  // PASSO 1: clicou em "Entrar"
  async function handleLogin(e) {
    e.preventDefault();
    setErro("");

    if (!captchaToken) {
      setErro("Por favor, confirme que você não é um robô.");
      return;
    }

    setCarregando(true);

    try {
      await iniciarLogin(email, senha, captchaToken);
      setModalAberto(true);
    } catch (error) {
      const msg =
        error?.response?.data?.mensagem || "Usuário ou senha inválidos.";
      setErro(msg);
      captchaRef.current?.reset();
      setCaptchaToken(null);
    } finally {
      setCarregando(false);
    }
  }

  // PASSO 2: enviou o código do modal
  async function handleConfirmarCodigo(e) {
    e.preventDefault();
    setErroModal("");

    if (!codigo || codigo.length !== 6) {
      setErroModal("Digite o código de 6 dígitos recebido por e-mail.");
      return;
    }

    setCarregandoModal(true);

    try {
      await confirmarCodigo(email, codigo);
      navigate("/home");
    } catch (error) {
      const msg =
        error?.response?.data?.mensagem || "Código inválido ou expirado.";
      setErroModal(msg);
      setCodigo("");
    } finally {
      setCarregandoModal(false);
    }
  }

  // Fechar modal e resetar estado
  function handleFecharModal() {
    setModalAberto(false);
    setCodigo("");
    setErroModal("");
    captchaRef.current?.reset();
    setCaptchaToken(null);
  }

  const handleCaptchaChange = useCallback((token) => {
    setCaptchaToken(token);
    setErro("");
  }, []);

  const handleCaptchaExpired = useCallback(() => {
    setCaptchaToken(null);
  }, []);

  return (
    <div className={style["pagina_conteudo"]}>
      <div className={style["login-container"]}>
        <img src={logo} className={style["logo"]} alt="Logo Tarefa360" />

        {/* Formulário principal */}
        <form onSubmit={handleLogin}>
          <div className={style["input-group"]}>
            <input
              type="email"
              placeholder="e-mail"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>

          <div className={style["input-group"]}>
            <input
              type="password"
              placeholder="senha"
              value={senha}
              onChange={(e) => setSenha(e.target.value)}
              required
            />
          </div>

          {/* Widget reCAPTCHA v2 "Não sou um robô" */}
          <div className={style["captcha-container"]}>
            <ReCAPTCHA
              ref={captchaRef}
              sitekey={RECAPTCHA_SITE_KEY}
              onChange={handleCaptchaChange}
              onExpired={handleCaptchaExpired}
            />
          </div>

          {erro && <p className={style["erro-mensagem"]}>{erro}</p>}

          <Button
            className={style["button_entrar"]}
            type="submit"
            disabled={carregando || !captchaToken}
          >
            {carregando ? "Verificando..." : "Entrar"}
          </Button>

          <Link to="/esqueciMinhaSenha" className={style["forgot-password"]}>
            esqueci minha senha
          </Link>
        </form>
      </div>

      {/* Modal de verificação 2FA */}
      {modalAberto && (
        <div className={style["modal-overlay"]} onClick={handleFecharModal}>
          <div
            className={style["modal-box"]}
            onClick={(e) => e.stopPropagation()}
          >
            <h3 className={style["modal-titulo"]}>Verificação de Acesso</h3>

            <p className={style["modal-descricao"]}>
              Enviamos um código de 6 dígitos para{" "}
              <strong>{email}</strong>.<br />
              Digite-o abaixo para acessar o sistema.
            </p>

            <form onSubmit={handleConfirmarCodigo}>
              <div className={style["input-group"]}>
                <input
                  type="text"
                  inputMode="numeric"
                  maxLength={6}
                  placeholder="000000"
                  value={codigo}
                  onChange={(e) =>
                    setCodigo(e.target.value.replace(/\D/g, "").slice(0, 6))
                  }
                  className={style["input-codigo"]}
                  autoFocus
                  required
                />
              </div>

              {erroModal && (
                <p className={style["erro-mensagem"]}>{erroModal}</p>
              )}

              <Button
                className={style["button_entrar"]}
                type="submit"
                disabled={carregandoModal || codigo.length !== 6}
              >
                {carregandoModal ? "Confirmando..." : "Confirmar Código"}
              </Button>

              <button
                type="button"
                className={style["modal-cancelar"]}
                onClick={handleFecharModal}
              >
                Cancelar
              </button>
            </form>

            <p className={style["modal-aviso"]}>
              O código expira em 10 minutos. Verifique também a caixa de spam.
            </p>
          </div>
        </div>
      )}
    </div>
  );
}