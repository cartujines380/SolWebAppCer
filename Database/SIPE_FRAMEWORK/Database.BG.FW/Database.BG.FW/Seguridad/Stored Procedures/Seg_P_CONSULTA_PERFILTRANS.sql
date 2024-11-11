



create procedure [Seguridad].[Seg_P_CONSULTA_PERFILTRANS]
@PV_idUsuario              VARCHAR(20),
@PV_idOrganizacion         INT,
@PV_idTrans                INT
AS
	-- Setea para que datepart de dias tenga Lunes=1
	set datefirst 1

       SELECT c.IdOpcion, c.Descripcion as DescOpcion
          FROM Seguridad.Seg_ROLUSUARIO a, Seguridad.Seg_HORARIODIA h, Seguridad.Seg_OPCIONTRANSROL b,
		Seguridad.Seg_OPCIONTRANS c, Seguridad.Seg_HORARIOTRANS ht, Seguridad.Seg_HORARIODIA hdt
          WHERE a.IdUsuario = @PV_idUsuario
            AND a.Estado = 'ACTIVE'
            AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
            AND substring(h.dias,datepart(dw,getdate()),1) = '1'
 
            AND a.idRol = b.IdRol AND b.IdOrganizacion = @PV_idOrganizacion
            AND b.IdTransaccion = c.IdTransaccion AND b.IdTransaccion = @PV_idTrans
            AND b.IdTransaccion = ht.IdTransaccion AND b.IdOpcion = ht.IdOpcion AND b.IdOrganizacion = ht.IdOrganizacion
            AND ht.IdHorario = hdt.IdHorario
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
    	    AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'






