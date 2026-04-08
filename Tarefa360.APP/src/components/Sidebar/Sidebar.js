import style from './_sidebar.module.css';
import logo from '../../assets/LogoBranco.png';
import { SidebarItem } from '../SidebarItem/SidebarItem';
import { GiSprint } from "react-icons/gi";
import { MdGroup, MdListAlt, MdAddTask, MdOutlineDashboard } from 'react-icons/md';
import { FiAlignLeft } from "react-icons/fi";
import { RiTeamFill } from "react-icons/ri";
import { Link } from 'react-router-dom';

export function Sidebar({ children }) {
  return (
    <div>
      <div className={style['sidebar-conteudo']}>
        <div className={style['sidebar-header']}>
          <Link to="/home">
            <img src={logo} alt="Logo-Tarefa360" className={style.logo} />
          </Link>
          <hr className={style.linha} />
        </div>
        <div className={style['sidebar-corpo']}>
          <SidebarItem texto="Usuários" link="/usuarios" logo={<MdGroup />} />
          <SidebarItem texto="Equipes" link="/equipes" logo={<RiTeamFill />} />
          <SidebarItem texto="Projetos" link="/projetos" logo={<MdListAlt />} />
          <SidebarItem texto="Sprint" link="/sprint" logo={<GiSprint />} />
          <SidebarItem texto="Histórias" link="/historias" logo={<FiAlignLeft />} />
          <SidebarItem texto="Tarefas" link="/tarefas" logo={<MdAddTask />} />

          <SidebarItem texto="Dashboard" link="/dashboard" logo={<MdOutlineDashboard />} />
        </div>
      </div>
      <div className={style['pagina-conteudo']}>
        {children}
      </div>
    </div>
  )
}
