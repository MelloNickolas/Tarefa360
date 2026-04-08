import { Link } from "react-router-dom";
import { Sidebar } from "../../../components/Sidebar/Sidebar";
import { Topbar } from "../../../components/Topbar/Topbar";
import style from "./_tarefa.module.css";
import { Modal, Table, Button } from "react-bootstrap";
import { useEffect, useState } from 'react';
import { MdDelete, MdEdit, MdCheckCircle, MdRestore } from 'react-icons/md';
import TarefaApi from "../../../services/TarefaApi";
import HistoriaApi from "../../../services/HistoriaApi"; // ✅ import correto
import { BotaoAdicionar } from "../../../components/BotaoAdicionar/BotaoAdicionar";
import { FiltroSelection } from "../../../components/FiltroSelect/FiltroSelect";


export function Tarefas() {
  const [tarefas, setTarefas] = useState([]);
  const [historias, setHistorias] = useState([]); // ✅ estado das histórias
  const [mostrarModalDelete, setMostrarModalDelete] = useState(false);
  const [mostrarModalConcluir, setMostrarModalConcluir] = useState(false);
  const [mostrarModalRetomar, setMostrarModalRetomar] = useState(false);
  const [tarefaSelecionada, setTarefaSelecionada] = useState(null);
  const [status, setStatus] = useState('Todos');
  const [historiaSelecionada, setHistoriaSelecionada] = useState(''); // ✅ era sprintSelecionada
  const opcoesStatus = ['Todos', 'Concluida', 'Pendente'];

  // ✅ ordenação corrigida: pendentes primeiro, concluídas por último
  function ordenarTarefas(lista) {
    return [...lista].sort((a, b) => a.concluido - b.concluido);
  }

  async function fetchTarefas(statusAtual) {
    try {
      const response = await TarefaApi.ListarPorStatusAsync(statusAtual);
      setTarefas(ordenarTarefas(response));
    } catch (error) {
      console.error('Erro ao buscar tarefas:', error);
    }
  }

  // ✅ carrega histórias para popular o select
  async function fetchHistorias() {
    try {
      const response = await HistoriaApi.ListarHistorias();
      setHistorias(response);
    } catch (error) {
      console.error('Erro ao buscar histórias:', error);
    }
  }

  const handleFiltrarPorStatus = (e) => {
    const novoStatus = e.target.value;
    setStatus(novoStatus);
    fetchTarefas(novoStatus); // ✅ reutiliza fetchTarefas com ordenação
  }

  const handleFiltrarPorHistoria = async (e) => {
    const id = e.target.value;
    setHistoriaSelecionada(id);

    try {
      if (id === '') {
        // ✅ sem filtro: volta a listar por status atual
        fetchTarefas(status);
      } else {
        const response = await TarefaApi.ListarPorHistoriaAsync(id); // ajuste o método conforme sua API
        setTarefas(ordenarTarefas(response));
      }
    } catch (error) {
      console.error('Erro ao filtrar por história:', error);
    }
  }

  const handleClickDelete = (tarefa) => {
    setTarefaSelecionada(tarefa);
    setMostrarModalDelete(true);
  }

  const handleDeletar = async () => {
    try {
      await TarefaApi.DeletarAsync(tarefaSelecionada.id);
      setMostrarModalDelete(false);
      fetchTarefas(status);
    } catch (error) {
      console.error("Erro ao deletar Tarefa: ", error);
    }
  }

  const handleClickConcluir = (tarefa) => {
    setTarefaSelecionada(tarefa);
    setMostrarModalConcluir(true);
  }

  const handleConcluir = async () => {
    try {
      await TarefaApi.ConcluirAsync(tarefaSelecionada.id);
      setMostrarModalConcluir(false);
      fetchTarefas(status);
    } catch (error) {
      console.error("Erro ao concluir Tarefa: ", error);
    }
  }

  const handleClickRetomar = (tarefa) => {
    setTarefaSelecionada(tarefa);
    setMostrarModalRetomar(true);
  }

  const handleRetomar = async () => {
    try {
      await TarefaApi.RetomarAsync(tarefaSelecionada.id);
      setMostrarModalRetomar(false);
      fetchTarefas(status);
    } catch (error) {
      console.error("Erro ao retomar Tarefa: ", error);
    }
  }

  useEffect(() => {
    fetchTarefas(status);
    fetchHistorias(); // ✅ carrega histórias ao montar
  }, []);

  return (
    <Sidebar>
      <Topbar>
        <div className={style['pagina-conteudo']}>
          <div className={style['pagina-header']}>
            <h3>Tarefas</h3>

            <div className={style['pagina-header-navbar']}>

              {/* ✅ Select populado com histórias da API */}
              <FiltroSelection
                value={historiaSelecionada}
                onChange={handleFiltrarPorHistoria}
                nomeFiltro="História"
              >
                <option value="">Todas</option>
                {historias.map((historia) => (
                  <option key={historia.id} value={historia.id}>
                    {historia.nome}
                  </option>
                ))}
              </FiltroSelection>

              <FiltroSelection
                value={status}
                onChange={handleFiltrarPorStatus}
                nomeFiltro="Status"
              >
                {opcoesStatus.map((s) => (
                  <option key={s} value={s}>{s}</option>
                ))}
              </FiltroSelection>

              <BotaoAdicionar to={"/tarefa/criar"}>Novo</BotaoAdicionar>
            </div>
          </div>

          <div className={style.tabela}>
            <Table responsive>
              <thead className={style['tabela-header']}>
                <tr>
                  <th>Titulo</th>
                  <th>Responsável</th>
                  <th>Ações</th>
                </tr>
              </thead>
              <tbody className={style['tabela-body']}>
                {tarefas.length === 0 ? (
                  <tr>
                    <td colSpan="4" className={style['mensagem-vazia']}>
                      Nenhuma tarefa encontrada
                    </td>
                  </tr>
                ) : (
                  tarefas.map((tarefa) => (
                    <tr key={tarefa.id} className={style.tabela_dados}>
                      <td>{tarefa.titulo}</td>
                      <td>{tarefa.usuario.nome}</td>
                      <td>
                        <div className={style['acoes-container']}> {/* ✅ ponto removido */}
                          {tarefa.concluido ? (
                            <button
                              className={style['botao-retomar']}
                              onClick={() => handleClickRetomar(tarefa)}
                              title="Retomar tarefa"
                            >
                              <MdRestore />
                            </button>
                          ) : (
                            <button
                              className={style['botao-concluir']}
                              onClick={() => handleClickConcluir(tarefa)}
                              title="Concluir tarefa"
                            >
                              <MdCheckCircle />
                            </button>
                          )}

                          <Link
                            to={`/tarefa/editar/${tarefa.id}`}
                            className={style['botao-editar']}
                            state={tarefa.id}
                            title="Editar tarefa"
                          >
                            <MdEdit />
                          </Link>

                          <button
                            className={style['botao-deletar']}
                            onClick={() => handleClickDelete(tarefa)}
                            title="Deletar tarefa"
                          >
                            <MdDelete />
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))
                )}
              </tbody>
            </Table>

            {/* Modais sem alteração */}
            <Modal show={mostrarModalDelete} onHide={() => setMostrarModalDelete(false)}>
              <Modal.Header closeButton>
                <Modal.Title>Confirmar</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                Tem certeza que deseja deletar a tarefa "{tarefaSelecionada?.titulo}"?
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={() => setMostrarModalDelete(false)}>Cancelar</Button>
                <Button variant="danger" onClick={handleDeletar}>Deletar</Button>
              </Modal.Footer>
            </Modal>

            <Modal show={mostrarModalConcluir} onHide={() => setMostrarModalConcluir(false)}>
              <Modal.Header closeButton>
                <Modal.Title>Concluir</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                Tem certeza que deseja concluir a tarefa "{tarefaSelecionada?.titulo}"?
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={() => setMostrarModalConcluir(false)}>Cancelar</Button>
                <Button variant="success" onClick={handleConcluir}>Concluir</Button>
              </Modal.Footer>
            </Modal>

            <Modal show={mostrarModalRetomar} onHide={() => setMostrarModalRetomar(false)}>
              <Modal.Header closeButton>
                <Modal.Title>Retomar</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                Tem certeza que deseja retomar a tarefa "{tarefaSelecionada?.titulo}"?
              </Modal.Body>
              <Modal.Footer>
                <Button variant="secondary" onClick={() => setMostrarModalRetomar(false)}>Cancelar</Button>
                <Button variant="warning" onClick={handleRetomar}>Retomar</Button>
              </Modal.Footer>
            </Modal>

          </div>
        </div>
      </Topbar>
    </Sidebar>
  );
}