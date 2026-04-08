import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import SprintApi from '../../../services/SprintApi';
import style from './_editarSprint.module.css';
import { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export function EditarSprint() {

  const navigate = useNavigate();
  const location = useLocation();


  const id = location.state;

  const [sprint, setSprint] = useState({
    titulo: '',
    descricao: '',
    ProjetoId: ''
  });

  const [loading, setLoading] = useState(false);
  const [erro, setErro] = useState("");

  // 🔍 Buscar dados da sprint
  useEffect(() => {

    const buscarDadosSprint = async () => {
      try {
        setLoading(true);

        const sprintData = await SprintApi.ObterSprintPorIdAsync(id);
        //console.log("compos sprint",sprintData)
        setSprint({
          titulo: sprintData.titulo,
          descricao: sprintData.descricao,
          projetoId: sprintData.projetoId
        });

      } catch (error) {
        console.error('Erro ao buscar sprint:', error);
        setErro("Erro ao carregar dados da sprint");
      } finally {
        setLoading(false);
      }
    };

    if(id){
      buscarDadosSprint();
    }

  }, [id]);


  const handleChange = (e) => {
    const { name, value } = e.target;

    setSprint(prevState => ({
      ...prevState,
      [name]: value
    }));
  };


  const handleSubmit = async (e) => {
    e.preventDefault();

    setErro("");

    if (!isFormValid()) {
      setErro("Preencha todos os campos");
      return;
    }

    try {
      setLoading(true);

      await SprintApi.AtualizarSprintAsync(
        id,
        sprint.titulo,
        sprint.descricao,
        sprint.projetoId
      );

      navigate('/sprint');

    } catch (error) {
      console.error("Erro ao atualizar:", error);
      setErro("Erro ao atualizar sprint");
    } finally {
      setLoading(false);
    }
  };

  const isFormValid = () => {
    return sprint.titulo && sprint.descricao;
  };

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style['pagina-conteudo']}>

            <h3>Editar Sprint</h3>

            
            {erro && <p className={style.erro}>{erro}</p>}

           
            {loading && <p>Carregando...</p>}

            <Form onSubmit={handleSubmit}>

              <Form.Group className="mb-3">
                <Form.Label>Título</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite o nome do sprint"
                  name="titulo"
                  value={sprint.titulo}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Descrição</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite a descrição do sprint"
                  name="descricao"
                  value={sprint.descricao}
                  onChange={handleChange}
                  required
                />
              </Form.Group>

              <Button
                variant="primary"
                type="submit"
                disabled={!isFormValid() || loading}
                className={style['btn-submit']}
              >
                {loading ? "Salvando..." : "Salvar"}
              </Button>

            </Form>

          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}