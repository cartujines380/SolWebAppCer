
create proc Proveedor.Pro_P_Mant_Documentos(
@PI_ParamXML xml
)
as
begin
	declare @e_tipo int,
			@e_CodTipoPersona varchar(5),
			@e_Codigo varchar(5),
			@e_Descripcion varchar(300),
			@e_EsObligatorio char(1),
			@e_Estado char(1),
			@e_UsuarioCreacion varchar(25),
			@e_nAccion char(1),
			@e_IdDocumentos int

	select  @e_tipo = nref.value('@Tipo','varchar(1)')
    from @PI_ParamXML.nodes('/Root') AS R(nref) 
	
	--Consulta
	if @e_tipo = 1
	begin
		select  
			IdDocumentos,
			CodTipoPersona,
			(select Detalle 
				from Proveedor.Pro_Tabla T
				inner join Proveedor.Pro_Catalogo C
				on T.Tabla = C.Tabla
				where TablaNombre = 'tbl_ClaseImpuesto'
				and C.Codigo = CodTipoPersona
				and T.Estado = 'A'
			and C.Estado = 'A') as NomTipoPersona,
			Codigo,
			Descripcion,
			EsObligatorio,
			FechaRegistro,
			Estado
		from Proveedor.Pro_Documentos with(nolock)

		return 0
	end

	
	--Insercion
	if @e_tipo = 2
	begin
		select  @e_nAccion = nref.value('@Accion','varchar(1)'),
				@e_IdDocumentos = nref.value('@IdDocumentos','int'),
				@e_CodTipoPersona = nref.value('@CodTipoPersona','varchar(5)'),
				@e_Descripcion = nref.value('@Descripcion','varchar(300)'),
				@e_EsObligatorio = nref.value('@EsObligatorio','varchar(1)'),
				@e_Estado = nref.value('@Estado','varchar(1)'),
				@e_UsuarioCreacion = nref.value('@UsuarioCreacion','varchar(15)')
		from @PI_ParamXML.nodes('/Root/Pagos') AS R(nref) 
		
		select @e_Codigo = upper(left(@e_Descripcion,1) + substring(@e_Descripcion,charindex(' ',@e_Descripcion)+1,1))
		
		if exists (select 1 
			from Proveedor.Pro_Documentos
			where Codigo = @e_Codigo
			and CodTipoPersona = CodTipoPersona
			and Estado = @e_Estado
		)
		begin
			select @e_Codigo = upper(left(@e_Descripcion,1) + substring(@e_Descripcion,charindex(' ',@e_Descripcion)+2,1))
		end

		begin try
			begin tran

			if @e_nAccion = 'R'
			begin
				insert into Proveedor.Pro_Documentos(
					CodTipoPersona,    Codigo,           Descripcion,  EsObligatorio, 
					FechaRegistro,     UsuarioCreacion,  Estado) 
				values(
					@e_CodTipoPersona, @e_Codigo,       @e_Descripcion, @e_EsObligatorio,
					getdate(),         @e_UsuarioCreacion, @e_Estado)
			end 
			if @e_nAccion = 'M'
			begin
				update Proveedor.Pro_Documentos
				set CodTipoPersona = @e_CodTipoPersona,
					Descripcion = @e_Descripcion, 
					EsObligatorio = @e_EsObligatorio, 
					Estado = @e_Estado, 
					UsuarioModificacion = @e_UsuarioCreacion,
					FechaModificacion = getdate()
				where IdDocumentos = @e_IdDocumentos
				
			end
			if @e_nAccion = 'E'
			begin
				update Proveedor.Pro_Documentos
				set Estado = 'I', 
					UsuarioModificacion = @e_UsuarioCreacion,
					FechaModificacion = getdate()
				where IdDocumentos = @e_IdDocumentos
			end

		end try
		begin catch
			select ERROR_NUMBER() as CodError,ERROR_MESSAGE() as MsgError
			rollback tran
			return 1
		end catch

		commit tran

		select 0 as CodError, '' as MsgError

		return 0
		
	end
	
end 
