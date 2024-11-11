CREATE TABLE [Proveedor].[Pro_Banco] (
    [CodPais]         VARCHAR (10)  NOT NULL,
    [CodBanco]        VARCHAR (15)  NOT NULL,
    [NomBanco]        VARCHAR (250) NULL,
    [Region]          VARCHAR (10)  NULL,
    [Direcion]        VARCHAR (250) NULL,
    [Poblacion]       VARCHAR (100) NULL,
    [CodSWIFT]        VARCHAR (100) NULL,
    [GrupoBancario]   VARCHAR (10)  NULL,
    [IndGiroCajapost] VARCHAR (10)  NULL,
    [IndBorrado]      VARCHAR (10)  NULL,
    [CodBancario]     VARCHAR (15)  NULL,
    CONSTRAINT [PK_Pro_Banco] PRIMARY KEY CLUSTERED ([CodBanco] ASC)
);

