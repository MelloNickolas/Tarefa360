import style from "./EditarUsuario.module.css";
import { Sidebar } from "../../Components/Sidebar/Sidebar";
import { Topbar } from "../../Components/Topbar/Topbar";
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import UsuarioAPI from "../../services/usuarioAPI";
import { Form } from "react-bootstrap";
import { Button } from "react-bootstrap";

export function EditarUsuario() {
  const location = useLocation();
  //pegar infos que vao ser passados na rota, normalmente quando aperta um botao voce passa um state
  // ou seja, em usuario nos estamos passando o ID com state
  const [idUsuario] = useState(location.state);

  const navigate = useNavigate();

  const [nome, setNome] = useState('');
  const [email, setEmail] = useState('');
  const [tipoUsuario, setTipoUsuario] = useState('')
  const [TiposUsuarios, setTiposUsuarios] = useState([]);

  const isFormValid = () => {
    return nome && email && tipoUsuario; //verificar se os states estao vazios
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (isFormValid()) {
      await UsuarioAPI.atualizarAsync(idUsuario, nome, email, tipoUsuario);
      navigate('/usuarios')
    } else {
      alert("Por favor, preencha todos os campos.")
    }
  }

  useEffect(() => {
    const buscarTiposUsuarios = async () => {
      try {
        const tipos = await UsuarioAPI.listarTiposUsuarioAsync();
        setTiposUsuarios(tipos);
      } catch (error) {
        console.error('Erro ao buscar tipos de usuários', error)
      }
    };

    const buscarDadosUsuario = async () => {
      try {
        const usuario = await UsuarioAPI.obterAsync(idUsuario);
        setTipoUsuario(usuario.tipoUsuario)
        setNome(usuario.nome)
        setEmail(usuario.email)
      } catch (error) {
        console.error('Erro ao buscar dados do usuários', error)
      }
    };

    buscarDadosUsuario();
    buscarTiposUsuarios();
  }, []);

  return <>
    <Sidebar>
      <Topbar>
        <div className={style.pagina_conteudo}>
          <h3>Editar Usuário</h3>

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


            {/* Nosso dropbox para tipoId*/}
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
                  <option key={tipo.id} value={tipo.id}>{tipo.nome}</option>
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