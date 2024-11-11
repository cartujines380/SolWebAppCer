
CREATE PROCEDURE [Seguridad].[Seg_P_DESREGISTRA_USUARIO]
@PV_idUsuario              varchar(20),
@PV_token                  varchar(32),
@PV_maquina                varchar(100)
AS
      -- Se le cambia el estado para desregistrarlo
        UPDATE Seguridad.Seg_REGISTRO
          SET Estado = 'I', FechaSalida = getdate()
          WHERE IdUsuario = @PV_idUsuario 
		and Estado = 'A'
		and IdIdentificacion = @PV_maquina
		and Token = @PV_token
 






