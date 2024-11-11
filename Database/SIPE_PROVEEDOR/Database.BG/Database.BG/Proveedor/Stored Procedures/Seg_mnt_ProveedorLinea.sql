CREATE procedure [Proveedor].[Seg_mnt_ProveedorLinea]
	( @PI_ParamXML xml )
AS
BEGIN

BEGIN TRY

	DECLARE
		 @criterio		CHAR(1)
	
		,@CodProveedor		VARCHAR(10)
	
		,@LINEAS		VARCHAR(50)

		,@PO_CodError	VARCHAR(5)
		,@PO_MsgError	VARCHAR(100)

		SET @PO_CodError = '00000'
		SET @PO_MsgError = ''



	SELECT
		@criterio		= nref.value('@CRITERIO','CHAR(1)'),
	
		@CodProveedor		= nref.value('@CODPROVEEDOR','VARCHAR(10)'),
	
		@LINEAS			= nref.value('@LINEAS','VARCHAR(50)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @criterio='C'
	begin
		  --select * from Seguridad.Seg_EmpleadoLinea where Usuario=@USUARIO
		  select *  from Proveedor.Proveedor_LineaNegocio where CodProveedor = @CodProveedor
	END

	IF @criterio='M'
	BEGIN

	
		BEGIN TRAN

		
		delete from Proveedor.Proveedor_LineaNegocio where CodProveedor = @CodProveedor
		

		INSERT INTO Proveedor.Proveedor_LineaNegocio(CodProveedor,CodLineaNegocio)
		SELECT @CodProveedor,		   
		       CAST(splitdata AS CHAR(1))
		FROM fnSplitString(@LINEAS,'|')


		IF (@@ROWCOUNT = 0)
		BEGIN
			
			ROLLBACK TRAN			
			RETURN
		END

		COMMIT TRAN
	


	END

	IF @criterio='E'
	BEGIN
		delete from Proveedor.Proveedor_LineaNegocio where CodProveedor = @CodProveedor
		
	END

END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
	SELECT @PO_CodError = '50000', @PO_MsgError = ERROR_MESSAGE()
END CATCH

END
