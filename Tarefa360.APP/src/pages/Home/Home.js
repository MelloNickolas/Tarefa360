import style from './_home.module.css';
import Button from "react-bootstrap/Button";
import { useNavigate } from "react-router-dom";
import { Sidebar } from '../../components/Sidebar/Sidebar';
import { Topbar } from '../../components/Topbar/Topbar';

export function Home() {
  const navigate = useNavigate();
    function abrirLogin(){
        navigate("/usuarios");
    }
  return (
    <div className={style.conteudo}>
            <Sidebar>
                <Topbar>
                <div className={style.pagina_conteudo}>
                    <h3>Home</h3>
                </div> 
                </Topbar> 
            </Sidebar> 
        </div>
  );
}