import style from "./Sidebar.module.css";
import logo from "../../Assets/LogoBranco.png"
import { SidebarItem } from "../Sidebaritem/Sidebaritem";
import { MdGroup } from "react-icons/md";

export function Sidebar({ children }) {
  return <>
    <div className={style.sidebar_conteudo}>
      <div className={style.sidebar_header}>
        <img src={logo} alt="Logo Tarefa360" className={style.logo} />
        <hr className={style.linha} />
      </div>

      <div className={style.sidebar_corpo}>
        <SidebarItem texto="Usuarios" link="/usuarios" logo={<MdGroup />} />
      </div>
    </div>

    <div className={style.pagina_conteudo}>
      {children}
    </div>
  </>
}