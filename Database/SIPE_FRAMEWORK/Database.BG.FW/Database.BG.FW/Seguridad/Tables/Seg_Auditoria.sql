CREATE TABLE [Seguridad].[Seg_Auditoria] (
    [IdAplicacion]     INT          NOT NULL,
    [IdOrganizacion]   INT          NULL,
    [IdTransaccion]    INT          NULL,
    [IdUsuario]        VARCHAR (20) NULL,
    [FechaMovi]        DATETIME     NULL,
    [IdIdentificacion] VARCHAR (20) NULL,
    [txtTransaccion]   XML          NULL,
    [UsuarioReemplazo] VARCHAR (20) NULL,
    [Estado]           CHAR (1)     NULL
) ON [Seguridad] TEXTIMAGE_ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Auditoria_001]
    ON [Seguridad].[Seg_Auditoria]([IdAplicacion] ASC, [IdOrganizacion] ASC, [IdTransaccion] ASC, [IdUsuario] ASC, [FechaMovi] ASC)
    ON [Indices];


GO
CREATE NONCLUSTERED INDEX [IDX_Auditoria_002]
    ON [Seguridad].[Seg_Auditoria]([IdUsuario] ASC, [FechaMovi] ASC)
    ON [Indices];

