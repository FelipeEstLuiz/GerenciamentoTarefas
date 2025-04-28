import Modal from 'react-modal';
import { toast } from 'react-toastify';
import axios from 'axios';

export default function ModalRemover({ isOpen, onClose, tarefa, fetchTarefas }) {

  const confirmarRemover = async () => {
    try {
      if (!tarefa) return;

      await axios.delete(`https://localhost:7053/api/app/v1/tarefas/${tarefa.id}`);

      toast.success('Tarefa removida com sucesso!');

      onClose();

      fetchTarefas();

    } catch (error) {
      console.error("Erro ao remover", error);

      const mensagens = error.response?.data;

      if (Array.isArray(mensagens)) {
        toast.error(mensagens.join(", "));
      } else if (typeof mensagens === "string") {
        toast.error(mensagens);
      } else {
        toast.error("Erro ao remover tarefa.");
      }
    }
  };

  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onClose}
      closeTimeoutMS={300}
      style={{
        content: {
          position: 'relative',
          inset: 'unset',
          padding: '30px',
          width: '400px',
          borderRadius: '10px',
          border: '1px solid #ccc',
          background: '#fff',
          overflow: 'auto',
          textAlign: 'center',
          margin: 'auto'
        },
        overlay: {
          backgroundColor: 'rgba(0,0,0,0.5)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
        }
      }}
    >
      <h2>Confirmar Remoção</h2>
      <p>Deseja realmente remover esta tarefa "{tarefa?.titulo}"?</p>
      <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'space-between' }}>
        <button onClick={onClose} className="btn btn-secondary botao-acao" style={{ background: '#007bff', color: 'white' }}>Cancelar</button>
        <button onClick={confirmarRemover} className="btn btn-danger botao-acao" style={{ background: 'rgb(244, 67, 54)', color: 'white' }}>Remover</button>
      </div>
    </Modal>
  );
}
