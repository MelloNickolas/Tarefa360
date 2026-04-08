import { Link } from 'react-router-dom';
import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import style from './_sprint.module.css';
import { Table } from 'react-bootstrap';
import { MdDelete, MdEdit } from 'react-icons/md';
import SprintApi from '../../../services/SprintApi';
import ProjetoApi from '../../../services/ProjetoApi';
import { FiltroNome } from '../../../components/FiltroNome/FiltroNome';
import { FiltroSelection } from '../../../components/FiltroSelect/FiltroSelect';
import { useEffect, useState } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { BotaoAdicionar } from '../../../components/BotaoAdicionar/BotaoAdicionar';

export function Sprint() {
  const [sprint, setSprint] = useState([]);
  const [mostrarModal, setMostrarModal] = useState(false);
  const [sprintSelecionado, setSprintSelecionado] = useState(null);
  const [projetos, setProjetos] = useState([]);
  const [projetoFiltro, setProjetoFiltro] = useState('');
  const [nomeFiltro, setNomeFiltro] = useState('');


  // FiltroNome já extrai o valor do evento — recebe string diretamente
  const handleBuscarPorNome = (valor) => setNomeFiltro(valor);

  async function fetchSprint() {
    try {
      const response = await SprintApi.ListarSprintAsync();
      setSprint(response);

    } catch (error) {
      console.error('Erro ao buscar sprint:', error);
    }
  }

  async function fetchProjetos() {
    try {
      const response = await ProjetoApi.ListarProjetoAsync(true);
      setProjetos(response);
    } catch (error) {
      console.error('Erro ao buscar projetos:', error);
    }
  }

  const handleClickDelete = (sprint) => {
    setSprintSelecionado(sprint);
    setMostrarModal(true);
  };

  const handleDeletar = async () => {
    try {
      await SprintApi.DeletarSprintAsync(sprintSelecionado.id);
      setSprint(sprint.filter((u) => u.id !== sprintSelecionado.id)); setMostrarModal(false);
      fetchSprint();
    }
    catch (error) {
      console.error('Erro ao deletar sprint:', error);
    }
  };

  function getNomeProjeto(projetoId) {
    const projeto = projetos.find(p => p.id === projetoId);
    return projeto ? projeto.nome : '—';
  }


  const sprintsFiltradas = sprint.filter((s) => {
    const filtroProjeto = projetoFiltro
      ? s.projetoId === Number(projetoFiltro)
      : true;

    const filtroNome = s.titulo
      ?.toLowerCase()
      .includes(nomeFiltro.toLowerCase());

    return filtroProjeto && filtroNome;
  });

  useEffect(() => {
    fetchSprint();
    fetchProjetos();
  }, []);

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <div className={style['pagina-header']}>
              <h3>Sprint</h3>

              <div className={style['pagina-header-navbar']}>
                <FiltroNome
                  onChange={handleBuscarPorNome}
                  placeholder="Buscar por nome..."
                />

                <FiltroSelection
                  nomeFiltro="Projeto"
                  value={projetoFiltro}
                  onChange={(e) => setProjetoFiltro(e.target.value)}
                >
                  <option value="">Todos os projetos</option>
                  {projetos.map((p) => (
                    <option key={p.id} value={p.id}>
                      {p.nome}
                    </option>
                  ))}
                </FiltroSelection>
                <BotaoAdicionar to={"/sprint/criar"}>Nova</BotaoAdicionar>
              </div>
            </div>
            <div className={style.tabela}>
              <Table responsive>
                {/* thead - colunas da tabela */}
                <thead className={style['tabela-header']}>
                  <tr>
                    <th>Titulo</th>
                    <th>Projeto</th>
                    <th>Ações</th>
                  </tr>
                </thead>
                {/* tbody - linhas da tabela */}
                <tbody className={style['tabela-body']}>
                  {sprint.length === 0 ? (
                    <tr>
                      <td colSpan="4" className={style['mensagem-vazia']}>Nenhum sprint encontrado.</td>
                    </tr>
                  ) : (
                    sprintsFiltradas.map((item) => (
                      <tr key={item.id}>
                        <td>{item.titulo}</td>
                        <td>{getNomeProjeto(item.projetoId)}</td>
                        <td>
                          <Link
                            to={`/sprint/editar/${item.id}`}
                            state={item.id}
                          >
                            <MdEdit />
                          </Link>

                          <button
                            className={style['botao-deletar']}
                            onClick={() => handleClickDelete(item)}
                          >
                            <MdDelete />
                          </button>
                        </td>
                      </tr>
                    ))
                  )}
                </tbody>
              </Table>

              <Modal show={mostrarModal} onHide={() => setMostrarModal(false)}>
                <Modal.Header closeButton>
                  <Modal.Title>Confirmar</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                  Tem certeza que deseja deletar o sprint "{sprintSelecionado?.titulo}"?
                </Modal.Body>
                <Modal.Footer>
                  <Button variant="secondary" onClick={() => setMostrarModal(false)}>
                    Cancelar
                  </Button>
                  <Button variant="danger" onClick={handleDeletar}>
                    Deletar
                  </Button>
                </Modal.Footer>
              </Modal>

            </div>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}

