import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import style from './_novaEquipe.module.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { MdDelete } from 'react-icons/md';
import { Table } from 'react-bootstrap';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import EquipeApi from '../../../services/EquipeApi';
import UsuarioApi from '../../../services/UsuarioApi';

export function NovaEquipe() {
  const navigate = useNavigate();

  const [equipe, setEquipe] = useState({ nome: '' });
  const [busca, setBusca] = useState('');
  const [sugestoes, setSugestoes] = useState([]);
  const [usuarioSelecionado, setUsuarioSelecionado] = useState(null);
  const [papelSelecionado, setPapelSelecionado] = useState('');
  const [papeisEquipe, setPapeisEquipe] = useState([]);
  const [membros, setMembros] = useState([]);

  useEffect(() => {
    EquipeApi.ListarPapeisEquipeAsync().then(res => {
      setPapeisEquipe(res);
      if (res.length > 0) setPapelSelecionado(res[0].id);
    });
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setEquipe(prev => ({ ...prev, [name]: value }));
  };

  const handleBusca = async (valor) => {
    setBusca(valor);
    setUsuarioSelecionado(null);

    if (valor.length < 2) return setSugestoes([]);

    const todos = await UsuarioApi.ListarAsync(true);

    setSugestoes(
      todos
        .filter(u => u.nome.toLowerCase().includes(valor.toLowerCase()))
        .slice(0, 5)
    );
  };

  const handleSelecionarUsuario = (usuario) => {
    setBusca(usuario.nome);
    setUsuarioSelecionado(usuario);
    setSugestoes([]);
  };

  const handleAdicionarMembro = () => {
    if (!usuarioSelecionado) {
      alert('Selecione um usuário!');
      return;
    }

    setMembros(prev => [
      ...prev,
      {
        id: Date.now(),
        nomeUsuario: usuarioSelecionado.nome,
        usuarioId: usuarioSelecionado.id,
        papeisEquipe: papelSelecionado
      }
    ]);

    setBusca('');
    setUsuarioSelecionado(null);
    setSugestoes([]);
  };

  const handleRemoverMembro = (id) => {
    setMembros(prev => prev.filter(m => m.id !== id));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!equipe.nome) {
      alert('Informe o nome da equipe');
      return;
    }

    try {
      const equipeId = await EquipeApi.CriarEquipeAsync(equipe.nome);

      for (const membro of membros) {

        console.log('VALOR:', membro.papeisEquipe);
        console.log('TIPO:', typeof membro.papeisEquipe);

        await EquipeApi.CriarUsuarioEquipeAsync(
          Number(membro.papeisEquipe),
          membro.usuarioId,
          equipeId
        );
      }

      navigate('/equipes');

    } catch (error) {
      console.error(error);
      alert('Erro ao criar equipe');
    }
  };

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>

            <h3>Criar Equipe</h3>

            <Form onSubmit={handleSubmit}>

              {/* NOME */}
              <Form.Group className="mb-3">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o nome da equipe"
                  name="nome"
                  value={equipe.nome}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <h4>Membros</h4>

              {/* BLOCO ADICIONAR MEMBRO */}
              <div className={style['adicionar-membro']}>

                {/* BUSCA */}
                <div className={style['adicionar-membro-item']}>
                  <input
                    type="text"
                    placeholder="Buscar usuário pelo nome..."
                    value={busca}
                    onChange={(e) => handleBusca(e.target.value)}
                    className={style['input-busca']}
                  />
                </div>

                {/* SELECT + BOTÃO */}
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

                  <button
                    type="button"
                    className={style['botao-adicionar']}
                    onClick={handleAdicionarMembro}
                  >
                    + Membro
                  </button>
                </div>

              </div>

              {/* SUGESTÕES */}
              {sugestoes.length > 0 && (
                <ul className={style['sugestoes']}>
                  {sugestoes.map((usuario) => (
                    <li
                      key={usuario.id}
                      onClick={() => handleSelecionarUsuario(usuario)}
                      className={style['sugestao']}
                    >
                      <span>{usuario.nome}</span>
                      <span className={style['sugestao-email']}>
                        - {usuario.email}
                      </span>
                    </li>
                  ))}
                </ul>
              )}

              {/* TABELA */}
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
                        <td colSpan="3" className={style['mensagem-vazia']}>Nenhum membro adicionado.</td>
                      </tr>
                    ) : (
                      membros.map((membro) => (
                        <tr key={membro.id}>
                          <td>{membro.nomeUsuario}</td>
                          <td>
                            {
                              papeisEquipe.find(
                                p => p.id === membro.papeisEquipe
                              )?.nome
                            }
                          </td>
                          <td>
                            <button
                              type="button"
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
                <Button type="submit">Salvar</Button>
              </div>

            </Form>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}