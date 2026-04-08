import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import SprintApi from '../../../services/SprintApi';
import style from './_novaSprint.module.css';
import { useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import ProjetoApi from '../../../services/ProjetoApi';
import { useState, useEffect } from 'react';

export function NovaSprint() {
  const [sprint, setSprint] = useState({
    titulo: '',
    descricao: '',
    projetoId: ''
  });

  const [projetos, setProjetos] = useState([]);

  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setSprint((prevState) => ({
      ...prevState,
      [name]: value
    }));
  };

  const isFormValid = () => {
    return sprint.titulo.trim() !== '' && sprint.descricao.trim() !== '' && sprint.projetoId !== '';
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!isFormValid()) {
      alert('Por favor, preencha todos os campos.');
      return;
    }

    try {
      setLoading(true);

      await SprintApi.CriarSprintAsync(
        sprint.titulo,
        sprint.descricao,
        sprint.projetoId
      );

      alert('Sprint criado com sucesso!');
      navigate('/sprint');

    } catch (error) {
      console.error(error);
      alert('Erro ao criar sprint.');
    } finally {
      setLoading(false);
    }
  };

  async function fetchProjetos() {
    const response = await ProjetoApi.ListarProjetoDropdown(true);
    setProjetos(response);
  }

  useEffect(() => {
    fetchProjetos();
  }, []);

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>
            <h3>Novo Sprint</h3>

            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3" controlId="formTitulo">
                <Form.Label>Título</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o título do sprint"
                  name="titulo"   // ✅ corrigido
                  value={sprint.titulo}
                  onChange={handleChange}
                />
              </Form.Group>

              <Form.Group className="mb-3" controlId="formDescricao">
                <Form.Label>Descrição</Form.Label>
                <Form.Control
                  as="textarea"
                  rows={3}
                  placeholder="Digite a descrição do sprint"
                  name="descricao"
                  value={sprint.descricao}
                  onChange={handleChange}
                />
              </Form.Group>
              <Form.Group className="mb-3" controlId="formProjetoId">
                <Form.Label>Projeto</Form.Label>
                <Form.Select
                  name="projetoId"
                  value={sprint.projetoId}
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

              <Button
                variant="primary"
                type="submit"
                disabled={!isFormValid() || loading}
                className={style['btn-submit']}
              >
                {loading ? 'Salvando...' : 'Salvar'}
              </Button>
            </Form>

          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}