
/*
declare @dato varchar(8000)
--set @dato = '<Usuario PS_IdUsuario="victormunoz" PS_Token="L77YQS5TQHX90YU3FISMO/SJVMR5IDTZ" PS_Maquina="127.0.0.1" PS_UsrSitio="BC574078432C8835C78E3F67674500F8" PS_TokenSitio="A958CD89BC72772214C3EAEA7887D2E0C332284FDD73D88A496B06FE9B783FA6FEE7F279B826F7F1" PS_MaqSitio="SIPECOM50" PS_IdEmpresa="1" PS_IdSucursal="5" PS_IdAplicacion="1" PS_Formulario="Seguridad.Seg_frmAuditoriaTransaccion.aspx" PS_Login="S" IdEmpresa="1" IdSucursal="5" IdAplicacion="1" Aplicacion="WebBAustroOnline" IdOrganizacion="1" Organizacion="Seguridad" IdTransaccion="115" Transaccion="Seguridad.Seg_P_INGRESA_APLICACION" FechaInicial="10-03-2006 00:00:01" FechaFinal="10-03-2006 23:59:59" />'
set @dato = '<?xml version="1.0" encoding="iso-8859-1"?><Usuario PS_IdUsuario="admin" PS_Token="7C6DCB65FD7E41259E3ECED456BEAF17C09DF679DEE3A5AA" PS_Maquina="127.0.0.1" PS_UsrSitio="ECB97DBC41BE797161C589BAF6996F70" PS_TokenSitio="A958CD89BC72772214C3EAEA7887D2E0C332284FDD73D88A496B06FE9B783FA6FEE7F279B826F7F1" PS_MaqSitio="SIPECOM50" PS_IdEmpresa="2" PS_IdSucursal="3" PS_IdAplicacion="1" PS_Formulario="seg_frmAuditoriaTransaccion.aspx" PS_Login="S" IdEmpresa="2" IdSucursal="3" IdAplicacion="0" Aplicacion="Todos" IdOrganizacion="0" Organizacion="Todos" IdTransaccion="0" Transaccion="Todos" FechaInicial="03-05-2007 00:00:01" FechaFinal="03-05-2007 23:59:59" />'
exec Seguridad.Seg_P_ConsAuditoriaTransacciones @dato

*/

CREATE PROCEDURE [Seguridad].[Seg_P_ConsAuditoriaTransacciones]
	( @PI_DocXML varchar (max) )
AS
DECLARE @VL_IdXML int
DECLARE @VL_IdEmpresa int, @VL_IdSucursal int,@VL_IdAplicacion int, @VL_IdOrganizacion int, @VL_IdTransaccion int
DECLARE @VL_Aplicacion varchar(100), @VL_Organizacion varchar(100), @VL_Transaccion varchar(100)
DECLARE @VL_FechaInicial datetime,  @VL_FechaFinal datetime
--Inicia la transaccion
EXEC SP_XML_PREPAREDOCUMENT @VL_IdXML OUTPUT, @PI_docXML
--Obtengo Empresa y Cliente
SELECT
	@VL_IdEmpresa		= IdEmpresa
	,@VL_IdSucursal		= IdSucursal
	,@VL_IdAplicacion	= IdAplicacion
	,@VL_Aplicacion		= Aplicacion
	,@VL_IdOrganizacion = IdOrganizacion --(Producto)
	,@VL_Organizacion	= Organizacion
	,@VL_IdTransaccion	= IdTransaccion
	,@VL_Transaccion	= Transaccion
	,@VL_FechaInicial	= FechaInicial
	,@VL_FechaFinal		= FechaFinal
FROM	OPENXML (@VL_IdXML, '/Usuario')
			WITH ( IdEmpresa int,	IdSucursal int,IdAplicacion int, Aplicacion varchar(100),
									IdOrganizacion int, Organizacion varchar(100),
									IdTransaccion int, Transaccion varchar(100),
									FechaInicial datetime, FechaFinal datetime)
IF (@@error <> 0)
BEGIN
	RETURN
END
--Libera el documento XML
EXEC SP_XML_REMOVEDOCUMENT @VL_IdXML
	SELECT  distinct	a.FechaMovi		,
			a.IdUsuario		,
			a.IdAplicacion	,
			ap.Descripcion  Aplicacion,
			a.IdOrganizacion,
			o.Descripcion	Organizacion,
			a.IdTransaccion ,
			t.Descripcion	Transaccion,
			a.IdIdentificacion Maquina,
			convert(varchar(max),a.txtTransaccion) TxtTransaccion,
			r.IdEmpresa	,
			r.IdSucursal
	FROM	Seguridad.Seg_Auditoria a
			INNER JOIN Seguridad.Seg_Aplicacion	  ap  ON (a.IdAplicacion    = ap.IdAplicacion)
			INNER JOIN Seguridad.Seg_Organizacion   o   ON (a.IdOrganizacion  = o.IdOrganizacion AND o.IdAplicacion     = ap.IdAplicacion)
			INNER JOIN Seguridad.Seg_Transaccion    t   ON (a.IdTransaccion   = t.IdTransaccion  AND t.IdOrganizacion   = o.IdOrganizacion)
			INNER JOIN Seguridad.Seg_OpcionTrans    ot  ON (ot.IdTransaccion  = t.IdTransaccion  AND ot.IdOrganizacion  = o.IdOrganizacion)
			INNER JOIN Seguridad.Seg_OpcionTRansRol otr ON (otr.IdTransaccion = t.IdTransaccion  AND otr.IdOrganizacion = o.IdOrganizacion)
			INNER JOIN Seguridad.Seg_Rol            r   ON (r.IdRol = otr.IdRol)
	WHERE	r.IdEmpresa = @VL_IdEmpresa AND (r.IdSucursal = 0 OR r.IdSucursal = @VL_IdSucursal)
      AND	(@VL_IdAplicacion   = 0 OR a.IdAplicacion   = @VL_IdAplicacion)
	  AND	(@VL_IdOrganizacion = 0 OR a.IdOrganizacion = @VL_IdOrganizacion)
	  AND	(@VL_IdTransaccion  = 0 OR a.IdTransaccion  = @VL_IdTransaccion)
	  AND	a.FechaMovi BETWEEN @VL_FechaInicial AND @VL_FechaFinal





