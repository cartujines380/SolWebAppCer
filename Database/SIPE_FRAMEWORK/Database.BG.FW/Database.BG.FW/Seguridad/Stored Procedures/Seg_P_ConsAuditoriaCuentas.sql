CREATE PROC [Seguridad].[Seg_P_ConsAuditoriaCuentas]
@PI_NumeroProducto varchar(16),
@PI_IdUsuario varchar(50),
@FechaDesde varchar(10),
@FechaHasta varchar(10)

AS
DECLARE @Fini datetime, @Ffin datetime
SET @Fini = convert(datetime,@FechaDesde + ' 00:00:01')
SET @Ffin = convert(datetime,@FechaHasta + ' 23:59:59')

	SELECT	a.FechaMovi, t.Descripcion DescTransaccion, a.IdIdentificacion
	FROM	Seguridad.Seg_Auditoria a inner join  Seguridad.Seg_Transaccion t
				on a.IdOrganizacion = t.IdOrganizacion 
				and a.IdTransaccion = t.Idtransaccion, 
				Participante.Par_RegistroCliente r				
	WHERE	a.IdUsuario = @PI_IdUsuario
			and FechaMovi BETWEEN @Fini and @Ffin
			and convert(varchar,a.txtTransaccion) like '%"' + @PI_NumeroProducto + '"%'
			and a.IdUsuario = r.IdUsuario
			and a.IdOrganizacion in (26,27)
	order by a.FechaMovi asc





