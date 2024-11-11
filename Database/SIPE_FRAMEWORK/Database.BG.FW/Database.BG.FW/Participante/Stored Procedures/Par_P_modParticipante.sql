CREATE  PROCEDURE [Participante].[Par_P_modParticipante]
@PI_docXML as varchar(max),
@PI_docXMLcep as varchar(max)
AS
declare @VL_idXML int
declare @VL_IdTipoIdentificacion varchar(10),@VL_IdParticipante int, @VL_TipoParticipante char,@VL_IdUsuario varchar(50)
declare @VL_Identificacion varchar(100), @VL_TipoPart int, @VL_Opident varchar(50), @Semilla varchar(100)
--Variables utilizadas en inicializacion de empresa
declare @VL_IniEmpresa bit, @VL_IniRolOficina bit, @VL_IdEmpPadre int, @VL_IdEmpModelo int
--Variable que sera utilizada para recuperar el  nivelCta(Contabilidad)
declare @VL_NivelCta varchar(50)
declare @VL_NombreEmpresa varchar(100)
declare @VL_IdEmpresa int, @VL_IdOficina int
declare @VL_IdentContacto varchar(100), @VL_IdPartContacto int
declare @VL_ActPart char(1)--Parametro utilizado para actuallizar parcialmente o total la tabla Participante.Par_Participante
		, @VL_TipoPartRegistro varchar(1)

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

-- Obtengo IdParticipante

Select  @VL_IdTipoIdentificacion = IdTipoIdentificacion,
	@VL_IdParticipante = IdParticipante,
	@VL_Identificacion = Identificacion,
	@VL_TipoParticipante = TipoParticipante,
	@VL_IdUsuario = IdUsuario,
	@VL_TipoPart = TipoPart,
	@VL_Opident = Opident,
	@VL_IdEmpresa = IdEmpresa,
	@VL_ActPart = ActPart,
	@VL_TipoPartRegistro = TipoPartRegistro
FROM OPENXML (@VL_idXML, '/ResultSet/Participante')
	WITH (IdTipoIdentificacion Varchar(10),IdParticipante int,Identificacion varchar(100),TipoParticipante char(1),
		IdUsuario varchar(50),TipoPart int, Opident varchar(50),
		IdEmpresa int, ActPart char(1), TipoPartRegistro varchar(1))

--Pregunto si existe para ser modificado
IF NOT EXISTS(SELECT 1 FROM Participante.Par_Participante WHERE IdParticipante = @VL_IdParticipante  )
BEGIN
	raiserror (52002,16,1,@VL_IdUsuario)
	return
END

--verifica si usuario existe
if exists( SELECT 1 FROM Participante.Par_RegistroCliente WHERE IdUsuario = @VL_IdUsuario
	and IdParticipante <> @VL_IdParticipante)
BEGIN
	raiserror (52000,16,1,@VL_IdUsuario)	
	return
END

-- Valida que Identificacion de participante No exista 
if @VL_Identificacion <>''
begin
	if exists( SELECT 1 FROM Participante.Par_Participante WHERE Identificacion = @VL_Identificacion
		and IdTipoIdentificacion = @VL_IdTipoIdentificacion
		and IdParticipante <> @VL_IdParticipante)
	BEGIN	
		raiserror (52001,16,1,@VL_Identificacion)	
		return
	END
end

--valida que no se repita Opident
if @VL_Opident <>''
Begin
	if exists( SELECT 1 FROM Participante.Par_Participante WHERE Opident = @VL_Opident
		and IdParticipante <> @VL_IdParticipante)
	BEGIN
		raiserror (52004,16,1,@VL_Opident)	
		return
	END
End

--Inicia la transaccion
BEGIN TRAN

set @VL_ActPart = isNull(@VL_ActPart,'')

If @VL_ActPart = 'N' --Actualiza parcialmente la tabla
	Begin
		--Modifica Participante Parcialmente
		UPDATE Participante.Par_participante
		SET IdUsuario = xp.IdUsuario
		FROM   Participante.Par_Participante p, OPENXML (@VL_idXML, '/ResultSet/Participante') WITH (IdParticipante int, IdUsuario varchar(50)) xp
		WHERE p.IdParticipante = xp.IdParticipante
	End
