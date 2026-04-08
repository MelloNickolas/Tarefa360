import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import style from './_editarEquipe.module.css';
import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { MdDelete, MdEdit } from 'react-icons/md';
import { Table, Modal } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import EquipeApi from '../../../services/EquipeApi';
import UsuarioApi from '../../../services/UsuarioApi';

export function EditarEquipe() {
  const navigate = useNavigate();
  const location = useLocation();
  const [id] = useState(location.state);

  const [equipe, setEquipe] = useState({ nome: '' });
  const [busca, setBusca] = useState('');
  const [sugestoes, setSugestoes] = useState([]);
  const [usuarioSelecionado, setUsuarioSelecionado] = useState(null);
  const [papelSelecionado, setPapelSelecionado] = useState('');
  const [papeisEquipe, setPapeisEquipe] = useState([]);
  const [membros, setMembros] = useState([]);
  const [mostrarModalEditar, setMostrarModalEditar] = useState(false);
  const [membroSelecionado, setMembroSelecionado] = useState(null);
  const [novoPapel, setNovoPapel] = useState('');

  async function fetchEquipe() {
    try {
      const response = await EquipeApi.ObterEquipePorIdAsync(id);
      setEquipe({ nome: response.nome });
    } catch (error) {
      console.error('Erro ao buscar equipe:', error);
    }
  }

  async function fetchMembros() {
    try {
      const response = await EquipeApi.ListarUsuarioEquipePorEquipeAsync(id);
      setMembros(response);
    } catch (error) {
      console.error('Erro ao buscar membros:', error);
    }
  }

  async function fetchPapeisEquipe() {
    try {
      const response = await EquipeApi.ListarPapeisEquipeAsync();
      setPapeisEquipe(response);
      if (response.length > 0) setPapelSelecionado(response[0].id);
    } catch (error) {
      console.error('Erro ao buscar papéis:', error);
    }
  }

  useEffect(() => {
    fetchEquipe();
    fetchMembros();
    fetchPapeisEquipe();
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEquipe(prevState => ({ ...prevState, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (isFormValid()) {
      await EquipeApi.AtualizarEquipeAsync(id, equipe.nome);
      navigate('/equipes');
    } else {
      alert('Por favor, preencha todos os campos obrigatórios.');
    }
  };

  const isFormValid = () => equipe.nome;

  console.log(papelSelecionado, typeof papelSelecionado);

  // autocomplete
  const handleBusca = async (valor) => {
    setBusca(valor);
    setUsuarioSelecionado(null);

    if (valor.length < 2) {
      setSugestoes([]);
      return;
    }

    try {
      const todos = await UsuarioApi.ListarAsync(true);
      const filtrados = todos
        .filter(u => u.nome.toLowerCase().includes(valor.toLowerCase()))
        .slice(0, 5);
      setSugestoes(filtrados);
    } catch (error) {
      console.error('Erro ao buscar usuários:', error);
    }
  };

  const handleSelecionarUsuario = (usuario) => {
    setBusca(usuario.nome);
    setUsuarioSelecionado(usuario);
    setSugestoes([]);
  };

  const getBordaStyle = () => {
    if (!busca) return {};
    if (usuarioSelecionado) return { borderColor: 'green', borderWidth: 2, borderStyle: 'solid' };
    return { borderColor: 'red', borderWidth: 2, borderStyle: 'solid' };
  };

  const handleAdicionarMembro = async () => {
    if (!usuarioSelecionado) {
      alert('Selecione um usuário da lista!');
      return;
    }
    try {
      await EquipeApi.CriarUsuarioEquipeAsync(papelSelecionado, usuarioSelecionado.id, id);
      setBusca('');
      setUsuarioSelecionado(null);
      setSugestoes([]);
      fetchMembros();
    } catch (error) {
      alert('Erro ao adicionar membro.');
    }
  };

  const handleRemoverMembro = async (usuarioEquipeId) => {
    try {
      await EquipeApi.DeletarUsuarioEquipeAsync(usuarioEquipeId);
      fetchMembros();
    } catch (error) {
      console.error('Erro ao remover membro:', error);
    }
  };

  const handleClickEditar = (membro) => {
    setMembroSelecionado(membro);
    setNovoPapel(membro.papeisEquipe);
    setMostrarModalEditar(true);
  };

  const handleAtualizarPapel = async () => {
    try {
      await EquipeApi.AtualizarPapelAsync(membroSelecionado.id, novoPapel);
      setMostrarModalEditar(false);
      fetchMembros();
    } catch (error) {
      console.error('Erro ao atualizar papel:', error);
    }
  };


  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <h3>Editar Equipe</h3>
            <Form onSubmit={handleSubmit}>

              <Form.Group className="mb-3" controlId="formNome">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o nome da equipe"
                  name="nome"
                  value={equipe.nome}
                  onChange={handleChange}
                  required
                  minLength={3}
                  maxLength={100}
                />
              </Form.Group>

              <h4>Membros</h4>


              <div className={style['adicionar-membro']}>
                <div className={style['adicionar-membro-item']}>
                  <input
                    type="text"
                    placeholder="Buscar usuário pelo nome..."
                    value={busca}
                    onChange={(e) => handleBusca(e.target.value)}
                    style={getBordaStyle()}
                    className={style['input-busca']}
                  />
                </div>



                <div className={style['adicionar-membro-item']}>
                  <Form.Select
                    value={papelSelecionado}
                    onChange={(e) => setPapelSelecionado(Number(e.target.value))}
                  >
                    {papeisEquipe.map((papel) => (
                      <option key={papel.id} value={papel.id}>
                        {papel.nome}
                      </option>
                    ))}
                  </Form.Select>

                  <button type="button" variant="warning" className={style['botao-adicionar']} onClick={handleAdicionarMembro}>+ Membro</button>

                </div>

              </div>

              <div>
                {sugestoes.length > 0 && (
                  <ul className={style['sugestoes']}>
                    {sugestoes.map((usuario) => (
                      <li key={usuario.id} onClick={() => handleSelecionarUsuario(usuario)} className={style['sugestao']}>
                        <span>{usuario.nome}</span>
                        <span className={style['sugestao-email']}>- {usuario.email}</span>
                      </li>
                    ))}
                  </ul>
                )}

              </div>
              <div className={style.tabela}>
                <Table responsive>
                  <thead>
                    <tr>
                      <th>Nome</th>
                      <th>Papel</th>
                      <th>Ações</th>
                    </tr>
                  </thead>
                  <tbody className={style['tabela-body']}>
                    {membros.length === 0 ? (
                      <tr>
                        <td colSpan="3">Nenhum membro encontrado.</td>
                      </tr>
                    ) : (
                      membros.map((membro) => (
                        <tr key={membro.id}>
                          <td>{membro.nomeUsuario}</td>
                          <td>
                            {papeisEquipe.find(p => p.id === membro.papeisEquipe)?.nome}
                          </td>
                          <td>
                            <button
                              type='button'
                              className={style['botao-editar']}
                              onClick={() => handleClickEditar(membro)}
                            >
                              <MdEdit />
                            </button>
                            <button
                              type='button'
                              className={style['botao-deletar']}
                              onClick={() => handleRemoverMembro(membro.id)}
                            >
                              <MdDelete />
                            </button>
                            
                          </td>
                        </tr>
                      ))
                    )}
                  </tbody>
                </Table>
              </div>

              <div className={style['botoes-footer']}>
                <Button variant="primary" type='submit'>
                  Salvar
                </Button>
              </div>

            </Form>

            <Modal show={mostrarModalEditar} onHide={() => setMostrarModalEditar(false)}>
              <Modal.Header closeButton>
                <Modal.Title>Editar Papel</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                <Form.Select
                  value={novoPapel}
                  onChange={(e) => setNovoPapel(Number(e.target.value))}
                >
                  {papeisEquipe.map((papel) => (
                    <option key={papel.id} value={papel.id}>
                      {papel.nome}
                    </option>
                  ))}
                </Form.Select>
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={() => setMostrarModalEditar(false)}>
                  Cancelar
                </Button>
                <Button variant="primary" onClick={handleAtualizarPapel}>
                  Salvar
                </Button>
              </Modal.Footer>
            </Modal>

          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
  
}