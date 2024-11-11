CREATE TABLE [Licitacion].[Lic_LogFlujoContrato] (
    [IdLogFlujo]       INT          IDENTITY (1, 1) NOT NULL,
    [IdAccion]         INT          NOT NULL,
    [IdContrato]       INT          NOT NULL,
    [IdEstadoContrato] INT          NOT NULL,
    [UsuarioRegistro]  VARCHAR (20) NOT NULL,
    [FechaRegistro]    DATETIME     NOT NULL,
    CONSTRAINT [PK_LogFlujoContrato] PRIMARY KEY CLUSTERED ([IdLogFlujo] ASC)
);

