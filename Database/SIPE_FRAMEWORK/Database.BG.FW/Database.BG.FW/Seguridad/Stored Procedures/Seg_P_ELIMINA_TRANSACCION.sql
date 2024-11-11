create procedure [Seguridad].[Seg_P_ELIMINA_TRANSACCION] 
@PV_idTransaccion     int,
@PV_Organizacion      int
AS

-- dmunoz 16/03/2010
-- eliminar la transaccion del Rol en caso que solo lo tenga el Rol 1
declare @lvRoles int
SELECT @lvRoles = count(1)
	FROM Seguridad.Seg_OpcionTransRol
	WHERE idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion

if @lvRoles = 1 
begin
	delete Seguridad.Seg_OpcionTransRol
	WHERE idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion and idRol = 1
end


if exists (SELECT idTransaccion
	FROM Seguridad.Seg_OpcionTransRol
	WHERE idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion)
BEGIN
	raiserror ('Existen Roles que contienen la transaccion',16,1)
	RETURN
END

if exists (SELECT idTransaccion
	FROM Seguridad.Seg_Autorizacion
	WHERE idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion)
BEGIN
	raiserror ('Existen Autorizaciones que contienen la transaccion',16,1)
	RETURN
END

if exists (SELECT idTransaccion
	FROM Seguridad.Seg_TransUsuario
	WHERE idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion)
BEGIN
	raiserror ('Existen Usuarios que contienen la transaccion',16,1)
	RETURN
END

BEGIN TRAN
--Elimina los horario de las transacciones
     DELETE Seguridad.Seg_HorarioTrans
	WHERE idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion

-- Elimina las opciones de las transacciones
     DELETE Seguridad.Seg_OpcionTrans
          Where idTransaccion = @PV_idTransaccion
                AND IdOrganizacion = @PV_Organizacion
IF @@error <> 0
BEGIN
	ROLLBACK TRAN
	raiserror ('Error en eliminacion de Opciones de la Transaccion',16,1)
	RETURN
END
-- Elimina las transaciones
     DELETE FROM Seguridad.Seg_TRANSACCION
          WHERE idtransaccion=@PV_idTransaccion
                AND idOrganizacion = @PV_Organizacion
IF @@error <> 0
BEGIN
	ROLLBACK TRAN
	raiserror ('Error en eliminacion de la Transaccion',16,1)
    RETURN
END
	 
COMMIT TRAN