Else
	Begin
		--Modifica Participante Totalmente
		UPDATE Participante.Par_participante
		SET IdTipoIdentificacion = xp.IdTipoIdentificacion, Identificacion = xp.Identificacion, IdUsuario = xp.IdUsuario,
			FechaRegistro = xp.FechaRegistro,
			IdPais = xp.IdPais, IdProvincia = xp.IdProvincia,
			IdCiudad = xp.IdCiudad ,CuentaContable = xp.CuentaContable,
			FechaExpira=xp.FechaExpira, TiempoExpira=xp.TiempoExpira,
			ChequeaEquipo = xp.ChequeaEquipo, Comentario =xp.Comentario,
			Opident = xp.Opident, IdNaturalezaNegocio = xp.IdNaturalezaNegocio, Estado = xp.Estado
		FROM   Participante.Par_Participante p, OPENXML (@VL_idXML, '/ResultSet/Participante') WITH (
					IdParticipante int,
					IdTipoIdentificacion varchar(10),Identificacion varchar(100),
					TipoParticipante char(1),IdUsuario varchar(50),
					Estado varchar(10),FechaRegistro datetime,
					IdPais varchar(10),IdProvincia varchar(10),
					IdCiudad varchar(10),CuentaContable varchar(50),
					FechaExpira datetime,TiempoExpira int,
					Comentario varchar(255),ChequeaEquipo bit,
					Opident varchar(50),IdNaturalezaNegocio varchar(10)) xp
		WHERE p.IdParticipante = xp.IdParticipante
	End
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 


-- Ingresar Registro del Cliente
-- Modificado por adm de cuentas
if not exists(SELECT 1 FROM Participante.Par_RegistroCliente rc,  
		OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH Participante.Par_RegistroCliente rcx
		WHERE rc.IdUsuario = isnull(rcx.IdUsuario,-1) )
begin
	insert into Participante.Par_RegistroCliente(IdUsuario,IdParticipante,RolAsignado,
			IdUsuarioRegistro,FechaRegistro,OrigenRegistro,
			TipoTarjeta,NumeroTarjeta,TipoIdent,
			NumIdent,TipoPlanTrans, TipoCliente,Estado,PregSecreta,
			RespSecreta,FraseSecreta,AdmProducto,IdTipoLogin)
		SELECT IdUsuario,IdParticipante,RolAsignado,
			IdUsuarioRegistro,getdate(),OrigenRegistro,
			TipoTarjeta,NumeroTarjeta,TipoIdent,
			NumIdent,TipoPlanTrans, TipoCliente,Estado,PregSecreta,
			RespSecreta,FraseSecreta,AdmProducto,IdTipoLogin
	FROM   OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH Participante.Par_RegistroCliente
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
END
ELSE
BEGIN
	--Modifica RegistroCliente
	UPDATE .Participante.Par_RegistroCliente
	SET PregSecreta = xp.PregSecreta, RespSecreta = xp.RespSecreta,
		TipoPlanTrans = xp.TipoPlanTrans, FraseSecreta = xp.FraseSecreta,
		AdmProducto = xp.AdmProducto,
		IdTipoLogin = xp.IdTipoLogin
	FROM   Participante.Par_RegistroCliente p, 
	OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH Participante.Par_RegistroCliente xp
	WHERE p.IdUsuario = xp.IdUsuario
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
END

-- Ingresa el valor de la semilla si es diferente de vacio
SELECT @Semilla = isnull(Semilla,'')
FROM   OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH (Semilla varchar(100))
IF len(@Semilla) > 0 
BEGIN
	INSERT Seguridad.Seg_Semilla(IdParticipante,FechaAct,Semilla)
	VALUES(@VL_IdParticipante,getdate(),@Semilla)
END
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

declare @VL_RolAsignado varchar(10), @VL_AdmProducto bit
SELECT @VL_RolAsignado = RolAsignado, @VL_AdmProducto = AdmProducto

FROM  Participante.Par_RegistroCliente WHERE IdUsuario = @VL_IdUsuario


--setea permisos
IF (@VL_TipoPartRegistro='A')
BEGIN
	-- Se la asigna el ro de ServiciosWeb para Aplicaciones
	exec Seguridad.seg_p_setRol @VL_IdUsuario,2
