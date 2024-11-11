


create PROCEDURE [Articulo].[Art_P_GrabaCatalogos] 
	 @PI_ParamXML xml
AS
BEGIN try
	begin tran
Declare @IdSolicitud bigint
declare  @Tabla int
/**************************************************************************************************************************************/
/*                  Carga:                       Articulos                                                                            */
/**************************************************************************************************************************************/

/*****************CATALOGO # 1 MARCAS*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_MarcaArticulo'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@BRAND_ID','VARCHAR(4)'), UPPER (nref.value('@BRAND_DESCR','VARCHAR(30)')),'A',nref.value('@BRAND_TYPE','VARCHAR(1)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_MARCAS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@BRAND_ID','VARCHAR(4)'))
 
end

/*****************CATALOGO # 2 PAIS*************/
Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Pais'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LAND1','VARCHAR(3)'),UPPER (nref.value('@LANDX50','VARCHAR(30)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_PAIS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LAND1','VARCHAR(3)'))
 
end

/*****************CATALOGO # 3 REGION*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Region'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LAND1','VARCHAR(3)') +'-'+nref.value('@BLAND','VARCHAR(3)'),UPPER (nref.value('@BEZEI','VARCHAR(20)')),'A',nref.value('@LAND1','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_REGION') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LAND1','VARCHAR(3)') +'-'+nref.value('@BLAND','VARCHAR(3)'))
 
end

/*****************CATALOGO # 4 GRADO ALCOHOL*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_GradoAlcohol'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@EXTWG','VARCHAR(18)'),UPPER (nref.value('@EWBEZ','VARCHAR(20)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_ALCOHOL') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@EXTWG','VARCHAR(18)'))
 
end

/*****************CATALOGO # 5 CLASIFICACION FISCAL(IVA)*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_ClasificacionFiscal'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@TAXKM','VARCHAR(1)') , UPPER (nref.value('@TAXKM','VARCHAR(1)') + '-' + nref.value('@VTEXT','VARCHAR(20)')),'A',nref.value('@TATYP','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CLASFIS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@TATYP','VARCHAR(4)'))
 
end

/*****************CATALOGO # 6 DEDUCIBLE*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Deducible'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@PLGTP','VARCHAR(4)')  , UPPER (nref.value('@PLGTP','VARCHAR(4)') + '-' + nref.value('@VTEXT','VARCHAR(20)')),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_DEDUCIBLE') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@PLGTP','VARCHAR(4)'))
 
end

/*****************CATALOGO # 7 INDICADOR DE RETENCION*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_IndicadorReten'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LABOR','VARCHAR(3)'), UPPER (nref.value('@LABOR','VARCHAR(3)') +'-'+nref.value('@LBTXT','VARCHAR(30)')),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_LABOR') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LABOR','VARCHAR(3)'))
 
end

/*****************CATALOGO # 8 UNIDAD DE MEDIDA*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_UnidadMedidaArt'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@MSEHI','VARCHAR(3)'),UPPER (nref.value('@MSEHL','VARCHAR(30)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_UNIMED') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MSEHI','VARCHAR(3)'))
 
end

--Catalago de Unidad Medida Conversion

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_UniMedConverArt'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@MSEHI','VARCHAR(3)'),UPPER (nref.value('@MSEHL','VARCHAR(30)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_UNIMED') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MSEHI','VARCHAR(3)'))
 
end

/*****************CATALOGO # 9 TIPOS DE EAN*************/

Select @Tabla = 0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_TiposEAN'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@NUMTP','VARCHAR(2)'),UPPER (nref.value('@NTBEZ','VARCHAR(40)')),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPOEAN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@NUMTP','VARCHAR(2)'))
 
end

