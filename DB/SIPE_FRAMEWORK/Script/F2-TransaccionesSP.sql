use SIPE_FRAMEWORK
go


DECLARE @IdOrg INT, @IdSrv27 INT
SET @IdOrg = 39
SET @IdSrv27 = 27


DECLARE @TRANS TABLE (
	[IdOrganizacion] [int] NOT NULL,
	[IdTransaccion] [int] NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Estado] [char](1) NULL,
	[PerfilContable] [int] NULL,
	[Parametros] [varchar](max) NULL,
	[Auditable] [char](1) NULL,
	[CostoNormal] [money] NULL,
	[CostoEspecial] [money] NULL,
	[IdServidor] [int] NULL,
	[NombreBase] [varchar](100) NULL,
	[NombreSP] [varchar](100) NULL,
	[IdServidorExec] [int] NULL,
	Menu bit,
	Monitor bit,
	XmlEntrada varchar,
	XmlSalida varchar,
	XmlValidador varchar
)


DECLARE @ROLTRAN TABLE (
	[IdRol] [int] NOT NULL,
	[IdOrganizacion] [int] NOT NULL,
	[IdTransaccion] [int] NOT NULL,
	[IdOpcion] [int] NOT NULL
)

--Portal - SP Contrato Licitacion
INSERT INTO @TRANS VALUES (@IdOrg, 1104, 'Portal - SP Contrato Licitacion', 'A', NULL,
'<SP nombre="[SIPE_PROVEEDOR].[Licitacion].[Lic_P_ContratoLicitacion]"> <Param nombre="@PI_ParamXML" tipo="xml" posicion="0" direccion="input" longitud="-1" /> </SP>',
'S', NULL, NULL, @IdSrv27, 'SIPE_PROVEEDOR', 'Licitacion.Lic_P_ContratoLicitacion', @IdSrv27, 0, 0, '', '', '')


INSERT INTO @ROLTRAN
SELECT 21, @IdOrg, IdTransaccion, 1
	FROM @TRANS

BEGIN TRAN

DELETE X FROM SIPE_FRAMEWORK.Seguridad.Seg_OpcionTransRol X
	WHERE EXISTS (SELECT 1 FROM @TRANS t WHERE t.IdOrganizacion = X.IdOrganizacion AND t.IdTransaccion = X.IdTransaccion)

DELETE X FROM SIPE_FRAMEWORK.Seguridad.Seg_HorarioTrans X
	WHERE EXISTS (SELECT 1 FROM @TRANS t WHERE t.IdOrganizacion = X.IdOrganizacion AND t.IdTransaccion = X.IdTransaccion)

DELETE X FROM SIPE_FRAMEWORK.Seguridad.Seg_OpcionTrans X
	WHERE EXISTS (SELECT 1 FROM @TRANS t WHERE t.IdOrganizacion = X.IdOrganizacion AND t.IdTransaccion = X.IdTransaccion)

DELETE X FROM SIPE_FRAMEWORK.Seguridad.Seg_Transaccion X
	WHERE EXISTS (SELECT 1 FROM @TRANS t WHERE t.IdOrganizacion = X.IdOrganizacion AND t.IdTransaccion = X.IdTransaccion)


INSERT INTO SIPE_FRAMEWORK.Seguridad.Seg_Transaccion
	SELECT * FROM @TRANS

INSERT INTO SIPE_FRAMEWORK.Seguridad.Seg_OpcionTrans (IdOrganizacion, IdTransaccion, IdOpcion, Descripcion, Nivel)
	SELECT IdOrganizacion, IdTransaccion, IdOpcion = 1, Descripcion = 'default', Nivel = 0 FROM @TRANS

INSERT INTO SIPE_FRAMEWORK.Seguridad.Seg_HorarioTrans (IdOrganizacion, IdTransaccion, IdOpcion, IdHorario)
	SELECT IdOrganizacion, IdTransaccion, IdOpcion = 1, IdHorario = 1 FROM @TRANS

INSERT INTO SIPE_FRAMEWORK.Seguridad.Seg_OpcionTransRol
	SELECT * FROM @ROLTRAN

COMMIT TRAN
