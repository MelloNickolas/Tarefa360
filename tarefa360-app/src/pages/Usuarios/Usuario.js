import { Link } from "react-router-dom"
import { Sidebar } from "../../Components/Sidebar/Sidebar"
import { Topbar } from "../../Components/Topbar/Topbar"
import style from "./Usuario.module.css"
import {
  Modal, Table, Button
} from "react-bootstrap"
import { MdDelete, MdEdit } from "react-icons/md"
import { useEffect, useState } from "react"
import UsuarioAPI from "../../services/usuarioAPI"

export function Usuario() {
  const [usuarios, setUsuarios] = useState([]); //estado para busca na api

  const [mostrarModal, setMostrarModal] = useState(false); // modal para deletar
  const [usuarioSelecionado, setUsuarioSelecionado] = useState(null); // guardar o Id para abrir o modal

  const handleClickDeletar = (usuario) => {
    setUsuarioSelecionado(usuario);
    setMostrarModal(true);
  };

  const handleDeletar = async () => {
    try {
      await UsuarioAPI.deletarAsync(usuarioSelecionado.idUsuario)
      setUsuarios(usuarios.filter(u => u.idUsuario !== usuarioSelecionado.idUsuario))
      //colocando um filtro ele vai mais rápido, ele nao renderiza a pagina inteira
    } catch (error) {
      console.error("Erro ao deletar Usuário : ", error)
    } finally {
      handleFecharModal()
    }
  }

  const handleFecharModal = () => {
    setMostrarModal(false)
    setUsuarioSelecionado(null)
    //resetando o estado do nosso modal
  }

  async function carregarUsuarios() {
    try {
      const listarUsuarios = await UsuarioAPI.listarAsync(true);
      setUsuarios(listarUsuarios);
      console.log(listarUsuarios)
    }
    catch (error) {
      console.error("Erro ao carregar Usuários: ", error)
    }
  }

  //toda vez que a pagina for renderizada chama a função carregar usuários
  useEffect(() => {
    carregarUsuarios();
  }, []);


  return <>
    <Sidebar>
      <Topbar>
        <div className={style.pagina_conteudo}>
          <div className={style.pagina_cabecalho}>
            <h3>Usuarios</h3>
            <Link to="/usuario/novo" className={style.botao_novo}>+ Novo</Link>
          </div>


          <div className={style.tabela}>
            <Table responsive>
              <thead className={style.tabela_cabecalho}>
                <tr>
                  <th>Nome: </th>
                  <th>Email: </th>
                  <th>Ações: </th>
                </tr>
              </thead>

              <tbody className={style.tabela_corpo}>
                {usuarios.map((usuario) => (
                  <tr key={usuario.idUsuario}>
                    <td>{usuario.nome}</td>
                    <td>{usuario.email}</td>
                    <td>
                      <Link to="/usuario/editar" state={usuario.idUsuario} className={style.botao_editar}>
                        <MdEdit></MdEdit>
                      </Link>
                      <button onClick={() => handleClickDeletar(usuario)} className={style.botao_deletar}>
                        <MdDelete></MdDelete>
                      </button>
                    </td>
                  </tr>
                ))}

              </tbody>
            </Table>
          </div>

          <Modal show={mostrarModal} onHide={handleFecharModal}>
            <Modal.Header>
              <Modal.Title>Confirmar</Modal.Title> 
            </Modal.Header>
            <Modal.Body>
              Tem certeza que deseja deletar o usuário {usuarioSelecionado?.nome}?
            </Modal.Body>
            <Modal.Footer>
              <Button variant="secondary" onClick={handleFecharModal}>
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
  </>
}