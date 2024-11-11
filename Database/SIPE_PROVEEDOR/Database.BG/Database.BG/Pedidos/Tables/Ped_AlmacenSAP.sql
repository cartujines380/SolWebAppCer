CREATE TABLE [Pedidos].[Ped_AlmacenSAP] (
    [IdEmpresa]          INT          NOT NULL,
    [CodAlmacen]         VARCHAR (4)  NOT NULL,
    [CodLegacy]          VARCHAR (3)  NOT NULL,
    [NomAlmacen]         VARCHAR (30) NOT NULL,
    [CodCiudad]          VARCHAR (12) NOT NULL,
    [CodAlmacenSRI]      VARCHAR (5)  NULL,
    [CodLegacyOriginal]  VARCHAR (3)  NOT NULL,
    [KioscoActivo]       VARCHAR (1)  NULL,
    [CodAlmacenOriginal] VARCHAR (4)  NULL
) ON [Pedidos];

