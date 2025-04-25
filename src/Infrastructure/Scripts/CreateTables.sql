IF OBJECT_ID('dbo.StatusTarefa', 'U') IS NULL
	CREATE TABLE StatusTarefa(
		Codigo INT NOT NULL CONSTRAINT PK_StatusTarefa PRIMARY KEY,
		Descricao VARCHAR(150) NOT NULL
	)


IF OBJECT_ID('dbo.StatusTarefa', 'U') IS NOT NULL
BEGIN
	IF NOT EXISTS(SELECT 1 FROM StatusTarefa WHERE Codigo = 0)
		INSERT INTO StatusTarefa (Codigo, Descricao) VALUES(0, 'Pendente')

	IF NOT EXISTS(SELECT 1 FROM StatusTarefa WHERE Codigo = 1)
		INSERT INTO StatusTarefa (Codigo, Descricao) VALUES(1, 'Em Progresso')

	IF NOT EXISTS(SELECT 1 FROM StatusTarefa WHERE Codigo = 2)
		INSERT INTO StatusTarefa (Codigo, Descricao) VALUES(2, 'Concluida')
END

IF OBJECT_ID('dbo.Tarefa', 'U') IS NULL
	CREATE TABLE Tarefa(
		Id INT IDENTITY(1,1) CONSTRAINT PK_Tarefa PRIMARY KEY,
		Titulo VARCHAR(100) NOT NULL,
		CodigoStatusTarefa INT NOT NULL
			CONSTRAINT FK_Tarefa_CodigoStatusTarefa_StatusTarefa_Codigo
			FOREIGN KEY(CodigoStatusTarefa)
			REFERENCES StatusTarefa (Codigo)
			CONSTRAINT DF_Tarefa_StatusTarefa DEFAULT(0),
		DataCriacao DATETIME NOT NULL CONSTRAINT DF_Tarefa_DataCriacao DEFAULT(SYSDATETIME()),
		DataConclusao DATETIME NULL,
		Descricao VARCHAR(500) NULL
	)