END
ELSE
BEGIN
	exec Seguridad.seg_p_setRol @VL_IdUsuario,3 --Rol de Registro
END
-- SI es Coordinador Rol= 11, Cliente=12, Proveedor=13



-- ELimina todos los medios de contactos de direcciones del PArticipante
delete Participante.Par_MedioContacto
FROM Participante.Par_MedioContacto, OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_E') WITH Participante.Par_MedioContacto xd
WHERE   Participante.Par_MedioContacto.IdParticipante = xd.IdParticipante
	and Participante.Par_MedioContacto.IdDireccion = xd.IdDireccion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

delete Participante.Par_MedioContacto
FROM Participante.Par_MedioContacto, OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_M') WITH Participante.Par_MedioContacto xd
WHERE   Participante.Par_MedioContacto.IdParticipante = xd.IdParticipante
	and Participante.Par_MedioContacto.IdDireccion = xd.IdDireccion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Primero borra los medios de contactos cuando biene por una actualizacion de registros
delete Participante.Par_MedioContacto
FROM Participante.Par_MedioContacto, OPENXML (@VL_idXML, '/ResultSet/MContactos/MContacto_A') WITH Participante.Par_MedioContacto xd
WHERE   Participante.Par_MedioContacto.IdParticipante = xd.IdParticipante
	and Participante.Par_MedioContacto.IdDireccion = xd.IdDireccion
	and Participante.Par_MedioContacto.IdMedioContacto = xd.IdMedioContacto
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END



-- Elimina las que no existen en xml
DELETE Participante.Par_Direccion
FROM Participante.Par_Direccion, OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_E') WITH Participante.Par_Direccion xd
WHERE   Participante.Par_Direccion.IdParticipante = xd.IdParticipante
	and Participante.Par_Direccion.IdDireccion = xd.IdDireccion

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Actualiza las Direcciones
UPDATE Participante.Par_Direccion
SET Direccion = xd.Direccion, NumCasa = xd.NumCasa, CallePrincipal = xd.CallePrincipal,
	CalleTransversal = xd.CalleTransversal, IdPais = xd.IdPais,	IdProvincia = xd.IdProvincia,
	IdCiudad = xd.IdCiudad, IdParroquia = xd.IdParroquia, IdBarrio = xd.IdBarrio,
	HorarioContacto = xd.HorarioContacto, NombreContacto = xd.NombreContacto
FROM  Participante.Par_Direccion d, OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_M') WITH Participante.Par_Direccion xd
WHERE d.IdParticipante = xd.IdParticipante AND d.IdDireccion = xd.IdDireccion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

Insert into Participante.Par_Direccion(IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
		NumCasa,CallePrincipal,CalleTransversal,IdPais,IdProvincia,IdCiudad,
		IdParroquia,IdBarrio,HorarioContacto,NombreContacto)
	SELECT IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
		NumCasa,CallePrincipal,CalleTransversal,IdPais,IdProvincia,IdCiudad,
		IdParroquia,IdBarrio,HorarioContacto,NombreContacto
 FROM   OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_M') WITH Participante.Par_Direccion d
  WHERE IdDireccion not in (Select IdDireccion
		FROM Participante.Par_Direccion 
		WHERE IdParticipante = @VL_IdParticipante )
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Ingresa las Nuevas Direcciones
Insert into Participante.Par_Direccion(IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
		NumCasa,CallePrincipal,CalleTransversal,IdPais,IdProvincia,IdCiudad,
		IdParroquia,IdBarrio,HorarioContacto,NombreContacto)
	SELECT IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
		NumCasa,CallePrincipal,CalleTransversal,IdPais,IdProvincia,IdCiudad,
		IdParroquia,IdBarrio,HorarioContacto,NombreContacto
 FROM   OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_N') WITH Participante.Par_Direccion d
  WHERE IdDireccion not in (Select IdDireccion
		FROM Participante.Par_Direccion 
		WHERE IdParticipante = @VL_IdParticipante )
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--=========================
--Verifica si el participante existe por medio de la identificacion del Contacto
SELECT @VL_IdPartContacto = IdPartContacto, @VL_IdentContacto = Identificacion
FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH (IdPartContacto int, Identificacion varchar(100))

