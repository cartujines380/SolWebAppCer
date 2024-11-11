CREATE  PROCEDURE [Participante].[Par_P_ingParticipante]
@PI_docXML varchar(max),
@PI_docXMLcep varchar(max),
@PO_IdParticipante int output
AS

-- CAMBIO
SET @PI_docXML = '<?xml version="1.0" encoding="iso-8859-1" ?>'+@PI_docXML

declare @VL_idXML int, @VL_idXMLcep int
declare @VL_IdTipoIdentificacion varchar(10),@VL_Identificacion varchar(100), @VL_TipoParticipante char
declare @VL_IdUsuario varchar(50), @VL_TipoPart int, @VL_Opident varchar(50), @Semilla varchar(100)
--Variables utilizadas en inicializacion de empresa
declare @VL_IniEmpresa bit, @VL_IniRolOficina bit, @VL_IdEmpPadre int, @VL_IdEmpModelo int
--Variable que sera utilizada para recuperar el  nivelCta(Contabilidad)
declare @VL_NivelCta varchar(50)
declare @VL_NombreEmpresa varchar(100)
declare @VL_Error int
declare @VL_IdEmpresa int, @VL_IdOficina int
declare @VL_IdentContacto varchar(100), @VL_IdPartContacto int
declare @VL_Cargo varchar(10), @VL_TipoPartRegistro varchar(1)
declare @VL_Clave varchar(300)

--set @PI_docXML=REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@PI_docXML,'Ñ','N'),'ñ','n'),'Á','A'),'É','E'),'Í','I'),'Ó','O'),'Ú','U'),'á','a'),'é','e'),'í','i'),'ó','o'),'ú','u')
--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML
exec sp_xml_preparedocument @VL_idXMLcep OUTPUT, @PI_docXMLcep
print 'region 1'
-- Obtengo IdParticipante
Select  @VL_IdTipoIdentificacion = IdTipoIdentificacion,
	@VL_Identificacion = Identificacion,
	@VL_TipoParticipante = TipoParticipante,
	@VL_IdUsuario = IdUsuario,
	@VL_TipoPart = TipoPart,
	@VL_Opident = Opident,
	@VL_IdEmpresa = IdEmpresa,
	@VL_TipoPartRegistro = TipoPartRegistro,
	@VL_Clave = Clave
FROM OPENXML (@VL_idXML, '/ResultSet/Participante')
	WITH (IdTipoIdentificacion Varchar(10),Identificacion varchar(100),TipoParticipante char(1),
			IdUsuario varchar(50),TipoPart int, Opident varchar(50),
			IdEmpresa int, TipoPartRegistro varchar(1), Clave varchar(300))
print 'region 2'
-- Valida que Identificacion de participante No exista 
-- Modificado por Administracion de Cuentas
if @VL_Identificacion <>''
begin
	if (LEN(@VL_Opident)<>13)
	BEGIN
		if exists( SELECT 1 FROM Participante.Par_Participante p
						WHERE p.Identificacion = @VL_Identificacion
							and p.IdTipoIdentificacion = @VL_IdTipoIdentificacion)
					
		BEGIN	
			raiserror ('Identificación ya esta registrada (%s)',16,1,@VL_Identificacion)	
			return
		END
	END
	--ELSE
	--BEGIN
	--	if exists( SELECT 1 FROM Participante.Par_Participante p
	--					WHERE p.Identificacion = @VL_Identificacion
	--						and p.IdTipoIdentificacion = @VL_IdTipoIdentificacion
	--						and p.Opident = @VL_Opident)
					
	--	BEGIN	
	--		raiserror ('Identificación ya esta registrada para este proveedor (%s)',16,1,@VL_Identificacion)	
	--		return
	--	END
	--END
--Esto esta pendiente	
	/*if exists( SELECT 1 FROM Participante.Par_Participante p, Participante.Par_RegistroCliente r 
					WHERE p.Identificacion = @VL_Identificacion 
					AND p.IdParticipante = r.IdParticipante
					AND r.RolAsignado <> 1 ) -- Que no este registrado como Propietario
	BEGIN	
		raiserror (52001,16,1,@VL_Identificacion)	
		return
	END
	*/
end
--verifica si usuario existe
if exists( SELECT 1 FROM Participante.Par_RegistroCliente WHERE IdUsuario = @VL_IdUsuario)
BEGIN
	raiserror ('Usuario ya esta registrado (%s)',16,1,@VL_IdUsuario)	
	return
END

----valida que no se repita Opident
--if @VL_Opident <>''
--Begin
--	if exists( SELECT 1 FROM Participante.Par_Participante WHERE Opident = @VL_Opident)
--	BEGIN
--		raiserror ('Opident ya esta registrado (%s)',16,1,@VL_Opident)	
--		return
--	END
--End
--Inicia la transaccion
BEGIN TRAN

