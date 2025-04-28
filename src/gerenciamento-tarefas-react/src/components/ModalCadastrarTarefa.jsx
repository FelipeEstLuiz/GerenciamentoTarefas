import Modal from 'react-modal';
import { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

export default function ModalCadastrarTarefa({ isOpen, onClose, fetchTarefas }) {
    const [titulo, setTitulo] = useState('');
    const [descricao, setDescricao] = useState('');
    const navigate = useNavigate();
    const [erros, setErros] = useState({});

    useEffect(() => {
        setTitulo('');
        setDescricao('');
        setErros({});
    }, [isOpen]);

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

        setErros(novosErros);
        return Object.keys(novosErros).length === 0;
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (!validar()) {
            toast.error('Corrija os campos destacados.');
            return;
        }

        cadastrar({
            titulo: titulo.trim(),
            descricao: descricao.trim() || undefined,
        });
    };

    const cadastrar = async (tarefa) => {
        try {
            await axios.post('https://localhost:7053/api/app/v1/tarefas', tarefa);

            toast.success('Tarefa criada com sucesso!');

            onClose();
            setTitulo('');
            setDescricao('');

            if (fetchTarefas) {
                fetchTarefas();
            }

            navigate('/tarefas');
        } catch (error) {
            console.error("Erro ao cadastrar", error);

            const mensagens = error.response?.data;

            if (Array.isArray(mensagens)) {
                toast.error(mensagens.join(", "));
            } else if (typeof mensagens === "string") {
                toast.error(mensagens);
            } else {
                toast.error("Erro ao cadastrar tarefa.");
            }
        }
    }

    return (
        <Modal
            isOpen={isOpen}
            onRequestClose={onClose}
            style={{
                content: {
                    position: 'relative',
                    inset: 'unset',
                    padding: '25px',
                    width: '500px',
                    borderRadius: '10px',
                    border: '1px solid #ccc',
                    background: '#fff',
                    overflow: 'auto',
                    textAlign: 'center',
                    margin: 'auto',
                    marginTop: '80px'
                },
                overlay: {
                    backgroundColor: 'rgba(0,0,0,0.5)',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }
            }}
        >
            <h2>Cadastrar Nova Tarefa</h2>
            <form onSubmit={handleSubmit}>
                <div style={{ textAlign: 'left', marginBottom: '15px' }}>
                    <label>Título:</label>
                    <input
                        type="text"
                        value={titulo}
                        onChange={(e) => setTitulo(e.target.value)}
                        style={{ width: '100%' }}
                        maxLength={100}
                    />
                    {erros.titulo && <div className="invalid-feedback">{erros.titulo}</div>}

                    <label>Descrição (opcional):</label>
                    <textarea
                        value={descricao}
                        onChange={(e) => setDescricao(e.target.value)}
                        style={{ width: '100%', marginTop: '10px' }}
                        maxLength={500}
                        rows="9"
                    />
                    {erros.descricao && <div className="invalid-feedback">{erros.descricao}</div>}
                </div>

                <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'space-between' }}>
                    <button type="button" onClick={onClose} className="btn btn-secondary botao-acao" style={{ background: 'blue', color: 'white' }}>
                        Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary botao-acao" style={{ background: 'rgb(76, 175, 80)', color: 'white' }}>
                        Salvar
                    </button>
                </div>
            </form>
        </Modal>
    );
}
