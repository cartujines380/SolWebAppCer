create procedure [Seguridad].[Seg_P_REGISTRA_AUDITORIA]
@PV_idUsuario              VARCHAR(20),
@PV_idAplicacion           INT,
@PV_maquina                VARCHAR(100),
@PV_txtTrans               VARCHAR(max),
--@PV_acttoken               char,
@PV_IdOrganizacion int,
@PV_IdTransaccion int,
@PV_Estado char = 'S'
AS

    INSERT INTO Seguridad.Seg_AUDITORIA(IdUsuario,IdAplicacion,FechaMovi,
				IdIdentificacion,txtTransaccion, IdOrganizacion,IdTransaccion,Estado)
    VALUES(@PV_idUsuario,@PV_idAplicacion,getdate(),@PV_maquina,@PV_txtTrans,
			@PV_IdOrganizacion,@PV_IdTransaccion,@PV_Estado)
    
 /*   if @PV_acttoken = 'S' 
       exec Seguridad.Seg_P_ACTUALIZA_TOKEN @PV_idUsuario,0,@PV_maquina,@PV_idAplicacion

 */





