﻿/*
'usrMF','45493929112007151130454939293015',
'00-08-A1-97-8D-5E',
'usrMF','45493929112007151130454939293015','00-08-A1-97-8D-5E',
4,1,184,1,2,0,null,null,null

declare  @PO_CodRetorno bit
exec Seguridad.Prueba 
'usrMF','45493929112007151130454939293015',
'00-08-A1-97-8D-5E',
1, --@PV_idAplicacion,
1,
163, --Transaccion de autorizacion de sitio
1,
1,
0,
null,
null,
@PO_CodRetorno  OUTPUT
SELECT @PO_CodRetorno

*/

CREATE   PROCEDURE [Seguridad].[prueba]
@PV_idUsuario            VARCHAR(20),
@PV_Token                VARCHAR(32),
@PV_Maquina              VARCHAR(100),
@PV_idAplicacion         INT,
@PV_idOrganizacion       INT,
@PV_IdTransaccion        INT,
@PV_IdOpcion             INT,
@PV_idEmpresa	INT,
@PV_idSucursal	INT,
@PV_ParamAut             VARCHAR(100),
@PV_Valor                VARCHAR(50),
@PO_CodRetorno     BIT OUTPUT
AS
    DECLARE @ln_cont INT
    DECLARE @ln_IdAutorizacion INT
    DECLARE @ln_horario INT

   -- Inicilmente CodRetorno=0 no tiene permiso
   SET  @PO_CodRetorno = 0

	-- Si es usuario de sitio no toma en cuenta localidad

   --Si transaccion es > 1000 no valida permisos
   IF  ( @PV_IdTransaccion >= 1000 ) 
	OR ( @PV_IdTransaccion in (16,53,204,211) and @PV_idOrganizacion = 2)
    OR ( @PV_IdTransaccion in (136,71,145,92,73,74,183,184,185,169,186) and @PV_idOrganizacion = 1 )
    OR ( @PV_IdTransaccion in (201,16,6,50) AND @PV_idOrganizacion = 5)
	
   BEGIN
	-- Retorna Verdadero 1
	SET @PO_CodRetorno = 1
	RETURN
  END
  ELSE
  BEGIN
    -- Revisa si el usuario esta activo, si esta registrado, si su tiempo no expiro

	-- Si el Token es nulo, no revisa que este login activo
	IF @PV_Token <> '' AND 
				NOT EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r,
							Participante.Par_Participante p,
							Seguridad.Seg_Registro b
				WHERE r.IdUsuario = @PV_idUsuario
				AND r.Estado = 1 AND r.IdParticipante = p.IdParticipante
				AND p.Estado = 'A'
				AND r.IdUsuario = b.IdUsuario AND b.Token = @PV_Token 
				AND b.IdIdentificacion = @PV_Maquina
				AND b.Estado = 'A'
    			AND (p.TiempoExpira = 0 OR (getdate() <= dateadd(mi,p.TiempoExpira,b.FechaUltTrans)))
				)
   BEGIN
			raiserror (51003,16,1,@PV_idUsuario) 
			 --SET @PV_RETORNO = 5  --Error de Usuario no esta habilitado
	END
    
print 'paso1'

      -- Recupera el horario de la transaccion
      SELECT @ln_horario = count(*)
      FROM Seguridad.Seg_HorarioTrans a, Seguridad.Seg_HorarioDia b
      WHERE  a.IdTransaccion = @PV_IdTransaccion AND a.IdOpcion = @PV_idOpcion
            AND a.IdOrganizacion = @PV_idOrganizacion
            AND a.IdHorario = b.IdHorario
           AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,b.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,b.HoraFin,20),12,8)),20)
            AND substring(b.dias,datepart(dw,getdate()),1) = '1'
print 'paso2'
 
         IF ISNULL(@ln_horario,0) > 0 
        BEGIN
            -- Recupera el horario
            SELECT @ln_horario = a.IdHorario
            FROM Seguridad.Seg_HorarioTrans a, Seguridad.Seg_HorarioDia b
            WHERE  a.IdTransaccion = @PV_IdTransaccion AND a.IdOpcion = @PV_idOpcion
            AND a.IdOrganizacion = @PV_idOrganizacion
            AND a.IdHorario = b.IdHorario
             AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,b.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,b.HoraFin,20),12,8)),20)
            AND substring(b.dias,datepart(dw,getdate()),1) = '1'

print 'paso3'

            --Verifica que tenga permisos
          SELECT @ln_cont = count(*)
          FROM  Seguridad.Seg_ROLUSUARIO a, Seguridad.Seg_ROL rol, 
		Seguridad.Seg_OPCIONTRANSROL b, Seguridad.Seg_HORARIODIA c
          WHERE a.IdUsuario = @PV_idUsuario
           AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
           AND a.Estado = 'ACTIVE' AND a.IdHorario = c.IdHorario             --chequea horario de perfil por usuario
          AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,c.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,c.HoraFin,20),12,8)),20)
            AND substring(c.dias,datepart(dw,getdate()),1) = '1'
  
            AND a.idRol = rol.IdRol
	    AND rol.IdEmpresa = @PV_idEmpresa AND ( rol.IdSucursal = 0 OR rol.IdSucursal = @PV_idSucursal)
            AND rol.IdRol = b.IdRol
	    AND b.IdTransaccion = @PV_IdTransaccion AND b.IdOpcion = @PV_idOpcion
            AND b.IdOrganizacion = @PV_idOrganizacion

