use SIPE_FRAMEWORK
go


DECLARE @IdOrg INT, @IdSrv INT, @IdTrx INT
SET @IdOrg = 39
SET @IdSrv = 0
SET @IdTrx = 4402

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
	[Menu] [bit] null,
	[Monitor][bit] null,
    [XmlEntrada][varchar](1) null,
	[XmlSalida][varchar](1) null,
	[XmlValidador][varchar](1) null
)

DECLARE @ROLTRAN TABLE (
	[IdRol] [int] NOT NULL,
	[IdOrganizacion] [int] NOT NULL,
	[IdTransaccion] [int] NOT NULL,
	[IdOpcion] [int] NOT NULL
)

declare @v_param varchar(1500)
set @v_param = '<SP></SP>'

--
INSERT INTO @TRANS VALUES (@IdOrg, @IdTrx, 'Módulo de Seguridad - Opcion: Logs de Acceso a Roles', 'A', NULL,
	@v_param,
	'S', NULL, NULL, 0, '','', @IdSrv,null,null,null,null,null)


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

insert Seguridad.Seg_OpcionTransRol (IdRol, IdOrganizacion, IdTransaccion, IdOpcion)
values 
(22,39,@IdTrx,'1') ,
(23,39,@IdTrx,'1') ,
(24,39,@IdTrx,'1') 

INSERT INTO SIPE_FRAMEWORK.Seguridad.Seg_HorarioTrans (IdOrganizacion, IdTransaccion, IdOpcion, IdHorario)
	SELECT IdOrganizacion, IdTransaccion, IdOpcion = 1, IdHorario = 1 FROM @TRANS


COMMIT TRAN
