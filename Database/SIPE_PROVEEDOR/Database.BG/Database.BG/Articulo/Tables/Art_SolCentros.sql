CREATE TABLE [Articulo].[Art_SolCentros] (
    [Centro]             VARCHAR (10) NOT NULL,
    [PerfilDistribucion] VARCHAR (10) NOT NULL,
    [IdDetalle]          BIGINT       NOT NULL,
    [IdSolicitud]        BIGINT       NOT NULL,
    [Estado]             BIT          NOT NULL
);