/*****************CATALOGO # 10 ORGANIZACION DE COMPRAS*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Orgcompras'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@BUKRS','VARCHAR(4)'),UPPER (nref.value('@BUKRS','VARCHAR(4)')+'-'+nref.value('@BUTXT','VARCHAR(25)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_SOCIEDADES') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@BUKRS','VARCHAR(4)'))
 
end


/*****************CATALOGO # 11 FRECUENCIA DE ENTREGA*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_FrecEntrega'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 select  @Tabla,ltrim(rtrim(nref.value('@WERKS','VARCHAR(4)'))) +'-'+ltrim(rtrim(nref.value('@MRPPP','VARCHAR(3)'))),UPPER (nref.value('@PPTXT','VARCHAR(40)')),'A',nref.value('@WERKS','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_FRECUENCIA') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=ltrim(rtrim(nref.value('@WERKS','VARCHAR(4)'))) +'-'+ltrim(rtrim(nref.value('@MRPPP','VARCHAR(3)'))))
 
end

/*****************CATALOGO # 12 TIPO DE MATERIAL*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_tipoMaterial_Art'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@MTART','VARCHAR(4)'),UPPER (nref.value('@MTART','VARCHAR(4)')+'-'+nref.value('@MTBEZ','VARCHAR(25)')),'A',nref.value('@MTREF','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPMAT') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MTART','VARCHAR(4)'))
 
end

/*****************CATALOGO # 13 CATEGORIA DE MATERIAL*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_CatMaterial'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@CODCATMAT','VARCHAR(10)'),UPPER (nref.value('@CODCATMAT','VARCHAR(10)')+'-'+nref.value('@DESCRIPCION','VARCHAR(60)')),'A',NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_CATMAT') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@CODCATMAT','VARCHAR(10)'))
 
end

/*****************CATALOGO # 14 GRUPO DE ARTICULOS*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_GrupoArticulo'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@MATKL','VARCHAR(9)'),UPPER (nref.value('@MATKL','VARCHAR(9)')+'-'+nref.value('@WGBEZ','VARCHAR(20)')),'A',NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_GRART') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MATKL','VARCHAR(9)'))
 
end

/*****************CATALOGO # 15 SURTIDO PARCIAL*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_SurtParcial'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LTSNR','VARCHAR(6)'),UPPER (nref.value('@LTSBZ','VARCHAR(20)')),'A',nref.value('@LIFNR','VARCHAR(20)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_SURTIDO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LTSNR','VARCHAR(6)'))
 
end


/*****************CATALOGO # 16 MATERIAS*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Materia_Art'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,convert(varchar,getdate(),12) + convert(varchar, ROW_NUMBER() OVER(ORDER BY nref.value('@WRKST','VARCHAR(48)') DESC))  ,UPPER (nref.value('@WRKST','VARCHAR(48)')),'A',NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_MATERIAS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Detalle=nref.value('@WRKST','VARCHAR(48)'))
 
end

/*****************CATALOGO # 17 INDICADOR DE PEDIDO*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_IndPedido'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@SERVV','VARCHAR(2)'),UPPER (nref.value('@SERVV','VARCHAR(2)')+'-'+nref.value('@VTEXT','VARCHAR(20)')),'A',NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_INDPEDIDO') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@SERVV','VARCHAR(2)'))
 
end

/*****************CATALOGO # 18 GRUPO DE COMPRAS*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_GrupoCompras'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@EKGRP','VARCHAR(3)'),UPPER (nref.value('@EKGRP','VARCHAR(3)')+'-'+nref.value('@EKNAM','VARCHAR(18)')),'A',NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_GRCOMPRAS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@EKGRP','VARCHAR(3)'))
 
end

/*****************CATALOGO # 19 CATEGORIA VALORACION*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_CatValoracion'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@BKLAS','VARCHAR(4)'), UPPER (nref.value('@BKLAS','VARCHAR(4)')+'-'+nref.value('@BKBEZ','VARCHAR(25)')),'A',nref.value('@KKREF','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CATVAL') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@BKLAS','VARCHAR(4)'))
 
end

/*****************CATALOGO # 20 CONDICION ALMACEN*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Condalmacen'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@RAUBE','VARCHAR(2)'),UPPER (nref.value('@RAUBE','VARCHAR(2)')+'-'+nref.value('@RBTXT','VARCHAR(20)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_CONDALMACEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@RAUBE','VARCHAR(2)'))
 
end

/*****************CATALOGO # 21 CLASE LISTA SURTIDOS*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Cllistsurtidos'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@BBTYP','VARCHAR(1)'),UPPER (nref.value('@BBTYP','VARCHAR(1)')+'-'+nref.value('@BBTEXT','VARCHAR(20)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_CLASLISTSURT') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@BBTYP','VARCHAR(1)'))
 
end

/*****************CATALOGO # 22 ESTATUS MATERIAL*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Estatusmaterial'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@MMSTA','VARCHAR(2)'),UPPER (nref.value('@MMSTA','VARCHAR(2)')+'-'+nref.value('@MTSTB','VARCHAR(20)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_STATUSMAT') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MMSTA','VARCHAR(2)'))
 
end

/*****************CATALOGO # 23 ESTATUS VENTAS*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Estatusventas'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@VMSTA','VARCHAR(4)'),UPPER (nref.value('@VMSTA','VARCHAR(4)')+'-'+nref.value('@VMSTB','VARCHAR(20)')),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_STATUSCD') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@VMSTA','VARCHAR(4)'))
 
end

/*****************CATALOGO # 24 PERFIL DISTRIBUCION*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_PerfDistribucion'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@FPRFM','VARCHAR(3)'),UPPER (nref.value('@FPRFM','VARCHAR(3)')+'-'+nref.value('@FPRTX','VARCHAR(40)')),'A',NULL
					
		FROM @PI_ParamXML.nodes('/Root/PT_PERFIL') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@FPRFM','VARCHAR(3)'))
 
end

/*****************CATALOGO # 25 NACIONAL-IMPORTADO*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Nacional_Importado'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@PRODH','VARCHAR(18)'),nref.value('@PRODH','VARCHAR(18)')+'-'+nref.value('@VTEXT','VARCHAR(40)'),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_JERARQUIAS') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@PRODH','VARCHAR(18)'))
 
end

/*****************CATALOGO # 26 NUMERO ALMACEN - PT_LGNUM*************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_lgnum'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LGNUM','VARCHAR(3)'),nref.value('@LGNUM','VARCHAR(3)')+'-'+nref.value('@LNUMT','VARCHAR(25)'),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_LGNUM') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LGNUM','VARCHAR(3)'))
 
end

/*****************CATALOGO # 27 TIPO ALMACEN *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Tipoalmacen'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@LGNUM','VARCHAR(3)')+'-'+ nref.value('@LGTYP','VARCHAR(3)'),nref.value('@LGTYP','VARCHAR(3)')+'-'+nref.value('@LTYPT','VARCHAR(25)'),'A',nref.value('@LGNUM','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_TIPOALM') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LGNUM','VARCHAR(3)')+'-'+ nref.value('@LGTYP','VARCHAR(3)'))
 
 end

 /*****************CATALOGO # 28 INDICADOR TIPO ALMACEN E/S *************/

 Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Indtipoalment'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@LGNUM','VARCHAR(3)') +'-'+nref.value('@LGTKZ','VARCHAR(3)'),nref.value('@LGTKZ','VARCHAR(3)')+'-'+nref.value('@LTKZT','VARCHAR(20)'),'A',nref.value('@LGNUM','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_INDTIPOALM') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LGNUM','VARCHAR(3)') +'-'+nref.value('@LGTKZ','VARCHAR(3)'))
 
 end

 Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Indtipoalmsal'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@LGNUM','VARCHAR(3)')+'-'+nref.value('@LGTKZ','VARCHAR(3)'),nref.value('@LGTKZ','VARCHAR(3)')+'-'+nref.value('@LTKZT','VARCHAR(20)'),'A',nref.value('@LGNUM','VARCHAR(3)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_INDTIPOALM') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@LGNUM','VARCHAR(3)')+'-'+nref.value('@LGTKZ','VARCHAR(3)'))
 
 end

 /*****************CATALOGO # 29 GRUPO BALANZAS *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Grupobalanzas'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@SCAGR','VARCHAR(4)'),nref.value('@SCAGR','VARCHAR(4)')+'-'+nref.value('@BEZEI20','VARCHAR(20)'),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_GRUPOBALANZA') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@SCAGR','VARCHAR(4)'))
 
end


 /*****************CATALOGO # 30 CANAL DISTRIBUCION *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Canaldistribucion'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@VTWEG','VARCHAR(2)'),nref.value('@VTWEG','VARCHAR(2)')+'-'+nref.value('@VTEXT','VARCHAR(20)'),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_DISTCAN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@VTWEG','VARCHAR(2)'))
 
end

 /*****************CATALOGO # 31 CATALOGACION *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Catalogacion'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla, nref.value('@ASORT','VARCHAR(10)'),nref.value('@ASORT','VARCHAR(10)')+'-'+nref.value('@NAME1','VARCHAR(40)'),'A',nref.value('@VTWEG','VARCHAR(2)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_CATALOGACION') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@ASORT','VARCHAR(10)'))
 
end

/*****************CATALOGO # 32 COLECCION *************/

Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_ColeccionArtic'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

 select  @Tabla, nref.value('@SAITY','VARCHAR(2)'),nref.value('@VTEXT','VARCHAR(40)'),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_COLECCION') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@SAITY','VARCHAR(4)'))
 
 end

 /*****************CATALOGO # 33 TEMPORADA *************/

 Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_Temporada'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

 select  @Tabla, nref.value('@SAISO','VARCHAR(4)'),nref.value('@VTEXT','VARCHAR(20)'),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_TEMPORADA') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@SAISO','VARCHAR(4)'))
 
 end

 /*****************CATALOGO # 34 VOLUMEN *************/

 Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_volumen'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

 select  @Tabla, nref.value('@MSEHI','VARCHAR(3)'),nref.value('@MSEHT','VARCHAR(10)'),'A',null
					
		FROM @PI_ParamXML.nodes('/Root/PT_VOLUMEN') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@MSEHI','VARCHAR(3)'))
 
 end

 
 /*****************CATALOGO # 35 VOLUMEN *************/

 Select @Tabla =0
