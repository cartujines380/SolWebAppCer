

CREATE PROCEDURE [Proveedor].[Pro_P_GrabaCatalogos] 
	 @PI_ParamXML xml
AS
BEGIN try
	begin tran
Declare @IdSolicitud bigint
declare  @Tabla int
/**************************************************************************************************************************************/
/*                  Carga:                       Proveedores                                                                            */
/**************************************************************************************************************************************/


/*****************CATALOGO # 1 PEDIDOS ALMACENES SAP*************/
delete almacenSAP from Pedidos.Ped_AlmacenSAP almacenSAP 
inner join @PI_ParamXML.nodes('/Root/PT_CENTROS') AS R(nref) 
 on (nref.value('@ZSAPRETAIL','VARCHAR(4)') IS NOT NULL)


--insert into  Pedidos.Ped_AlmacenSAP(idEmpresa,CodAlmacen,CodLegacy,NomAlmacen,CodCiudad,CodAlmacenSRI,CodLegacyOriginal)
--select  1, nref.value('@ZSAPRETAIL','VARCHAR(4)') ,nref.value('@CODALMA','VARCHAR(3)'), nref.value('@NAME1','VARCHAR(30)') , nref.value('@CITY_CODE','VARCHAR(12)')
--   , nref.value('@CODALMASRI','VARCHAR(5)'), nref.value('@CODALMA','VARCHAR(3)')
					
--		FROM @PI_ParamXML.nodes('/Root/PT_CENTROS') AS R(nref) 
--		WHERE NOT EXISTS(
--				SELECT TOP 1  1 FROM [Pedidos].[Ped_Almacen] A
--					WHERE A.CodAlmacen = nref.value('@ZSAPRETAIL','VARCHAR(4)')
--						AND A.CodLegacyOriginal = nref.value('@CODALMA','VARCHAR(3)'))
--				AND nref.value('@ZSAPRETAIL','VARCHAR(4)') <> '' AND nref.value('@CODALMA','VARCHAR(3)') <> ''
		--where  not exists(select top 1 1 from  Pedidos.Ped_AlmacenSAP b where b.idEmpresa= 1 and b.CodAlmacen= nref.value('@ZSAPRETAIL','VARCHAR(4)') )

INSERT INTO [Pedidos].[Ped_AlmacenSAP]
		(IdEmpresa, CodAlmacen, CodLegacy, NomAlmacen, CodCiudad, CodAlmacenSRI, CodLegacyOriginal)
		SELECT
			IdEmpresa = 1,
			CodAlmacen =
			CASE
					
					WHEN (LEN(CodAlmacen) >= 2 and SUBSTRING(CodAlmacen,1,2) = '00')
						THEN substring(CodAlmacen,3,2)	
				    WHEN (LEN(CodAlmacen) > 2 and SUBSTRING(CodAlmacen,1,1) = '0')
						THEN substring(CodAlmacen,2,2)		
					ELSE CodAlmacen
				END,
			
			CodLegacy =
				CASE
					WHEN (LEN(CodLegacy) > 2 and SUBSTRING(CodLegacy,1,1) = '0')
						THEN substring(CodLegacy,2,2)
					ELSE CodLegacy
				END,
			NomAlmacen = MIN(NomAlmacen),
			CodCiudad = MIN(CodCiudad),
			CodAlmacenSRI = MIN(CodAlmacenSRI),
			CodLegacyOriginal = CodLegacy
		FROM (
			SELECT
				CodAlmacen = nref.value('@WERKS','VARCHAR(4)'),  
				CodLegacy = CASE WHEN nref.value('@CODALMA','VARCHAR(3)') = '' THEN '0' ELSE nref.value('@CODALMA','VARCHAR(3)') END,
				NomAlmacen = nref.value('@NAME1','VARCHAR(30)'),
				CodCiudad = nref.value('@CITY_CODE','VARCHAR(12)'),
				CodAlmacenSRI = nref.value('@CODALMASRI','VARCHAR(5)')
			FROM @PI_ParamXML.nodes('/Root/PT_CENTROS') as R(nref)
			WHERE NOT EXISTS(
				SELECT TOP 1  1 FROM [Pedidos].[Ped_AlmacenSAP] A
					WHERE A.CodAlmacen = nref.value('@WERKS','VARCHAR(4)')
						AND A.CodLegacyOriginal = nref.value('@CODALMA','VARCHAR(3)'))
				AND nref.value('@WERKS','VARCHAR(4)') <> '' 
			) TBL
		GROUP BY CodAlmacen, CodLegacy


