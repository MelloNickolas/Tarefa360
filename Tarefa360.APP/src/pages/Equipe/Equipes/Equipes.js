import { Link } from "react-router-dom";
import { Sidebar } from "../../../components/Sidebar/Sidebar";
import { Topbar } from "../../../components/Topbar/Topbar";
import style from "./_equipe.module.css";
import { Modal, Table, Button } from "react-bootstrap";
import { useEffect, useState } from 'react';
import { MdDelete, MdEdit, MdCheckCircle, MdRestore, MdAddCircleOutline } from 'react-icons/md';
import EquipeApi from "../../../services/EquipeApi";
import { BotaoAdicionar } from "../../../components/BotaoAdicionar/BotaoAdicionar";


export function Equipes() {
  const [equipes, setEquipes] = useState([])
  const [mostrarModal, setMostrarModal] = useState(false);
  const [equipeSelecionada, setEquipeSelecionada] = useState(null);


  async function fetchEquipes() {
    try {
      const response = await EquipeApi.ListarEquipesComMembrosAsync();
      setEquipes(response);
    } catch (error) {
      console.error('Erro ao buscar equipes:', error);
    }
  }

  const handleClickDelete = (equipe) => {
    setEquipeSelecionada(equipe);
    setMostrarModal(true);
  };

  const handleDeletar = async () => {
    try {
      await EquipeApi.DeletarEquipeAsync(equipeSelecionada.id);
      setMostrarModal(false);
      fetchEquipes();
    } catch (error) {
      console.error('Erro ao deletar equipe:', error);
    }
  };

  console.log(equipes);
  useEffect(() => {
    fetchEquipes();
  }, []);

  return <>
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <div className={style['pagina-header']}>
              <h3>Equipes</h3>
              <BotaoAdicionar to={"/equipe/criar"}>Nova</BotaoAdicionar>
            </div>

            <div className={style['equipes-grid']}>
              {equipes.length === 0 ? (
                <p>Nenhuma equipe foi encontrada.</p>
              ) : (
                equipes.map((equipe) => (
                  <div key={equipe.id} className={style['equipe-card']}>
                    <div className={style['equipe-card-topo']}>
                      <h4>{equipe.nome}</h4>
                      <div className={style['equipe-card-acoes']}>
                        <Link
                          to={`/equipe/editar/${equipe.id}`}
                          className={style['botao-editar']}
                          state={equipe.id}
                        >
                          <MdEdit />
                        </Link>
                        <button
                          className={style['botao-deletar']}
                          onClick={() => handleClickDelete(equipe)}
                        >
                          <MdDelete />
                        </button>
                      </div>
                    </div>

                    <div className={style['equipe-card-membros']}>
                      {equipe.membros.length === 0 ? (
                        <span>Sem membros</span>
                      ) : (
                        equipe.membros.map((membro, index) => (
                          <span key={index}>
                            {membro.nomeUsuario}
                          </span>
                        ))
                      )}
                    </div>
                  </div>
                ))
              )}

            </div>
            <Modal show={mostrarModal} onHide={() => setMostrarModal(false)}>
              <Modal.Header closeButton>
                <Modal.Title>Confirmar</Modal.Title>
              </Modal.Header>
              <Modal.Body>
                Tem certeza que deseja deletar a equipe "{equipeSelecionada?.nome}"?
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
        </Topbar>
      </Sidebar>
    </div>
  </>
}