if (@VL_IdPartContacto <> '')  --quiere decir que el contacto si esta ingresado como participante
begin
	--Aqui no se va a permitir cambiar informacion de la tabla Participante.Par_Participante
	if exists( SELECT 1 FROM Participante.Par_Participante WHERE IdParticipante = @VL_IdPartContacto)
	BEGIN
		UPDATE Participante.Par_Persona
		SET Apellido1 = xe.Apellido1,Apellido2 = xe.Apellido2 ,Nombre1 = xe.Nombre1,
			Nombre2 = xe.Nombre2
		FROM   Participante.Par_Persona e, OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Persona xe
		WHERE e.IdParticipante = @VL_IdPartContacto
		IF (@@error <> 0)
		BEGIN
		  ROLLBACK TRAN
		  RETURN
		END
		
		--Verifica si es contacto del participante, si no existe entonces lo relaciona 		
		if not exists( SELECT 1 FROM Participante.Par_Contacto WHERE IdParticipante = @VL_IdParticipante and IdPartContacto = @VL_IdPartContacto)
		Begin
			--se realciona al contacto con Participante Principal
			Insert into Participante.Par_Contacto(IdParticipante,IdPartContacto,IdTipoContacto)
				SELECT IdParticipante,@VL_IdPartContacto,IdTipoContacto
			FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Contacto d		
			IF (@@error <> 0)
			BEGIN
			  ROLLBACK TRAN
			  RETURN
			END									
		End
	END	
end
Else --Indica que el contacto no esta realcionado con el participante
Begin
	If exists( SELECT 1 FROM Participante.Par_Participante WHERE Identificacion = @VL_IdentContacto)
	  -- quiere decir que la identificacion del contacto existe y se debe recuperar 
	  -- el IdParticipante
		Begin
			SELECT @VL_IdPartContacto = IdParticipante  FROM Participante.Par_Participante WHERE Identificacion = @VL_IdentContacto				
					
			--Actualizo Datos en tabla Persona
			UPDATE Participante.Par_Persona
			SET Apellido1 = xe.Apellido1,Apellido2 = xe.Apellido2 ,Nombre1 = xe.Nombre1,
				Nombre2 = xe.Nombre2
			FROM   Participante.Par_Persona e, OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Persona xe
			WHERE e.IdParticipante = @VL_IdPartContacto
			IF (@@error <> 0)
				BEGIN
				  ROLLBACK TRAN
				  RETURN
				END

			-- Relaciona Contacto con Participante Principal
			Insert into Participante.Par_Contacto(IdParticipante,IdPartContacto,IdTipoContacto)
				SELECT IdParticipante,@VL_IdPartContacto,IdTipoContacto
			 FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Contacto d
			 WHERE @VL_IdPartContacto not in (Select IdPartContacto from Participante.Par_Contacto
							where IdParticipante = @VL_IdParticipante)
			IF (@@error <> 0)
				BEGIN
				  ROLLBACK TRAN
				  RETURN
				END 
		End
	Else -- se lo ingresa como nuevo
		Begin			
			--Se crea al Contacto como Participante
			INSERT INTO Participante.Par_Participante(TipoParticipante,IdTipoIdentificacion,Identificacion,Estado,FechaRegistro,IdUsuario)
				SELECT TipoParticipante,IdTipoIdentificacion,Identificacion,Estado,getdate(),IdUsuario
			FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Participante
			IF (@@error <> 0)
			BEGIN
			  ROLLBACK TRAN
			  RETURN
			END
			--Recupera IdParticipante del contacto que se esta ingresando
			SET @VL_IdPartContacto = @@IDENTITY

			-- Contacto se crea como una persona
			INSERT INTO Participante.Par_Persona(IdParticipante,Apellido1,Apellido2,Nombre1,Nombre2)
				SELECT @VL_IdPartContacto, Apellido1,Apellido2,Nombre1,Nombre2
			FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Persona
			IF (@@error <> 0)
			BEGIN
			  ROLLBACK TRAN
			  RETURN
			END 

			--Relacionar contacto con Participante principal
			Insert into Participante.Par_Contacto(IdParticipante,IdPartContacto,IdTipoContacto)
				SELECT IdParticipante,@VL_IdPartContacto,IdTipoContacto
			 FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Contacto d			 
			IF (@@error <> 0)
			BEGIN
			  ROLLBACK TRAN
			  RETURN
			END 
		 
		End
