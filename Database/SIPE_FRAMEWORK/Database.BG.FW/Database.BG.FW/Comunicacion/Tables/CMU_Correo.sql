CREATE TABLE [Comunicacion].[CMU_Correo] (
    [IdMensaje]        INT           NOT NULL,
    [CorreoPrincipal]  VARCHAR (100) NOT NULL,
    [CorreoSecundario] VARCHAR (100) NULL,
    [Motivo]           VARCHAR (200) NULL,
    [Mensaje]          VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([IdMensaje] ASC, [CorreoPrincipal] ASC) ON [Comunicacion],
    CONSTRAINT [FK_Mensaje_Correo] FOREIGN KEY ([IdMensaje]) REFERENCES [Comunicacion].[CMU_Mensaje] ([IdMensaje])
) ON [Comunicacion] TEXTIMAGE_ON [Comunicacion];


GO
CREATE NONCLUSTERED INDEX [IDX_Correo_001]
    ON [Comunicacion].[CMU_Correo]([IdMensaje] ASC)
    ON [Indices];

