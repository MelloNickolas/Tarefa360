import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { RotaPrivada } from "./routes/RotaPrivada";
import { Login }           from './pages/Login/Login';
import { EsqueciMinhaSenha } from './pages/EsqueciMinhaSenha/EsqueciMinhaSenha';
import { Home }            from './pages/Home/Home';
import { Usuarios }        from './pages/Usuario/Usuarios/Usuarios';
import { NovoUsuario }     from './pages/Usuario/NovoUsuario/NovoUsuario';
import { EditarUsuario }   from './pages/Usuario/EditarUsuario/EditarUsuario';
import { Projetos }        from './pages/Projeto/Projetos/Projetos';
import { NovoProjeto }     from './pages/Projeto/NovoProjeto/NovoProjeto';
import { EditarProjeto }   from './pages/Projeto/EditarProjeto/EditarProjeto';
import { Historias }       from './pages/Historia/Historias/Historias';
import { NovaHistoria }    from './pages/Historia/NovaHistoria/NovaHistoria';
import { EditarHistoria }  from './pages/Historia/EditarHistoria/EditarHistoria';
import { Tarefas }         from './pages/Tarefa/Tarefas/Tarefas';
import { NovaTarefa }      from './pages/Tarefa/NovaTarefa/NovaTarefa';
import { EditarTarefa }    from './pages/Tarefa/EditarTarefa/EditarTarefa';
import { Equipes }         from './pages/Equipe/Equipes/Equipes';
import { EditarEquipe }    from './pages/Equipe/EditarEquipe/EditarEquipe';
import { NovaEquipe }      from './pages/Equipe/NovaEquipe/NovaEquipe';
import { Sprint }          from './pages/Sprint/Sprints/Sprint';
import { EditarSprint }    from './pages/Sprint/EditarSprint/EditarSprint';
import { NovaSprint }      from './pages/Sprint/NovaSprint/NovaSprint';
import { DashboardProjetos } from "./pages/Dashboards/Dashboard";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login"             element={<Login />} />
        <Route path="/esqueciMinhaSenha" element={<EsqueciMinhaSenha />} />

        <Route path="/home"                element={<RotaPrivada><Home /></RotaPrivada>} />
        <Route path="/usuarios"            element={<RotaPrivada><Usuarios /></RotaPrivada>} />
        <Route path="/usuario/criar"       element={<RotaPrivada><NovoUsuario /></RotaPrivada>} />
        <Route path="/usuario/editar/:id"  element={<RotaPrivada><EditarUsuario /></RotaPrivada>} />
        <Route path="/projetos"            element={<RotaPrivada><Projetos /></RotaPrivada>} />
        <Route path="/projeto/criar"       element={<RotaPrivada><NovoProjeto /></RotaPrivada>} />
        <Route path="/projeto/editar/:id"  element={<RotaPrivada><EditarProjeto /></RotaPrivada>} />
        <Route path="/historias"           element={<RotaPrivada><Historias /></RotaPrivada>} />
        <Route path="/historia/criar"      element={<RotaPrivada><NovaHistoria /></RotaPrivada>} />
        <Route path="/historia/editar/:id" element={<RotaPrivada><EditarHistoria /></RotaPrivada>} />
        <Route path="/tarefas"             element={<RotaPrivada><Tarefas /></RotaPrivada>} />
        <Route path="/tarefa/criar"        element={<RotaPrivada><NovaTarefa /></RotaPrivada>} />
        <Route path="/tarefa/editar/:id"   element={<RotaPrivada><EditarTarefa /></RotaPrivada>} />
        <Route path="/equipes"             element={<RotaPrivada><Equipes /></RotaPrivada>} />
        <Route path="/equipe/editar/:id"   element={<RotaPrivada><EditarEquipe /></RotaPrivada>} />
        <Route path="/equipe/criar"        element={<RotaPrivada><NovaEquipe /></RotaPrivada>} />
        <Route path="/sprint"              element={<RotaPrivada><Sprint /></RotaPrivada>} />
        <Route path="/sprint/editar/:id"   element={<RotaPrivada><EditarSprint /></RotaPrivada>} />
        <Route path="/sprint/criar"        element={<RotaPrivada><NovaSprint /></RotaPrivada>} />
        <Route path="/dashboard"           element={<RotaPrivada><DashboardProjetos /></RotaPrivada>} />

        <Route path="*" element={<Login />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
