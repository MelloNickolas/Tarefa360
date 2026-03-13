import { Link } from "react-router-dom";
import style from "./Sidebaritem.module.css";


export function SidebarItem({ texto, link, logo}) {
  return <>
    <Link to={link} className={style.sidebar_item}>
      {logo}
      <h3 className={style.texto_link}>{texto}</h3>
    </Link>
  </>
}