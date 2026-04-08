import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import UsuarioApi from '../../../services/UsuarioApi';
import style from './_novoUsuario.module.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export function NovoUsuario() {
  const [usuario, setUsuario] = useState({
    nome: '',
    email: '',
    senha: '',
    tipoUsuario: ''
  });

  const [tiposUsuarios, setTiposUsuarios] = useState([]);

  const navigate = useNavigate();

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

    fetchTiposUsuarios();
  }, []);

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
      await UsuarioApi.CriarAsync(usuario.nome, usuario.email, usuario.senha, usuario.tipoUsuario);
      navigate('/Usuarios');
    } else {
        alert('Por favor, preencha todos os campos obrigatórios.');
    }
};

  const isFormValid = () => {
    return usuario.nome && usuario.email && usuario.senha && usuario.tipoUsuario;
  }

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
        <div className={style['pagina-conteudo']}>
            <h3>Novo Usuário</h3>
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
              <Form.Group className="mb-3" controlId="formSenha">
                <Form.Label>Senha</Form.Label>
                <Form.Control
                  type="password"
                  placeholder="Digite a senha"
                  name="senha"
                  value={usuario.senha}
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
                  <option value="">Selecione...</option>
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