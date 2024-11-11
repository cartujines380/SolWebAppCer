CREATE TABLE [Proveedor].[Pro_Documentos] (
    [IdDocumentos]        INT           IDENTITY (1, 1) NOT NULL,
    [CodTipoPersona]      VARCHAR (5)   NOT NULL,
    [Codigo]              VARCHAR (5)   NOT NULL,
    [Descripcion]         VARCHAR (200) NOT NULL,
    [EsObligatorio]       VARCHAR (1)   NOT NULL,
    [FechaRegistro]       DATETIME      NOT NULL,
    [UsuarioCreacion]     VARCHAR (20)  NULL,
    [FechaModificacion]   DATETIME      NULL,
    [UsuarioModificacion] VARCHAR (20)  NULL,
    [Estado]              VARCHAR (5)   NOT NULL,
    PRIMARY KEY CLUSTERED ([IdDocumentos] ASC)
);


GO
CREATE NONCLUSTERED INDEX [idx_Pro_Documentos_01]
    ON [Proveedor].[Pro_Documentos]([CodTipoPersona] ASC);


GO
CREATE NONCLUSTERED INDEX [idx_Pro_Documentos_02]
    ON [Proveedor].[Pro_Documentos]([Codigo] ASC);

