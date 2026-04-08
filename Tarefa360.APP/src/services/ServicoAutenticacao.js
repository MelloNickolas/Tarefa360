import { HTTPClient } from "./client";

// PASSO 1 DO LOGIN
// Envia email + senha + token do reCAPTCHA para a API.
// A API valida tudo e dispara o e-mail com o código de 6 dígitos.
export async function iniciarLogin(email, senha, captchaToken) {
  const response = await HTTPClient.post("/auth/iniciar-login", {
    email,
    senha,
    captchaToken,
  });
  return response.data;
}

// PASSO 2 DO LOGIN
// Envia o email + código digitado pelo usuário no modal.
// A API valida o código e retorna accessToken + refreshToken.
export async function confirmarCodigo(email, codigo) {
  const response = await HTTPClient.post("/auth/confirmar-codigo", {
    email,
    codigo,
  });

  const { accessToken, refreshToken } = response.data;

  localStorage.setItem("token", accessToken);
  localStorage.setItem("refreshToken", refreshToken);

  return response.data;
}

// LOGOUT
// Remove os tokens locais e avisa a API para revogar o refreshToken.
export async function logout() {
  const refreshToken = localStorage.getItem("refreshToken");

  localStorage.removeItem("token");
  localStorage.removeItem("refreshToken");

  if (refreshToken) {
    try {
      await HTTPClient.post("/auth/logout", { refreshToken });
    } catch {
      // Silencia o erro — o usuário já está deslogado localmente
    }
  }
}

export function getToken() {
  return localStorage.getItem("token");
}

export function getRefreshToken() {
  return localStorage.getItem("refreshToken");
}