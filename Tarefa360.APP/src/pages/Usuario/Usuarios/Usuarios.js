import { Link } from 'react-router-dom';
import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import style from './_usuarios.module.css';
import { Table } from 'react-bootstrap';
import { MdDelete, MdEdit } from 'react-icons/md';
import UsuarioApi from '../../../services/UsuarioApi';
import { useEffect, useState } from 'react';
import  Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import { BotaoAdicionar } from '../../../components/BotaoAdicionar/BotaoAdicionar';

export function Usuarios() {
const [usuarios, setUsuarios] = useState([]);
const [mostrarModal, setMostrarModal] = useState(false);
const [usuarioSelecionado, setUsuarioSelecionado] = useState(null);

async function fetchUsuarios() {
  try{
    const response = await UsuarioApi.ListarAsync(true);
    setUsuarios(response);

  }catch(error){
    console.error('Erro ao buscar usuários:', error);
  }
}

const handleClickDelete = (usuario) => {
  setUsuarioSelecionado(usuario);
  setMostrarModal(true);
};

const handleDeletar = async () => {
  try {
    await UsuarioApi.DeletarAsync(usuarioSelecionado.id);
    setUsuarioSelecionado(usuarios.filter((u) => u.id !== usuarioSelecionado.id));
    setMostrarModal(false);
    fetchUsuarios();
  }
  catch (error) {
    console.error('Erro ao deletar usuário:', error);
  }
};

useEffect(() => {
  fetchUsuarios();
}, []);

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
        <div className={style['pagina-conteudo']}>
          <div className={style['pagina-header']}>
            <h3>Usuarios</h3>
            <BotaoAdicionar to={"/usuario/criar"}>Novo</BotaoAdicionar>

          </div>
          <div className={style.tabela}>
            <Table responsive>
              {/* thead - colunas da tabela */}
              <thead className={style['tabela-header']}>
                <tr>
                  <th>Nome</th>
                  <th>Email</th>
                  <th>Ações</th>
                </tr>
              </thead>
              {/* tbody - linhas da tabela */}
              <tbody className={style['tabela-body']}>
                {usuarios.length === 0 ? (
                  <tr>
                    <td colSpan="3">Nenhum usuário encontrado.</td>
                  </tr>
                ) : (
                  usuarios.map((usuario) => (
                    <tr key={usuario.id}>
                      <td>{usuario.nome}</td>
                      <td>{usuario.email}</td>
                      <td>
                        <Link 
                        to={`/usuario/editar/${usuario.id}`} 
                        className={style['botao-editar']}
                        state={usuario.id}
                        >
                          <MdEdit/>
                        </Link>

                        <button
                          className={style['botao-deletar']}
                          onClick={() => handleClickDelete(usuario)}
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
                Tem certeza que deseja deletar o usuário "{usuarioSelecionado?.nome}"?
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
