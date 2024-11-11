CREATE procedure [Proveedor].[Seg_Cons_ProveedorLinea]
	( @PI_ParamXML xml )
AS
BEGIN

	DECLARE
		 @criterio		CHAR(1)
		,@nombreCom		VARCHAR(100)
		,@CodProveedor		VARCHAR(10)

	SELECT
		@criterio		= nref.value('@CRITERIO','CHAR(1)'),
		@nombreCom			= nref.value('@NOMBRE','VARCHAR(100)')
		--@CodProveedor		= nref.value('@CODPROVEEDOR','VARCHAR(10)')
	FROM @PI_ParamXML.nodes('/Root') AS R(nref)

	IF @criterio='R'
	begin		
		
		select p.CodProveedor, p.Ruc, p.NomComercial  from Proveedor.Pro_Proveedor p
		where p.CodProveedor = @nombreCom

	END

	IF @criterio='N'
	begin		
		

		select p.CodProveedor, p.Ruc, p.NomComercial  from Proveedor.Pro_Proveedor p
		where p.NomComercial like '%'+@nombreCom+'%'

	END

	IF @criterio='T'
	begin
		select p.CodProveedor, p.Ruc, p.NomComercial  from Proveedor.Pro_Proveedor p
		inner join Seguridad.Seg_Usuario u on u.CodProveedor = p.CodProveedor 
		where u.EsAdmin = 1
	END

END




