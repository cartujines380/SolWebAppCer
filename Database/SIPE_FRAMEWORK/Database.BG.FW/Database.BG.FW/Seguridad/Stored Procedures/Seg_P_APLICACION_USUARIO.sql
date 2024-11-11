



-- Seguridad.Seg_P_APLICACION_USUARIO 'victormunoz',1,5

CREATE  procedure [Seguridad].[Seg_P_APLICACION_USUARIO]
@PI_IdUsuario           varchar(20),
@PI_Token				varchar(50),
@PI_Maquina				varchar(100),
@PI_IdEmpresa		    int,
@PI_IdSucursal		    int

AS
  SET NOCOUNT ON
  --Recupera la Identificacion que tiene registrado actualmente
DECLARE @VL_MacMaquina varchar(20)

SELECT @VL_MacMaquina = MacMaquina
FROM Seguridad.Seg_Registro
WHERE IdUsuario = @PI_IdUsuario AND Token = @PI_Token 
	AND IdIdentificacion = @PI_Maquina AND Estado = 'A'

	-- Recupera las aplicaciones que puede accesar por rol
	SELECT  distinct Apl.IdAplicacion, Apl.Nombre, 
			link as Url
	      FROM Seguridad.Seg_ROL rol, Seguridad.Seg_ROLUSUARIO a, Seguridad.Seg_HORARIODIA h, Seguridad.Seg_OPCIONTRANSROL b,
			Seguridad.Seg_HORARIOTRANS ht, Seguridad.Seg_HORARIODIA hdt, Seguridad.Seg_OPCIONTRANS c, 
			Seguridad.Seg_TRANSACCION d,Seguridad.Seg_ORGANIZACION org, Seguridad.Seg_Aplicacion Apl
          WHERE rol.IdEmpresa = @PI_IdEmpresa
          AND (rol.IdSucursal = 0 OR rol.IdSucursal = @PI_IdSucursal)
            AND rol.idRol= a.idRol
            AND a.IdUsuario = @PI_IdUsuario
            AND a.Estado = 'ACTIVE'
            AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
            AND (     a.IdIdentificacion is null OR a.IdIdentificacion = ''  
				   OR a.IdIdentificacion = @PI_Maquina
				   OR a.IdIdentificacion = @VL_MacMaquina )
			-- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PI_IdEmpresa,@PI_IdSucursal,a.IdHorario) = 0
            AND a.IdHorario = h.IdHorario
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
            AND substring(h.dias,datepart(dw,getdate()),1) = '1'
            AND a.idRol = b.IdRol --AND b.IdOrganizacion = @PV_idOrganizacion
            AND b.IdTransaccion = c.IdTransaccion AND b.IdOpcion = c.IdOpcion  AND b.IdOrganizacion = c.idOrganizacion
            AND c.Idtransaccion = d.IdTransaccion  AND c.IdOrganizacion = d.idOrganizacion
	    AND d.idtransaccion = 2000
	    AND d.IdOrganizacion = org.IdOrganizacion
	    AND org.IdAplicacion = Apl.IdAplicacion
	    AND Apl.TipoServidor = 1 --Para aplicaciones solamente.
	    AND Apl.Link <> ''  AND Apl.Link <> 'INTERNO' -- solo las aplicaciones que tengan un link
        AND b.IdTransaccion = ht.IdTransaccion AND b.IdOpcion = ht.IdOpcion 
		AND b.IdOrganizacion = ht.IdOrganizacion
		-- Revisa que excluya los feriados
		AND Seguridad.Seg_F_DiaFeriado(@PI_IdEmpresa,@PI_IdSucursal,ht.IdHorario) = 0
        AND ht.IdHorario = hdt.IdHorario
        AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
   	    AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'
	   -- ORDER BY Apl.IdAplicacion
	
	UNION
	
	-- Recupera las aplicaciones que puede accesar por transaccion
	SELECT  distinct Apl.IdAplicacion, Apl.Nombre, 
			link as Url
	      FROM Seguridad.Seg_TRANSUSUARIO tu, 
			Seguridad.Seg_HORARIODIA h, 
			Seguridad.Seg_HORARIOTRANS ht, 
			Seguridad.Seg_HORARIODIA hdt, 
			Seguridad.Seg_OPCIONTRANS c, 
			Seguridad.Seg_TRANSACCION d,
			Seguridad.Seg_ORGANIZACION org, 
			Seguridad.Seg_Aplicacion Apl
          WHERE tu.IdUsuario = @PI_IdUsuario
            AND tu.Estado = 'A'
            AND getdate()  BETWEEN tu.FechaInicial AND tu.FechaFinal
            AND (     tu.IdIdentificacion is null OR tu.IdIdentificacion = ''  
				   OR tu.IdIdentificacion = @PI_Maquina
				   OR tu.IdIdentificacion = @VL_MacMaquina )
			-- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PI_IdEmpresa,@PI_IdSucursal,tu.IdHorario) = 0
            AND tu.IdHorario = h.IdHorario
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
            AND substring(h.dias,datepart(dw,getdate()),1) = '1'
            AND tu.IdTransaccion = c.IdTransaccion AND tu.IdOpcion = c.IdOpcion  AND tu.IdOrganizacion = c.idOrganizacion
            AND c.Idtransaccion = d.IdTransaccion  AND c.IdOrganizacion = d.idOrganizacion
	    AND d.idtransaccion = 2000
	    AND d.IdOrganizacion = org.IdOrganizacion
	    AND org.IdAplicacion = Apl.IdAplicacion
	    AND Apl.TipoServidor = 1 --Para aplicaciones solamente.
        AND Apl.Link <> '' AND Apl.Link <> 'INTERNO' -- solo las aplicaciones que tengan un link
        AND tu.IdTransaccion = ht.IdTransaccion AND tu.IdOpcion = ht.IdOpcion 
		AND tu.IdOrganizacion = ht.IdOrganizacion
		-- Revisa que excluya los feriados
		AND Seguridad.Seg_F_DiaFeriado(@PI_IdEmpresa,@PI_IdSucursal,ht.IdHorario) = 0
        AND ht.IdHorario = hdt.IdHorario
        AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
   	    AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'
	   
	-- SOLO PARA DESARROLLAR EN DIFERENTES SITIO EL MISMO APLICATIVO (NO APLICA EN PRODUCCION)
		--UNION
		--//SELECT  27, 'MTRA (Desarrollo)','http://sgyedes01/WebMtra/Default.aspx'
		/*
        UNION
		SELECT  119, 'Gestion Cobranzas (CVERA)','http://172.16.23.236/WebGestionCobranzas/Default.aspx'
		UNION
		SELECT  119, 'Gestion Cobranzas (RZUNIGA)','http://172.16.23.235/WebGestionCobranzas/Default.aspx'
		UNION
		SELECT  169, 'Respaldo de videos (DMUNOZ)','http://192.168.0.21/WebRespaldosVideos/Respaldos/frmBandejaSolicitud.aspx'
        UNION
		SELECT  169, 'Respaldo de videos (MSILVA)','http://192.168.0.67/WebRespaldosVideos/Respaldos/frmBandejaSolicitud.aspx'	
		UNION
		SELECT  119, 'INMAELECTRO','http://192.168.0.110/WebCae/Default.aspx'	
		*/
	--
	ORDER BY Apl.IdAplicacion
	




