import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import SprintApi from '../../../services/SprintApi';
import UsuarioApi from '../../../services/UsuarioApi';
import ProjetoApi from '../../../services/ProjetoApi';
import HistoriaApi from '../../../services/HistoriaApi';
import TarefaApi from '../../../services/TarefaApi';
import style from './_novaTarefa.module.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import { Typeahead } from 'react-bootstrap-typeahead';
import 'react-bootstrap-typeahead/css/Typeahead.css';

export function NovaTarefa() {
  const [tarefa, setTarefa] = useState({
    titulo: '',
    descricao: '',
    estimativa: '',
    tipoTarefaID: '',
    usuarioID: '',
    projetoID: '',
    historiaID: '',
    sprintID: ''
  });

  const [tiposTarefa, setTiposTarefa] = useState([]);
  const [usuarios, setUsuarios] = useState([]);
  const [sprints, setSprints] = useState([]);
  const [projetos, setProjetos] = useState([]);
  const [historiasPorProjetoID, setHistoriasPorProjetoID] = useState([]);
  const [erro, setErro] = useState('');
  const [salvando, setSalvando] = useState(false);

  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target ? e.target : e;
    if (name === 'estimativa' && value !== '' && Number(value) > 999) return;
    setTarefa(prev => ({ ...prev, [name]: value }));
  };

  async function fetchProjetos() {
    const response = await ProjetoApi.ListarProjetoAsync(true);
    setProjetos(response);
  }

  async function fetchHistoriasPorProjetoID(projetoID) {
    if (!projetoID) return;
    const response = await HistoriaApi.ListarHistoriasPorProjeto(projetoID);
    setHistoriasPorProjetoID(response);
  }

  async function fetchUsuarios() {
    const response = await UsuarioApi.ListarAsync(true);
    setUsuarios(response);
  }

  async function fetchTiposTarefa() {
    const response = await TarefaApi.ListarTiposTarefa();
    setTiposTarefa(response);
  }

  async function fetchSprintsPorProjeto(projetoID) {
    if (!projetoID) { setSprints([]); return; }
    const response = await SprintApi.ListarSprintAsync();
    setSprints(response.filter(s => Number(s.projetoId) === Number(projetoID)));
  }

  useEffect(() => {
    fetchProjetos();
    fetchUsuarios();
    fetchTiposTarefa();
  }, []);

  useEffect(() => {
    if (tarefa.projetoID) {
      fetchHistoriasPorProjetoID(tarefa.projetoID);
      fetchSprintsPorProjeto(tarefa.projetoID);
    } else {
      setHistoriasPorProjetoID([]);
      setSprints([]);
    }
  }, [tarefa.projetoID]);

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
      setErro('Por favor, preencha todos os campos obrigatórios.');
      return;
    }

    setSalvando(true);
    try {
      await TarefaApi.CriarAsync(tarefa);
      navigate('/tarefas');
    } catch (error) {
      const msg =
        error?.response?.data?.mensagem ||
        error?.response?.data?.title ||
        'Erro ao salvar a tarefa. Verifique os dados e tente novamente.';
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
            <h3>Nova Tarefa</h3>

            {erro && <div className={style['erro-mensagem']}>{erro}</div>}

            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3">
                <Form.Label>Título *</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o título da tarefa"
                  name="titulo"
                  value={tarefa.titulo}
                  onChange={handleChange}
                  minLength={3}
                  maxLength={100}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Descrição *</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite a descrição da tarefa"
                  name="descricao"
                  value={tarefa.descricao}
                  onChange={handleChange}
                  minLength={3}
                  maxLength={500}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Estimativa (horas)</Form.Label>
                <Form.Control
                  type="number"
                  placeholder="Ex: 8"
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
                  placeholder="Selecione um projeto"
                  emptyLabel="Nenhum projeto encontrado"
                  onChange={(selected) => {
                    const projeto = selected[0];
                    setTarefa(prev => ({
                      ...prev,
                      projetoID: projeto?.id || '',
                      sprintID: '',
                      historiaID: ''
                    }));
                  }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Sprint</Form.Label>
                <Typeahead
                  id="sprint"
                  labelKey="titulo"
                  options={sprints}
                  placeholder="Selecione uma sprint"
                  emptyLabel={!tarefa.projetoID ? 'Selecione um projeto primeiro' : 'Nenhuma sprint encontrada'}
                  disabled={!tarefa.projetoID}
                  onChange={(selected) => {
                    const sprint = selected[0];
                    handleChange({ name: 'sprintID', value: sprint?.id ?? '' });
                  }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>História *</Form.Label>
                <Typeahead
                  id="historia"
                  labelKey="nome"
                  options={historiasPorProjetoID}
                  placeholder="Selecione uma história"
                  emptyLabel={!tarefa.projetoID ? 'Selecione um projeto primeiro' : 'Nenhuma história encontrada'}
                  disabled={!tarefa.projetoID}
                  onChange={(selected) => {
                    const historia = selected[0];
                    handleChange({ name: 'historiaID', value: historia?.id ?? '' });
                  }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Tipo *</Form.Label>
                <Typeahead
                  id="tipoTarefa"
                  labelKey="nome"
                  options={tiposTarefa}
                  placeholder="Selecione um tipo"
                  emptyLabel="Nenhum tipo encontrado"
                  onChange={(selected) => {
                    const tipo = selected[0];
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
                  placeholder="Selecione um usuário"
                  emptyLabel="Nenhum usuário encontrado"
                  onChange={(selected) => {
                    const usuario = selected[0];
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
