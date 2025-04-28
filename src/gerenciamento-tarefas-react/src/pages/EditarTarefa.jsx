import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import axios from 'axios';
import { toast } from 'react-toastify';

export default function EditarTarefa() {
    const { id } = useParams();
    const navigate = useNavigate();
    const [titulo, setTitulo] = useState('');
    const [descricao, setDescricao] = useState('');
    const [status, setStatus] = useState(0);
    const [dataConclusao, setDataConclusao] = useState('');
    const [erros, setErros] = useState({});
    const [dataCriacao, setDataCriacao] = useState('');

    useEffect(() => {
        async function carregarTarefa() {
            try {
                const response = await axios.get(`https://localhost:7053/api/app/v1/tarefas/${id}`);
                const tarefa = response.data;
                setTitulo(tarefa.titulo || '');
                setDescricao(tarefa.descricao || '');
                setStatus(tarefa.codigoStatusTarefa || 0);
                setDataConclusao(tarefa.dataConclusao || '');
                setDataCriacao(tarefa.dataCriacao || '');
            } catch (error) {
                console.error('Erro ao carregar tarefa', error);
                toast.error('Erro ao carregar tarefa.');
            }
        }
        carregarTarefa();
    }, [id]);

    const validar = () => {
        const novosErros = {};

        if (!titulo) {
            novosErros.titulo = 'O título é obrigatório.';
        } else if (titulo.length > 100) {
            novosErros.titulo = 'Título deve conter no máximo 100 caracteres.';
        }

        if (descricao && descricao.length > 500) {
            novosErros.descricao = 'A descrição deve conter no máximo 500 caracteres.';
        }

        if (status === 2 && !dataConclusao) {
            novosErros.dataConclusao = 'A data de conclusão é obrigatória para status Concluído.';
        }

        setErros(novosErros);
        return Object.keys(novosErros).length === 0;
    };

    const cancelar = () => {
        setTitulo('');
        setDescricao('');
        setStatus(0);
        setDataConclusao('');
        setDataCriacao('');
        setErros({});
        navigate('/tarefas')
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validar()) {
            toast.error('Corrija os campos destacados.');
            return;
        }

        try {
            await axios.put(`https://localhost:7053/api/app/v1/tarefas/${id}`, {
                id: Number(id),
                titulo,
                descricao,
                statusTarefa: status,
                dataConclusao: dataConclusao || null,
            });

            toast.success('Tarefa atualizada com sucesso!');
            navigate('/tarefas');
        } catch (error) {
            console.error('Erro ao atualizar tarefa', error);
            toast.error('Erro ao atualizar tarefa.');
        }
    };

    return (
        <div style={{ padding: '70px 50px' }}>
            <h2>Editar Tarefa</h2>
            <form onSubmit={handleSubmit}>
                <div style={{ marginBottom: '15px' }}>

                    <div style={{ display: 'flex', justifyContent: 'space-between', gap: '20px' }} >
                        <div style={{ fles: '1', minWidth: '100px', gap: '20px' }} >
                            <label>ID:</label>
                            <input type="text" value={id} disabled style={{ width: '100%' }} />
                        </div>
                        <div style={{ fles: '1', minWidth: '100px', gap: '20px' }} >
                            <label>Data de Criação:</label>
                            <input
                                type="datetime-local"
                                value={dataCriacao?.substring(0, 16)}
                                disabled
                                style={{ width: '100%' }}
                            />
                        </div>

                        <div style={{ fles: '1', minWidth: '100px', gap: '20px', width: '100%' }} >
                            <label>Título:</label>
                            <input
                                type="text"
                                value={titulo}
                                onChange={(e) => setTitulo(e.target.value)}
                                className={`form-control ${erros.titulo ? 'is-invalid' : ''}`}
                                style={{ width: '100%' }}
                                maxLength={100}
                            />
                            {erros.titulo && <div className="invalid-feedback">{erros.titulo}</div>}
                        </div>
                    </div>

                    <div style={{ fles: '1', minWidth: '100px', paddingTop: '10px', width: '100%' }} >
                        <label>Descrição (opcional):</label>
                        <textarea
                            value={descricao}
                            onChange={(e) => setDescricao(e.target.value)}
                            className={`form-control ${erros.descricao ? 'is-invalid' : ''}`}
                            style={{ width: '100%', padding: '10px' }}
                            maxLength={500}
                            rows={5}
                        />
                        {erros.descricao && <div className="invalid-feedback">{erros.descricao}</div>}
                    </div>

                    <div style={{ display: 'grid', justifyContent: 'space-between', gap: '10px' }} >
                        <div style={{ fles: '1', minWidth: '100px', gap: '20px' }} >
                            <label>Status:</label>
                            <select
                                value={status}
                                onChange={(e) => setStatus(Number(e.target.value))}
                                style={{ width: '100%' }}
                                required
                            >
                                <option value={0}>Pendente</option>
                                <option value={1}>Em Progresso</option>
                                <option value={2}>Concluído</option>
                            </select>
                        </div>
                        <div style={{ fles: '1', minWidth: '100px', gap: '20px' }} >
                            <label>Data de Conclusão (opcional):</label>
                            <input
                                type="datetime-local"
                                value={dataConclusao}
                                onChange={(e) => {
                                    setDataConclusao(e.target.value);
                                    if (e.target.value) setStatus(2);
                                }}
                                style={{ width: '100%' }}
                                className={`form-control ${erros.dataConclusao ? 'is-invalid' : ''}`}
                            />
                            {erros.dataConclusao && <div className="invalid-feedback">{erros.dataConclusao}</div>}

                        </div></div>
                </div>

                <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'space-between' }}>
                    <button
                        type="button" onClick={cancelar}
                        className="btn btn-secondary botao-acao"
                        style={{ background: '#007bff', color: 'white' }}
                    >
                        Cancelar
                    </button>
                    <button
                        type="submit"
                        className="btn btn-success botao-acao"
                        style={{ background: '#28a745', color: 'white' }}
                    >
                        Salvar
                    </button>
                </div>
            </form >
        </div >
    );
}
