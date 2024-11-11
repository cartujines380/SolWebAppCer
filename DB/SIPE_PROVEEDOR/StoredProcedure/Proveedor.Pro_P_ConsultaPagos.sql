use SIPE_PROVEEDOR
go

if exists(select 1 from sysobjects
	where name = 'Pro_P_ConsultaPagos'
	and xtype = 'P'
)
begin
	drop proc Proveedor.Pro_P_ConsultaPagos
end
go

create procedure Proveedor.Pro_P_ConsultaPagos(
@PI_ParamXML xml
)
as
begin
	declare @e_tipo_consulta varchar(5),
			@e_fecha_inicio datetime = null,
			@e_fecha_fin datetime = null,
			@e_Identificacion varchar(13) = null,
			@v_numTablaTmp int = 0,
			@v_numTablaReal int = 0,
			@v_idMsgPagos int = 0,
			@v_nombreArchivo varchar(50)

	declare	@v_tabla_pagos table (
		TipoIdentificacion varchar(1),
		Identificacion varchar(13),
		CodProv varchar(40),
		FormaPago varchar(20),
		Fecha datetime,
		Valor decimal,
		Factura varchar(200)
	)
	
	select  @e_Identificacion = nref.value('@Identificacion','varchar(13)'),
			@e_tipo_consulta = nref.value('@Tipo','varchar(1)'),
			@e_fecha_inicio  = nref.value('@FechaDesde','varchar(10)') + ' 00:00:00.000',
			@e_fecha_fin     = nref.value('@FechaHasta','varchar(10)') + ' 23:59:58.999'
    from @PI_ParamXML.nodes('/Root/Pagos') AS R(nref) 

	if @e_tipo_consulta is null
	begin
		SELECT top 1 @e_tipo_consulta =
			CAST(y.item.query('data(Tipo)') AS varchar(5)) 
		FROM
			@PI_ParamXML.nodes('Root/Pagos') y(item)	
	end

	if @e_Identificacion = ''
	begin
		set @e_Identificacion = null
	end

	if @e_tipo_consulta = 'T'
	begin
		select	ROW_NUMBER() OVER(ORDER BY Identificacion ASC) AS Num,
				Identificacion,
				isnull((select top 1 NomComercial
					from Proveedor.Pro_Proveedor
					where Ruc = Pa.Identificacion),'NN') NomComercial,
				CodProveedorAx,
				Factura,
				(select Detalle 
					from Proveedor.Pro_Tabla T
						inner join  Proveedor.Pro_Catalogo C
						on T.Tabla = C.Tabla
					where T.TablaNombre = 'tbl_FormaPagoBG'
					and C.Codigo = FormaPago
					and T.Estado = 'A'
					and C.Estado = 'A'
				) FormaPago,
				FechaPago,
				Valor,
				(select C.DescAlterno 
					from Proveedor.Pro_Tabla T
						inner join  Proveedor.Pro_Catalogo C
						on T.Tabla = C.Tabla
					where T.TablaNombre = 'tbl_FormaPagoBG'
					and C.Codigo = FormaPago
					and T.Estado = 'A'
					and C.Estado = 'A'
				) Detalle
		from Proveedor.Pro_ConPagos Pa with(nolock)
		where Identificacion = ISNULL(@e_Identificacion,Identificacion)

		return 0
	end

	if @e_tipo_consulta = 'F'
	begin
		select	ROW_NUMBER() OVER(ORDER BY Identificacion ASC) AS Num,
				Identificacion,
				isnull((select top 1 NomComercial
					from Proveedor.Pro_Proveedor
					where Ruc = Pa.Identificacion),'NN') NomComercial,
				CodProveedorAx,
				Factura,
				(select Detalle 
					from Proveedor.Pro_Tabla T
						inner join  Proveedor.Pro_Catalogo C
						on T.Tabla = C.Tabla
					where T.TablaNombre = 'tbl_FormaPagoBG'
					and C.Codigo = FormaPago
					and T.Estado = 'A'
					and C.Estado = 'A'
				) FormaPago,
				FechaPago,
				Valor,
				(select C.DescAlterno 
					from Proveedor.Pro_Tabla T
						inner join  Proveedor.Pro_Catalogo C
						on T.Tabla = C.Tabla
					where T.TablaNombre = 'tbl_FormaPagoBG'
					and C.Codigo = FormaPago
					and T.Estado = 'A'
					and C.Estado = 'A'
				) Detalle
		from Proveedor.Pro_ConPagos Pa with(nolock)
		where FechaPago >= @e_fecha_inicio
		and FechaPago <= @e_fecha_fin
		and Identificacion = ISNULL(@e_Identificacion,Identificacion)

		return 0
	end

	--Inserta en tabla de pagos
	if @e_tipo_consulta = 'I'
	begin
		begin try
			insert into @v_tabla_pagos
			SELECT 
				CAST(y.item.query('data(TipoIdentificacion)') AS varchar(1)) ,
				CAST(y.item.query('data(Identificacion)') AS varchar(13)) ,
				CAST(y.item.query('data(CodProv)') AS varchar(40)) ,
				CAST(y.item.query('data(FormaPago)') AS varchar(20)) ,
				CAST(y.item.query('data(Fecha)') AS varchar(10)) ,
				CAST(y.item.query('data(Valor)') AS varchar(13)) ,
				CAST(y.item.query('data(Factura)') AS varchar(200))		
			FROM
				@PI_ParamXML.nodes('Root/Pagos') y(item)

			set @v_numTablaTmp = @@ROWCOUNT

			insert into Proveedor.Pro_ConPagos(
				TipoIdentificacion, Identificacion, CodProveedorAx, 
				Factura, FormaPago, FechaPago, 
				Valor, Detalle, FechaCreacion
			)
			select	A.TipoIdentificacion,
					A.Identificacion,
					A.CodProv,
					A.Factura,
					A.FormaPago,
					A.Fecha,
					ROUND((convert(numeric,A.Valor) / 100),2),
					(select C.DescAlterno 
						from Proveedor.Pro_Tabla T
							inner join  Proveedor.Pro_Catalogo C
							on T.Tabla = C.Tabla
						where T.TablaNombre = 'tbl_FormaPagoBG'
						and C.Codigo = A.FormaPago
						and T.Estado = 'A'
						and C.Estado = 'A'
					),
					GETDATE()
			from @v_tabla_pagos A

			set @v_numTablaReal = @@ROWCOUNT
		end try
		begin catch
			insert into Proveedor.Pro_PagosBitacora(
			  Proceso,FechaRegistro,Servicio,Detalle,Accion,Estado
			)
			values(
				'Pro_P_ConsultaPagos',GETDATE(),'IngresaPagos','Error: ['+ convert(varchar,ERROR_NUMBER())+ '] - ' + ERROR_MESSAGE(),'I','A'
			)
		end catch

		--Retorna
		declare @v_fecha1 datetime,
				@v_fecha2 datetime

		select  @v_fecha1 = format(GETDATE(),'yyyy-MM-dd') + ' 00:00:00.000',
				@v_fecha2 = format(GETDATE(),'yyyy-MM-dd') + ' 23:59:59.000'
		
		select	tmpa.Identificacion, 
				tmpa.Factura, 
				(select Detalle 
					from Proveedor.Pro_Tabla T
						inner join  Proveedor.Pro_Catalogo C
						on T.Tabla = C.Tabla
					where T.TablaNombre = 'tbl_FormaPagoBG'
					and C.Codigo = tmpa.FormaPago
					and T.Estado = 'A'
					and C.Estado = 'A'
				) FormaPago, 
				tmpa.Fecha, 
				pa.Valor, 
				pa.Detalle
		from @v_tabla_pagos tmpa
		inner join Proveedor.Pro_ConPagos pa
		on tmpa.Identificacion = pa.Identificacion
		and tmpa.CodProv = pa.CodProveedorAx
		and tmpa.Factura = pa.Factura
		and tmpa.FormaPago = pa.FormaPago
		and ROUND((convert(numeric,tmpa.Valor) / 100),2) = pa.Valor
		where pa.FechaCreacion >= @v_fecha1
		  and pa.FechaCreacion <= @v_fecha2
		order by tmpa.Identificacion, tmpa.Fecha

		return 0
	end
	
	--Inserta tabla bitacora
	if @e_tipo_consulta = 'B'
	begin
		insert into Proveedor.Pro_PagosBitacora(
		  Proceso,FechaRegistro,Servicio,Detalle,Accion,Estado
		)
		SELECT 
			CAST(y.item.query('data(Proceso)') AS varchar(25)) ,
			GETDATE(),
			CAST(y.item.query('data(Servicio)') AS varchar(25)) ,
			CAST(y.item.query('data(Detalle)') AS varchar(5000)),
			CAST(y.item.query('data(Accion)') AS varchar(5)),
			'A'	
		FROM
			@PI_ParamXML.nodes('Root/Pagos') y(item)

		return 0;
	end

	--Consulta de Correo y nombre comercial
	if @e_tipo_consulta = 'D'
	begin
		SELECT @e_Identificacion =
			CAST(y.item.query('data(Identificacion)') AS varchar(13)) 
		FROM
			@PI_ParamXML.nodes('Root/Pagos') y(item)	

		select top 1 NomComercial, isnull(CorreoE,'') CorreoE
		from Proveedor.Pro_Proveedor
		where Ruc = @e_Identificacion

		return 0
	end

	--Notificacion flash
	if @e_tipo_consulta = 'N'
	begin
		insert into Proveedor.Pro_MensajesFlash(
			Identificacion, Titulo, Mensaje, Estado, FechaCreacion
		)
		SELECT 
			CAST(y.item.query('data(Identificacion)') AS varchar(13)),
			CAST(y.item.query('data(Titulo)') AS varchar(50)),
			CAST(y.item.query('data(Mensaje)') AS varchar(8000)),
			'I',
			GETDATE()
		FROM
			@PI_ParamXML.nodes('Root/Pagos') y(item)	

		return 0
		
	end

	--Consulta de Mensajes no leídos
	if @e_tipo_consulta = 'CN'
	begin
	
		SELECT	@e_Identificacion = CAST(y.item.query('data(Identificacion)') AS varchar(13)),
				@v_idMsgPagos = convert(int,CAST(y.item.query('data(Id)') AS varchar(10)))
		FROM
			@PI_ParamXML.nodes('Root/Pagos') y(item)
		

		if(@v_idMsgPagos > 0)
		begin
			update Proveedor.Pro_MensajesFlash
			set Estado = 'L'
			where Id = @v_idMsgPagos
			and Estado = 'I'
		end

		select Id, Titulo, Mensaje, (case when Estado = 'L' then 'Leido' else 'Pendiente' end) Estado
		from Proveedor.Pro_MensajesFlash
		where Identificacion = @e_Identificacion		

		return 0
	end

	--Consulta de pago procesado
	if @e_tipo_consulta = 'P'
	begin
		SELECT @v_nombreArchivo =
			CAST(y.item.query('data(NomArchivo)') AS varchar(50)) 
		FROM
			@PI_ParamXML.nodes('Root/Pagos') y(item)	


		if exists(select 1
				from Proveedor.Pro_PagosBitacora 
				where Detalle = @v_nombreArchivo 
				and Accion = 'P' 
				and Estado = 'A'
		)
		begin
			select 1 as retorno
		end
		else
		begin
			select 0 as retorno
		end

		return 0

	end

end

go

