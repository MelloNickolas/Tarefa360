import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import { useState, useEffect } from "react";
import DashboardProjetosApi from "../../services/DashboardProjetosApi";
import style from "./_dashboard.module.css";
import { FiZap, FiTrendingUp, FiCheckSquare } from "react-icons/fi";
import { MdOutlineBugReport, MdOutlineTask } from "react-icons/md";

export function DashboardProjetos() {
  const iconesPorTipo = {
    1: <MdOutlineBugReport />,
    2: <FiZap />,
    3: <FiTrendingUp />,
    4: <MdOutlineTask />,
  };

  const [qtdTotalHistorias, setQtdTotalHistorias] = useState(0);
  const [qtdTotalTarefas, setQtdTotalTarefas] = useState(0);
  const [qtdTarefasConc, setQtdTarefasConc] = useState(0);
  const [qtdBugsConc, setQtdBugsConc] = useState(0);
  const [qtdBugsNaoConc, setQtdBugsNaoConc] = useState(0);
  const [qtdHorasConc, setQtdHorasConc] = useState(0);
  const [qtdHorasTotal, setQtdHorasTotal] = useState(0);
  const [historiasAgrupadas, setHistoriasAgrupadas] = useState([]);
  const [tarefasConcDataAtual, setTarefasConcDataAtual] = useState([]);
  const [loading, setLoading] = useState(true);
  const [erros, setErros] = useState([]);
  const [projetoHoverId, setProjetoHoverId] = useState(null);
  const [posicaoTooltip, setPosicaoTooltip] = useState({ x: 0, y: 0 });

  useEffect(() => {
    const fetchDashboardData = async () => {
      setLoading(true);
      const novosErros = [];

      try {
        const historiasTotais = await DashboardProjetosApi.QtdTotalHistorias();
        setQtdTotalHistorias(historiasTotais || 0);
      } catch {
        novosErros.push("Histórias totais");
      }

      try {
        const [tarefasConc, tarefasNaoConc] = await Promise.all([
          DashboardProjetosApi.QtdTotalTarefasPorConclusao(true),
          DashboardProjetosApi.QtdTotalTarefasPorConclusao(false),
        ]);
        setQtdTarefasConc(tarefasConc || 0);
        setQtdTotalTarefas((tarefasConc || 0) + (tarefasNaoConc || 0));
      } catch {
        novosErros.push("Tarefas por conclusão");
      }

      try {
        const lista = await DashboardProjetosApi.ListarTarefasConcluidasDataAtual();
        setTarefasConcDataAtual(Array.isArray(lista) ? lista : []);
      } catch {

        setTarefasConcDataAtual([]);
      }

      try {
        const [bugsConc, bugsNaoConc] = await Promise.all([
          DashboardProjetosApi.QtdTotalTarefasPorTipoPorConclusao(1, true),
          DashboardProjetosApi.QtdTotalTarefasPorTipoPorConclusao(1, false),
        ]);
        setQtdBugsConc(bugsConc || 0);
        setQtdBugsNaoConc(bugsNaoConc || 0);
      } catch {
        novosErros.push("Bugs");
      }

      try {
        const [horasConc, horasNaoConc] = await Promise.all([
          DashboardProjetosApi.QtdHorasPorConclusao(true),
          DashboardProjetosApi.QtdHorasPorConclusao(false),
        ]);
        setQtdHorasConc(horasConc || 0);
        setQtdHorasTotal((horasConc || 0) + (horasNaoConc || 0));
      } catch {
        novosErros.push("Horas");
      }

      try {
        const historiasProjeto = await DashboardProjetosApi.QtdHistoriasPorProjetoAgrupado();
        setHistoriasAgrupadas(Array.isArray(historiasProjeto) ? historiasProjeto : []);
      } catch {
        novosErros.push("Histórias por projeto");
      }

      setErros(novosErros);
      setLoading(false);
    };

    fetchDashboardData();
  }, []);

  const statusCards = [
    {
      title: "Horas",
      value: qtdHorasConc,
      subtitle: "Concluídas",
      extra: `${qtdTotalTarefas} tarefas no total`,
    },
    {
      title: "Histórias",
      value: qtdTotalHistorias,
      subtitle: "Total",
      extra: `${historiasAgrupadas.length} projetos`,
    },
    {
      title: "Bugs",
      value: qtdBugsConc,
      subtitle: "Resolvidos",
      extra: `${qtdBugsNaoConc} pendentes`,
    },
  ];

  const tarefasConcluidas = tarefasConcDataAtual.slice(0, 4).map((tarefa, idx) => ({
    id: tarefa?.id ?? idx + 1,
    title: tarefa?.titulo ?? "Sem título",
    status: "Concluído",
    time: tarefa?.dataConclusao
      ? new Date(tarefa.dataConclusao).toLocaleTimeString([], {
          hour: "2-digit",
          minute: "2-digit",
        })
      : "—",
    responsavel: tarefa?.usuario?.nome ?? "—",
    icon: tarefa?.tipoTarefaID,
  }));

  const projetoMembers = [
    { name: "Estimativa (horas)", count: qtdHorasTotal },
    { name: "Concluído (horas)", count: qtdHorasConc },
  ];

  const totalHistorias = historiasAgrupadas.reduce(
    (acc, item) => acc + (item.qtd_Historias || 0),
    0
  );

  let acumulado = 0;
  const dadosGrafico = historiasAgrupadas.map((item) => {
    const porcentagem = totalHistorias > 0 ? item.qtd_Historias / totalHistorias : 0;
    const dash = porcentagem * 314;
    const offset = acumulado;
    acumulado += dash;
    return { ...item, dash, offset };
  });

  const cores = ["#FFA726", "#EF5350", "#42A5F5", "#66BB6A", "#AB47BC"];

  const calculateposicaoTooltip = (offset, dash) => {
    const midAngle = ((offset + dash / 2) / 314) * 2 * Math.PI - Math.PI / 2;
    const svgX = 60 + Math.cos(midAngle) * 70;
    const svgY = 60 + Math.sin(midAngle) * 70;
    const scale = 140 / 120;
    return { x: svgX * scale, y: svgY * scale };
  };

  const handleSegmentHover = (projectId, offset, dash) => {
    setProjetoHoverId(projectId);
    setPosicaoTooltip(calculateposicaoTooltip(offset, dash));
  };

  if (loading) {
    return (
      <div className={style.conteudo}>
        <Sidebar>
          <Topbar>
            <div className={style["pagina-conteudo"]}>
              <div className={style["loading-container"]}>
                <div className={style["spinner"]}></div>
                <p>Carregando dashboard...</p>
              </div>
            </div>
          </Topbar>
        </Sidebar>
      </div>
    );
  }

  return (
    <div className={style.conteudo}>
      <Sidebar>
        <Topbar>
          <div className={style["pagina-conteudo"]}>
            <h3>Dashboard Projetos</h3>


            {erros.length > 0 && (
              <div className={style["aviso-parcial"]}>
                ⚠️ Alguns dados não puderam ser carregados: {erros.join(", ")}.
              </div>
            )}

            <div className={style["status-container"]}>
              {statusCards.map((card, index) => (
                <div key={index} className={style["status-card"]}>
                  <div className={style["card-header"]}>
                    <h5>{card.title}</h5>
                  </div>
                  <div className={style["card-content"]}>
                    <div className={style["main-value"]}>{card.value}</div>
                    <div className={style["card-details"]}>
                      <span className={style["subtitle"]}>{card.subtitle}</span>
                      <span className={style["extra"]}>{card.extra}</span>
                    </div>
                  </div>
                </div>
              ))}
            </div>

            {/* Main Content */}
            <div className={style["main-grid"]}>
              {/* Tarefas Concluídas Hoje */}
              <div className={style["left-section"]}>
                <div className={style["card"]}>
                  <h4 className={style["card-title"]}>Tarefas concluídas hoje</h4>
                  <div className={style["tarefas-list"]}>
                    {tarefasConcluidas.length === 0 ? (
                      <p className={style["sem-dados"]}>Nenhuma tarefa concluída hoje.</p>
                    ) : (
                      tarefasConcluidas.map((tarefa) => (
                        <div key={tarefa.id} className={style["tarefa-item"]}>
                          <div className={`${style["tarefa-icon"]} ${style[`icon-${tarefa.icon}`]}`}>
                            {iconesPorTipo[tarefa.icon] || <FiCheckSquare />}
                          </div>
                          <div className={style["tarefa-content"]}>
                            <p className={style["tarefa-title"]}>{tarefa.title}</p>
                            <div className={style["tarefa-meta"]}>
                              <span className={style["status"]}>{tarefa.status}</span>
                              <span className={style["time"]}>{tarefa.time}</span>
                              <span className={style["extra"]}>| {tarefa.responsavel}</span>
                            </div>
                          </div>
                        </div>
                      ))
                    )}
                  </div>
                </div>
              </div>

              {/* Histórias por Projeto */}
              <div className={style["right-section"]}>
                <div className={style["card"]}>
                  <h4 className={style["card-title"]}>Projetos</h4>
                  <div className={style["projeto-container"]}>
                    <div className={style["projeto-info"]}>
                      {projetoMembers.map((member, index) => (
                        <div key={index} className={style["projeto-item"]}>
                          <span className={style["projeto-label"]}>{member.name}</span>
                          <span className={style["projeto-value"]}>{member.count}</span>
                        </div>
                      ))}
                    </div>

                    <div className={style["chart-wrapper"]}>
                      {dadosGrafico.length === 0 ? (
                        <p className={style["sem-dados"]}>Sem dados de projetos.</p>
                      ) : (
                        <>
                          <svg className={style["donut-chart"]} viewBox="0 0 120 120">
                            {dadosGrafico.map((item, index) => (
                              <circle
                                key={item.projetoId}
                                cx="60"
                                cy="60"
                                r="50"
                                fill="none"
                                stroke={cores[index % cores.length]}
                                strokeWidth="15"
                                strokeDasharray={`${item.dash} 314`}
                                strokeDashoffset={`-${item.offset}`}
                                style={{ cursor: "pointer" }}
                                onMouseEnter={() =>
                                  handleSegmentHover(item.projetoId, item.offset, item.dash)
                                }
                                onMouseLeave={() => setProjetoHoverId(null)}
                              />
                            ))}
                            <text
                              x="60"
                              y="65"
                              textAnchor="middle"
                              fontSize="16"
                              fontWeight="bold"
                              fill="#333"
                            >
                              {totalHistorias}
                            </text>
                          </svg>

                          {projetoHoverId && (
                            <div
                              className={style["tooltip"]}
                              style={{ left: `${posicaoTooltip.x}px`, top: `${posicaoTooltip.y}px` }}
                            >
                              <div className={style["tooltip-content"]}>
                                <div className={style["tooltip-title"]}>
                                  {historiasAgrupadas.find((h) => h.projetoId === projetoHoverId)?.projeto ||
                                    `Projeto ${projetoHoverId}`}
                                </div>
                                <div className={style["tooltip-value"]}>
                                  {historiasAgrupadas.find((h) => h.projetoId === projetoHoverId)
                                    ?.qtd_Historias || 0}{" "}
                                  histórias
                                </div>
                              </div>
                            </div>
                          )}
                        </>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}
