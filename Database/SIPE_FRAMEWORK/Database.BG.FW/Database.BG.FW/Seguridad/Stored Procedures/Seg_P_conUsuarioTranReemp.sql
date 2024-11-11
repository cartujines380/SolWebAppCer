

create procedure [Seguridad].[Seg_P_conUsuarioTranReemp] 
@PV_IdUsuario      VARCHAR(20 )
AS
         SELECT distinct ut.IdTransaccion, ut.IdOpcion, ut.IdOrganizacion,
		t.Descripcion, ut2.IdUsuario as UserReemp,
		convert(varchar,ut.FechaInicial,110) as FechaInicial,
		convert(varchar,ut.FechaFinal,110) as FechaFinal
         FROM Seguridad.Seg_TransUsuario ut left outer join Seguridad.Seg_TransUsuario ut2
					ON ut.IdTransaccion = ut2.IdTransaccion AND ut.IdOpcion = ut2.IdOpcion
					AND ut.IdOrganizacion = ut2.IdOrganizacion		
				INNER JOIN Seguridad.Seg_Transaccion t
					ON t.IdTransaccion = ut.IdTransaccion AND t.IdOrganizacion = ut.IdOrganizacion
 
         WHERE ut.IdUsuario = @PV_IdUsuario
	       AND ut2.UsrReemplaza = @PV_IdUsuario
               



