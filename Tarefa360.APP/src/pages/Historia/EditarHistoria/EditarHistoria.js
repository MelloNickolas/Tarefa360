import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import HistoriaAPI from '../../../services/HistoriaApi';
import ProjetoApi from '../../../services/ProjetoApi';
import style from './_editarHistoria.module.css';
import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export function EditarHistoria() {
  const navigate = useNavigate();
  const location = useLocation();

  const [projetos, setProjetos] = useState([]);

  async function fetchProjetos() {
    const response = await ProjetoApi.ListarProjetoDropdown(true);
    setProjetos(response);
  }

  useEffect(() => {
    fetchProjetos();
  }, []);



  const [id] = useState(location.state);

  const [historia, setHistoria] = useState({
    nome: '',
    descricao: '',
    projetoId: ''
  });

  // toda vez que o componente for renderizado, ele irá buscar os tipos de usuários disponíveis para preencher o dropdown
  useEffect(() => {

    const buscarDadosHistoria = async () => {
      try {
        const historiaData = await HistoriaAPI.ObterHistoriaPorId(id);
        setHistoria({
          nome: historiaData.nome,
          descricao: historiaData.descricao,
          projetoId: historiaData.projetoId
        });
      } catch (error) {
        console.error('Erro ao buscar historia:', error);
      }
    };

    buscarDadosHistoria();
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setHistoria(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (isFormValid()) {
      await HistoriaAPI.AtualizarHistoria(id, historia.nome, historia.descricao, historia.projetoId)
      navigate('/historias');
    } else {
      console.log(historia.nome, historia.descricao, historia.projetoId);
      alert('Por favor, preencha todos os campos obrigatórios.');
    }
  };

  const isFormValid = () => {
    return historia.nome && historia.descricao && historia.projetoId;
  }

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <h3>Editar História</h3>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3" controlId="formNome">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o nome da História"
                  name="nome"
                  value={historia.nome}
                  onChange={handleChange}
                  required
                  minLength={3}
                  maxLength={100}
                />
              </Form.Group>
              <Form.Group className="mb-3" controlId="formDescricao">
                <Form.Label>Descrição</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite a descrição da história"
                  name="descricao"
                  value={historia.descricao}
                  onChange={handleChange}
                  required
                  maxLength={500}
                />
              </Form.Group>

              <Form.Group className="mb-3" controlId="formProjeto">
                <Form.Label>Projeto</Form.Label>
                <Form.Select
                  name="projetoId"
                  value={historia.projetoId}
                  onChange={handleChange}
                  required
                >
                  <option value="">Selecione um projeto</option>
                  {projetos.map((projeto) => (
                    <option key={projeto.id} value={projeto.id}>
                      {projeto.nome}
                    </option>
                  ))}
                </Form.Select>
              </Form.Group>

              <Button variant="primary" type="submit" disabled={!isFormValid()} className={style['btn-submit']}>
                Salvar
              </Button>
            </Form>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}