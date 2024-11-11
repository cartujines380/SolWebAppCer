
/* Modificado por Administracion de Usuarios */ 

/*
'usrMF','45493929112007151130454939293015',
'00-08-A1-97-8D-5E',
'usrMF','45493929112007151130454939293015','00-08-A1-97-8D-5E',
4,1,184,1,2,0,null,null,null

declare  @PO_CodRetorno bit
exec Seguridad.Seg_P_VERIFICA_TRANSACCION 
'aplSAT','90488601012008510237904886013751',
'192.168.0.60',
2, --@PV_idAplicacion,
1,
553, --Transaccion de autorizacion de sitio
1,
2,
0,
null,
null,
@PO_CodRetorno  OUTPUT
SELECT @PO_CodRetorno
*/

CREATE   PROCEDURE [Seguridad].[Seg_P_VERIFICA_TRANSACCION]
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
@PV_txtTransaccion		VARCHAR(max) = null,
@PO_CodRetorno     BIT OUTPUT
AS
	-- Variables de Error
	DECLARE @ErrorMessage NVARCHAR(4000),@ErrorSeverity INT,@ErrorState INT 
    -- Variable locales
	DECLARE @ln_cont INT, @ln_IdAutorizacion INT, @ln_horario INT, @VL_MacMaquina varchar(20)

    -- Inicilmente CodRetorno=0 no tiene permiso
    SET  @PO_CodRetorno = 0
	BEGIN TRY
									print '1'

		--Si transaccion es > 1000 no valida permisos
	--	IF  @PV_IdTransaccion in (71,184,183,185,169,186,163) and @PV_idOrganizacion = 1
	--IF ( @PV_IdTransaccion >= 1000 ) 
		IF 
		( @PV_IdTransaccion in (16,53,204,211) and @PV_idOrganizacion = 2)
			OR ( @PV_IdTransaccion in (136,71,145,92,73,74,183,184,185,169,186,218) and @PV_idOrganizacion = 1 )
			OR ( @PV_IdTransaccion in (201,16,6,50) AND @PV_idOrganizacion = 5)
			OR ( @PV_IdTransaccion in (550,551) AND @PV_idOrganizacion = 1)
	
		BEGIN
			SET @PO_CodRetorno = 1
			--RETURN
		END
		ELSE -- Verifica la transaccion
		BEGIN

			-- Revisa si el usuario esta activo, si esta registrado, si su tiempo no expiro
			/*SELECT @ln_cont = count(*)
			  FROM Seguridad.Seg_Usuario a, Seguridad.Seg_Registro b
			FROM Participante.par_Participante a, Seguridad.Seg_Registro b
			WHERE a.IdUsuario = @PV_idUsuario AND a.Estado = 'A'
			AND a.IdUsuario = b.IdUsuario AND b.Token = @PV_Token AND b.IdIdentificacion = @PV_Maquina
			AND b.Estado = 'A'
			AND (a.TiempoExpira = 0 OR (getdate() <= dateadd(mi,a.TiempoExpira,b.FechaUltTrans)))
			IF ISNULL(@ln_cont,0) > 0 
			*/
			-- Si el Token es nulo, no revisa que este login activo @PV_Token <> '' AND
			IF NOT EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r										
							WHERE r.IdUsuario = @PV_idUsuario							
							)
				BEGIN
									print '2'	
									
									IF EXISTS( SELECT 1 FROM Seguridad.Seg_TRANSACCION
						WHERE   IdOrganizacion = @PV_idOrganizacion 
								AND idTransaccion = @PV_IdTransaccion 
								AND Auditable = 'S' ) 
								BEGIN
								print 'registra auditoria'
						 EXEC  Seguridad.Seg_P_REGISTRA_AUDITORIA 
						 @PV_idUsuario,
						 @PV_idAplicacion,
						 @PV_Maquina,
						 @PV_txtTransaccion,
			--			     @PV_Token,
						 @PV_IdOrganizacion,
						 @PV_IdTransaccion,
						 'S' -- Si tene permisos
			END

						--raiserror ('Usuario no existe. %s',16,1,@PV_idUsuario) 
						SET @PO_CodRetorno = 1
						return 
				END
				IF NOT EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r,
										Participante.Par_Participante p
							WHERE r.IdUsuario = @PV_idUsuario
							AND r.Estado = 1 AND r.IdParticipante = p.IdParticipante
							AND p.Estado = 'A'
							)
				BEGIN

						raiserror ('Usuario no activo %s',16,1,@PV_idUsuario) 
						return
				END

			IF @PV_Token <> ''
			BEGIN
				IF NOT EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r,
										Participante.Par_Participante p,
										Seguridad.Seg_Registro b
							WHERE r.IdUsuario = @PV_idUsuario
							AND r.Estado = 1 AND r.IdParticipante = p.IdParticipante
							AND p.Estado = 'A'
							AND r.IdUsuario = b.IdUsuario AND b.Token = @PV_Token 
							--AND b.IdIdentificacion = @PV_Maquina
							AND b.Estado = 'A'
    						AND (p.TiempoExpira = 0 OR (getdate() <= dateadd(mi,p.TiempoExpira,b.FechaUltTrans)))
							)
				BEGIN
						raiserror ('Usuario sin token de sesion valido. %s',16,1,@PV_idUsuario) 
						return
						 --SET @PV_RETORNO = 5  --Error de Usuario no esta habilitado
				END
			END 
			
			-- RECUPERA LA MACADDRESS PARA CONVALIDAR CON LOS ROLES
			SELECT @VL_MacMaquina = MacMaquina
			FROM Seguridad.Seg_Registro
			WHERE IdUsuario = @PV_idUsuario AND Token = @PV_Token 
				AND IdIdentificacion = @PV_Maquina AND Estado = 'A'

			if not exists ( select 1 from seguridad.seg_opcionTrans a
			WHERE  a.IdTransaccion = @PV_IdTransaccion AND a.IdOpcion = @PV_idOpcion
				AND a.IdOrganizacion = @PV_idOrganizacion)
			begin
				raiserror ('Transaccion no definida',16,1) 
				return 
			end
															print '2'

			-- Recupera el horario de la transaccion
			SELECT @ln_horario = a.IdHorario --@ln_horario = count(*)
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
 
		  IF @ln_horario > 0 
		  BEGIN
			-- VERIFICA QUE LA TRANSACCION NO ESTE EN UN DIA FERIADO
			IF Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,@ln_horario) = 1
			BEGIN
				raiserror ('Es día feriado',16,1) 
				return
			END
						

			--Verifica que tenga permisos
			SELECT @ln_cont = count(*)
			FROM  Seguridad.Seg_ROLUSUARIO a, Seguridad.Seg_ROL rol, 
			Seguridad.Seg_OPCIONTRANSROL b, Seguridad.Seg_HORARIODIA c
			WHERE 
			--a.IdUsuario = @PV_idUsuario
			--AND 
			getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
			AND (     a.IdIdentificacion is null OR a.IdIdentificacion = ''  
				   OR a.IdIdentificacion = @PV_Maquina
				   OR a.IdIdentificacion = @VL_MacMaquina )
			-- Revisa que excluya los feriados
			AND Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,a.IdHorario) = 0
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
			AND (@PV_idOrganizacion = 0 OR b.IdOrganizacion = @PV_idOrganizacion)

												print '3'

            IF @ln_cont = 0 
            BEGIN
               SELECT @ln_cont = count(*)
                FROM  Seguridad.Seg_TRANSUSUARIO a, Seguridad.Seg_HORARIODIA c
                WHERE a.IdUsuario = @PV_idUsuario
                AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
				-- Revisa que excluya los feriados
				AND Seguridad.Seg_F_DiaFeriado(@PV_idEmpresa,@PV_idSucursal,a.IdHorario) = 0
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
            IF @ln_cont > 0 
            BEGIN
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
						IF ISNULL(@ln_IdAutorizacion,0) > 0 
						BEGIN
							SET @PO_CodRetorno = 1
							--set @PV_RETORNO = 0 --Tiene acceso a la transacion
						END
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
							  BEGIN
								raiserror (51004,16,1)
								return 
							  END
								--SET @PV_RETORNO = 8 -- Necesita Autorizacion
						END
					END     
					ELSE
					BEGIN
						SET @PO_CodRetorno = 1 --Tiene acceso a la transacion
					END
				END   
				ELSE
				BEGIN