print 'paso4'

            IF @ln_cont = 0 

            BEGIN
		print 'paso44'
               SELECT @ln_cont = count(*)
                FROM  Seguridad.Seg_TRANSUSUARIO a, Seguridad.Seg_HORARIODIA c
                WHERE a.IdUsuario = @PV_idUsuario
                AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
                AND a.Estado = 'A' AND a.IdHorario = c.IdHorario             --chequea horario de perfil por usuario
          	AND  getdate() BETWEEN 
	        convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,c.HoraInicio,20),12,8)),20)
                  AND 
		convert(datetime,(substring(convert(char,getdate(),20),1,10) 
		+ ' ' + substring(convert(char,c.HoraFin,20),12,8)),20)
            	AND substring(c.dias,datepart(dw,getdate()),1) = '1'
                  AND a.IdTransaccion = @PV_IdTransaccion AND a.IdOpcion = @PV_idOpcion
                AND a.IdOrganizacion = @PV_idOrganizacion
             END
print 'paso5'

            IF @ln_cont > 0 
            BEGIN
		print 'paso55'
	              -- Chequea si la trnasaccion tiene autorizaciones
	              SELECT @ln_IdAutorizacion = count(*)
	                FROM Seguridad.Seg_Autorizacion a
	                WHERE -- a.IdEmpresa = @PV_IdEmpresa
					a.IdTransaccion = @PV_IdTransaccion
	                  AND a.IdOpcion = @PV_idOpcion 
			  AND a.IdOrganizacion = @PV_idOrganizacion
	                  AND a.IdHorario = @ln_horario
			  AND a.Parametro = @PV_ParamAut

	              IF ISNULL(@ln_IdAutorizacion,0) > 0 
	              BEGIN
	                  --chequea si el valor cumple con la condicion de operador
                    SELECT @ln_IdAutorizacion = count(*)
	                    FROM Seguridad.Seg_Autorizacion a
	                   WHERE -- a.IdEmpresa = @PV_IdEmpresa
			        a.IdTransaccion = @PV_IdTransaccion
	                    AND a.IdOpcion = @PV_idOpcion AND a.IdOrganizacion = @PV_idOrganizacion
	                    AND a.IdHorario = @ln_horario
	                    AND a.Parametro = @PV_ParamAut
	                    AND (
				   (a.Operador = 'Igual' and a.ValorAutorizado = @PV_Valor)
				OR (a.Operador = 'Diferente' and convert(float,a.ValorAutorizado) <> convert(float,@PV_Valor) )
				OR (a.Operador = 'Menor' and convert(float,a.ValorAutorizado) < convert(float,@PV_Valor) )
				OR (a.Operador = 'Mayor' and convert(float,a.ValorAutorizado) > convert(float,@PV_Valor) )
				)
print 'paso6'

                    IF ISNULL(@ln_IdAutorizacion,0) > 0 
			SET @PO_CodRetorno = 1
				--set @PV_RETORNO = 0 --Tiene acceso a la transacion
	                    ELSE
	                    BEGIN
	                       -- chequea si el tiene autorizacion por usuario
	                       SELECT @ln_IdAutorizacion = a.IdAutorizacion
	                      FROM Seguridad.Seg_Autorizacion A, Seguridad.Seg_AutorizacionUsuario b
	                      WHERE --a.IdEmpresa = @PV_IdEmpresa
			           a.IdTransaccion = @PV_IdTransaccion
	                        AND a.IdOpcion = @PV_idOpcion AND a.IdOrganizacion = @PV_idOrganizacion
	                        AND a.IdHorario = @ln_horario
				AND a.Parametro = @PV_ParamAut
	                        AND a.IdAutorizacion = b.IdAutorizacion
	                      AND b.IdUsuario = @PV_IdUsuario
	                      AND (
				   (a.Operador = 'Igual' and b.valor = @PV_Valor)
				OR (a.Operador = 'Diferente' and convert(float,a.ValorAutorizado) <> convert(float,@PV_Valor) )
				OR (a.Operador = 'Menor' and convert(float,a.ValorAutorizado) < convert(float,@PV_Valor) )
				OR (a.Operador = 'Mayor' and convert(float,a.ValorAutorizado) > convert(float,@PV_Valor) )
				)
	                      AND ( (getdate() BETWEEN b.FechaInicio AND b.FechaFin) AND ( NumAutorizacion > 0 OR NumAutorizacion = 999 ))
	
	                      IF ISNULL(@ln_IdAutorizacion,0) > 0
	                      BEGIN 
	                        -- Se reduce el NumAutorizaciones si es 99
	                        UPDATE Seguridad.Seg_AutorizacionUsuario
	                        SET NumAutorizacion = NumAutorizacion - 1
	                        Where IdAutorizacion = @ln_IdAutorizacion
				      AND IdUsuario = @PV_IdUsuario
				      AND NumAutorizacion <> 999
			
				SET @PO_CodRetorno = 1
			    --SET @PV_RETORNO = 0 --Tiene acceso a la transacion
	                      END  
	                      ELSE
					raiserror (51004,16,1) 
	                        	--SET @PV_RETORNO = 8 -- Necesita Autorizacion
	                    END
		END     
              ELSE
                SET @PO_CodRetorno = 1 --Tiene acceso a la transacion
          END   
           ELSE
	raiserror (51001,16,1,@PV_idUsuario) 
            	 -- SET @PV_RETORNO = 7 -- Usuario no tiene permisos
     END     
      ELSE
	raiserror (51002,16,1) 
       -- SET @PV_RETORNO = 6 -- Transaccion fuera de horario
 
END    
  


