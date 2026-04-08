import { Sidebar } from '../../../components/Sidebar/Sidebar';
import { Topbar } from '../../../components/Topbar/Topbar';
import ProjetoApi from '../../../services/ProjetoApi';
import style from './_novoProjeto.module.css';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';

export function NovoProjeto() {
  const [projeto, setProjeto] = useState({
    nome: '',
    descricao: ''
  });

  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProjeto(prevState => ({
      ...prevState,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    // Lógica para enviar os dados do novo projeto
    if(isFormValid()) {
      await ProjetoApi.CriarProjetoAsync(projeto.nome, projeto.descricao);
      navigate('/Projetos');
    } else {
        alert('Por favor, preencha todos os campos obrigatórios.');
    }
};

  const isFormValid = () => {
    return projeto.nome && projeto.descricao;
  }

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
        <div className={style['pagina-conteudo']}>
            <h3>Novo Projeto</h3>
            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3" controlId="formNome">
                <Form.Label>Nome</Form.Label>
                <Form.Control 
                  type="text" 
                  placeholder="Digite o nome do projeto" 
                  name="nome" 
                  value={projeto.nome} 
                  onChange={handleChange} 
                  required
                />
              </Form.Group>
              <Form.Group className="mb-3" controlId="formDescricao">
                <Form.Label>Descrição</Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Digite a descrição do projeto"
                  name="descricao"
                  value={projeto.descricao}
                  onChange={handleChange}
                  required
                />
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