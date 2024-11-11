
CREATE procedure [Seguridad].[Seg_P_CONSULTA_TRANUSUARIO] 
@PV_Codusuario      VARCHAR(20 )
AS
          SELECT distinct tu.idtransaccion,t.descripcion "DesTransaccion",
		 tu.IdOpcion ,ot.descripcion "DesOpcion",
		tu.idorganizacion , o.descripcion "DesOrganizacion",   
		tu.idhorario ,h.descripcion "DesHorario",
                convert(varchar,tu.fechainicial,110) as "FechaInicial", 
                convert(varchar,tu.fechafinal,110) as "FechaFinal", 
		tu.estado , tu.tipoidentificacion , 
                tu.ididentificacion , 
		tu.usrreemplaza , 
		Participante.Par_F_getNombreUsuario(tu.usrreemplaza) as NomUserReemp
         FROM Seguridad.Seg_TransUsuario tu,
              Seguridad.Seg_ORGANIZACION o, Seguridad.Seg_Transaccion t,
              Seguridad.Seg_Opciontrans ot, Seguridad.Seg_Horario h
              
         WHERE tu.idusuario = @PV_Codusuario
               and tu.idorganizacion=o.idorganizacion
               and tu.IdTransaccion=t.Idtransaccion
               and tu.idorganizacion= t.idorganizacion
               and tu.idtransaccion=ot.idtransaccion
               and tu.idorganizacion=ot.idorganizacion
               and tu.idopcion=ot.idopcion
               and tu.idhorario = h.idhorario 
        






