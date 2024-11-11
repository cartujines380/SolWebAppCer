
/*
Seguridad.Seg_P_PERMISO_TRANSACCION 'usrMF','45493929112007151130454939293015',
'00-08-A1-97-8D-5E',
'usrMF','45493929112007151130454939293015','00-08-A1-97-8D-5E',
4,1,184,1,2,0,null,null,null

<?xml version="1.0" encoding="iso-8859-1"?>
<Usuario PS_IdUsuario="admin" PS_Token="96299229062007211140962992294021" 
PS_Maquina="127.0.0.1" PS_UsrSitio="usrMF" 
PS_TokenSitio="45493929112007151130454939293015" 
PS_MaqSitio="00-08-A1-97-8D-5E" PS_IdEmpresa="2" PS_IdSucursal="0" 
PS_IdAplicacion="4" PS_IdOrganizacion="0" PS_Login="N" Perfil="S" />

select * from seguridad.seg_registro
where idusuario = 'usrMF'

*/

CREATE procedure [Seguridad].[Seg_P_PERMISO_TRANSACCION]
@PV_UsrSitio            VARCHAR(20),
@PV_TokenSitio          VARCHAR(32),
@PV_MaqSitio            VARCHAR(100),
@PV_idUsuario            VARCHAR(20),
@PV_Token                VARCHAR(32),
@PV_Maquina              VARCHAR(100),
@PV_idAplicacion         INT,
@PV_idOrganizacion       INT,
@PV_IdTransaccion        INT,
@PV_IdOpcion             INT,
@PV_idEmpresa		   INT,
@PV_idSucursal			int,
@PV_ParamAut             VARCHAR(100),
@PV_Valor                VARCHAR(50),
@PV_txtTransaccion       VARCHAR(max)

AS
Set @PV_MaqSitio = SUBSTRING(@PV_MaqSitio,1,20)
Set @PV_Maquina = SUBSTRING(@PV_Maquina,1,20)

DECLARE @PO_CodRetorno BIT
--DECLARE @VL_Parametros varchar(max), @VL_Valor varchar(100), @IdServidor int

-- Revisa que la transaccion no sea de Tipo Menu
	IF EXISTS(SELECT 1 FROM Seguridad.Seg_Transaccion
				WHERE IdOrganizacion = @PV_idOrganizacion
					AND IdTransaccion = @PV_IdTransaccion
					AND Menu = 1 )
	BEGIN
		raiserror ('Transaccion es de tipo Menu, no puede ejecutar SP.',16,1)
		RETURN
	END
-- Verifica que el Sitio de donde se hace la transaccion este autorizado
EXEC Seguridad.Seg_P_VERIFICA_TRANSACCION
@PV_UsrSitio,
@PV_TokenSitio,
@PV_MaqSitio,
1, --@PV_idAplicacion,
1,
163, --Transaccion de autorizacion de sitio
1,
2,
0,
null,
null,
null,
@PO_CodRetorno  OUTPUT
IF @PO_CodRetorno = 1 -- El sitio esta autorizado
BEGIN
	-- Verifica que el usuario tenga permiso para la trnasaccion
	EXEC Seguridad.Seg_P_VERIFICA_TRANSACCION
	@PV_idUsuario,
	@PV_Token,
	@PV_Maquina,
	@PV_idAplicacion,
	@PV_idOrganizacion,
	@PV_IdTransaccion,
	@PV_IdOpcion,
	@PV_idEmpresa,
	@PV_idSucursal,
	@PV_ParamAut,
	@PV_Valor,
	@PV_txtTransaccion,
	@PO_CodRetorno  OUTPUT
	set @PO_CodRetorno=1
	IF @PO_CodRetorno = 1 -- Tiene permisos
	BEGIN
	    -- Recupera el Stored Procedure y parametros
	    -- El IdServidor debe ser del Servidor que lo ejecuta
		SELECT Parametros, isnull(IdServidorExec,0) as IdServidor
		FROM Seguridad.Seg_TRANSACCION
		WHERE IdTransaccion = @PV_IdTransaccion
			AND IdOrganizacion = @PV_IdOrganizacion
	END
END
ELSE -- No tiene permiso el sitio
	raiserror ('Sitio no esta autorizado',16,1)

