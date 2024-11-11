CREATE TABLE [Seguridad].[Seg_Autorizacion] (
    [IdAutorizacion]  INT           IDENTITY (1, 1) NOT NULL,
    [IdOrganizacion]  INT           NULL,
    [IdTransaccion]   INT           NULL,
    [IdOpcion]        INT           NULL,
    [IdHorario]       INT           NULL,
    [Parametro]       VARCHAR (100) NOT NULL,
    [Operador]        VARCHAR (50)  NOT NULL,
    [ValorAutorizado] VARCHAR (50)  NULL,
    CONSTRAINT [PK_Autorizacion] PRIMARY KEY CLUSTERED ([IdAutorizacion] ASC) ON [Seguridad],
    CONSTRAINT [FK_HorarioTrans_Autorizacion] FOREIGN KEY ([IdOrganizacion], [IdTransaccion], [IdOpcion], [IdHorario]) REFERENCES [Seguridad].[Seg_HorarioTrans] ([IdOrganizacion], [IdTransaccion], [IdOpcion], [IdHorario])
) ON [Seguridad];


GO
CREATE NONCLUSTERED INDEX [IDX_Autorizacion_001]
    ON [Seguridad].[Seg_Autorizacion]([IdOrganizacion] ASC, [IdTransaccion] ASC, [IdOpcion] ASC, [IdHorario] ASC)
    ON [Indices];