--Ingresa Participante
insert into Participante.Par_participante(IdTipoIdentificacion,Identificacion,
		TipoParticipante,IdUsuario,IdNaturalezaNegocio,Estado,
		FechaRegistro,IdPais, IdProvincia,IdCiudad,CuentaContable,
		FechaExpira,TiempoExpira,ChequeaEquipo,Comentario,Opident,TipoPartRegistro, Clave)
	SELECT IdTipoIdentificacion, Identificacion, TipoParticipante,
		IdUsuario,isnull(IdNaturalezaNegocio,1),Estado, getdate(),
		IdPais, IdProvincia,IdCiudad,CuentaContable,
		FechaExpira,TiempoExpira,ChequeaEquipo,Comentario,Opident,TipoPartRegistro, Clave
FROM   OPENXML (@VL_idXML, '/ResultSet/Participante') WITH Participante.Par_participante
print 'region 4'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
--Recupera IdParticipante
SET @PO_IdParticipante = @@IDENTITY

-- Ingresar Registro del Cliente
insert into Participante.Par_RegistroCliente(IdUsuario,IdParticipante,RolAsignado,IdUsuarioRegistro,
		FechaRegistro,OrigenRegistro,
		TipoTarjeta,NumeroTarjeta,TipoIdent,
		NumIdent,TipoPlanTrans, TipoCliente,Estado,PregSecreta,
		RespSecreta,FraseSecreta,AdmProducto,IdTipoLogin)
	SELECT IdUsuario,@PO_IdParticipante,RolAsignado,IdUsuarioRegistro,
		getdate(),OrigenRegistro,
		TipoTarjeta,NumeroTarjeta,TipoIdent,
		NumIdent,TipoPlanTrans, TipoCliente,Estado,PregSecreta,
		RespSecreta,FraseSecreta,AdmProducto,IdTipoLogin
FROM   OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH Participante.Par_RegistroCliente
print 'region 5'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Ingresa el valor de la semilla si es diferente de vacio
SELECT @Semilla = isnull(Semilla,'')
FROM   OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH (Semilla varchar(100))
IF len(@Semilla) > 0 
BEGIN
	INSERT Seguridad.Seg_Semilla(IdParticipante,FechaAct,Semilla)
	VALUES(@PO_IdParticipante,getdate(),@Semilla)
END
print 'region 6'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

IF (@VL_TipoPartRegistro='A')
BEGIN
	-- Se la asigna el ro de ServiciosWeb para Aplicaciones
	exec Seguridad.seg_p_setRol @VL_IdUsuario,2
END
ELSE
BEGIN
	IF (@VL_TipoPartRegistro='C')
	BEGIN
	-- ROL DE CLIENTE PSD EN EL CASO DEL REGISTRO DE PERSONAS EN AD EXTERNO PSD
	exec Seguridad.seg_p_setRol @VL_IdUsuario,13
	END
	-- Se la asigna el ro de Registro
	exec Seguridad.seg_p_setRol @VL_IdUsuario,3 --Rol de Registro
	--Agrego valores por defecto para el control de Claves
	Insert into Seguridad.Seg_Clave(IdParticipante, EsClaveNuevo, EsClaveCambio, EsClaveBloqueo,
									FechaUltModClave, FechaUltClaveErr, NumIntentosClaveErr, ImagenSecreta)
		values (@PO_IdParticipante,'S', 'N', 'N', GETDATE(), '1900-01-01', 0, '')
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
END

declare @VL_RolAsignado varchar(10), @VL_AdmProducto bit
SELECT @VL_RolAsignado = RolAsignado, @VL_AdmProducto = AdmProducto
FROM  OPENXML (@VL_idXML, '/ResultSet/RegistroCliente') WITH Participante.Par_RegistroCliente
print 'region 7'

--Ingresa Direcciones
Insert into Participante.Par_Direccion(IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
		NumCasa,CallePrincipal,CalleTransversal,IdPais,IdProvincia,IdCiudad,
		IdParroquia,IdBarrio,HorarioContacto,NombreContacto)
	SELECT @PO_IdParticipante,IdDireccion,IdTipoDireccion,Direccion,
		NumCasa,CallePrincipal,CalleTransversal,IdPais,IdProvincia,IdCiudad,
		IdParroquia,IdBarrio,HorarioContacto,NombreContacto
 FROM   OPENXML (@VL_idXML, '/ResultSet/Direcciones/Direccion_N') WITH Participante.Par_Direccion
 print 'region 7'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

--Ingresa Medio de Contacto de Direcciones
Insert into Participante.Par_MedioContacto(IdParticipante,IdDireccion,IdMedioContacto,
		IdTipoMedioContacto,Valor,ValorAlt)
	SELECT @PO_IdParticipante,IdDireccion,IdMedioContacto,
		IdTipoMedioContacto,Valor,ValorAlt
