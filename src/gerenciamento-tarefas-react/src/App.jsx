import { Routes, Route, Navigate  } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import Sobre from './pages/Sobre';
import Tarefas from './pages/Tarefas';
import EditarTarefa from './pages/EditarTarefa';

function App() {
  return (
    <>
      <Navbar />
      <div className="content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/tarefas" element={<Tarefas />} />
          <Route path="/tarefas/editar/:id" element={<EditarTarefa />} /> {/* aqui */}
          <Route path="/sobre" element={<Sobre />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </>
  );
}

export default App;
