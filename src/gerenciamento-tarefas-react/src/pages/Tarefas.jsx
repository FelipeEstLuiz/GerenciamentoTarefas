import React, { useEffect, useState } from 'react';
import axios from 'axios';
import DataTable from 'react-data-table-component';
import './Tarefas.css';
import Modal from 'react-modal';
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import ModalRemover from '../components/ModalRemover';
import { Link } from 'react-router-dom';
import Navbar from '../components/Navbar';

Modal.setAppElement('#root');

export default function Tarefas() {
  const [tarefas, setTarefas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalRows, setTotalRows] = useState(0);
  const [tarefaSelecionada, setTarefaSelecionada] = useState({
    id: null,
    titulo: "",
    descricao: "",
    statusTarefa: 0,
    dataConclusao: null,
  });
  const [modalRemoverIsOpen, setModalRemoverIsOpen] = useState(false);
  const perPage = 5;

  const fetchTarefas = async (page) => {
    setLoading(true);
    try {
      const response = await axios.get('https://localhost:7053/api/app/v1/tarefas', {
        params: { page, limit: perPage },
      });

      setTarefas(response.data.data);
      setTotalRows(response.data.totalData);
    } catch (error) {
      console.error('Erro ao carregar tarefas:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTarefas(currentPage);
  }, [currentPage]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const abrirModalRemover = (tarefa) => {

    console.log(tarefa.permiteRemover)

    if (tarefa.permiteRemover == true) {
      setTarefaSelecionada(tarefa);
      setModalRemoverIsOpen(true);
    }
  };

  const columns = [
    {
      name: 'Ações',
      cell: row => (
        <div style={{ display: 'flex', gap: '10px' }}>
          <Link to={`/tarefas/editar/${row.id}`}
            style={{ background: '#4caf50', color: 'white', border: 'none', padding: '5px', borderRadius: '4px' }}
          >
            Editar
          </Link>
          <button
            onClick={() => abrirModalRemover(row)}
            style={{
              background: '#f44336',
              color: 'white',
              border: 'none',
              padding: '5px 10px',
              borderRadius: '4px'
            }}
          >
            Remover
          </button>
        </div>
      ),
      ignoreRowClick: true,
      button: true,
    },
    { name: 'Id', selector: row => row.id },
    { name: 'Título', selector: row => row.titulo },
    { name: 'Descrição', selector: row => row.descricao, },
    { name: 'Status', selector: row => row.status },
    { name: 'Data Criação', selector: row => row.dataCriacao },
    { name: 'Data Conclusão', selector: row => row.dataConclusao },
  ];

  return (
    <div className="page-container">
      <Navbar fetchTarefas={fetchTarefas} />{}
      <div className="data-table-container">
        <h2>Minhas Tarefas</h2>
        <ToastContainer />
        <DataTable
          columns={columns}
          data={tarefas}
          progressPending={loading}
          pagination
          paginationServer
          paginationTotalRows={totalRows}
          paginationPerPage={perPage}
          paginationRowsPerPageOptions={[5]}
          onChangePage={handlePageChange}
          highlightOnHover
          pointerOnHover
          responsive
          noHeader
          noDataComponent="Nenhuma tarefa encontrada."
        />

        <ModalRemover
          isOpen={modalRemoverIsOpen}
          onClose={() => setModalRemoverIsOpen(false)}
          tarefa={tarefaSelecionada}
          fetchTarefas={fetchTarefas}
        />

      </div>
    </div>
  );
}