select @Tabla=Tabla from Proveedor.Pro_Tabla a 
where a.TablaNombre='tbl_tipoAlmBod'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

 select  @Tabla, nref.value('@WERKS','VARCHAR(4)') +'-' + nref.value('@LGORT','VARCHAR(4)'),nref.value('@LGOBE','VARCHAR(16)'),'A',nref.value('@WERKS','VARCHAR(4)')
					
		FROM @PI_ParamXML.nodes('/Root/PT_ALMACENES') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@WERKS','VARCHAR(4)') +'-' + nref.value('@LGORT','VARCHAR(4)'))
 
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
where a.TablaNombre='tbl_LineaNegocio'

if isnull(@Tabla,0)>0
begin
insert into Proveedor.Pro_Catalogo(Tabla, Codigo,Detalle, Estado,DescAlterno)
 

select  @Tabla,nref.value('@ZZLINNEG','VARCHAR(2)'),nref.value('@DELINNEG','VARCHAR(40)'),'A', ''
					
		FROM @PI_ParamXML.nodes('/Root/PT_LINEG') AS R(nref) 
		where  not exists(select top 1 1 from Proveedor.Pro_Catalogo b where b.Tabla=@Tabla and b.Codigo=nref.value('@ZZLINNEG','VARCHAR(2)'))
 
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
		
		exec SP_PROV_Error @sp='[Art_P_GrabaCatalogos]'
END CATCH