/*****************CATALOGO # 1 TIPO DE IDENTIFICACION*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_tipoIdentificacion'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_TIPONIF') AS R(nref) 
 on (nref.value('@J_1ATODC','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@J_1ATODC','VARCHAR(2)'), UPPER (nref.value('@TEXT30','VARCHAR(30)')) ,'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPONIF') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@J_1ATODC','VARCHAR(2)'))
 
end

/*****************CATALOGO # 1.1 TRATAMIENTO*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Tratamiento'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_TRATAMIENTO') AS R(nref) 
 on (nref.value('@TITLE','VARCHAR(4)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@TITLE','VARCHAR(4)'), UPPER (nref.value('@TITLE_MEDI','VARCHAR(30)')) ,'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_TRATAMIENTO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@TITLE','VARCHAR(4)'))
 delete from Proveedor.Pro_Catalogo where Codigo not in ('0001', '0002', '0005') and Tabla = @Tabla
end

/*****************CATALOGO # 2 CANTONES  *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_cantones'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_CANTONES') AS R(nref) 
 on (nref.value('@CANTON','VARCHAR(8)') IS NOT NULL)
where tabla = @Tabla


insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@CANTON','VARCHAR(8)') , UPPER (nref.value('@NOMBRE','VARCHAR(30)')),'A', nref.value('@PROVINCIA','VARCHAR(8)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CANTONES') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=  nref.value('@CANTON','VARCHAR(8)'))
 
end

/*****************CATALOGO # 2.1 CIUDADES  *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Ciudad'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_CIUDAD') AS R(nref) 
 on (nref.value('@CITY_CODE','VARCHAR(12)') IS NOT NULL)
where tabla = @Tabla

delete ciudadSAP from Pedidos.Ped_CiudadSAP ciudadSAP 
inner join @PI_ParamXML.nodes('/Root/PT_CIUDAD') AS R(nref) 
 on (nref.value('@CITY_CODE','VARCHAR(12)') IS NOT NULL)



insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@CITY_CODE','VARCHAR(12)') , UPPER (nref.value('@CITY_NAME','VARCHAR(40)')),'A', nref.value('@LAND1','VARCHAR(3)') + '-' + nref.value('@BLAND','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CIUDAD') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo= nref.value('@CITY_CODE','VARCHAR(12)') )


insert into Pedidos.Ped_CiudadSAP(idEmpresa,CodCiudad,NomCiudad, CodPais,CodProvincia)
select  1, nref.value('@CITY_CODE','VARCHAR(12)') , UPPER (nref.value('@CITY_NAME','VARCHAR(40)')), nref.value('@LAND1','VARCHAR(3)') , nref.value('@BLAND','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CIUDAD') AS R(nref) 
		where  not exists(select top 1 1 from  Pedidos.Ped_CiudadSAP b where b.idEmpresa= 1 and b.CodCiudad= nref.value('@CITY_CODE','VARCHAR(12)') )

end

/*****************CATALOGO # 3 LINEA DE NEGOCIO  *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_LineaNegocio'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_LINNEG_RETAIL') AS R(nref) 
 on (nref.value('@CLASS','VARCHAR(18)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@CLASS','VARCHAR(18)'), UPPER (nref.value('@KSCHL','VARCHAR(40)')),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_LINNEG_RETAIL') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@CLASS','VARCHAR(18)'))
 
end

/*****************CATALOGO # 4 CLASE DE IMPUESTO  *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_ClaseImpuesto'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_CLASIMP') AS R(nref) 
 on (nref.value('@J_1AFITP','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@J_1AFITP','VARCHAR(2)'), UPPER (nref.value('@TEXT60','VARCHAR(60)')),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_CLASIMP') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@J_1AFITP','VARCHAR(2)'))
 
end

/*****************CATALOGO # 5 IDIOMA  *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Idioma'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_IDIOMA') AS R(nref) 
 on (nref.value('@LAISO','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LAISO','VARCHAR(2)'), UPPER (nref.value('@SPTXT','VARCHAR(16)')),'A', nref.value('@SPRSL','VARCHAR(1)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_IDIOMA') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LAISO','VARCHAR(2)'))
 
end

/*****************CATALOGO # 6 SECTOR COMERCIAL  *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_SectorComercial'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_SECTCOM') AS R(nref) 
 on (nref.value('@MINDK','VARCHAR(3)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@MINDK','VARCHAR(3)'),UPPER (nref.value('@MTEXT','VARCHAR(30)')),'A', NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_SECTCOM') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MINDK','VARCHAR(3)'))
 
end

/*****************CATALOGO # 7 DEPARTAMENTO  *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_DepartaContacto'

if isnull(@Tabla,0)>0
begin
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_DEPARTAMENTO') AS R(nref) 
 on (nref.value('@ABTNR','VARCHAR(4)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@ABTNR','VARCHAR(4)'),UPPER (nref.value('@VTEXT','VARCHAR(20)')),'A', NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_DEPARTAMENTO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@ABTNR','VARCHAR(4)'))
 
end


/*****************CATALOGO # 8 PROVINCIAS  *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Provincias'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_PROVINCIAS') AS R(nref) 
 on (nref.value('@PROVINCIA','VARCHAR(8)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@PROVINCIA','VARCHAR(8)'),UPPER (nref.value('@NOMBRE','VARCHAR(30)')),'A', NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_PROVINCIAS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@PROVINCIA','VARCHAR(8)'))
 
end

/*****************CATALOGO # 9 RAMO *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Ramo'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_RAMO') AS R(nref) 
 on (nref.value('@BRSCH','VARCHAR(4)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@BRSCH','VARCHAR(4)'),UPPER(nref.value('@BRTXT','VARCHAR(20)')),'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_RAMO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@BRSCH','VARCHAR(4)'))
 
end

/*****************CATALOGO # 10 CUENTAS ASOCIADAS *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_CuentaAsociada'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_CUENTASOC') AS R(nref) 
 on (nref.value('@KTOKK','VARCHAR(4)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@KTOKK','VARCHAR(4)') + '-' + nref.value('@SAKNR','VARCHAR(10)'),UPPER(nref.value('@TXT20','VARCHAR(20)')),'A', nref.value('@SAKNR','VARCHAR(10)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CUENTASOC') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@KTOKK','VARCHAR(4)') + '-' + nref.value('@SAKNR','VARCHAR(10)'))
 
end


/*****************CATALOGO # 11 GRUPO TESORERIA *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_GrupoTesoreria'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_GRTESORERIA') AS R(nref) 
 on (nref.value('@GRUPP','VARCHAR(10)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@GRUPP','VARCHAR(10)') , UPPER(nref.value('@TEXTL','VARCHAR(30)')),'A', nref.value('@TEXTK','VARCHAR(10)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_GRTESORERIA') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@GRUPP','VARCHAR(10)'))
 
end



/*****************CATALOGO # 12 CONDICION PAGO *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_CondicionPago'

if isnull(@Tabla,0)>0
begin
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_CONDPAGO') AS R(nref) 
 on (nref.value('@ZTAGG','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@ZTAGG','VARCHAR(2)') + '-' + nref.value('@ZTERM','VARCHAR(4)') , UPPER(nref.value('@TEXT1','VARCHAR(50)')),'A', nref.value('@ZTAGG','VARCHAR(2)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CONDPAGO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo= nref.value('@ZTAGG','VARCHAR(2)') + '-' + nref.value('@ZTERM','VARCHAR(4)'))
 
end


/*****************CATALOGO # 13 VIA DE PAGO*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_ViaPago'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_VIAPAGO') AS R(nref) 
 on (nref.value('@ZLSCH','VARCHAR(1)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@ZLSCH','VARCHAR(1)'),UPPER(nref.value('@TEXT1','VARCHAR(30)')),'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_VIAPAGO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@ZLSCH','VARCHAR(1)'))
 
end


/*****************CATALOGO # 14 GRUPO DE CUENTAS *************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_GrupoCuenta'

if isnull(@Tabla,0)>0
begin
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_GRCTAS') AS R(nref) 
 on (nref.value('@KTOKK','VARCHAR(4)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@KTOKK','VARCHAR(4)'),UPPER(nref.value('@TXT30','VARCHAR(30)')),'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_GRCTAS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@KTOKK','VARCHAR(4)'))
 
end



/*****************CATALOGO # 15 FUNCION DE CONTACTO*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_FuncionContacto'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_FUNCION') AS R(nref) 
 on (nref.value('@PAFKT','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@PAFKT','VARCHAR(2)'),UPPER(nref.value('@VTEXT','VARCHAR(20)')),'A', nref.value('@ZZABTNR','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_FUNCION') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@PAFKT','VARCHAR(2)'))
 
end

/*****************CATALOGO # 16 TIPO RETENCION (IVA - FUENTE)*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_RetencionIva'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_TIPORETEN') AS R(nref) 
 on (nref.value('@WITHT','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla

insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@WITHT','VARCHAR(2)'),UPPER(nref.value('@TEXT40','VARCHAR(40)')),'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPORETEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@WITHT','VARCHAR(2)'))
 
end
delete from Proveedor.Pro_Catalogo where tabla = @Tabla and Detalle not like '%IVA%'
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_RetencionFuente'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_TIPORETEN') AS R(nref) 
 on (nref.value('@WITHT','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@WITHT','VARCHAR(2)'),UPPER(nref.value('@TEXT40','VARCHAR(40)')),'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPORETEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@WITHT','VARCHAR(2)'))
 
end
delete from Proveedor.Pro_Catalogo where tabla = @Tabla and Detalle not like '%FUENTE%'
/*****************CATALOGO # 17 INDICADOR RETENCION (IVA - FUENTE)*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_RetencionIva2'

if isnull(@Tabla,0)>0
begin
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_INDRETEN') AS R(nref) 
 on (nref.value('@WITHT','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@WITHT','VARCHAR(2)') +'-' +nref.value('@WT_WITHCD','VARCHAR(2)'),UPPER(nref.value('@TEXT40','VARCHAR(40)')),'A', nref.value('@WITHT','VARCHAR(2)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_INDRETEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo= nref.value('@WITHT','VARCHAR(2)') +'-' +nref.value('@WT_WITHCD','VARCHAR(2)') )
 
end

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_RetencionFuente2'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_INDRETEN') AS R(nref) 
 on (nref.value('@WITHT','VARCHAR(2)') IS NOT NULL)
where tabla = @Tabla
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@WITHT','VARCHAR(2)') +'-' +nref.value('@WT_WITHCD','VARCHAR(2)'),UPPER(nref.value('@TEXT40','VARCHAR(40)')),'A', nref.value('@WITHT','VARCHAR(2)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_INDRETEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo= nref.value('@WITHT','VARCHAR(2)') +'-' +nref.value('@WT_WITHCD','VARCHAR(2)') )
 
end



/*****************CATALOGO # 18 GRUPO DE COMPRAS*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_orgcomp'

if isnull(@Tabla,0)>0
begin
delete catal from Proveedor.Pro_Catalogo catal
inner join @PI_ParamXML.nodes('/Root/PT_ORGCOMP') AS R(nref) 
 on (nref.value('@EKORG','VARCHAR(4)') IS NOT NULL)
where tabla = @Tabla
--delete from Proveedor.Pro_Catalogo where tabla = @Tabla
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@EKORG','VARCHAR(4)'),UPPER(nref.value('@EKOTX','VARCHAR(20)')),'A', null
					
		FROM @PI_ParamXML.nodes('/Root/PT_ORGCOMP') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@EKORG','VARCHAR(4)'))
 
end





/* CATALOGOS PENDIENTES O QUE SE QUITARON
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Centros'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@WERKS','VARCHAR(4)'),nref.value('@NAME1','VARCHAR(30)'),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_CENTROS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@WERKS','VARCHAR(4)'))
 
end





Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Indareaalmacen'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@LGNUM','VARCHAR(3)') +'-'+nref.value('@LGBKZ','VARCHAR(3)'),nref.value('@LBKZT','VARCHAR(20)'),'A',nref.value('@LGNUM','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_INDAREAALM') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LGNUM','VARCHAR(3)') +'-'+nref.value('@LGBKZ','VARCHAR(3)'))
 
 end

 
 Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Almacen'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@WERKS','VARCHAR(4)') +'-'+ nref.value('@LGORT','VARCHAR(4)'),nref.value('@LGOBE','VARCHAR(16)'),'A',nref.value('@WERKS','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_ALMACEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@WERKS','VARCHAR(4)') +'-'+ nref.value('@LGORT','VARCHAR(4)'))
 
end

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_tipoMaterial_Art'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@MTART','VARCHAR(4)'),nref.value('@MTBEZ','VARCHAR(25)'),'A',nref.value('@MTREF','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPMAT') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MTART','VARCHAR(4)'))
 
end
*/


IF @@TRANCOUNT > 0
			COMMIT	TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Pro_P_GrabaCatalogos]'
END CATCH