FROM   OPENXML (@VL_idXML, '/ResultSet/MContactos/MContacto') WITH Participante.Par_MedioContacto
print 'region 8'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
--================================
--Ingresa Contacto como nuevo participante,  en esta seccion solo ingresa uno
--================================
SELECT @VL_IdentContacto = Identificacion
FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Participante
print 'region 9'
If exists( SELECT 1 FROM Participante.Par_Participante WHERE Identificacion = @VL_IdentContacto)
  -- quiere decir que la identificacion del contacto existe y se debe recuperar 
  -- el IdParticipante
	begin
		SELECT @VL_IdPartContacto = IdParticipante  FROM Participante.Par_Participante WHERE Identificacion = @VL_IdentContacto				
	end
Else -- se lo ingresa como nuevo
	begin
		
		--Se crea al Contacto como Participante
		INSERT INTO Participante.Par_Participante(TipoParticipante,IdTipoIdentificacion,Identificacion,Estado,FechaRegistro)
			SELECT TipoParticipante,IdTipoIdentificacion,Identificacion,Estado,getdate()
		FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Participante
		print 'region 10'
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
		print 'region 11'
		IF (@@error <> 0)
		BEGIN
		  ROLLBACK TRAN
		  RETURN
		END 		 		
	end

--Ingreso Contacto,  en este ejemplo solo
Insert into Participante.Par_Contacto(IdParticipante,IdPartContacto,IdTipoContacto)
	SELECT @PO_IdParticipante,@VL_IdPartContacto,IdTipoContacto
FROM   OPENXML (@VL_idXML, '/ResultSet/ParticipanteContacto/Contacto_N') WITH Participante.Par_Contacto
print 'region 12'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END

--Ingresa Contactos que ya existen ingresados como participantes

Insert into Participante.Par_Contacto(IdParticipante,IdPartContacto,IdTipoContacto)
	SELECT @PO_IdParticipante,IdPartContacto,IdTipoContacto
 FROM   OPENXML (@VL_idXML, '/ResultSet/Contactos/Contacto_N') WITH Participante.Par_Contacto
 print 'region 13'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Ingresar Cuentas del Participante
Insert into Participante.Par_CuentaParticipante(IdParticipante,Cuenta,IdTipoCuenta,IdBanco,
		OficialCuenta,Telefono,DescCuentaBanco)
	SELECT @PO_IdParticipante,Cuenta,IdTipoCuenta,IdBanco,
		OficialCuenta,Telefono,DescCuentaBanco
FROM   OPENXML (@VL_idXML, '/ResultSet/Cuentas/Cuenta_N') WITH Participante.Par_CuentaParticipante
print 'region 14'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

--Ingresa Parametros del participante
Insert into Participante.Par_ParametroParticipante(IdParticipante,IdParametro,IdTipoParametro,Valor)
	SELECT @PO_IdParticipante,IdParametro,IdTipoParametro,Valor
FROM   OPENXML (@VL_idXML, '/ResultSet/Parametros/Parametro_N') WITH Participante.Par_ParametroParticipante
print 'region 16'
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- Dependiendo del Tipo de Participante se ingresa Persona o Empresa
IF @VL_TipoParticipante = 'P'
BEGIN
	insert into Participante.Par_Persona(IdParticipante,Apellido1,Apellido2,Nombre1,
		Nombre2,IdTitulo,Sexo,FechaNacimiento,EstadoCivil, Ruc, IdEmpRel)
		SELECT @PO_IdParticipante, Apellido1,Apellido2,Nombre1,
		Nombre2,IdTitulo,Sexo,FechaNacimiento,EstadoCivil, Ruc,IdEmpRel
	FROM   OPENXML (@VL_idXML, '/ResultSet/Persona') WITH Participante.Par_Persona
	print 'region 17'
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
END
ELSE --Tipo de Participante es Empresa
BEGIN
	SELECT  @VL_NombreEmpresa = Nombre, @VL_IdEmpPadre = IdEmpresaPadre,			
			@VL_IniRolOficina = IniRolOficina
	FROM OPENXML (@VL_idXML, '/ResultSet/Empresa')	
			WITH (Nombre varchar(100),IdEmpresaPadre int, TipoEmpresa char(1), IniRolOficina int)
