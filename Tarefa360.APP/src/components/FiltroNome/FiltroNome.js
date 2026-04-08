import style from "./_filtroNome.module.css";

export function FiltroNome({ onChange, placeholder }) {
  return (
    <input
      type="text"
      placeholder={placeholder ?? 'Buscar...'}
      className={style.filtro_input}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}
