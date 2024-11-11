CREATE TABLE [dbo].[Contacto] (
    [Identificacion] VARCHAR (13) NOT NULL,
    [Nombres]        VARCHAR (50) NOT NULL,
    [Apellidos]      VARCHAR (50) NOT NULL,
    [FechaNac]       DATE         NOT NULL,
    [Direccion]      VARCHAR (50) NULL,
    [Telefono]       VARCHAR (30) NULL,
    [Celular]        VARCHAR (10) NULL,
    [Estado]         VARCHAR (1)  NOT NULL,
    [Email]          VARCHAR (50) NULL
);

