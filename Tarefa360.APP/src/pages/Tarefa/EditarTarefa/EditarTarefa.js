import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import UsuarioApi from '../../../services/UsuarioApi';
import HistoriaAPI from '../../../services/HistoriaApi';
import ProjetoApi from '../../../services/ProjetoApi';
import TarefaApi from '../../../services/TarefaApi';
import style from './_editarTarefa.module.css';
import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { Typeahead } from 'react-bootstrap-typeahead';
import 'react-bootstrap-typeahead/css/Typeahead.css';

export function EditarTarefa() {
  const navigate  = useNavigate();
  const location  = useLocation();
  const [id]      = useState(location.state);

  const [tarefa, setTarefa] = useState({
    titulo: '',
    descricao: '',
    estimativa: '',
    tipoTarefaID: '',
    usuarioID: '',
    projetoID: '',
    historiaID: ''
  });

  const [tiposTarefa, setTiposTarefa]           = useState([]);
  const [usuarios, setUsuarios]                 = useState([]);
  const [projetos, setProjetos]                 = useState([]);
  const [historiasPorProjetoID, setHistoriasPorProjetoID] = useState([]);
  const [selectedProjeto, setSelectedProjeto]   = useState([]);
  const [selectedHistoria, setSelectedHistoria] = useState([]);
  const [selectedTipo, setSelectedTipo]         = useState([]);
  const [selectedUsuario, setSelectedUsuario]   = useState([]);
  const [erro, setErro]                         = useState('');
  const [salvando, setSalvando]                 = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target ? e.target : e;
    if (name === 'estimativa' && value !== '' && Number(value) > 999) return;
    setTarefa(prev => ({ ...prev, [name]: value }));
  };

  async function fetchProjetos() {
    const response = await ProjetoApi.ListarProjetoAsync(true);
    setProjetos(response);
    return response;
  }

  async function fetchUsuarios() {
    const response = await UsuarioApi.ListarAsync(true);
    setUsuarios(response);
    return response;
  }

  async function fetchTiposTarefa() {
    const response = await TarefaApi.ListarTiposTarefa();
    setTiposTarefa(response);
    return response;
  }

  async function fetchHistorias(projetoID) {
    if (!projetoID) return [];
    const response = await HistoriaAPI.ListarHistoriasPorProjeto(projetoID);
    setHistoriasPorProjetoID(response);
    return response;
  }

  useEffect(() => {
    const carregarDados = async () => {
      try {
        const [projetosData, usuariosData, tiposData] = await Promise.all([
          fetchProjetos(),
          fetchUsuarios(),
          fetchTiposTarefa()
        ]);

        const tarefaData = await TarefaApi.ObterPorIdAsync(id);

        setTarefa({
          titulo:      tarefaData.titulo,
          descricao:   tarefaData.descricao,
          estimativa:  tarefaData.estimativa ?? '',
          tipoTarefaID: tarefaData.tipoTarefaID,
          usuarioID:   tarefaData.usuario.id,
          projetoID:   tarefaData.projetoID,
          historiaID:  tarefaData.historiaID
        });

        const projetoSelecionado = projetosData.find(p => p.id === tarefaData.projetoID);
        setSelectedProjeto(projetoSelecionado ? [projetoSelecionado] : []);

        const historias = await fetchHistorias(tarefaData.projetoID);
        const historiaSelecionada = historias.find(h => h.id === tarefaData.historiaID);
        setSelectedHistoria(historiaSelecionada ? [historiaSelecionada] : []);

        const tipoSelecionado = tiposData.find(t => t.id === tarefaData.tipoTarefaID);
        setSelectedTipo(tipoSelecionado ? [tipoSelecionado] : []);

        const usuarioSelecionado = usuariosData.find(u => u.id === tarefaData.usuario.id);
        setSelectedUsuario(usuarioSelecionado ? [usuarioSelecionado] : []);

      } catch (error) {
        setErro('Erro ao carregar dados da tarefa.');
        console.error(error);
      }
    };

    if (id) carregarDados();
  }, [id]);

  const isFormValid = () =>
    tarefa.titulo &&
    tarefa.descricao &&
    tarefa.tipoTarefaID &&
    tarefa.usuarioID &&
    tarefa.projetoID &&
    tarefa.historiaID;

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErro('');

    if (!isFormValid()) {
      setErro('Preencha todos os campos obrigatórios.');
      return;
    }

    const payload = {
      titulo:       tarefa.titulo,
      descricao:    tarefa.descricao,
      tipoTarefaID: Number(tarefa.tipoTarefaID),
      usuarioID:    Number(tarefa.usuarioID),
      projetoID:    Number(tarefa.projetoID),
      historiaID:   Number(tarefa.historiaID),
      ...(tarefa.estimativa !== '' && { estimativa: Number(tarefa.estimativa) })
    };

    setSalvando(true);
    try {
      await TarefaApi.AtualizarAsync(id, payload);
      navigate('/tarefas');
    } catch (error) {
      const msg =
        error?.response?.data?.mensagem ||
        error?.response?.data?.title ||
        'Erro ao atualizar a tarefa. Tente novamente.';
      setErro(msg);
    } finally {
      setSalvando(false);
    }
  };

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <h3>Editar Tarefa</h3>

            {erro && <div className={style['erro-mensagem']}>{erro}</div>}

            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3">
                <Form.Label>Título *</Form.Label>
                <Form.Control
                  type="text"
                  name="titulo"
                  value={tarefa.titulo}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Descrição *</Form.Label>
                <Form.Control
                  type="text"
                  name="descricao"
                  value={tarefa.descricao}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Estimativa (horas)</Form.Label>
                <Form.Control
                  type="number"
                  name="estimativa"
                  value={tarefa.estimativa}
                  onChange={handleChange}
                  min={1}
                  max={999}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Projeto *</Form.Label>
                <Typeahead
                  id="projeto"
                  labelKey="nome"
                  options={projetos}
                  selected={selectedProjeto}
                  onChange={(selected) => {
                    const projeto = selected[0];
                    setSelectedProjeto(selected);
                    handleChange({ name: 'projetoID', value: projeto?.id ?? '' });
                    fetchHistorias(projeto?.id);
                    setSelectedHistoria([]);
                  }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>História *</Form.Label>
                <Typeahead
                  id="historia"
                  labelKey="nome"
                  options={historiasPorProjetoID}
                  selected={selectedHistoria}
                  onChange={(selected) => {
                    const historia = selected[0];
                    setSelectedHistoria(selected);
                    handleChange({ name: 'historiaID', value: historia?.id ?? '' });
                  }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Tipo *</Form.Label>
                <Typeahead
                  id="tipo"
                  labelKey="nome"
                  options={tiposTarefa}
                  selected={selectedTipo}
                  onChange={(selected) => {
                    const tipo = selected[0];
                    setSelectedTipo(selected);
                    handleChange({ name: 'tipoTarefaID', value: tipo?.id ?? '' });
                  }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Usuário Responsável *</Form.Label>
                <Typeahead
                  id="usuario"
                  labelKey="nome"
                  options={usuarios}
                  selected={selectedUsuario}
                  onChange={(selected) => {
                    const usuario = selected[0];
                    setSelectedUsuario(selected);
                    handleChange({ name: 'usuarioID', value: usuario?.id ?? '' });
                  }}
                />
              </Form.Group>

              <div className={style['btn-container']}>
                <Button variant="success" type="button" onClick={() => navigate('/tarefas')}>
                  Voltar
                </Button>
                <Button type="submit" disabled={!isFormValid() || salvando}>
                  {salvando ? 'Salvando...' : 'Salvar'}
                </Button>
              </div>
            </Form>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}
