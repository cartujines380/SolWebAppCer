
CREATE procedure [Seguridad].[Seg_P_IsPermisoAplTransOpcion]
@PI_IdUsuario              VARCHAR(20),
@PI_IdIdentificacion varchar(100), --Maquina activa
@PI_IdAplicacion           INT,
@PI_IdEmpresa		   INT,
@PI_IdSucursal		int,
@PI_IdOrganizacion	int,
@PI_IdTransaccion	int,
@PI_IdOpcion	int,
@PI_txtTransaccion varchar(max),
@PO_Permiso		int output
AS

	SET @PO_Permiso = 0
-- Verifica que el usuario tenga permiso para la trnasaccion
-- Solo para usuarios aplicativos, no loign, TOken=''
	EXEC Seguridad.Seg_P_VERIFICA_TRANSACCION
	@PI_IdUsuario,
	'', -- Que solo cheque si tiene asignado la transaccion sin login
	@PI_IdIdentificacion,
	@PI_IdAplicacion,
	@PI_IdOrganizacion,
	@PI_IdTransaccion,
	@PI_IdOpcion,
	@PI_IdEmpresa,
	@Pi_IdSucursal,
	'',
	'',
	@PI_txtTransaccion,
	@PO_Permiso  OUTPUT	