End

--Actualiza Participantes Contactos
UPDATE Participante.Par_Contacto
SET IdTipoContacto = xc.IdTipoContacto
FROM  Participante.Par_Contacto c, OPENXML (@VL_idXML, '/ResultSet/Contactos/Contacto_M') WITH Participante.Par_Contacto xc
WHERE c.IdParticipante = xc.IdParticipante 
	AND c.IdPartContacto = xc.IdPartContacto
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Elimina Contactos
DELETE Participante.Par_Contacto
FROM Participante.Par_Contacto,  OPENXML (@VL_idXML, '/ResultSet/Contactos/Contacto_E') WITH Participante.Par_Contacto xc
WHERE Participante.Par_Contacto.IdParticipante = @VL_IdParticipante 
	AND Participante.Par_Contacto.IdPartContacto = xc.IdPartContacto
 
-- Ingresa los Contactos
Insert into Participante.Par_Contacto(IdParticipante,IdPartContacto,IdTipoContacto)
	SELECT IdParticipante,IdPartContacto,IdTipoContacto
 FROM   OPENXML (@VL_idXML, '/ResultSet/Contactos/Contacto_N') WITH Participante.Par_Contacto d
 WHERE IdPartContacto not in (Select IdPartContacto from Participante.Par_Contacto
				where IdParticipante = @VL_IdParticipante)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 


--Ingresa Medio de Contacto de Direcciones
Insert into Participante.Par_MedioContacto(IdParticipante,IdDireccion,IdMedioContacto,
		IdTipoMedioContacto,Valor,ValorAlt)
	SELECT @VL_IdParticipante,x.IdDireccion,x.IdMedioContacto,
		x.IdTipoMedioContacto,x.Valor,x.ValorAlt
FROM   OPENXML (@VL_idXML, '/ResultSet/MContactos/MContacto') WITH Participante.Par_MedioContacto x
WHERE IdMedioContacto not in (Select IdMedioContacto from Participante.Par_MedioContacto
				where IdParticipante = @VL_IdParticipante
						and IdDireccion = x.IdDireccion)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

--Ingresa Medio de Contacto de Direcciones
Insert into Participante.Par_MedioContacto(IdParticipante,IdDireccion,IdMedioContacto,
		IdTipoMedioContacto,Valor,ValorAlt)
	SELECT @VL_IdParticipante,IdDireccion,IdMedioContacto,
		IdTipoMedioContacto,Valor,ValorAlt
FROM   OPENXML (@VL_idXML, '/ResultSet/MContactos/MContacto_A') WITH Participante.Par_MedioContacto
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Elimina las referencias bancarias que no existen en xml
DELETE Participante.Par_CuentaParticipante
FROM Participante.Par_CuentaParticipante, OPENXML (@VL_idXML, '/ResultSet/Cuentas/Cuenta_E') WITH Participante.Par_CuentaParticipante xc
WHERE Participante.Par_CuentaParticipante.IdParticipante = @VL_IdParticipante 
	AND Participante.Par_CuentaParticipante.Cuenta = xc.Cuenta

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
	
