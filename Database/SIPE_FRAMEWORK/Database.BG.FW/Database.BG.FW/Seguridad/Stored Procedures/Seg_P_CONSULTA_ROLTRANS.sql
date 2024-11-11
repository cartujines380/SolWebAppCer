
create procedure [Seguridad].[Seg_P_CONSULTA_ROLTRANS]
@PV_idRol           int
AS 

        SELECT  oprol.idorganizacion, 
		org.descripcion as DesOrganizacion, 
                oprol.idtransaccion , 
		trans.descripcion as "DesTransaccion",
               oprol.idopcion , 
		opci.descripcion as "DesOpcion"
        FROM Seguridad.Seg_OpcionTransRol oprol, Seguridad.Seg_transaccion trans, 
             Seguridad.Seg_organizacion org, Seguridad.Seg_opciontrans opci
        WHERE oprol.IdRol                  = @PV_idRol
        AND org.idorganizacion	           = oprol.idorganizacion
        and (oprol.idtransaccion           = oprol.idtransaccion
	      AND trans.idtransaccion 	         = oprol.idtransaccion
	      AND trans.idorganizacion           = oprol.idorganizacion)	 
        and (opci.idopcion		             = oprol.idopcion
	      AND opci.idtransaccion 	           = oprol.idtransaccion
	      AND opci.idorganizacion            = oprol.idorganizacion)







