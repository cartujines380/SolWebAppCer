
CREATE  procedure [Seguridad].[Seg_P_VerificaUser]
@PI_IdUsuario              varchar(20),
@PI_Token                  varchar(32),
@PI_Maquina                varchar(100),
@PI_IdAplicacion                int,
@PO_Estado				 bit output
AS
  SET NOCOUNT ON
  DECLARE @IdUsuariojefe varchar(50), @IdParticipante int , 
  @Identificacion varchar(100), @NombreParticipante varchar(100),
  @Roles varchar(max)
-- Recupera Informacion del Usuario.. quien es su Lider
SELECT @IdUsuariojefe = rcj.IdUsuario
FROM Participante.Par_RegistroCliente rc INNER JOIN Participante.Par_Empleado e
		ON rc.IdParticipante = e.IdParticipante
		INNER JOIN Participante.Par_Empleado ej
		ON ej.IdOrganigrama = e.IdOrganigrama AND ej.IdCargo = 22 -- lider
		INNER JOIN Participante.Par_RegistroCliente rcj
		ON rcj.IdParticipante = ej.IdParticipante
WHERE rc.IdUsuario = @PI_IdUsuario
-- Recupera Informacion del Participante:
-- Recupera Informacion del Usuario
		 SELECT @IdParticipante = p.IdParticipante,
		        @Identificacion = p.Identificacion,
			    @NombreParticipante = Participante.Par_F_getNombreParticipante(p.IdParticipante) 			
		FROM	Participante.par_RegistroCliente r,
				Participante.par_participante p
		  WHERE r.IdUsuario = @PI_IdUsuario
			and r.IdParticipante = p.IdParticipante
			
-- Recupera informacion de los Roles
	SELECT @Roles = CONVERT( VARCHAR(max), (
		SELECT IdRol
			FROM Seguridad.Seg_RolUsuario
			WHERE IdUsuario = @PI_IdUsuario
			FOR XML RAW('Rol'), ROOT('Roles')
		))

  	-- Se va a controlar que si el usuario esta registrado, 
	IF EXISTS ( SELECT 1 FROM Seguridad.Seg_Registro 
				WHERE IdUsuario = @PI_IdUsuario and Estado = 'A'
					AND Token = @PI_Token AND IdIdentificacion = @PI_Maquina )
	BEGIN
		SET @PO_Estado = 1 -- Si esta registrado
		-- Retorna los parametros de conexion si tiene RetParamClie=S
		IF EXISTS(SELECT 1 FROM Seguridad.Seg_ParamAplicacion
					WHERE IdAplicacion = @PI_IdAplicacion 
							AND Parametro = 'RetParamClie' AND Valor = 'S')
		BEGIN
			SELECT Parametro, Valor, Encriptado
			FROM Seguridad.Seg_ParamAplicacion		
			WHERE IdAplicacion = @PI_IdAplicacion
			UNION
			SELECT 'IdUsuarioJefe',@IdUsuarioJefe,0	
			UNION
			SELECT 'Identificacion',@Identificacion,0	
			UNION
			SELECT 'IdParticipante',convert(varchar,@IdParticipante),0	
			UNION
			SELECT 'NombreParticipante',@NombreParticipante,0	
			UNION
			SELECT 'Roles',@Roles,0
			
		END
		ELSE
		BEGIN
			SELECT Parametro, Valor, Encriptado
			FROM Seguridad.Seg_ParamAplicacion		
			WHERE IdAplicacion = @PI_IdAplicacion AND Parametro = 'RetServClie'
			UNION
			SELECT 'IdUsuarioJefe',@IdUsuariojefe,0
			UNION
			SELECT 'Identificacion',@Identificacion,0	
			UNION
			SELECT 'IdParticipante',convert(varchar,@IdParticipante),0	
			UNION
			SELECT 'NombreParticipante',@NombreParticipante,0
			UNION
			SELECT 'Roles',@Roles,0
		END
	END
	ELSE
		SET @PO_Estado = 0 -- No esta registrado

	


