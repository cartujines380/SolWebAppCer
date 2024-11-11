CREATE TABLE [Participante].[Par_DocumentoParticipante] (
    [IdParticipante]  INT           NOT NULL,
    [IdDocumento]     INT           NOT NULL,
    [IdTipoDocumento] VARCHAR (10)  NOT NULL,
    [TipoArchivo]     VARCHAR (100) NULL,
    [LongArchivo]     INT           NULL,
    [NombreArchivo]   VARCHAR (100) NULL,
    [Documento]       IMAGE         NULL,
    [Descripcion]     VARCHAR (100) NULL,
    [FechaDocumento]  DATETIME      NULL,
    CONSTRAINT [PK_DocumentoParticipante] PRIMARY KEY CLUSTERED ([IdParticipante] ASC, [IdDocumento] ASC) ON [Participante],
    CONSTRAINT [FK_Participante_DocumentoParticipante] FOREIGN KEY ([IdParticipante]) REFERENCES [Participante].[Par_Participante] ([IdParticipante])
) ON [Participante] TEXTIMAGE_ON [Participante];

