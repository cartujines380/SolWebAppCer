CREATE TABLE [Seguridad].[Seg_OpcionTrans] (
    [IdOrganizacion] INT           NOT NULL,
    [IdTransaccion]  INT           NOT NULL,
    [IdOpcion]       INT           NOT NULL,
    [Descripcion]    VARCHAR (200) NULL,
    [Nivel]          INT           NULL,
    CONSTRAINT [PK_OpcionTrans] PRIMARY KEY CLUSTERED ([IdOrganizacion] ASC, [IdTransaccion] ASC, [IdOpcion] ASC) ON [Seguridad],
    CONSTRAINT [FK_Transaccion_OpcionTrans] FOREIGN KEY ([IdOrganizacion], [IdTransaccion]) REFERENCES [Seguridad].[Seg_Transaccion] ([IdOrganizacion], [IdTransaccion])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_OpcionTrans_001]
    ON [Seguridad].[Seg_OpcionTrans]([IdOrganizacion] ASC, [IdTransaccion] ASC)
    ON [Indices];

