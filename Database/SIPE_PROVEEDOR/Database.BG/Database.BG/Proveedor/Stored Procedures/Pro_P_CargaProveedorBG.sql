
create procedure Proveedor.Pro_P_CargaProveedorBG
as
begin
	declare @v_contar_proveedor int,
			@v_contar_contacto int,
			@v_contar_linegocio int,
			@v_contar_provdetalle int,
			@v_contar_detadi numeric(9,2),
			@v_contar_t_proveedor int,
			@v_contar_t_contacto int,
			@v_contar_t_linegocio int,
			@v_idPro int

	select @v_contar_t_proveedor = COUNT(1)
	from Proveedor.CMPro_Proveedor_tmp
	select @v_contar_t_contacto = COUNT(1)
	from Proveedor.CMPro_Contacto_tmp
	select @v_contar_t_linegocio = COUNT(1)
	from Proveedor.CMPro_LineaNegocio_tmp

	--Proveedores
	begin try
		begin tran
			select @v_idPro = MAX(CodProveedor) from Proveedor.Pro_Proveedor

			insert into Proveedor.Pro_Proveedor(
				CodProveedor,Ruc,TipoProveedor,NomComercial,DirCalleNum,DirPisoEdificio,DirCallePrinc,
				DirDistrito,DirCodPostal,Poblacion,Pais,Region,Idioma,Telefono,Movil,Fax,CorreoE,Gendocelec,FechaCertifica,
				Apoderadonom,ApoderadoApe,ApoderadoIdFiscal,PlazoEntregaPrev,FechaMod,CodProveedorBG
			)
			select 
				@v_idPro+ROW_NUMBER() OVER(ORDER BY CodProveedor ASC),
				Ruc,TipoProveedor,NombreProveedor,DirCalleNum,DirPisoEdificio,DirPrincipal,
				DirDistrito,DirCodPostal,Ciudad,Pais,Region,Idioma,Phone,Movil,Fax,email,Gendocelec,FechaCreacion,
				Apoderadonom,ApoderadoApe,ApoderadoFiscal,Null,FechaModificacion,CodProveedor
			from Proveedor.CMPro_Proveedor_tmp
			where CodProveedor not in (
				select isnull(P.CodProveedorBG,'') from Proveedor.Pro_Proveedor P
			)

			set @v_contar_proveedor = @@ROWCOUNT
		commit tran
	end try 
	begin catch
		rollback tran
		select @@ERROR CodErrorPro,ERROR_MESSAGE() Msg_Error
	end catch

	--Contactos
	begin try
		begin tran
			insert into Proveedor.Pro_Contacto(
				CodProveedor,TipoIdentificacion,Identificacion,Nombre2,Nombre1,Apellido2,Apellido1,CodSapContacto,
				PreFijo,DepCliente,Departamento,Funcion,RepLegal,Estado,NotElectronica,NotTransBancaria,Ext,Telefono,Celular,Correo
			)
			select P.CodProveedor,TipoIdentificacion,isnull(Identificacion,'0999999999'),Nombre2,Nombre1,Apellido2,Apellido1,null,
			PreFijo,Null,Departamento,Funcion,RepLegal,Estado,Null,Null,Ext,T.Telefono,Celular,Correo
			from Proveedor.CMPro_Contacto_tmp T
			inner join Proveedor.Pro_Proveedor P
			on T.CodProveedor = P.CodProveedorBG
			where P.CodProveedor not in (
				select PC.CodProveedor from Proveedor.Pro_Contacto PC
			)

			set @v_contar_contacto = @@ROWCOUNT
		commit tran
	end try 
	begin catch
		rollback tran
		select @@ERROR CodErrorCont,ERROR_MESSAGE() Msg_Error
	end catch

	--Linea de Negocio
	begin try
		begin tran
			insert into Proveedor.Pro_LineaNegocio
			select P.CodProveedor,LineaNegocio,Principal 
			from Proveedor.CMPro_LineaNegocio_tmp T
			inner join Proveedor.Pro_Proveedor P
			on T.CodProveedor = P.CodProveedorBG
			where P.CodProveedor not in (
				select PL.CodProveedor from Proveedor.Pro_LineaNegocio PL
			)

			set @v_contar_linegocio = @@ROWCOUNT
		commit tran
	end try 
	begin catch
		rollback tran
		select @@ERROR CodErrorLin,ERROR_MESSAGE() Msg_Error
	end catch

	--Detalle Proveedor
	begin try
		begin tran
			insert into Proveedor.Pro_ProveedorDetalle(
				CodProveedor,EsCritico,ProcesoBrindaSoporte,EsServiciosAux,
				IdArea,IdFormasPago,IdStatus,LlenadoDocVinculacion,VinculadoBG,Recurrente,FirmadoSegInfo,
				FirmadoPCI,CalificadoContNegocio,Sgs,Calificacion,FecTermCalificacion,UsuarioCreacion,FechaCreacion
			)
			select CodProveedor,ProveedorCritico,ProcesoSoporte,ServAuxiliares,
				Area,FormaPago,Status,LlenadoDocVin,VinculadoBG,Recurrente,FirmSegInfo,
				FirmadoPCI,CalContNeg,SGS,CalObtenida,FechaTermCal,'AD-CM',GETDATE()
			from Proveedor.CMPro_Proveedor_tmp 
			where CodProveedor not in (
				select isnull(P.CodProveedor,'') from Proveedor.Pro_ProveedorDetalle P
			)
			
			set @v_contar_provdetalle = @@ROWCOUNT

		commit tran
	end try
	begin catch
		rollback tran
		select @@ERROR CodErrorDet,ERROR_MESSAGE() Msg_Error
	end catch

	--Detalles Adicionales

	begin try
		begin tran
			set @v_contar_detadi = 0

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'NR',NivelRiesgo,2021
			from Proveedor.CMPro_Proveedor_tmp 
			
			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'IS1',Cumplimiento,0
			from Proveedor.CMPro_Proveedor_tmp 

			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'ED',EvalDes2018,2018
			from Proveedor.CMPro_Proveedor_tmp 

			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'ED',EvalDes2019,2019
			from Proveedor.CMPro_Proveedor_tmp 

			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'ED',EvalDes2020,2020
			from Proveedor.CMPro_Proveedor_tmp 

			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'FA',Fact2019,2019
			from Proveedor.CMPro_Proveedor_tmp 

			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'FA',Fact2020,2020
			from Proveedor.CMPro_Proveedor_tmp 

			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 

			insert into Proveedor.Pro_ProveedorDetAdicional(
				CodProveedor,IdTipoDetAd,Valor,Tiempo
			)
			select CodProveedor,'FA',Fact2021,2021
			from Proveedor.CMPro_Proveedor_tmp 
			
			set @v_contar_detadi = @v_contar_detadi + @@ROWCOUNT 
			set @v_contar_detadi = @v_contar_detadi / 8

		commit tran
	end try
	begin catch
		rollback tran
		select @@ERROR CodErrorDet,ERROR_MESSAGE() Msg_Error
	end catch

	select 'Proveedor: ' + CONVERT(varchar,@v_contar_proveedor) + ' de ' + CONVERT(varchar,@v_contar_t_proveedor) Proveedor	
	select 'Contacto: ' + CONVERT(varchar,@v_contar_contacto) + ' de ' + CONVERT(varchar,@v_contar_t_contacto) Contacto
	select 'Lin.Negocio: ' + CONVERT(varchar,@v_contar_linegocio) + ' de ' + CONVERT(varchar,@v_contar_t_linegocio) LinNegocio
	select 'Detalles: ' + CONVERT(varchar,@v_contar_provdetalle) Detalles
	select 'DetAdicional: ' + CONVERT(varchar,@v_contar_detadi) Adicional
end

