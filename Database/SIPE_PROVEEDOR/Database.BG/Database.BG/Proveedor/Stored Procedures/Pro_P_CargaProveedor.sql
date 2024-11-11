CREATE  PROCEDURE [Proveedor].[Pro_P_CargaProveedor]
	@PI_ParamXML xml
AS
SET ARITHABORT ON;
BEGIN TRY
	BEGIN TRAN

	DECLARE @W_CODIGOPROVEEDOR VARCHAR(20)

	SELECT @W_CODIGOPROVEEDOR=MAX(CodProveedor)+1
	FROM Proveedor.Pro_Proveedor

	UPDATE A
   SET 

    [Ruc] = nref.value('@STCD1','VARCHAR(13)'),
	[TipoProveedor] = nref.value('@KTOKK','VARCHAR(13)'),
	[NomComercial]= nref.value('@NAME1','VARCHAR(35)'),
	[DirCalleNum]= nref.value('@STRAS','VARCHAR(30)'),
	[DirPisoEdificio] = nref.value('@FLOOR','VARCHAR(10)'),
	[DirCallePrinc] = nref.value('@STR_SUPPL3','VARCHAR(40)'),
	DirDistrito = nref.value('@ORT02','VARCHAR(35)'),
	DirCodPostal = nref.value('@PSTLZ','VARCHAR(10)'),
	Poblacion = nref.value('@ORT01','VARCHAR(35)'),
	Pais = nref.value('@LAND1','VARCHAR(3)'), 
	Region = nref.value('@REGIO','VARCHAR(3)'),
	Idioma = nref.value('@SPRAS','VARCHAR(1)'),
	Telefono = nref.value('@TELF1','VARCHAR(16)'),
	Movil= nref.value('@TELF2','VARCHAR(16)'), 
	Fax = nref.value('@TELFX','VARCHAR(31)'),
	CorreoE = nref.value('@SMTP_ADDR','VARCHAR(241)'), 
	GenDocElec = nref.value('@ZZDOCELEC','VARCHAR(1)'), 
	FechaCertifica = nref.value('@CERDT','datetime'),
	IndMinoria = nref.value('@MINDK','VARCHAR(3)'),  
	ApoderadoNom = nref.value('@ZZIOPLN','VARCHAR(100)'),
	ApoderadoApe = nref.value('@ZZIOPLA','VARCHAR(100)'),
	[ApoderadoIdFiscal] = nref.value('@ZZSTCDAP','VARCHAR(16)'), 
	[PlazoEntregaPrev] = nref.value('@PLIFZ','VARCHAR(3)'),	
	[FechaMod] = nref.value('@FECHAMOD','datetime')

        from [Proveedor].[Pro_Proveedor] A
	  inner join @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV') as R(nref) on A.CodProveedor=@W_CODIGOPROVEEDOR--nref.value('@LIFNR','VARCHAR(10)')





