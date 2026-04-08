import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import HistoriaAPI from '../../../services/HistoriaApi';
import ProjetoApi from '../../../services/ProjetoApi';
import style from './_novaHistoria.module.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export function NovaHistoria() {
  const [historia, setHistoria] = useState({
    nome: '',
    descricao: '',
    projetoId: ''
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setHistoria(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const [projetos, setProjetos] = useState([]);

  async function fetchProjetos() {
    const response = await ProjetoApi.ListarProjetoDropdown(true);
    setProjetos(response);
  }

  useEffect(() => {
    fetchProjetos();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Lógica para enviar os dados do novo projeto
    if (isFormValid()) {
      await HistoriaAPI.CriarHistoria(historia.nome, historia.descricao, historia.projetoId);
      navigate('/historias');
    } else {
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
            <h3>Nova História</h3>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3" controlId="formNome">
                <Form.Label>Nome</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o nome da história"
                  name="nome"
                  value={historia.nome}
                  onChange={handleChange}
                  minLength={3}
                  maxLength={100}
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3" controlId="formDescricao">
                <Form.Label>Descrição</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite a descricao da história"
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