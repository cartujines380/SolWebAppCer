CREATE TABLE [Proveedor].[Pro_Proveedor] (
    [CodProveedor]          VARCHAR (10)  NOT NULL,
    [Ruc]                   VARCHAR (13)  NULL,
    [TipoProveedor]         VARCHAR (13)  NULL,
    [NomComercial]          VARCHAR (500) NULL,
    [DirCalleNum]           VARCHAR (30)  NULL,
    [DirPisoEdificio]       VARCHAR (10)  NULL,
    [DirCallePrinc]         VARCHAR (45)  NULL,
    [DirDistrito]           VARCHAR (35)  NULL,
    [DirCodPostal]          VARCHAR (10)  NULL,
    [Poblacion]             VARCHAR (35)  NULL,
    [Pais]                  VARCHAR (3)   NULL,
    [Region]                VARCHAR (3)   NULL,
    [Idioma]                VARCHAR (1)   NULL,
    [Telefono]              VARCHAR (40)  NULL,
    [Movil]                 VARCHAR (16)  NULL,
    [Fax]                   VARCHAR (31)  NULL,
    [CorreoE]               VARCHAR (241) NULL,
    [GenDocElec]            VARCHAR (1)   NULL,
    [FechaCertifica]        DATETIME      NULL,
    [IndMinoria]            VARCHAR (3)   NULL,
    [ApoderadoNom]          VARCHAR (100) NULL,
    [ApoderadoApe]          VARCHAR (100) NULL,
    [ApoderadoIdFiscal]     VARCHAR (16)  NULL,
    [PlazoEntregaPrev]      VARCHAR (3)   NULL,
    [FechaMod]              DATETIME      NULL,
    [CodProveedorBG]        VARCHAR (25)  NULL,
    [CodClaseContribuyente] VARCHAR (10)  NULL,
    [Extension]             VARCHAR (4)   NULL,
    CONSTRAINT [PK_Pro_Proveedor] PRIMARY KEY CLUSTERED ([CodProveedor] ASC) ON [Proveedor]
) ON [Proveedor];


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20160502-144201]
    ON [Proveedor].[Pro_Proveedor]([Ruc] ASC)
    ON [Proveedor];

