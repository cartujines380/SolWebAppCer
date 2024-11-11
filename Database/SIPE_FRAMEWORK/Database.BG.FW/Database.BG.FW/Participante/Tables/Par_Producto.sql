CREATE TABLE [Participante].[Par_Producto] (
    [IdParticipante]     INT           NOT NULL,
    [TipoProducto]       VARCHAR (10)  NOT NULL,
    [NumeroProducto]     VARCHAR (16)  NOT NULL,
    [EstadoProducto]     BIT           NOT NULL,
    [NomPropietarioProd] VARCHAR (100) NULL,
    [Oficial]            VARCHAR (100) NULL,
    CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [TipoProducto] ASC, [NumeroProducto] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_Producto] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante];

