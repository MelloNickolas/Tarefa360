import { Link } from "react-router-dom";
import style from "./_botaoAdicionar.module.css"

export function BotaoAdicionar({ to, children }) {
  return (
    <Link to={to} className={style.botao_adicionar}>
      <span>+</span>
      <span>{children}</span>
    </Link>
  );
}