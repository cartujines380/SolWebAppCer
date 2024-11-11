
CREATE   procedure [Participante].[Par_P_ConsIdent]
@PI_IdUsuario varchar(50),
@PO_TipoIdent char(1) output,
@PO_Ident varchar(15) output,
@PO_Frase varchar(200) output,
@PO_Nombre varchar(100) output,
@PO_IdParticipante int output,
@PO_IdEmpresaExterna int output,
@PO_IdTipoEmpleado int output,
@PO_Administrador char output,
@PO_Cargo varchar(10) output,
@PO_Correo varchar(100) output

AS
DECLARE @RolAsignado varchar(10), @IdPartDueno int
	SELECT @PO_TipoIdent = p.IdTipoIdentificacion, @PO_Ident = p.Identificacion, 
		@PO_Frase = r.FraseSecreta,
		@PO_Nombre = Participante.Par_F_getNombreParticipante(p.IdParticipante),
		@PO_IdParticipante = p.IdParticipante,
		@RolAsignado = r.RolAsignado
	FROM Participante.Par_Participante p, Participante.Par_RegistroCliente r
	WHERE p.idUsuario = @PI_IdUsuario
	AND p.IdParticipante = r.IdParticipante
	
	-- Recupera id si tiene empresa padre
	SELECT @PO_IdTipoEmpleado = IdTipoEmpleado, 
		   @PO_IdEmpresaExterna = IdEmpresaPertenece,
		   @PO_Cargo = IdCargo
	FROM Participante.Par_Empleado
	WHERE IdParticipante = @PO_IdParticipante
	-- Pregunta si tiene el rol de administrador
	IF EXISTS(SELECT 1 FROM Seguridad.Seg_RolUsuario
				WHERE IdRol = 1 AND IDUsuario = @PI_IdUsuario)
		SET @PO_Administrador = 'S'
	ELSE
		SET @PO_Administrador = 'N'

	-- Pregunta si es un usuario delegado para traer el correo del dueño de la cuenta
	IF @RolAsignado = 1 -- Es el dueño de la cuenta
		SELECT @PO_Correo = valor
		FROM Participante.Par_F_MCParticipante(@PO_IdParticipante,1,3)
	ELSE -- es delegado
	BEGIN
		SELECT @IdPartDueno = r2.IdParticipante,
			   @PO_Nombre = Participante.Par_F_getNombreParticipante(r2.IdParticipante)
		FROM Par_RegistroCliente r1 INNER JOIN Par_RegistroCliente r2
				ON r1.IdUsuarioRegistro = r2.IdUsuario
		WHERE r1.IdParticipante = @PO_IdParticipante
		SELECT @PO_Correo = valor
		FROM Participante.Par_F_MCParticipante(@IdPartDueno,1,3)
	END


