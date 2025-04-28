import { Link } from 'react-router-dom';
import './Navbar.css';
import { useState } from 'react';
import ModalCadastrarTarefa from './ModalCadastrarTarefa';

export default function Navbar({ fetchTarefas }) { 
  const [modalCadastroAberto, setModalCadastroAberto] = useState(false);

  return (
    <>
      <nav className="navbar">
        <h1>Gerenciamento de Tarefas</h1>
        <div className="links">
          <Link to="/home">Home</Link>
          <Link to="/tarefas">Tarefas</Link>
          <button onClick={() => setModalCadastroAberto(true)} className="link-button" >Nova Tarefa</button>
          <Link to="/sobre">Sobre</Link>
        </div>
      </nav>
      <ModalCadastrarTarefa
        isOpen={modalCadastroAberto}
        onClose={() => setModalCadastroAberto(false)}
        fetchTarefas={fetchTarefas}
      />
    </>
  );
}