print 'region 18'
	if exists( SELECT 1 FROM Participante.Par_Empresa WHERE Nombre = @VL_NombreEmpresa
		and IdParticipante <> @PO_IdParticipante)
	BEGIN	
		ROLLBACK TRAN
		RAISERROR('Nombre de la Empresa ya existe (%s)',16,1,@VL_NombreEmpresa)	
		RETURN
	END

	--Ingrso de Empresa	
	insert into Participante.Par_Empresa(IdParticipante,Nombre,IdCategoriaEmpresa,Nivel,
		IdEmpresaPadre,Licencia,Marca,NumeroPatronal,
		IdRazonSocial)
		SELECT @PO_IdParticipante,Nombre,IdCategoriaEmpresa,Nivel,
		IdEmpresaPadre,Licencia,Marca,NumeroPatronal,
		IdRazonSocial
	FROM   OPENXML (@VL_idXML, '/ResultSet/Empresa') WITH Participante.Par_Empresa
	IF (@@error <> 0)
	BEGIN
		ROLLBACK TRAN
		RETURN
	END 				
	
	
	--Insertar Organigramas default para una empresa
  	INSERT INTO Participante.par_organigrama(IdOrganigrama,IdEmpresa,Descripcion,IdOrganigramaPadre,Nivel)
		SELECT 1,@PO_IdParticipante,'Empleado',0,2
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 	 
	
END

-- Ingresa Clientes
IF @VL_TipoPart = 1 
BEGIN
	insert into Participante.Par_cliente(IdParticipante,IdEmpresa,IdOficina,PorcentajeDescuento,
		IdVendedor,ContribuyenteEspecial,IdCalificacion,Iva,Estado,GastoAnual)
		SELECT @PO_IdParticipante,IdEmpresa,IdOficina,PorcentajeDescuento,
		IdVendedor,ContribuyenteEspecial,IdCalificacion,Iva,Estado,GastoAnual
	FROM   OPENXML (@VL_idXMLcep, '/Participante/Clientes/Cliente_N') WITH Participante.Par_Cliente
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END
	-- Asigna rol de cliente
	--exec Seguridad.seg_p_setRol @VL_IdUsuario, 12 --Rol de Cliente 	
END

-- Ingresa Empleados
IF @VL_TipoPart = 2
BEGIN
	insert into Participante.Par_Empleado(IdParticipante,IdTipoEmpleado,
		IdEmpresa,IdOficina,IdEmpresaPertenece,IdOrganigrama,
		IdCargo,Sueldo,HorasExtras,LibretaSeguro,
		FechaIngSeguro,FechaNotEgreso,IdMoneda,Estado)
		SELECT @PO_IdParticipante,IdTipoEmpleado,
		IdEmpresa,IdOficina,IdEmpresaPertenece,IdOrganigrama,
		IdCargo,Sueldo,HorasExtras,LibretaSeguro,
		FechaIngSeguro,FechaNotEgreso,IdMoneda,Estado
	FROM   OPENXML (@VL_idXMLcep, '/Participante/Empleados/Empleado_N') WITH Participante.Par_Empleado
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 
	
	-- Asigna rol de coordinador
	--exec Seguridad.seg_p_setRol @VL_IdUsuario, 11 --Rol de Cordinador
	
	--Cargas Familiares
	INSERT INTO Participante.Par_CargaFamiliar
		(IdParticipante,IdCargaFamiliar,IdTipoCarga,
		 IdTipoIdentificacion,Identificacion,Nombre,FechaNac)
		 SELECT @PO_IdParticipante,IdCargaFamiliar,IdTipoCarga,
		 IdTipoIdentificacion,Identificacion,Nombre,FechaNac
	FROM   OPENXML (@VL_idXMLcep, '/Participante/CargaFamiliar/Carga_N') WITH Participante.Par_CargaFamiliar
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END 	
END

-- Ingresa Proveedores
IF @VL_TipoPart = 3
BEGIN
	insert into Participante.Par_Proveedor(IdParticipante,IdEmpresa,IdOficina,ContribuyenteEspecial,Estado)
		SELECT @PO_IdParticipante,IdEmpresa,IdOficina,
		ContribuyenteEspecial,Estado
	FROM   OPENXML (@VL_idXMLcep, '/Participante/Proveedores/Proveedor_N') WITH Participante.Par_Proveedor
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END	
	-- Asigna rol de Proveedor
	--exec Seguridad.seg_p_setRol @VL_IdUsuario, 13 --Rol de Proveedor
END

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
	
	--Isertar Zonas de Empresa en Catalogos.Ctl_CatalogoEmpresa
	INSERT INTO Catalogo.Ctl_CatalogoEmpresa(IdEmpresa,IdTabla,Codigo)
		SELECT @PO_IdParticipante,IdTabla,Codigo
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
		exec Participante.Par_P_InicializarModuloNuevaEmpresa @PO_IdParticipante
		IF (@@error <> 0)
		BEGIN
		  ROLLBACK TRAN
		  RETURN
		END			
       end
END

	
COMMIT TRAN

--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXMLcep
EXEC sp_xml_removedocument @VL_idXML


