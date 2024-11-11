CREATE TABLE [Proveedor].[Pro_sociedad] (
    [IdSociedad]            VARCHAR (5)   NOT NULL,
    [NombreSociedad]        VARCHAR (100) NOT NULL,
    [Activar]               BIT           NOT NULL,
    [Estado]                VARCHAR (2)   NOT NULL,
    [url]                   VARCHAR (100) NULL,
    [Licencia]              VARCHAR (100) NULL,
    [RepresentanteLegal]    VARCHAR (100) NULL,
    [RucSociedad]           VARCHAR (13)  NULL,
    [CodActividadEconomica] VARCHAR (20)  NULL,
    [Direccion]             VARCHAR (100) NULL,
    [Locacion]              VARCHAR (35)  NULL,
    [Correo]                VARCHAR (241) NULL,
    [Telefono]              VARCHAR (40)  NULL,
    CONSTRAINT [PK_pro_sociedad] PRIMARY KEY CLUSTERED ([IdSociedad] ASC)
);