INSERT INTO [Proveedor].[Pro_Proveedor]
           ([CodProveedor]            ,[Ruc]                       ,[TipoProveedor]           ,[NomComercial]
           ,[DirCalleNum]             ,[DirPisoEdificio]           ,[DirCallePrinc]           ,[DirDistrito]
           ,[DirCodPostal]           ,[Poblacion]                 ,[Pais]            ,[Region]
           ,[Idioma]           ,[Telefono]           ,[Movil]           ,[Fax]           ,[CorreoE]
           ,[GenDocElec]           ,[FechaCertifica]           ,[IndMinoria]           ,[ApoderadoNom]
           ,[ApoderadoApe]           ,[ApoderadoIdFiscal]           ,[PlazoEntregaPrev]           ,[FechaMod])

	select distinct
	CodProveedor=@W_CODIGOPROVEEDOR,--nref.value('@LIFNR','VARCHAR(10)'),
    [Ruc] = nref.value('@STCD1','VARCHAR(13)'),
	[TipoProveedor] = nref.value('@KTOKK','VARCHAR(13)'),
	[NomComercial]= nref.value('@NAME1','VARCHAR(35)'),
	[DirCalleNum]= nref.value('@STRAS','VARCHAR(30)'),
	[DirPisoEdificio] = nref.value('@FLOOR','VARCHAR(10)'),
	[DirCallePrinc] = nref.value('@STR_SUPPL3','VARCHAR(40)'),
	DirDistrito = nref.value('@ORT02','VARCHAR(35)'),
	DirCodPostal = nref.value('@PSTLZ','VARCHAR(10)'),
	Poblacion = nref.value('@ORT01','VARCHAR(35)'),
	Pais = nref.value('@LAND1','VARCHAR(3)'), 
	Region = nref.value('@REGIO','VARCHAR(3)'),
	Idioma = nref.value('@SPRAS','VARCHAR(1)'),
	Telefono = nref.value('@TELF1','VARCHAR(16)'),
	Movil= nref.value('@TELF2','VARCHAR(16)'), 
	Fax = nref.value('@TELFX','VARCHAR(31)'),
	CorreoE = nref.value('@SMTP_ADDR','VARCHAR(241)'), 
	GenDocElec = nref.value('@ZZDOCELEC','VARCHAR(1)'), 
	FechaCertifica = convert(datetime, nref.value('@CERDT','varchar(10)'),111),
	IndMinoria = nref.value('@MINDK','VARCHAR(3)'),  
	ApoderadoNom = nref.value('@ZZIOPLN','VARCHAR(100)'),
	ApoderadoApe = nref.value('@ZZIOPLA','VARCHAR(100)'),
	[ApoderadoIdFiscal] = nref.value('@ZZSTCDAP','VARCHAR(16)'), 
	[PlazoEntregaPrev] = nref.value('@PLIFZ','VARCHAR(3)'),	
	[FechaMod] = convert(datetime, nref.value('@FECHAMOD','varchar(10)'),111)

	from @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV') as R(nref)
	where not exists(select top 1  1 from [Proveedor].[Pro_Proveedor] a  where  a.CodProveedor=nref.value('@LIFNR','VARCHAR(10)'))
	


	UPDATE A
   SET 
   	[Tratamiento] = nref.value('@ANRED','VARCHAR(30)'),
	[NomPila]= nref.value('@NAMEV','VARCHAR(35)'), 
	[Nombre] = nref.value('@NAME1','VARCHAR(35)'),
	[DepCliente] = nref.value('@ABTPA','VARCHAR(12)'),
	[Departamento] = nref.value('@ABTNR','VARCHAR(4)'),
	[Funcion] = nref.value('@PAFKT','VARCHAR(2)'),
	[Telefono1]= nref.value('@TELF1','VARCHAR(16)'),
	[Telefono2] = nref.value('@TEL_NUMBER','VARCHAR(30)'),
	CorreoE = nref.value('@SMTP_ADDR','VARCHAR(241)')

          from [Proveedor].[Pro_ProveedorContacto] A
	  inner join @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV/CONTACT') as R(nref) on A.CodProveedor=@W_CODIGOPROVEEDOR--nref.value('@LIFNR','VARCHAR(10)') and 
	  AND A.CodContacto=nref.value('@PARNR','VARCHAR(15)')



   

	insert into  [Proveedor].[Pro_ProveedorContacto](

	[CodProveedor] ,
	[CodContacto]  ,
	[Tratamiento],
	[NomPila],
	[Nombre],
	[DepCliente],
	[Departamento] ,
	[Funcion],
	[Telefono1] ,
	[Telefono2],
	CorreoE)
	select distinct
	[CodProveedor]= @W_CODIGOPROVEEDOR,--nref.value('@LIFNR','VARCHAR(10)'), 
   	[CodContacto] = nref.value('@PARNR','VARCHAR(15)'),

	[Tratamiento] = nref.value('@ANRED','VARCHAR(30)'),
	[NomPila]= nref.value('@NAMEV','VARCHAR(35)'), 
	[Nombre] = nref.value('@NAME1','VARCHAR(35)'),
	[DepCliente] = nref.value('@ABTPA','VARCHAR(12)'),
	[Departamento] = nref.value('@ABTNR','VARCHAR(4)'),
	[Funcion] = nref.value('@PAFKT','VARCHAR(2)'),
	[Telefono1]= nref.value('@TELF1','VARCHAR(16)'),
	[Telefono2] = nref.value('@TEL_NUMBER','VARCHAR(30)'),
	CorreoE = nref.value('@SMTP_ADDR','VARCHAR(241)')

	from @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV/CONTACT') as R(nref)
	where not exists(select top 1  1 from [Proveedor].[Pro_ProveedorContacto] a  where  A.CodProveedor=@W_CODIGOPROVEEDOR AND --nref.value('@LIFNR','VARCHAR(10)') and 
	  A.CodContacto=nref.value('@PARNR','VARCHAR(15)'))



	UPDATE A
   SET 
   	
	[CuentaTitular]= nref.value('@KOINH','VARCHAR(60)')
	
   from [Proveedor].[Pro_ProveedorBanco] A
	  inner join @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV/BANK') as R(nref) on A.CodProveedor=@W_CODIGOPROVEEDOR AND --nref.value('@LIFNR','VARCHAR(10)') and 
	  A.CodBanco=nref.value('@BANKL','VARCHAR(15)') and [CuentaNum] = isnull( nref.value('@BANKN','VARCHAR(18)'),'')

 
	
	insert into  [Proveedor].[Pro_ProveedorBanco](

	[CodProveedor] ,
	[CodBanco],
	[CuentaNum],
	[CuentaTitular])
	select distinct
	[CodProveedor]= @W_CODIGOPROVEEDOR,--nref.value('@LIFNR','VARCHAR(10)'), 
   	CodBanco = nref.value('@BANKL','VARCHAR(15)'),
	[CuentaNum] =isnull( nref.value('@BANKN','VARCHAR(18)'),''),
	[CuentaTitular]= nref.value('@KOINH','VARCHAR(60)')

	
	from @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV/BANK') as R(nref)
	where not exists(select top 1  1 from [Proveedor].[Pro_ProveedorBanco] a  where  A.CodProveedor=@W_CODIGOPROVEEDOR AND --nref.value('@LIFNR','VARCHAR(10)') and 
	  A.CodBanco=nref.value('@BANKL','VARCHAR(15)') and  [CuentaNum] = isnull( nref.value('@BANKN','VARCHAR(18)'),''))



