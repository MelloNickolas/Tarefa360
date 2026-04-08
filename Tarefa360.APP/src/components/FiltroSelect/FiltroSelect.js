import style from './_filtroSelect.module.css';

export function FiltroSelection({ value, onChange, children, nomeFiltro }) {
  return (
    <div className={style.filtro_group}>
      <label>{nomeFiltro} :</label>
      <select value={value} onChange={onChange}>
        {children}
      </select>
    </div>
  );
}