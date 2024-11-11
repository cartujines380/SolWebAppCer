
CREATE procedure [Seguridad].[Seg_P_CONSULTA_TRANSACCIONID]
@PV_idTransaccion  int,
      @PV_idOrganizacion     int
AS
  declare @In_Count int
  select @In_Count = ISNULL(max(idOpcion),0)
     from Seguridad.Seg_Opciontrans
   where IdTransaccion= @PV_idTransaccion and
         IdOrganizacion= @PV_idOrganizacion

     SELECT trans.estado,
            trans.descripcion, 
            org.idorganizacion,
            org.descripcion as DesOrganizacion,
	    Auditable, @In_Count as MaximaOpcion, Parametros,
	    convert(varchar,isnull(apl.TipoServidor,0)) + '-' + convert(varchar,isnull(trans.IdServidor,0)) as IdServidor
	    ,NombreBase,NombreSP,IdServidorExec
		,isnull(Menu,0) Menu,isnull(Monitor,0) Monitor,XmlEntrada,XmlSalida, XmlValidador 
        FROM Seguridad.Seg_TRANSACCION trans 
				INNER JOIN Seguridad.Seg_ORGANIZACION org
				 ON org.Idorganizacion = trans.idorganizacion
				LEFT OUTER JOIN Seguridad.Seg_Aplicacion apl
				  ON apl.IdAplicacion = trans.IdServidor
     WHERE trans.idtransaccion=@PV_idTransaccion
     AND trans.idorganizacion = @PV_idOrganizacion
   
  



