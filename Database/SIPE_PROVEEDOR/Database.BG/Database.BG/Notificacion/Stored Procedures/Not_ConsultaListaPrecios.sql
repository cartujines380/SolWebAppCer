


create PROCEDURE [Notificacion].[Not_ConsultaListaPrecios]
@PI_ParamXML xml
	
AS
SET ARITHABORT ON;
BEGIN TRY
	
	Declare @TipoLista varchar(10)
	Declare @CodProveedor varchar(10)
	Declare @Usuario varchar(20)
	Declare @Motivo varchar(10)
	Declare @Observacion varchar(300)
	Declare @Acepto bit
    Declare @Ruc varchar(13)
    Declare @PagIncial bigint
	Declare @PagFinal bigint
	Declare @Fecini Datetime
	Declare @Fecfin Datetime
	DECLARE @DIAS INT

	SELECT @DIAS= isnull( cast( b.Detalle as int),15)
		FROM [Proveedor].[Pro_Tabla] a
		INNER JOIN [Proveedor].[Pro_Catalogo] b on a.Tabla=b.Tabla and b.Estado= A.Estado
		WHERE a.TablaNombre='tbl_DiasVigenciaPedidos' and a.Estado='A' and b.Codigo='DIA'
	select @Fecfin = GETDATE()
	select @Fecini = GETDATE() - @DIAS

	select  @TipoLista=nref.value('@TipoLista','varchar(10)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @Ruc=nref.value('@Ruc','varchar(13)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @PagIncial=nref.value('@RegInicial','bigint')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @PagFinal=nref.value('@RegFinal','bigint')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @PagFinal=nref.value('@RegFinal','bigint')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 

	select  @CodProveedor=nref.value('@CodProveedor','varchar(10)'),
	         @Usuario=nref.value('@Usuario','varchar(20)'),
			 @Acepto = Convert(bit,nref.value('@Acepto','varchar(1)')),
			 @Motivo = nref.value('@Usuario','varchar(10)'),
			 @Observacion = nref.value('@Observacion','varchar(300)')
    from @PI_ParamXML.nodes('/Root/SecNotificacion') AS R(nref) 
	
	DECLARE @ALMACENES TABLE(CodAlmacen VARCHAR(4))

	INSERT INTO @ALMACENES
						SELECT a.CodAlmacen FROM [Pedidos].[Ped_Almacen] a
	INSERT INTO @ALMACENES SELECT RIGHT(('00'+ CodAlmacen), 3) FROM @ALMACENES WHERE LEN(CodAlmacen) < 3

begin
		if (@TipoLista ='1')
		begin
		     Select Convert(varchar(max),Codigo) as Codigo, Descripcion, Precio, FechaPublicacion, 
	               NumProveedor, UnidadMedida, rownumber
            FROM (select Convert(bigint,Codigo) as Codigo, Descripcion, Precio, FechaPublicacion, 
	                     NumProveedor, UnidadMedida,
	              ROW_NUMBER() over (order by FechaPublicacion)  rownumber
		          from Notificacion.ProductosComi with(nolock)) AS TABLACONROWNUMBER
            WHERE  rownumber BETWEEN @PagIncial AND @PagFinal 
			Select COUNT(*) as total from Notificacion.ProductosComi
			
	    end	
		
		if (@TipoLista ='2')
		begin	
			Select Convert(varchar(max),Codigo) as Codigo , Descripcion, Precio, FechaPublicacion, 
	               NumProveedor, UnidadMedida, rownumber
            FROM (select  Convert(bigint,Codigo) as Codigo, Descripcion, Precio, FechaPublicacion, 
	                     NumProveedor, UnidadMedida,
	              ROW_NUMBER() over (order by FechaPublicacion)  rownumber
		          from Notificacion.ProductosMini with(nolock)) AS TABLACONROWNUMBER
            WHERE  rownumber BETWEEN @PagIncial AND @PagFinal
			Select  COUNT(*) as total from Notificacion.ProductosMini
			
	    end	
		
		if (@TipoLista ='3')
		begin
		SELECT *
            FROM  [Notificacion].[Rotacion_Productos]  PO INNER JOIN Proveedor.Pro_Proveedor PE ON PE.Ruc= @Ruc
			where CAST(PO.ruc AS INT)=PE.CodProveedor
	    end	
        
		if (@TipoLista ='4')
		begin
		--select CodAlmacen, NomAlmacen, CodCiudad from Pedidos.Ped_Almacen
		select CodAlmacen, NomAlmacen, CodCiudad from Pedidos.Ped_AlmacenSAP
		order by NomAlmacen
	    end	

		if (@TipoLista ='5')
		begin
		select idRol as CodAlmacen, IdDepartamento as NomAlmacen, IdFuncion as CodCiudad
		from Proveedor.Pro_RolDepartamento
	    end	
		
		if (@TipoLista ='7')
		begin
			select CodAlmacenOriginal as CodAlmacen, NomAlmacen, CodCiudad from Pedidos.Ped_AlmacenSAP
			WHERE KioscoActivo = 'X'
			order by NomAlmacen

		end


		if (@TipoLista <> '1' and @TipoLista <> '2' and  @TipoLista <> '3' and @TipoLista <> '4' and @TipoLista <> '5'
		     and  @TipoLista <> '7')
		begin
		    SELECT Convert(varchar(10),FechaPedido,103) as CodAlmacen , COUNT(*) as NomAlmacen, '0' as CodCiudad
			FROM [Pedidos].[Ped_Pedido] p with(nolock)
			WHERE
									 p.FechaPedido >= CONVERT(DATETIME, @Fecini, 103)
									AND p.IdEmpresa = '1' AND p.CodProveedor = @TipoLista									
									AND p.EsDescargado = CONVERT(BIT, 0) and  p.EsImpreso = CONVERT(BIT, 0)
									AND p.Estado IN ('FP', 'FT', 'PE', 'FI')
									AND EXISTS (SELECT TOP 1 1 FROM @ALMACENES a WHERE a.CodAlmacen = p.CodAlmacen)
									
			group by  FechaPedido
			order by FechaPedido
	    end	
		
end



END TRY
BEGIN CATCH
	exec SP_PROV_Error @sp='[Not_ConsultaListaPrecios]'
END CATCH


