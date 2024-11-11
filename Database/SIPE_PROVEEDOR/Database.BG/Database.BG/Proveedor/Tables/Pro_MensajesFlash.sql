CREATE TABLE [Proveedor].[Pro_MensajesFlash] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Identificacion] VARCHAR (13)   NOT NULL,
    [Titulo]         VARCHAR (50)   NOT NULL,
    [Mensaje]        VARCHAR (8000) NOT NULL,
    [Estado]         VARCHAR (1)    NOT NULL,
    [FechaCreacion]  DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx1]
    ON [Proveedor].[Pro_MensajesFlash]([Identificacion] ASC);


GO
CREATE NONCLUSTERED INDEX [idx2]
    ON [Proveedor].[Pro_MensajesFlash]([FechaCreacion] ASC);

