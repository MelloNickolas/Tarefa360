import { Link } from "react-router-dom";
import { Sidebar } from "../../../components/Sidebar/Sidebar";
import { Topbar } from "../../../components/Topbar/Topbar";
import { BotaoAdicionar } from "../../../components/BotaoAdicionar/BotaoAdicionar";
import style from "./_historia.module.css";
import { Modal, Table, Button } from "react-bootstrap";
import { useEffect, useState } from 'react';
import { MdDelete, MdEdit } from 'react-icons/md';
import HistoriaAPI from "../../../services/HistoriaApi";
import ProjetoApi from "../../../services/ProjetoApi";
import { FiltroSelection } from "../../../components/FiltroSelect/FiltroSelect";
import { FiltroNome } from "../../../components/FiltroNome/FiltroNome";

export function Historias() {
  const [historias, setHistorias] = useState([]);
  const [mostrarModal, setMostrarModal] = useState(false);
  const [historiaSelecionada, setHistoriaSelecionada] = useState(null);

  const [projetos, setProjetos] = useState([]);
  const [projetoSelecionado, setProjetoSelecionado] = useState('');

  async function fetchHistorias() {
    try {
      const response = await HistoriaAPI.ListarHistorias();
      setHistorias(response);

    } catch (error) {
      console.error('Erro ao buscar histórias:', error);
    }
  }

  async function fetchProjetos() {
    try {
      const response = await ProjetoApi.ListarProjetoDropdown(true);
      setProjetos(response);
    } catch (error) {
      console.error('Erro ao buscar projetos:', error);
    }
  }

  const handleFiltrarPorProjeto = async (e) => {
    const id = e.target.value;
    setProjetoSelecionado(id);

    if (id === '') {
      fetchHistorias(); // se selecionou "Todos", busca todas
    } else {
      const response = await HistoriaAPI.ListarHistoriasPorProjeto(id);
      setHistorias(response);
    }
  }

  const handleBuscarPorNome = async (nome) => {
    if (nome === '') {
      fetchHistorias();
    } else {
      const response = await HistoriaAPI.ObterHistoriaPorNome(nome);
      setHistorias(response);
    }
  }

  const handleClickDelete = (historia) => {
    setHistoriaSelecionada(historia);
    setMostrarModal(true);
  }

  const handleDeletar = async () => {
    try {
      await HistoriaAPI.DeletarHistoria(historiaSelecionada.id);
      setMostrarModal(false);
      fetchHistorias();
    } catch (error) {
      console.error("Erro ao deletar História: ", error)
    }
  }


  useEffect(() => {
    fetchHistorias();
    fetchProjetos();
  }, []);

  return <>
    <Sidebar>
      <Topbar>
        <div className={style['pagina-conteudo']}>
          <div className={style['pagina-header']}>
            <h3>Histórias</h3>

            <div className={style['pagina-header-navbar']}>
              <FiltroNome
                onChange={handleBuscarPorNome}
                placeholder="Buscar por nome..."
              ></FiltroNome>

              <FiltroSelection
                value={projetoSelecionado}
                onChange={handleFiltrarPorProjeto}
                nomeFiltro="Projetos"
              >
                <option value="" >Todos</option>
                {projetos.map((projeto) => (
                  <option key={projeto.id} value={projeto.id}>
                    {projeto.nome}
                  </option>
                ))}
              </FiltroSelection>

              <BotaoAdicionar to="/historia/criar">Novo</BotaoAdicionar>
            </div>
          </div>


          <div className={style.tabela}>
            <Table responsive>
              {/* thead - colunas da tabela */}
              <thead className={style['tabela-header']}>
                <tr>
                  <th>Nome</th>
                  <th>Descrição</th>
                  <th>Projeto</th>
                  <th>Ações</th>
                </tr>
              </thead>
              {/* tbody - linhas da tabela */}
              <tbody className={style['tabela-body']}>
                {historias.length === 0 ? (
                  <tr>
                    <td colSpan="4" className={style['mensagem-vazia']}>
                      Nenhuma história encontrada.
                    </td>
                  </tr>
                ) : (
                  historias.map((historia) => (
                    <tr key={historia.id} className={style.tabela_dados}>
                      <td>{historia.nome}</td>
                      <td>{historia.descricao}</td>
                      <td>{historia.nomeProjeto}</td>
                      <td>

                        <Link
                          to={`/historia/editar/${historia.id}`}
                          className={style['botao-editar']}
                          state={historia.id}
                        >
                          <MdEdit />
                        </Link>

                        <button
                          className={style['botao-deletar']}
                          onClick={() => handleClickDelete(historia)}
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
                Tem certeza que deseja deletar a história "{historiaSelecionada?.nome}"?
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
  </>
}