-- Actualizar referencias bancarias
UPDATE Participante.Par_CuentaParticipante
SET IdTipoCuenta = xd.IdTipoCuenta, IdBanco = xd.IdBanco, 
OficialCuenta = xd.OficialCuenta, Telefono = xd.Telefono,
DescCuentaBanco = xd.DescCuentaBanco
FROM  Participante.Par_CuentaParticipante d, OPENXML (@VL_idXML, '/ResultSet/Cuentas/Cuenta_M') WITH Participante.Par_CuentaParticipante xd
WHERE d.IdParticipante = xd.IdParticipante AND d.Cuenta = xd.Cuenta
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END
--Ingreso de referencias bancarias
Insert into Participante.Par_CuentaParticipante(IdParticipante,Cuenta,IdTipoCuenta,IdBanco,
		OficialCuenta,Telefono,DescCuentaBanco)
	SELECT IdParticipante,Cuenta,IdTipoCuenta,IdBanco,
		OficialCuenta,Telefono,DescCuentaBanco
 FROM   OPENXML (@VL_idXML, '/ResultSet/Cuentas/Cuenta_N') WITH Participante.Par_CuentaParticipante d
  WHERE Cuenta not in (Select Cuenta
		FROM Participante.Par_CuentaParticipante 
		WHERE IdParticipante = @VL_IdParticipante )
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- ELimina todos los parametros del PArticipante
DELETE Participante.Par_ParametroParticipante
	FROM Participante.Par_ParametroParticipante p, OPENXML (@VL_idXML, '/ResultSet/Parametros/Parametro_E') WITH Participante.Par_ParametroParticipante xp
	WHERE p.IdParticipante = @VL_IdParticipante 
		AND p.IdParametro = xp.IdParametro						
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END
--Actualiza Parametros
UPDATE Participante.Par_ParametroParticipante
SET IdTipoParametro = xp.IdTipoParametro, Valor = xp.Valor
FROM  Participante.Par_ParametroParticipante p, OPENXML (@VL_idXML, '/ResultSet/Parametros/Parametro_M') WITH Participante.Par_ParametroParticipante xp
WHERE p.IdParticipante = @VL_IdParticipante 
		AND p.IdParametro = xp.IdParametro						
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Ingresa Parametros del participante
Insert into Participante.Par_ParametroParticipante(IdParticipante,IdParametro,IdTipoParametro,Valor)
	SELECT @VL_IdParticipante,IdParametro,IdTipoParametro,Valor
 FROM   OPENXML (@VL_idXML, '/ResultSet/Parametros/Parametro_N') WITH Participante.Par_ParametroParticipante
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Dependiendo del Tipo de Participante se modifica Persona o Empresa
IF @VL_TipoParticipante = 'P' --Persona
BEGIN
	UPDATE Participante.Par_Persona
	SET Apellido1 = xe.Apellido1,Apellido2 = xe.Apellido2 ,Nombre1 = xe.Nombre1,
		Nombre2 = xe.Nombre2,IdTitulo = xe.IdTitulo,
		Sexo = xe.Sexo,FechaNacimiento=xe.FechaNacimiento,
		EstadoCivil=xe.EstadoCivil, Ruc = xe.Ruc, IdEmpRel = xe.IdEmpRel
	FROM   Participante.Par_Persona e, OPENXML (@VL_idXML, '/ResultSet/Persona') WITH Participante.Par_Persona xe
	WHERE e.IdParticipante = xe.IdParticipante
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
END
ELSE --Empresa
BEGIN
	SELECT  @VL_NombreEmpresa = Nombre,
			@VL_IdEmpPadre = IdEmpresaPadre,			
			@VL_IniRolOficina = IniRolOficina
	FROM OPENXML (@VL_idXML, '/ResultSet/Empresa')
			WITH (Nombre varchar(100),IdEmpresaPadre int, TipoEmpresa char(1), IniRolOficina int)

	if exists( SELECT 1 FROM Participante.Par_Empresa WHERE Nombre = @VL_NombreEmpresa
			and IdParticipante <> @VL_IdParticipante)
	BEGIN	
		ROLLBACK TRAN
		RAISERROR(52005,16,1,@VL_NombreEmpresa)	
		RETURN
	END
	
	--Modifica Empresa
	UPDATE Participante.Par_Empresa
	SET Nombre = xe.Nombre,IdCategoriaEmpresa = xe.IdCategoriaEmpresa ,Nivel = xe.Nivel,
		IdEmpresaPadre = xe.IdEmpresaPadre,
		Licencia = xe.Licencia, Marca = xe.Marca, NumeroPatronal = xe.NumeroPatronal,
		IdRazonSocial = xe.IdRazonSocial
	FROM   Participante.Par_Empresa e, OPENXML (@VL_idXML, '/ResultSet/Empresa') WITH Participante.Par_Empresa xe
	WHERE e.IdParticipante = xe.IdParticipante
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END	
	
	--Elimina lotes de Hacienda
	DELETE Participante.Par_LoteHac
	FROM Participante.Par_LoteHac lo, OPENXML (@VL_idXML, '/ResultSet/LotesHac/Lote_E') WITH Participante.Par_LoteHac xl
	WHERE lo.IdEmpresa = @VL_IdParticipante 
		AND lo.IdLote = xl.IdLote
						
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END
	
	--Modifica lotes de Hacienda
	UPDATE Participante.Par_LoteHac
	SET Tamaño = xl.Tamaño,IdTipoCultivo = xl.IdTipoCultivo, Copropietario = xl.Copropietario
	FROM  Participante.Par_LoteHac l, OPENXML (@VL_idXML, '/ResultSet/LotesHac/Lote_M') WITH Participante.Par_LoteHac xl
	WHERE l.IdEmpresa = xl.IdEmpresa AND l.IdLote = xl.IdLote
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END
	--Insertar Lotes de Hacienda
	INSERT INTO Participante.Par_LoteHac(IdEmpresa,IdLote,Tamaño,IdTipoCultivo,Copropietario)
		SELECT IdEmpresa,IdLote,Tamaño,IdTipoCultivo,Copropietario
	FROM   OPENXML (@VL_idXML, '/ResultSet/LotesHac/Lote_N') WITH Participante.Par_LoteHac
	WHERE IdLote not in (Select IdLote
		FROM Participante.Par_LoteHac 
		WHERE IdEmpresa = @VL_IdParticipante )
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
	
