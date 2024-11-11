CREATE TABLE [Proveedor].[Pro_DatoAdjuntoSolicitado] (
    [IdDatoSolicitado] BIGINT       IDENTITY (1, 1) NOT NULL,
    [CodDocumento]     VARCHAR (10) NULL,
    [SectorComercial]  VARCHAR (10) NULL,
    [Estado]           BIT          NULL,
    CONSTRAINT [PK_PRO_DATOADJUNTOSOLICITADO] PRIMARY KEY CLUSTERED ([IdDatoSolicitado] ASC)
);

