import { Link } from 'react-router-dom';
import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import style from './_projetos.module.css';
import { Table } from 'react-bootstrap';
import { MdDelete, MdEdit } from 'react-icons/md';
import ProjetoApi from '../../../services/ProjetoApi';
import { useEffect, useState } from 'react';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { BotaoAdicionar } from '../../../components/BotaoAdicionar/BotaoAdicionar';

export function Projetos() {
  const [projetos, setProjetos] = useState([]);
  const [mostrarModal, setMostrarModal] = useState(false);
  const [projetoSelecionado, setProjetoSelecionado] = useState(null);

  async function fetchProjetos() {
    try {
      const response = await ProjetoApi.ListarProjetoAsync(true);
      setProjetos(response);

    } catch (error) {
      console.error('Erro ao buscar projeto:', error);
    }
  }

  const handleClickDelete = (projeto) => {
    setProjetoSelecionado(projeto);
    setMostrarModal(true);
  };

  const handleDeletar = async () => {
    try {
      await ProjetoApi.DeletarProjetoAsync(projetoSelecionado.id);
      setProjetoSelecionado(projetos.filter((u) => u.id !== projetoSelecionado.id));
      setMostrarModal(false);
      fetchProjetos();
    }
    catch (error) {
      console.error('Erro ao deletar projeto:', error);
    }
  };

  useEffect(() => {
    fetchProjetos();
  }, []);

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <div className={style['pagina-header']}>
              <h3>Projetos</h3>
              <BotaoAdicionar to={"/projeto/criar"}>Novo</BotaoAdicionar>

            </div>
            <div className={style.tabela}>
              <Table responsive>
                {/* thead - colunas da tabela */}
                <thead className={style['tabela-header']}>
                  <tr>
                    <th>Nome</th>
                    <th>Ações</th>
                  </tr>
                </thead>
                {/* tbody - linhas da tabela */}
                <tbody className={style['tabela-body']}>
                  {projetos.length === 0 ? (
                    <tr>
                      <td colSpan="4" className={style['mensagem-vazia']}>Nenhum projeto encontrado.</td>
                    </tr>
                  ) : (
                    projetos.map((projeto) => (
                      <tr key={projeto.id}>
                        <td>{projeto.nome}</td>
                        <td>
                          <Link
                            to={`/projeto/editar/${projeto.id}`}
                            className={style['botao-editar']}
                            state={projeto.id}
                          >
                            <MdEdit />
                          </Link>

                          <button
                            className={style['botao-deletar']}
                            onClick={() => handleClickDelete(projeto)}
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
                  Tem certeza que deseja deletar o projeto "{projetoSelecionado?.nome}"?
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