END

if @VL_TipoPart = 1
-- Modifica Clientes
BEGIN
	exec Participante.Par_P_modCliente @PI_docXMLcep
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END		
END
if @VL_TipoPart = 2
-- Modifica Empleados
BEGIN
	exec Participante.Par_P_modEmpleado @PI_docXMLcep, @VL_IdUsuario 
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END	
END
if @VL_TipoPart = 3
-- Modifica Proveedores
BEGIN
	exec Participante.Par_P_modProveedor @PI_docXMLcep
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END
	
END

-- Si es Proveedor se le asigna el rol si tiene usuario valido
IF EXISTS(SELECT 1 FROM Participante.Par_Proveedor 
				WHERE IdParticipante = @VL_IdParticipante) 
	IF len(isnull(@VL_IdUsuario,'')) > 0  
		exec Seguridad.seg_p_setRol @VL_IdUsuario,13 --Rol de proveedor
-- aumentar para el caso de contacto usarios

/*========================================*/
--Inicializa Parametros de Empresa
/*========================================*/
IF @VL_TipoPart = 4
BEGIN
	--Se obtiene el nombre de la empresa
	SELECT 	@VL_NombreEmpresa = Nombre,
	       	@VL_IniEmpresa = IniEmpresa,
		@VL_IdEmpModelo = IdEmpModelo,
		@VL_NivelCta = NivelCta
		FROM   OPENXML (@VL_idXML, '/ResultSet/Empresa') 
				WITH  (Nombre varchar(100),IniEmpresa bit,IdEmpModelo int,NivelCta int,IniRolOficina bit)

	--Elimina Zonas de Empresa
	DELETE Catalogo.Ctl_CatalogoEmpresa
	WHERE IdEmpresa = @VL_IdParticipante and IdTabla = 1 --Zonas
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END

	--Isertar Zonas de Empresa en Catalogos.Ctl_CatalogoEmpresa
	INSERT INTO Catalogo.Ctl_CatalogoEmpresa(IdEmpresa,IdTabla,Codigo)
		SELECT @VL_IdParticipante,IdTabla,Codigo
	FROM   OPENXML (@VL_idXML, '/ResultSet/Zonas/Zona') WITH Catalogo.Ctl_CatalogoEmpresa
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
	
	--Inicializar Parametros de Empresa
	if @VL_IniEmpresa = 1  
	   begin
		
		/*===============================================*/
		--Inicializa parametros de empresa en BD Participante
		/*===============================================*/
		exec Participante.Par_P_InicializarModuloNuevaEmpresa @VL_IdParticipante
		IF (@@error <> 0)
		BEGIN
		  ROLLBACK TRAN
		  RETURN
		END		
	   end
	
END



COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML








