/*
exec Seguridad.Seg_P_ConsRolUsuarioActivo 'cvera',1,5


*/
CREATE procedure [Seguridad].[Seg_P_ConsRolUsuarioActivo]
@PV_idUsuario              VARCHAR(20),
@PV_idEmpresa		   INT,
@PV_idSucursal			int
AS
	-- Setea para que datepart de dias tenga Lunes=1
	set datefirst 1
--Recupera la Identificacion que tiene registrado actualmente
DECLARE @VL_IdIdentificacion varchar(100)
SELECT @VL_IdIdentificacion = IdIdentificacion
FROM Seguridad.Seg_Registro
WHERE IdUsuario = @PV_idUsuario AND Estado = 'A'

	SELECT   distinct r.IdRol Rol, r.Descripcion
		FROM Seguridad.Seg_ROL r, Seguridad.Seg_ROLUSUARIO a, Seguridad.Seg_HORARIODIA h
          WHERE r.IdEmpresa = @PV_idEmpresa 
			AND (r.IdSucursal = 0 OR r.IdSucursal = @PV_idSucursal)
            AND r.idRol= a.idRol
            AND a.IdUsuario = @PV_idUsuario
            AND a.Estado = 'ACTIVE'
            AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
			AND (   a.IdIdentificacion is null OR a.IdIdentificacion = ''  
				 OR a.IdIdentificacion = @VL_IdIdentificacion )
            AND a.IdHorario = h.IdHorario
            AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
            AND substring(h.dias,datepart(dw,getdate()),1) = '1'
        ORDER BY r.Descripcion




