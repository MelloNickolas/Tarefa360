import axios from 'axios';

export const HTTPClient = axios.create({
    baseURL: "https://localhost:7205",
    headers: {
        'Content-Type': 'application/json;charset=UTF-8',
    },
});

// Interceptor do Axios = ele executa ANTES de cada requisição sair do front-end.
// Aqui usamos ele para adicionar automaticamente o token JWT no header Authorization. 

HTTPClient.interceptors.request.use((config) => {

    // Recupera o token salvo no navegador após o login. 
    const token = localStorage.getItem("token");

    // Se existir token, adicionamos no header da requisição.
    // O backend espera exatamente este formato: Authorization: Bearer SEU_TOKEN 

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    
    // Retorna a configuração final da requisição 
    return config;
});

// Quando a API retorna 401 (token expirado), tenta renovar usando
// o refreshToken. Se conseguir, reenvia a requisição original.
// Se não conseguir, redireciona para /login.
let estaRenovando = false;
let filaEspera = [];

function processarFila(erro, token = null) {
  filaEspera.forEach(({ resolve, reject }) => {
    if (erro) {
      reject(erro);
    } else {
      resolve(token);
    }
  });
  filaEspera = [];
}

HTTPClient.interceptors.response.use(
  (response) => response,

  async (error) => {
    const reqOriginal = error.config;

    if (error.response?.status !== 401 || reqOriginal._jaRenovou) {
      return Promise.reject(error);
    }

    reqOriginal._jaRenovou = true;

    if (estaRenovando) {
      return new Promise((resolve, reject) => {
        filaEspera.push({ resolve, reject });
      }).then((token) => {
        reqOriginal.headers.Authorization = `Bearer ${token}`;
        return HTTPClient(reqOriginal);
      });
    }

    estaRenovando = true;

    try {
      const refreshToken = localStorage.getItem("refreshToken");

      if (!refreshToken) {
        localStorage.removeItem("token");
        localStorage.removeItem("refreshToken");
        window.location.href = "/login";
        return Promise.reject(error);
      }

      const { data } = await HTTPClient.post("/auth/renovar-token", {
        refreshToken,
      });

      localStorage.setItem("token", data.accessToken);
      localStorage.setItem("refreshToken", data.refreshToken);

      processarFila(null, data.accessToken);

      reqOriginal.headers.Authorization = `Bearer ${data.accessToken}`;
      return HTTPClient(reqOriginal);
    } catch (errRenovacao) {
      processarFila(errRenovacao, null);
      localStorage.removeItem("token");
      localStorage.removeItem("refreshToken");
      window.location.href = "/login";
      return Promise.reject(errRenovacao);
    } finally {
      estaRenovando = false;
    }
  }
);