--LIFNR	CHAR	10	ID de Proveedor	CodProveedor
--BANKL	CHAR	15	Código bancario	CodBanco
--BANKN	CHAR	18	Nº cuenta bancaria	CuentaNum
--KOINH	CHAR	60	Titular de la cuenta	CuentaTitular


	insert into [Proveedor].[Pro_ProveedorLegacy](
[CodProveedor],[CodLegacy],Principal)
	select distinct

   	[CodProveedor] = nref.value('@LIFNR','VARCHAR(10)'),
	[CodLegacy] = nref.value('@KOLIF','VARCHAR(10)'),
	'Z'
	from @PI_ParamXML.nodes('/CARGAPROPVEEDOR/PROV/LEGACY') as R(nref)
	where not exists(select top 1  1 from [Proveedor].[Pro_ProveedorLegacy] a  where  A.CodProveedor=nref.value('@LIFNR','VARCHAR(10)') and 
	  A.CodLegacy=nref.value('@KOLIF','VARCHAR(10)') )

	update a
	set Principal='X'
	from   [Proveedor].[Pro_ProveedorLegacy] a
	where a.CodLegacy not IN (select b.CodLegacy from SIPE_PROVEEDOR.Proveedor.Pro_ProveedorLegacy b  group by CodLegacy having COUNT(1) > 1)
        and Principal='Z' and  isnumeric(CodLegacy)=1 and  len(CodLegacy)<5


	update a
	set Principal=''
	from   [Proveedor].[Pro_ProveedorLegacy] a
	where Principal='Z'
 
 --UPDATE [Pedidos].[Ped_Pedido]
	--		SET Estado = 'PE', CodProveedor = l.CodProveedor
	--	FROM [Pedidos].[Ped_Pedido] p
	--		INNER JOIN [Proveedor].[Pro_ProveedorLegacy] l
	--			ON p.CodProveedor = l.CodLegacy and l.Principal ='X' 
	--		WHERE p.Estado = 'ER'

IF @@TRANCOUNT > 0
		COMMIT	TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0
		ROLLBACK TRAN
		
		exec SP_PROV_Error @sp='[Pro_P_CargaProveedor]'
END CATCH


