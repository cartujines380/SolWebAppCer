
CREATE procedure [Seguridad].[Seg_P_IsPermisoUserTransOpcion]
@PI_IdUsuario              VARCHAR(20),
@PI_IdAplicacion           INT,
@PI_IdEmpresa		   INT,
@PI_IdSucursal		int,
@PI_IdOrganizacion	int,
@PI_IdTransaccion	int,
@PI_IdOpcion	int,
@PI_IdIdentificacion varchar(100), --Maquina activa
@PI_Token varchar(50), -- Token activo
@PI_txtTransaccion varchar(max),
@PI_ParamAut varchar(100) = '',
@PI_Valor varchar(50) = '',
@PO_Permiso		int output
AS


	SET @PO_Permiso = 0
-- select * from Seguridad.Seg_auditoria
--Auditoria 
/*	  IF exists( select IdTransaccion FROM Seguridad.Seg_TRANSACCION
		WHERE IdOrganizacion = @PI_IdOrganizacion AND idTransaccion = @PI_IdTransaccion 
			AND Auditable = 'S' ) 
		         exec  Seguridad.Seg_P_REGISTRA_AUDITORIA 
			     @PI_IdUsuario,
			     @PI_IdAplicacion,
			     @PI_IdIdentificacion,
			     @PI_txtTransaccion,
			     @PI_IdOrganizacion,
			     @PI_IdTransaccion
*/
-- Verifica que el usuario tenga permiso para la trnasaccion
	EXEC Seguridad.Seg_P_VERIFICA_TRANSACCION
	@PI_IdUsuario,
	@PI_Token, -- Que solo cheque si tiene asignado la transaccion sin login
	@PI_IdIdentificacion,
	@PI_IdAplicacion,
	@PI_IdOrganizacion,
	@PI_IdTransaccion,
	@PI_IdOpcion,
	@PI_IdEmpresa,
	@Pi_IdSucursal,
	@PI_ParamAut,
	@PI_Valor,
	@PI_txtTransaccion,
	@PO_Permiso  OUTPUT	






