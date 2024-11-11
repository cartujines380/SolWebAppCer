

CREATE procedure [Seguridad].[Seg_P_CONSULTA_AUTORHORA]
	@PV_idTransaccion      	int,
       	@PV_idOrganizacion     	int	

AS
	select distinct a.IdOpcion, a.IdHorario, a.IdAutorizacion, a.Parametro, a.Operador, a.ValorAutorizado
    	from Seguridad.Seg_Autorizacion a join Seguridad.Seg_HORARIOTRANS ht on a.idTransaccion = ht.Idtransaccion and a.idOrganizacion = ht.idOrganizacion
    	where a.idTransaccion = @PV_idTransaccion and
      		a.idOrganizacion = @PV_idOrganizacion 
		





