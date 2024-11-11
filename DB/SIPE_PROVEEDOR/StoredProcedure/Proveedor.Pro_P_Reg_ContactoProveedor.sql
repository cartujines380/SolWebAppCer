USE SIPE_PROVEEDOR
GO

IF EXISTS (select 1 from sysobjects where name = 'Pro_P_Reg_ContactoProveedor' and type='P')
begin
    drop procedure  Proveedor.Pro_P_Reg_ContactoProveedor
    if exists (select 1 from sysobjects where name = 'Pro_P_Reg_ContactoProveedor' and type = 'P')
      PRINT '<<< DROP PROCEDURE Pro_P_Reg_ContactoProveedor -- ERROR -- >>>'
    else
      PRINT '== DROP PROCEDURE Pro_P_Reg_ContactoProveedor *OK* =='
end
GO

CREATE PROCEDURE Proveedor.Pro_P_Reg_ContactoProveedor(
	@PI_ParamXML xml,
	@PI_ParamAlm xml,
	@PI_Tipo  varchar(5),
	@PI_CodProveedor varchar(10)
)
AS
BEGIN
	declare @msg_proc		varchar(150),
			@Ident			varchar(15),
			@Existentes		varchar(600) = '',
			@IdContacto		int = 1,
			@ContadorExito	int = 0,
			@cod_error		int = 1000,
			@codProv		int = 0,
			@tmp_contacto   int = 0

	declare @tb_Contacto table(
		CodProveedor  int, 
		TipoIdentificacion varchar(2), 
		Identificacion varchar(20),  
		Nombre1 varchar(60),  
		Nombre2 varchar(60),  
		Apellido1 varchar(60),  
		Apellido2 varchar(60),  
		Prefijo varchar(12),
		CodDepartamento varchar(5),
		CodFuncion varchar(5),
		Estado char(1),
		TelfFijo varchar(10),
		TelfFijoEXT  varchar(5),
		TelfMovil  varchar(10),
		email varchar(60),
		NotElectronica	smallint ,
		NotTransBancaria smallint ,
		RecActas smallint,
		RepLegal smallint
		
	)
	
	--Ingreso y Actualizacion
	if @PI_Tipo = '1'
	begin
		insert into @tb_Contacto
		select CodProveedor=nref.value('@CodProveedor','int'),
				TipoIdentificacion=nref.value('@TipoIdentificacion','varchar(2)'),
				Identificacion = nref.value('@Identificacion','varchar(15)'),
				Nombre1 = nref.value('@Nombre1','varchar(60)'),
				Nombre2 = nref.value('@Nombre2','varchar(60)'),
				Apellido1 = nref.value('@Apellido1','varchar(60)'),
				Apellido2 = nref.value('@Apellido2','varchar(60)'),
				PreFijo = nref.value('@PreFijo','varchar(12)'),
				Departamento = nref.value('@Departamento','varchar(5)'),
				Funcion = nref.value('@Funcion','varchar(5)'),
				Estado = nref.value('@Estado','varchar(1)'),
				TelfFijo = nref.value('@TelfFijo','varchar(10)'),
				TelfFijoEXT = nref.value('@TelfFijoEXT','varchar(5)'),
				TelfMovil = nref.value('@TelfMovil','varchar(10)'),
				email = nref.value('@EMAIL','varchar(60)'),
				NotElectronica = nref.value('@NotElectronica','int'),
				NotTransBancaria = nref.value('@NotTransBancaria','int'),
				RecActas = nref.value('@RecActas','int'),
				RepLegal = nref.value('@RepLegal','int')
		from @PI_ParamXML.nodes('/Root/Contacto') AS R(nref) 
		
		declare contacto cursor 
		READ_ONLY  for
		select CodProveedor,Identificacion 
		from @tb_Contacto 
		group by CodProveedor,Identificacion

		open contacto  

		fetch next from contacto
		into @codProv, @Ident

		WHILE @@FETCH_STATUS = 0  
		BEGIN 
			if exists (
				select 1 
				from Proveedor.Pro_ContactoProveedor
				where CodProveedor = @codProv
				and Identificacion = @Ident
			)
			begin
				select @tmp_contacto = IdContacto
				from Proveedor.Pro_ContactoProveedor
				where CodProveedor = @codProv
				and Identificacion = @Ident

				
				--actualizacion de contactos
				update P
				set P.TipoIdentificacion	= C.TipoIdentificacion,	
					P.Identificacion		= C.Identificacion,  
					P.Nombre1				= C.Nombre1,
					P.Nombre2				= C.Nombre2,
					P.Apellido1				= C.Apellido1,  
					P.Apellido2				= C.Apellido2,
					P.Prefijo				= C.Prefijo,
					P.Estado				= C.Estado	,
					P.TelfFijo				= C.TelfFijo,
					P.TelfFijoEXT			= C.TelfFijoEXT,
					P.TelfMovil				= C.TelfMovil,
					P.email					= C.email,
					P.NotElectronica		= C.NotElectronica,
					P.NotTransBancaria		= C.NotTransBancaria,
					P.RecActas				= C.RecActas,
					P.RepLegal				= C.RepLegal,
					P.FechaActualizacion	= getdate()
				from Proveedor.Pro_ContactoProveedor P
					inner join @tb_Contacto C
					on P.Identificacion = C.Identificacion
				where P.IdContacto = @tmp_contacto
				and P.CodProveedor = @PI_CodProveedor
				and P.Identificacion = @Ident

				if @@ROWCOUNT > 0
				begin
					--Departamento
					--Se elimina todo y se registra nuevamente
					delete D
					from Proveedor.Pro_ContactoProveedor P
						inner join @tb_Contacto C
						on P.CodProveedor = C.CodProveedor
						inner join Proveedor.Pro_ContactoDepartamento D
						on P.IdContacto = D.IdContacto
					where D.Identificacion = @Ident
					and D.Estado = '1'

					insert into Proveedor.Pro_ContactoDepartamento(
						IdContacto	,	Identificacion	,	CodDepartamento,
						CodFuncion	,	Estado
					) 
					select P.IdContacto	,	P.Identificacion	,	C.CodDepartamento ,
							C.CodFuncion	,	1
					from Proveedor.Pro_ContactoProveedor P
						inner join @tb_Contacto C
						on P.Identificacion = C.Identificacion
					where P.CodProveedor = @codProv
					and P.Identificacion = @Ident
					and P.IdContacto = @tmp_contacto
					
					--Almacen
					if exists (select 1
						from @PI_ParamAlm.nodes('/Root/UsrAlmacen') AS R(nref)
						where nref.value('@Identificacion','varchar(15)') = @Ident			
					)
					begin
						--Se elimina todo y se registra nuevamente
						delete A
						from Proveedor.Pro_ContactoAlmacen A
							inner join Proveedor.Pro_ContactoProveedor P
							on  P.IdContacto = A.IdContacto
						where P.CodProveedor = @PI_CodProveedor

						insert into Proveedor.Pro_ContactoAlmacen(
							IdContacto , CodAlmacen , CodPais		,	 CodCiudad ,
							CodRegion  , Estado		, FechaRegistro 
						)
						select 
							P.IdContacto,
							nref.value('@CodAlmacen','varchar(3)'),
							nref.value('@CodPais','varchar(3)'),
							nref.value('@CodCiudad','varchar(20)'),
							nref.value('@CodRegion','varchar(3)'),
							1,
							getdate()
						from Proveedor.Pro_ContactoProveedor P
							inner join @PI_ParamAlm.nodes('/Root/UsrAlmacen') AS R(nref)
							on P.Identificacion = nref.value('@Identificacion','varchar(15)')
						where nref.value('@Identificacion','varchar(15)') = @Ident							
						and P.IdContacto = @tmp_contacto
					end
					
					select  @Existentes = @Existentes + @Ident + ' | '
				end
			end 
			else
			begin
				--Seg_ContactoProveedor
				insert into Proveedor.Pro_ContactoProveedor(
					CodProveedor	, 	TipoIdentificacion		,	Identificacion ,  
					Nombre1			,	Nombre2					,	Apellido1 ,  
					Apellido2		,	Prefijo					,	Estado	,
					TelfFijo		,	TelfFijoEXT				,	TelfMovil	,
					email			,	NotElectronica			,	NotTransBancaria	,
					RecActas		,	RepLegal				,	FechaRegistro	,
					FechaActualizacion
				)
				select distinct
					CodProveedor	, 	TipoIdentificacion		,	Identificacion ,  
					Nombre1			,	Nombre2					,	Apellido1 ,  
					Apellido2		,	Prefijo					,	Estado	,				
					TelfFijo		,	TelfFijoEXT				,	TelfMovil	,
					email			,	NotElectronica			,	NotTransBancaria	,
					RecActas		,	RepLegal				,	getdate()	,
					null
				from @tb_Contacto 
				where CodProveedor = @codProv
				and Identificacion = @Ident

				set @IdContacto = @@IDENTITY

				if @IdContacto > 0
				begin
					--Seg_ContactoDepartamento
					insert into Proveedor.Pro_ContactoDepartamento(
						IdContacto	,	Identificacion	,	CodDepartamento,
						CodFuncion	,	Estado
					) 
					select @IdContacto	,	Identificacion	,	CodDepartamento ,
							CodFuncion	,	1
					from @tb_Contacto 
					where CodProveedor = @codProv
					and Identificacion = @Ident
				
					if @@ROWCOUNT > 0
					begin
						--Seg_ContactoAlmacen
						insert into Proveedor.Pro_ContactoAlmacen(
							IdContacto , CodAlmacen , CodPais		,	 CodCiudad ,
							CodRegion  , Estado		, FechaRegistro 
						)
						select 
							IdContacto		= @IdContacto,
							CodAlmacen		= nref.value('@CodAlmacen','varchar(3)'),
							CodPais			= nref.value('@CodPais','varchar(3)'),
							CodCiudad		= nref.value('@CodCiudad','varchar(20)'),
							CodRegion		= nref.value('@CodRegion','varchar(3)'),
							Estado			= 1,
							FechaRegistro	= getdate()
						from @PI_ParamAlm.nodes('/Root/UsrAlmacen') AS R(nref) 
						where nref.value('@Identificacion','varchar(15)') = @Ident

						select @ContadorExito = @ContadorExito + 1
					end
					else
					begin
						select  @cod_error = 1001, 
								@msg_proc = 'Hubo problemas al realizar el registro del Contacto '+@Ident
					end
				end
			end

			FETCH NEXT FROM contacto   
			INTO @codProv, @Ident  
		END   
		CLOSE contacto;  
		DEALLOCATE contacto

		if LEN(@Existentes) > 0
		begin
			select	@cod_error = 1000, 
					@msg_proc = 'Se actualizaron los contactos '+@Existentes+' de forma exitosa'
		end
		else
		begin
			select	@cod_error = 1000, 
					@msg_proc = 'Se registraron '+convert(varchar,@ContadorExito)+' contacto(s) con éxito'
		end

		select	@cod_error cod_error,
				@msg_proc msg_proc
	end

	--Consulta General
	if @PI_Tipo = '2'
	begin
		select distinct
			CodProveedor		, 	TipoIdentificacion		,		a.Identificacion,  
			Nombre1				,	Nombre2					,  		Apellido1		,  
			Apellido2			,	b.CodDepartamento		,		b.CodFuncion	,
			Prefijo				,	a.Estado				,		TelfFijo		,
			TelfFijoEXT			,	TelfMovil				,		email			,
			NotElectronica		,	NotTransBancaria		,		RecActas		,
			RepLegal
		from Proveedor.Pro_ContactoProveedor a
			inner join Proveedor.Pro_ContactoDepartamento b
			on a.IdContacto = b.IdContacto
		where a.CodProveedor = @PI_CodProveedor
		and a.Identificacion = b.Identificacion
		order by Nombre1,Nombre2
	end

	--Consulta de Almacen
	if @PI_Tipo = '3'
	begin
		select	a.CodProveedor	,	a.Identificacion	, b.CodAlmacen	, 
				b.CodCiudad		,	b.CodPais			, b.CodRegion 
		from Proveedor.Pro_ContactoProveedor a
			inner join Proveedor.Pro_ContactoAlmacen b
			on a.IdContacto = b.IdContacto
		where a.CodProveedor = @PI_CodProveedor
		
	end

	return 0
END
GO

if exists(select 1 from sysobjects where name='Pro_P_Reg_ContactoProveedor' and type = 'P')
  PRINT '== CREATE PROCEDURE Pro_P_Reg_ContactoProveedor *OK* =='
 else
  PRINT '<<< CREATE PROCEDURE Pro_P_Reg_ContactoProveedor -- ERROR -- >>>'
GO
