import style from './_topbar.module.css';
import { MdLogout } from 'react-icons/md';
import { useNavigate } from 'react-router-dom';
import { logout } from '../../services/ServicoAutenticacao';

export function Topbar({ children }) {
  const navigate = useNavigate();

  async function handleLogout() {
    await logout();
    navigate('/login');
  }

  return (
    <div>
      <div className={style['topbar-conteudo']}>
        <button
          onClick={handleLogout}
          className={style['botao-logout']}
          title="Sair do sistema"
          aria-label="Sair"
        >
          <MdLogout />
        </button>
      </div>
      <div className={style['pagina-conteudo']}>
        {children}
      </div>
    </div>
  );
}
