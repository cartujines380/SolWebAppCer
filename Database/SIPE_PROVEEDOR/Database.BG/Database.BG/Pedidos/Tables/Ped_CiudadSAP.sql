CREATE TABLE [Pedidos].[Ped_CiudadSAP] (
    [IdEmpresa]    INT          NOT NULL,
    [CodCiudad]    VARCHAR (12) NOT NULL,
    [NomCiudad]    VARCHAR (40) NOT NULL,
    [CodPais]      VARCHAR (3)  NOT NULL,
    [CodProvincia] VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_Ped_CiudadSAP] PRIMARY KEY CLUSTERED ([IdEmpresa] ASC, [CodCiudad] ASC) ON [Pedidos]
) ON [Pedidos];