--					raiserror (51001,16,1,@PV_idUsuario) 
					return 
            		-- SET @PV_RETORNO = 7 -- Usuario no tiene permisos
				END
			END     
			ELSE
			BEGIN
				raiserror (51002,16,1) 
				return 
				-- SET @PV_RETORNO = 6 -- Transaccion fuera de horario
			END

			-- Registra Auditoria si no es la transaccion de Valida Sitio
			IF  NOT (@PV_idOrganizacion = 1 AND @PV_IdTransaccion = 163)
			BEGIN
			  IF EXISTS( SELECT 1 FROM Seguridad.Seg_TRANSACCION
						WHERE   IdOrganizacion = @PV_idOrganizacion 
								AND idTransaccion = @PV_IdTransaccion 
								AND Auditable = 'S' ) 

								print 'registra auditoria'
						 EXEC  Seguridad.Seg_P_REGISTRA_AUDITORIA 
						 @PV_idUsuario,
						 @PV_idAplicacion,
						 @PV_Maquina,
						 @PV_txtTransaccion,
			--			     @PV_Token,
						 @PV_IdOrganizacion,
						 @PV_IdTransaccion,
						 'S' -- Si tene permisos
			END
		END
	END TRY	
	BEGIN CATCH
		IF EXISTS( SELECT 1 FROM Seguridad.Seg_TRANSACCION
					WHERE   IdOrganizacion = @PV_idOrganizacion 
							AND idTransaccion = @PV_IdTransaccion 
							AND Auditable = 'S' ) 
					 EXEC  Seguridad.Seg_P_REGISTRA_AUDITORIA 
					 @PV_idUsuario,
					 @PV_idAplicacion,
					 @PV_Maquina,
					 @PV_txtTransaccion,
					 @PV_IdOrganizacion,
					 @PV_IdTransaccion,
					 'N' -- Registra que se intento ejecutar, pero no tiene permisos
		--Preguntar si existe transaccion
		--IF XACT_STATE() IN (1,-1)
		--	ROLLBACK TRAN
		-- Produce un RAISERROR con el msg de error
		SELECT 
			@ErrorMessage	= ERROR_MESSAGE(),
			@ErrorSeverity	= ERROR_SEVERITY(),
			@ErrorState		= ERROR_STATE()
		IF (@ErrorState<1 OR @ErrorState>127)
		BEGIN
			SET @ErrorState=1
		END
		RAISERROR (@ErrorMessage,@ErrorSeverity,@ErrorState)
	END CATCH

