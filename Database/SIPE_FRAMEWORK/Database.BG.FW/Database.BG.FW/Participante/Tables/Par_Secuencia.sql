CREATE TABLE [Participante].[Par_Secuencia] (
    [IdSecuencia] INT          NOT NULL,
    [IdTabla]     INT          NOT NULL,
    [Nombre]      VARCHAR (50) NULL,
    CONSTRAINT [PK_Secuencia] PRIMARY KEY CLUSTERED ([IdSecuencia] ASC, [IdTabla] ASC) ON [Participante]
) ON [Participante];

