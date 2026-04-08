import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import UsuarioApi from '../../../services/UsuarioApi';
import style from './_editarUsuario.module.css';
import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export function EditarUsuario() {
    const navigate = useNavigate();
    const location = useLocation();

    const [id] = useState(location.state);

    const [tiposUsuarios, setTiposUsuarios] = useState([]);
    const [usuario, setUsuario] = useState({
    nome: '',
    email: '',
    tipoUsuario: ''
    });

  // toda vez que o componente for renderizado, ele irá buscar os tipos de usuários disponíveis para preencher o dropdown
  useEffect(() => {
    const fetchTiposUsuarios = async () => {
      try {
        // Lógica para buscar os tipos de usuários disponíveis
        const tipos = await UsuarioApi.ListarTiposUsuarioAsync(); 
        setTiposUsuarios(tipos);
      } catch (error) {
        console.error('Erro ao buscar tipos de usuários:', error);
      }
    };

      const buscarDadosUsuario = async () => {
        try {
          const usuarioData = await UsuarioApi.ObterPorIdAsync(id);
          setUsuario({
            nome: usuarioData.nome,
            email: usuarioData.email,
            tipoUsuario: usuarioData.tipoUsuarioId
          });
        } catch (error) {
          console.error('Erro ao buscar usuário:', error);
        }
    };

    fetchTiposUsuarios();
    buscarDadosUsuario();
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUsuario(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Lógica para enviar os dados do novo usuário
    if(isFormValid()) {
      await UsuarioApi.AtualizarAsync(id, usuario.nome, usuario.email, usuario.tipoUsuario);
      navigate('/Usuarios');
    } else {
        console.log(usuario.nome, usuario.email, usuario.tipoUsuario);
        alert('Por favor, preencha todos os campos obrigatórios.');
    }
};

  const isFormValid = () => {
    return usuario.nome && usuario.email && usuario.tipoUsuario;
  }

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
        <div className={style['pagina-conteudo']}>
            <h3>Editar Usuário</h3>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3" controlId="formNome">
                <Form.Label>Nome</Form.Label>
                <Form.Control 
                  type="text" 
                  placeholder="Digite o nome" 
                  name="nome" 
                  value={usuario.nome} 
                  onChange={handleChange} 
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3" controlId="formEmail">
                <Form.Label>Email</Form.Label>
                <Form.Control
                  type="email"
                  placeholder="Digite o email"
                  name="email"
                  value={usuario.email}
                  onChange={handleChange}
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3" controlId="formTipoUsuario">
                <Form.Label>Tipo de Usuário</Form.Label>
                <Form.Control
                  as="select"
                  name="tipoUsuario"
                  value={usuario.tipoUsuario}
                  onChange={handleChange}
                  required
                >
                  {tiposUsuarios.map((tipo) => (
                    <option value={tipo.id}>
                      {tipo.nome}
                    </option>
                  ))}
                </Form.Control>
              </Form.Group>
              <Button variant="primary" type="submit" disabled={!isFormValid()} className={style['btn-submit']}>
                Salvar
              </Button>
            </Form>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}