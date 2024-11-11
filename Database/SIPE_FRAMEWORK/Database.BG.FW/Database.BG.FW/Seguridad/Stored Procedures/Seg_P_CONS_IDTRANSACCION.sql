



create procedure [Seguridad].[Seg_P_CONS_IDTRANSACCION]
@PV_idUsuario                 int,
    @PV_idAplicacion              int
AS
       SELECT tr.idTransaccion
       FROM Seguridad.Seg_ROLUSUARIO rus, Seguridad.Seg_OPCIONTRANSROL otr,
		Seguridad.Seg_TRANSACCION tr, Seguridad.Seg_ORGANIZACION org
       WHERE rus.idUsuario=@PV_idUsuario            AND
             rus.idRol=otr.idRol                   AND
             otr.idTransaccion=tr.idTransaccion    AND
         --    tr.tipo='M'  AND
             otr.idOrganizacion=org.idorganizacion AND
             org.idAplicacion=@PV_idAplicacion

  





