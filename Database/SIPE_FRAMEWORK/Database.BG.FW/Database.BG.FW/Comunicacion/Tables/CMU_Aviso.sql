CREATE TABLE [Comunicacion].[CMU_Aviso] (
    [IdAviso]          INT           IDENTITY (1, 1) NOT NULL,
    [IdUsuarioOrigen]  VARCHAR (50)  NOT NULL,
    [FechaEnvia]       DATETIME      NOT NULL,
    [IdUsuarioDestino] VARCHAR (50)  NOT NULL,
    [Estado]           CHAR (1)      NOT NULL,
    [Mensaje]          VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([IdAviso] ASC) ON [Comunicacion]
) ON [Comunicacion] TEXTIMAGE_ON [Comunicacion];


GO
CREATE NONCLUSTERED INDEX [IDX_Aviso_001]
    ON [Comunicacion].[CMU_Aviso]([IdUsuarioDestino] ASC, [FechaEnvia] ASC)
    ON [Indices];

