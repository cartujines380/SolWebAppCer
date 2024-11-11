CREATE procedure [Seguridad].[Seg_mnt_EmpleLinea]
	( @PI_ParamXML xml )
AS
BEGIN

BEGIN TRY

	DECLARE
		 @criterio		CHAR(1)
		,@RUC			VARCHAR(13)
		,@USUARIO		VARCHAR(100)
		,@IDEMPRESA		INT
		,@LINEAS		VARCHAR(50)

		,@PO_CodError	VARCHAR(5)
		,@PO_MsgError	VARCHAR(100)

		SET @PO_CodError = '00000'
		SET @PO_MsgError = ''



	SELECT
		@criterio		= nref.value('@CRITERIO','CHAR(1)'),
		@RUC			= nref.value('@RUC','VARCHAR(13)'),
		@USUARIO		= nref.value('@USUARIO','VARCHAR(100)'),
		@IDEMPRESA		= nref.value('@IDEMPRESA','INT'),
		@LINEAS			= nref.value('@LINEAS','VARCHAR(50)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @criterio='C'
	begin
		  select * from Seguridad.Seg_EmpleadoLinea where Usuario=@USUARIO
	END

	IF @criterio='M'
	BEGIN

	IF (select count(*) from Seguridad.Seg_Empleado where Usuario=@USUARIO)=0
	begin
		BEGIN TRAN

		INSERT INTO Seguridad.Seg_Empleado(IdEmpresa,Ruc,Usuario,IdParticipante,TipoIdent,Identificacion,Apellido1,Apellido2,Nombre1,Nombre2,Cargo,CorreoE,Estado)
		SELECT IdEmpresa, substring(NumIdent,0,14), IdUsuario, IdParticipante, TipoParticipante, substring(NumIdent,0,14), Apellido1, Apellido2, Nombre1,Nombre2,'A',correo,'A'
		FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados
		WHERE IdUsuario=@USUARIO

		if (select [28] from SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados where IdUsuario= @USUARIO)=1
		begin
			INSERT INTO Seguridad.Seg_AprobacionNivel(IdEmpresa,Modulo,Nivel,Ruc,Usuario) 
			SELECT IdEmpresa, 'ART', 'APR', substring(NumIdent,0,14), IdUsuario
			FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados 
			WHERE IdUsuario=@USUARIO

			INSERT INTO Seguridad.Seg_AprobacionNivel(IdEmpresa,Modulo,Nivel,Ruc,Usuario)
			SELECT IdEmpresa, 'PRV', 'APR', substring(NumIdent,0,14), IdUsuario
			FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados 
			WHERE IdUsuario=@USUARIO
		end

		if (select [29] from SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados where IdUsuario= @USUARIO)=1
		begin
			INSERT INTO Seguridad.Seg_AprobacionNivel(IdEmpresa,Modulo,Nivel,Ruc,Usuario)
			SELECT IdEmpresa, 'ART', 'APG', substring(NumIdent,0,14), IdUsuario
			FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados 
			WHERE IdUsuario=@USUARIO

			INSERT INTO Seguridad.Seg_AprobacionNivel(IdEmpresa,Modulo,Nivel,Ruc,Usuario)
			SELECT IdEmpresa, 'PRV', 'APG', substring(NumIdent,0,14), IdUsuario
			FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados 
			WHERE IdUsuario=@USUARIO
		end

		if (select [30] from SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados where IdUsuario= @USUARIO)=1
		begin
			INSERT INTO Seguridad.Seg_AprobacionNivel(IdEmpresa,Modulo,Nivel,Ruc,Usuario)
			SELECT IdEmpresa, 'ART', 'APM', substring(NumIdent,0,14), IdUsuario
			FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados 
			WHERE IdUsuario=@USUARIO

			INSERT INTO Seguridad.Seg_AprobacionNivel(IdEmpresa,Modulo,Nivel,Ruc,Usuario)
			SELECT IdEmpresa, 'PRV', 'APM', substring(NumIdent,0,14), IdUsuario
			FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados 
			WHERE IdUsuario=@USUARIO
		end

		DELETE Seguridad.Seg_EmpleadoLinea WHERE Usuario=@USUARIO

		SELECT @IDEMPRESA=IdEmpresa,@RUC=substring(NumIdent,0,14),@USUARIO=IdUsuario 
		FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados where IdUsuario=@USUARIO 

		INSERT INTO Seguridad.Seg_EmpleadoLinea(IdEmpresa,Ruc,Usuario,Linea)
		SELECT @IDEMPRESA,
		       @RUC,
		       @USUARIO,
		       CAST(splitdata AS varchar(10))
		FROM fnSplitString(@LINEAS,'|')


		IF (@@ROWCOUNT = 0)
		BEGIN
			print 'hola'
			ROLLBACK TRAN			
			RETURN
		END

		COMMIT TRAN
	end

	IF (select count(*) from Seguridad.Seg_Empleado where Usuario=@USUARIO)=1
	begin

		DELETE Seguridad.Seg_EmpleadoLinea WHERE Usuario=@USUARIO		

		SELECT @IDEMPRESA=IdEmpresa,@RUC=substring(NumIdent,0,14),@USUARIO=IdUsuario 
		FROM SIPE_FRAMEWORK.Seguridad.Seg_VistaEmpleados where IdUsuario=@USUARIO

		INSERT INTO Seguridad.Seg_EmpleadoLinea(IdEmpresa,Ruc,Usuario,Linea)
		SELECT @IDEMPRESA,
		       @RUC,
		       @USUARIO,
		       CAST(splitdata AS varchar(10))
		FROM fnSplitString(@LINEAS,'|')
		
	end


	END

	IF @criterio='E'
	BEGIN
		delete Seguridad.Seg_Empleado where Usuario=@USUARIO
		delete Seguridad.Seg_AprobacionNivel where Usuario=@USUARIO
		delete Seguridad.Seg_EmpleadoLinea where Usuario=@USUARIO
	END

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	SELECT @PO_CodError = '50000', @PO_MsgError = ERROR_MESSAGE()
END CATCH

END
