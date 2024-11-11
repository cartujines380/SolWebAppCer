/*
select * from seguridad.seg_registro where idusuario = 'usrMF' and Estado = 'A'

Seguridad.Seg_P_CONSULTA_PERFIL 'admFramewk',
'00000025022011140422000000252214',
'127.0.0.1',1,2,0,26,'M'


Seguridad.Seg_P_CONSULTA_PERFIL 'admFramewk',1,2,3,100,'T'

*/

CREATE procedure [Seguridad].[Seg_P_CONSULTA_PERFIL2]
@PV_idUsuario              VARCHAR(20),
@PV_Token			varchar(50),
@PV_Maquina			varchar(100),
@PV_idAplicacion           INT,
@PV_idEmpresa		   INT,
@PV_idSucursal			int,
@PV_idOrganizacion		int,
@PV_Perfil char='N'
AS
	-- Setea para que datepart de dias tenga Lunes=1
	--set datefirst 1
--Recupera la Identificacion que tiene registrado actualmente
DECLARE @VL_MacMaquina varchar(20)

SELECT @VL_MacMaquina = MacMaquina
FROM Seguridad.Seg_Registro
WHERE IdUsuario = @PV_idUsuario AND Token = @PV_Token 
	AND IdIdentificacion = @PV_Maquina AND Estado = 'A'

-- Recupera Transacciones que tienen permisos a nivel de roles
        SELECT   distinct d.IdOrganizacion as Organizacion, d.IdTransaccion as Transaccion
				,org.Descripcion as DescOrg, d.Descripcion as DescTrans, 
				c.IdOpcion as Opcion, c.Descripcion as DescOpcion
		FROM Seguridad.Seg_ROL rol,
			 Seguridad.Seg_ROLUSUARIO a,
			 Seguridad.Seg_HORARIODIA h,
			 Seguridad.Seg_OPCIONTRANSROL b,
			 Seguridad.Seg_HORARIOTRANS ht,
			 Seguridad.Seg_HORARIODIA hdt,
			 Seguridad.Seg_OPCIONTRANS c,
			 Seguridad.Seg_TRANSACCION d,
			Seguridad.Seg_ORGANIZACION org,
			Seguridad.Seg_Aplicacion apl
          WHERE rol.IdEmpresa = @PV_idEmpresa 
			AND (rol.IdSucursal = 0 OR rol.IdSucursal = @PV_idSucursal)
            AND rol.idRol= a.idRol
            AND a.IdUsuario = @PV_idUsuario
            AND a.Estado = 'ACTIVE'
            AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
            AND ( a.IdIdentificacion is null OR a.IdIdentificacion = ''  
				   OR a.IdIdentificacion = @PV_Maquina
				   OR a.IdIdentificacion = @VL_MacMaquina )
			-- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,a.IdHorario) = 0
			AND a.IdHorario = h.IdHorario
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
            AND substring(h.dias,datepart(dw,getdate()),1) = '1'
            AND a.idRol = b.IdRol
            AND (@PV_idOrganizacion = 0 OR b.IdOrganizacion = @PV_idOrganizacion)
            AND b.IdTransaccion = c.IdTransaccion AND b.IdOpcion = c.IdOpcion  AND b.IdOrganizacion = c.idOrganizacion
            AND c.Idtransaccion = d.IdTransaccion  AND c.IdOrganizacion = d.idOrganizacion
	    AND ( @PV_Perfil = 'T' OR (@PV_Perfil = 'M' AND d.Menu = 1 ) ) -- d.idtransaccion >= 2000) )
	    AND d.IdOrganizacion = org.IdOrganizacion
	    AND org.IdAplicacion = apl.IdAplicacion
	    AND (apl.IdAplicacion = @PV_IdAplicacion OR apl.Link = 'INTERNO')
            AND b.IdTransaccion = ht.IdTransaccion AND b.IdOpcion = ht.IdOpcion AND b.IdOrganizacion = ht.IdOrganizacion
            -- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,ht.IdHorario) = 0
            AND ht.IdHorario = hdt.IdHorario
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
   	    AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'
	   -- ORDER BY Organizacion,Transaccion
	UNION
--	Recupera transacciones a nivel individual
    SELECT distinct d.IdOrganizacion as Organizacion, d.IdTransaccion as Transaccion
				,org.Descripcion as DescOrg, d.Descripcion as DescTrans, 
				c.IdOpcion as Opcion, c.Descripcion as DescOpcion
	FROM  Seguridad.Seg_TRANSUSUARIO tu, 
		  Seguridad.Seg_HORARIODIA h,
		  Seguridad.Seg_HORARIOTRANS ht,
		  Seguridad.Seg_OPCIONTRANS c,
		  Seguridad.Seg_TRANSACCION d,
		  Seguridad.Seg_ORGANIZACION org,
		  Seguridad.Seg_Aplicacion a,
		  Seguridad.Seg_HORARIODIA hdt
    WHERE   tu.IdUsuario = @PV_idUsuario
            AND tu.Estado = 'A'
            AND getdate()  BETWEEN tu.FechaInicial AND tu.FechaFinal
			AND ( tu.IdIdentificacion is null OR tu.IdIdentificacion = ''  
				   OR tu.IdIdentificacion = @PV_Maquina
				   OR tu.IdIdentificacion = @VL_MacMaquina )
			-- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,tu.IdHorario) = 0
			AND tu.IdHorario = h.IdHorario
            AND  getdate() BETWEEN convert(datetime,(substring(convert(char,getdate(),20),1,10) 
									+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
                  AND 
									convert(datetime,(substring(convert(char,getdate(),20),1,10) 
									+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
            AND substring(h.dias,datepart(dw,getdate()),1) = '1'
            AND (@PV_idOrganizacion = 0 OR tu.IdOrganizacion = @PV_idOrganizacion)
            AND tu.IdTransaccion = c.IdTransaccion AND tu.IdOpcion = c.IdOpcion  AND tu.IdOrganizacion = c.idOrganizacion
            AND c.Idtransaccion = d.IdTransaccion  AND c.IdOrganizacion = d.idOrganizacion
	    AND ( @PV_Perfil = 'T' OR (@PV_Perfil = 'M' AND d.Menu = 1 )) --d.idtransaccion >= 2000) )
	    AND d.IdOrganizacion = org.IdOrganizacion
	    AND org.IdAplicacion = a.IdAplicacion 
		AND (a.IdAplicacion = @PV_IdAplicacion OR a.Link = 'INTERNO') 
		AND a.IdEmpresa = @PV_idEmpresa 
        AND tu.IdTransaccion = ht.IdTransaccion AND tu.IdOpcion = ht.IdOpcion AND tu.IdOrganizacion = ht.IdOrganizacion
        -- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,ht.IdHorario) = 0
        AND ht.IdHorario = hdt.IdHorario
        AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
   	    AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'
	ORDER BY Organizacion,Transaccion

