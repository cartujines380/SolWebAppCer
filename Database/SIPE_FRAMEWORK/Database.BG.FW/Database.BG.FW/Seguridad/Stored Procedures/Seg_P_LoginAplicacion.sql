CREATE Procedure [Seguridad].[Seg_P_LoginAplicacion]
		@IdAplicacion			int,
		@IdUsuario              varchar(20),
		@Token                  varchar(32),
		@Maquina                varchar(100),
		@MacMaquina				varchar(20),
		@RecuperaMT				varchar(1) = 'N'
AS
  Set @Maquina = SUBSTRING(@Maquina,1,20)
	
  SET NOCOUNT ON
  DECLARE @IdEmpresa int, @IdTipoLogin varchar(10)
  
	IF NOT EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r										
							WHERE r.IdUsuario = @IdUsuario							
							)
				BEGIN
						raiserror ('Usuario no existe %s',16,1,@IdUsuario) 
						return
				END
	IF NOT EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r,
										Participante.Par_Participante p
							WHERE r.IdUsuario = @IdUsuario
							AND r.Estado = 1 AND r.IdParticipante = p.IdParticipante
							AND p.Estado = 'A'
							)
				BEGIN					
						raiserror ('Usuario no activo %s',16,1,@IdUsuario) 
						return
				END
 	IF EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r, 
							Participante.Par_Participante p
				WHERE r.IdUsuario = @IdUsuario
				AND r.Estado = 1 AND r.IdParticipante = p.IdParticipante
				AND p.Estado = 'A'
				AND convert(datetime,(substring(convert(char,FechaExpira,20),1,10) + ' 00:00:00.000')) >=
				convert(datetime,(substring(convert(char,getdate(),20),1,10) + ' 00:00:00.000')) )
	BEGIN
    
    	-- Recupera Parametros de la Aplicación si tiene permisos por ROLES a SUARIO
		SELECT  @IdEmpresa = rol.IdEmpresa
			  FROM	Seguridad.Seg_ROL rol, 
					Seguridad.Seg_ROLUSUARIO a,
					Seguridad.Seg_HORARIODIA h, 
					Seguridad.Seg_OPCIONTRANSROL b,
					Seguridad.Seg_HORARIOTRANS ht, 
					Seguridad.Seg_HORARIODIA hdt, 
					Seguridad.Seg_OPCIONTRANS c, 
					Seguridad.Seg_TRANSACCION d,
					Seguridad.Seg_ORGANIZACION org
			  WHERE rol.idRol= a.idRol AND rol.IdRol <> 3
				AND a.IdUsuario = @IdUsuario
				AND a.Estado = 'ACTIVE'
				AND getdate()  BETWEEN a.FechaInicial AND a.FechaFinal
				AND (  a.IdIdentificacion is null OR a.IdIdentificacion = ''  
					 OR a.IdIdentificacion = @Maquina
					 OR a.IdIdentificacion = @MacMaquina )
				-- Revisa que excluya los feriados
				AND Seguridad.Seg_F_DiaFeriado(rol.IdEmpresa,rol.IdSucursal,a.IdHorario) = 0
				AND a.IdHorario = h.IdHorario
				AND  getdate() BETWEEN 
				convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
					  AND 
			convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
				AND substring(h.dias,datepart(dw,getdate()),1) = '1'
				AND a.idRol = b.IdRol 
				AND b.IdTransaccion = c.IdTransaccion 
				AND b.IdOpcion = c.IdOpcion  
				AND b.IdOrganizacion = c.idOrganizacion
				AND c.Idtransaccion = d.IdTransaccion
				AND c.IdOrganizacion = d.idOrganizacion
				-- AND d.idtransaccion between 2000 and 2999
				AND d.IdOrganizacion = org.IdOrganizacion
				AND org.IdAplicacion = @IdAplicacion
				AND b.IdTransaccion = ht.IdTransaccion 
				AND b.IdOpcion = ht.IdOpcion 
				AND b.IdOrganizacion = ht.IdOrganizacion
				-- Revisa que excluya los feriados
				AND Seguridad.Seg_F_DiaFeriado(rol.IdEmpresa,rol.IdSucursal,ht.IdHorario) = 0
				AND ht.IdHorario = hdt.IdHorario
				AND  getdate() BETWEEN 
				convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
					  AND 
			convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
   			AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'
   		
   		IF @IdEmpresa > 0 -- Pregunta por Transaccion a usuario
		BEGIN  
			-- Recupera Parametros de la Aplicación si tiene permisos por TRANSACCIONES a SUARIO
			SELECT  @IdEmpresa = apl.IdEmpresa
			  FROM	Seguridad.Seg_TRANSUSUARIO tu, 
					Seguridad.Seg_HORARIODIA h, 
					Seguridad.Seg_HORARIOTRANS ht, 
					Seguridad.Seg_HORARIODIA hdt, 
					Seguridad.Seg_OPCIONTRANS c, 
					Seguridad.Seg_TRANSACCION d,
					Seguridad.Seg_ORGANIZACION org,
					Seguridad.Seg_APLICACION apl
			  WHERE tu.IdUsuario = @IdUsuario
				AND tu.Estado = 'A'
				AND getdate()  BETWEEN tu.FechaInicial AND tu.FechaFinal
				AND (  tu.IdIdentificacion is null OR tu.IdIdentificacion = ''  
					 OR tu.IdIdentificacion = @Maquina
					 OR tu.IdIdentificacion = @MacMaquina )
				-- Revisa que excluya los feriados, asume todas en localidad
				AND Seguridad.Seg_F_DiaFeriado(apl.IdEmpresa,0,tu.IdHorario) = 0
				AND tu.IdHorario = h.IdHorario
				AND  getdate() BETWEEN 
				convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,h.HoraInicio,20),12,8)),20)
					  AND 
			convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,h.HoraFin,20),12,8)),20)
				AND substring(h.dias,datepart(dw,getdate()),1) = '1'
				AND tu.IdTransaccion = c.IdTransaccion 
				AND tu.IdOpcion = c.IdOpcion  
				AND tu.IdOrganizacion = c.idOrganizacion
				AND c.Idtransaccion = d.IdTransaccion
				AND c.IdOrganizacion = d.idOrganizacion
				-- AND d.idtransaccion between 2000 and 2999
				AND d.IdOrganizacion = org.IdOrganizacion
				AND org.IdAplicacion = @IdAplicacion
				AND org.IdAplicacion = apl.IdAplicacion
				AND tu.IdTransaccion = ht.IdTransaccion 
				AND tu.IdOpcion = ht.IdOpcion 
				AND tu.IdOrganizacion = ht.IdOrganizacion
				-- Revisa que excluya los feriados
				AND Seguridad.Seg_F_DiaFeriado(apl.IdEmpresa,0,ht.IdHorario) = 0
				AND ht.IdHorario = hdt.IdHorario
				AND  getdate() BETWEEN 
				convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,hdt.HoraInicio,20),12,8)),20)
					  AND 
			convert(datetime,(substring(convert(char,getdate(),20),1,10) 
			+ ' ' + substring(convert(char,hdt.HoraFin,20),12,8)),20)
   			AND substring(hdt.dias,datepart(dw,getdate()),1) = '1'
		END
		
   		IF @IdEmpresa > 0  
   		BEGIN
   			-- Dependiendo el Tipo de Login del usuario Aplicativo, se toma la medida
			SELECT @IdTipoLogin = IdTipoLogin
				FROM Participante.Par_RegistroCliente
					WHERE IdUsuario = @IdUsuario
			IF @IdTipoLogin = '1' -- Se quita el login activo
				UPDATE Seguridad.Seg_Registro 
				Set Estado = 'I'
				WHERE IdUsuario = @IdUsuario and Estado = 'A'	
			
			IF @IdTipoLogin = '2' -- No permite dar login, si ya tiene una conexion activo
			BEGIN
				IF EXISTS (SELECT 1 FROM Seguridad.Seg_Registro
							WHERE IdUsuario = @IdUsuario and Estado = 'A')
				BEGIN
					-- El usuario no esta activo, retorna solo el mensaje
  					raiserror ('Usuario Aplicativo esta registrado en otra maquina',16,1)	
					RETURN
				END
			END
			-- TipoLogin = 3 -- Multiple Reconexion, no se chequea nada
		
			-- Registrado el usuario
		  INSERT INTO Seguridad.Seg_REGISTRO (IdUsuario,FechaIngreso,IdAplicacion,Estado,Token,IdIdentificacion,
										FechaUltTrans,IdEmpresa, Dominio,MacMaquina)
			VALUES(@IdUsuario,getdate(),@IdAplicacion,'A',@Token, @Maquina,
										getdate(),@IdEmpresa, '',@MacMaquina)
	      
			-- Recupera los parametros de conexion de la aplicacion
			SELECT Parametro, Valor, Encriptado
			FROM Seguridad.Seg_ParamAplicacion		
			WHERE IdAplicacion = @IdAplicacion
			
			IF (@RecuperaMT = 'S')
			BEGIN
				-- consulta los servidores asociados a la aplicacion, con sus parametros
				SELECT sa.IdServidor, sa.TipoServidor, pa.Parametro, pa.Valor, pa.Encriptado
				FROM Seguridad.Seg_ServAplicacion sa 
						INNER JOIN Seguridad.Seg_ParamAplicacion pa
						ON pa.IdAplicacion = sa.IdServidor
						INNER JOIN Seguridad.seg_Aplicacion a
						ON a.IdAplicacion = sa.IdServidor
				WHERE sa.IdAplicacion = @IdAplicacion
				ORDER BY sa.IdServidor
				-- consulta los puertos asociados a la aplicacion
				SELECT Puerto, IdRol, QueueIN, QueueOUT, MaxHilos, BackLog,
					   Separador, MaxReadIdleMs, MaxSendIdleMs, TipoTrama, PosTransaccion
				  FROM Seguridad.Seg_PuertoAplicacion
				  WHERE IdAplicacion = @IdAplicacion
			END
		END
   	 	ELSE
   	 		-- El usuario no tiene permisos para ninguna aplicacion
      		raiserror ('Usuario Aplicativo no tiene permisos para ninguna aplicacion',16,1)	
     END 
     ELSE
      -- El usuario no esta activo, retorna solo el mensaje
      	raiserror ('Usuario no esta activo',16,1)	



