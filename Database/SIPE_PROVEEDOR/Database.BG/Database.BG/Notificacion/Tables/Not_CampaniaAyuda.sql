CREATE TABLE [Notificacion].[Not_CampaniaAyuda] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [CodProveedor]  VARCHAR (10)  NOT NULL,
    [Usuario]       VARCHAR (20)  NOT NULL,
    [Acepto]        BIT           NOT NULL,
    [Motivo]        VARCHAR (10)  NULL,
    [Observacion]   VARCHAR (300) NULL,
    [FechaRegistro] DATETIME      NOT NULL,
    CONSTRAINT [PK_Not_CampaniaAyuda] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campania ayuda a damnificados terremoto', @level0type = N'SCHEMA', @level0name = N'Notificacion', @level1type = N'TABLE', @level1name = N'Not_CampaniaAyuda';

