CREATE TABLE [Seguridad].[Seg_SecuenciaMT] (
    [operacion_mt]  BIGINT        IDENTITY (1, 1) NOT NULL,
    [Trama]         VARCHAR (MAX) NULL,
    [FechaTrama]    DATETIME      NULL,
    [IdAplicacion]  INT           NULL,
    [IdTransaccion] INT           NULL
);

