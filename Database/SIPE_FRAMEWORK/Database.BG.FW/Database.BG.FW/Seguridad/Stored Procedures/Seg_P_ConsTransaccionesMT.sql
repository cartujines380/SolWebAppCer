
CREATE Procedure [Seguridad].[Seg_P_ConsTransaccionesMT]
	@Parametros		xml
AS

SELECT
	  r.IdRol
	, r.IdOrganizacion
	, t.IdTransaccion
	, r.IdOpcion
	, t.Parametros
	--, t.NombreBase
	--, t.NombreSP
	, isnull(t.IdServidorExec, 0) as IdServidor
	, t.XmlEntrada
	, t.XmlSalida
	, t.Auditable
	, t.Monitor
	, o.IdAplicacion 
	, a.TipoServidor 
	, t.XmlValidador
INTO #OUTPUT
FROM		Seguridad.Seg_Transaccion t
 INNER JOIN Seguridad.Seg_Aplicacion a
  on t.IdServidorExec = a.IdAplicacion 
INNER JOIN	Seguridad.Seg_OpcionTransRol r
	ON		t.IdOrganizacion = r.IdOrganizacion
		AND	t.IdTransaccion = r.IdTransaccion
		AND r.IdRol IN (SELECT distinct nref.value('@IdRol','int') FROM @Parametros.nodes('//row') AS R(nref) 
)
INNER JOIN Seguridad.Seg_Organizacion o
on r.IdOrganizacion = o.IdOrganizacion 

WHERE t.Estado = 'A' AND t.Menu = 0
ORDER BY r.IdRol

SELECT * FROM #OUTPUT

SELECT o.IdServidor,
 (SELECT s.Parametro, s.Valor, s.Encriptado
  FROM Seguridad.Seg_ParamAplicacion s 
  WHERE s.IdAplicacion = o.IdServidor
  FOR XML RAW('item'), ROOT('params')) as params
FROM (SELECT distinct IdServidor FROM #OUTPUT) o
