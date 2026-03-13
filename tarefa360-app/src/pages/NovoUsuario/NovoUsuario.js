import { useEffect, useState } from "react";
import { Sidebar } from "../../Components/Sidebar/Sidebar";
import { Topbar } from "../../Components/Topbar/Topbar";
import style from "./NovoUsuario.module.css";
import { useNavigate } from "react-router-dom";
import UsuarioAPI from "../../services/usuarioAPI";
import Form from "react-bootstrap/Form";
import { Button } from "react-bootstrap";

export function NovoUsuario() {
  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');
  const [tipoUsuario, setTipoUsuario] = useState('') // vai guardar o id como número

  const [TiposUsuarios, setTiposUsuarios] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchTiposUsuarios = async () => {
      try {
        const tipos = await UsuarioAPI.listarTiposUsuarioAsync();
        setTiposUsuarios(tipos);
      } catch (error) {
        console.error('Erro ao buscar tipos de usuários', error)
      }
    }
    fetchTiposUsuarios();
  }, []);

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (isFormValid()) {
      await UsuarioAPI.criarAsync(nome, email, senha, Number(tipoUsuario)); // ✅ converte para número aqui
      navigate('/usuarios')
    } else {
      alert('Por favor, preencha todos os campos!')
    }
  };

  const isFormValid = () => {
    return nome && email && senha && tipoUsuario;
  };

  return <>
    <Sidebar>
      <Topbar>
        <div className={style.pagina_conteudo}>
          <h3>Novo Usuario</h3>

          <Form onSubmit={handleSubmit}>
            <Form.Group controlId="formNome" className="mb-3">
              <Form.Label>Nome</Form.Label>
              <Form.Control
                type="text"
                placeholder="Digite seu nome"
                name="nome"
                value={nome}
                onChange={(event) => setNome(event.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formEmail" className="mb-3">
              <Form.Label>Email</Form.Label>
              <Form.Control
                type="email"
                placeholder="Digite seu Email"
                name="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formSenha" className="mb-3">
              <Form.Label>Senha</Form.Label>
              <Form.Control
                type="password"
                placeholder="Digite sua senha"
                name="senha"
                value={senha}
                onChange={(event) => setSenha(event.target.value)}
                required
              />
            </Form.Group>

            <Form.Group controlId="formTipoUsuario" className="mb-3">
              <Form.Label>Tipo de Usuário</Form.Label>
              <Form.Control
                as="select"
                name="tipoUsuario"
                value={tipoUsuario}
                onChange={(event) => setTipoUsuario(event.target.value)}
                required
              >
                <option value="">Selecione o Tipo de Usuário</option>
                {TiposUsuarios.map((tipo) => (
                  <option key={tipo.id} value={tipo.id}>{tipo.nome}</option> // ✅ key adicionado
                ))}
              </Form.Control>
            </Form.Group>

            <Button variant="primary" type="submit" disabled={!isFormValid()}>
              Salvar
            </Button>
          </Form>

        </div>
      </Topbar>
    </Sidebar>
  </>
}