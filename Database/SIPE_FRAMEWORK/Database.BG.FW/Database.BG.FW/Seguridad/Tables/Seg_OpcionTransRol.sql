CREATE TABLE [Seguridad].[Seg_OpcionTransRol] (
    [IdRol]          INT NOT NULL,
    [IdOrganizacion] INT NOT NULL,
    [IdTransaccion]  INT NOT NULL,
    [IdOpcion]       INT NOT NULL,
    CONSTRAINT [PK_OpcionTransRol] PRIMARY KEY CLUSTERED ([IdRol] ASC, [IdOrganizacion] ASC, [IdTransaccion] ASC, [IdOpcion] ASC) ON [Seguridad],
    CONSTRAINT [FK_OpcionTrans_OpcionTransRol] FOREIGN KEY ([IdOrganizacion], [IdTransaccion], [IdOpcion]) REFERENCES [Seguridad].[Seg_OpcionTrans] ([IdOrganizacion], [IdTransaccion], [IdOpcion]),
    CONSTRAINT [FK_Rol_OpcionTransRol] FOREIGN KEY ([IdRol]) REFERENCES [Seguridad].[Seg_Rol] ([IdRol])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_OpcionTransRol_001]
    ON [Seguridad].[Seg_OpcionTransRol]([IdOrganizacion] ASC, [IdTransaccion] ASC, [IdOpcion] ASC)
    ON [Indices];

