/*
 exec Seguridad.Seg_P_REGISTRA_USUARIO 'kcrillo','12345678901234111890123456789012','sipecom60',1,1,'',''
	select * from participante.par_registrocliente where IdTipoLogin idusuario = 'usrPrueba3'
	update participante.par_registrocliente 
	set IdTipoLogin = '2'
	where idusuario = 'usrPrueba3'
	
*/

CREATE  procedure [Seguridad].[Seg_P_REGISTRA_USUARIO]
@PV_idUsuario              varchar(20),
@PV_token                  varchar(32),
@PV_maquina                varchar(100),
@PV_MacMaquina                varchar(20),
@PV_idaplicacion	int,
@PV_idempresa		    int,
@PV_Dominio varchar(100)
AS
  SET NOCOUNT ON
  declare @ln_cont int
  declare @msg varchar(200), @IdTipoLogin varchar(10)
  SET @msg = ''
  
 -- 	IF EXISTS (SELECT 1 FROM Participante.Par_RegistroCliente r,
	--						Participante.Par_Participante p
	--			WHERE r.IdUsuario = @PV_idUsuario
	--			AND r.Estado = 1 AND r.IdParticipante = p.IdParticipante
	--			AND p.Estado = 'A'
	--			AND convert(datetime,(substring(convert(char,FechaExpira,20),1,10) + ' 00:00:00.000')) >=
	--			convert(datetime,(substring(convert(char,getdate(),20),1,10) + ' 00:00:00.000')) )
	--BEGIN
		-- Dependiendo el Tipo de Login del usuario, se toma la medida
		SELECT @IdTipoLogin = IdTipoLogin
			FROM Participante.Par_RegistroCliente
				WHERE IdUsuario = @PV_idUsuario
		IF @IdTipoLogin = '1' -- Se quita el login activo
			UPDATE Seguridad.Seg_Registro 
			Set Estado = 'I'
			WHERE IdUsuario = @PV_idUsuario and Estado = 'A'	
		
		IF @IdTipoLogin = '2' -- No permite dar login, si ya tiene una conexion activo
		BEGIN
			IF EXISTS (SELECT 1 FROM Seguridad.Seg_Registro
						WHERE IdUsuario = @PV_idUsuario and Estado = 'A')
			BEGIN
				-- El usuario no esta activo, retorna solo el mensaje
  				raiserror ('Usuario esta registrado en otra maquina',16,1)	
				RETURN
			END
		END
		-- TipoLogin = 3 -- Multiple Reconexion, no se chequea nada

	-- Recupera el ultimo login
	SELECT @msg = @msg + 
			' Ultimo Fecha de Ingreso: ' 
			+ convert(varchar,r.FechaIngreso) 
			+ ' en la maquina: ' + r.IdIdentificacion
	FROM Seguridad.Seg_Registro r
	WHERE IdUsuario = @PV_idUsuario
		AND Token = ( SELECT TOP 1 r1.Token FROM Seguridad.Seg_Registro r1
				WHERE r1.IdUsuario = @PV_idUsuario
				ORDER BY FechaIngreso DESC
			     )

		-- raiserror (52004,16,1,@PV_idUsuario)
            -- Registrado el usuario
          INSERT INTO Seguridad.Seg_REGISTRO (IdUsuario,FechaIngreso,IdAplicacion,Estado,Token,IdIdentificacion,
						FechaUltTrans,IdEmpresa, Dominio,MacMaquina)
            VALUES(@PV_idUsuario,getdate(),@PV_idaplicacion,'A',@PV_token,@PV_maquina,
						getdate(),@PV_idempresa, @PV_Dominio,@PV_MacMaquina)

	
		-- recuperar roles administrativos
			DECLARE @concat VARCHAR(40), @abreviatura VARCHAR(10), @prioridad int
			SET @concat=''
			DECLARE cursor_ CURSOR STATIC
			FOR 
			  select ro.Abreviatura,
				CASE 
				WHEN ru.IdRol = 24 THEN 1
				WHEN ru.IdRol = 27 THEN 2
				WHEN ru.IdRol = 26 THEN 3
				WHEN ru.IdRol = 25 THEN 4
				END AS B
				 from Seguridad.Seg_Rol ro inner join
				Seguridad.Seg_RolUsuario ru on ru.IdRol=ro.IdRol
				where ru.IdRol in (24,25,26,27) and ru.IdUsuario = @PV_idUsuario
				order by B

			OPEN cursor_
			FETCH cursor_ INTO @abreviatura,@prioridad

			WHILE (@@FETCH_STATUS = 0)
			BEGIN		
				IF @concat<>''
				BEGIN
					SET @concat=@concat+' | '+@abreviatura
				END
					ELSE
					BEGIN
						SET @concat=@concat+@abreviatura
					END
				FETCH cursor_ INTO @abreviatura,@prioridad
			END;

			CLOSE cursor_
			DEALLOCATE cursor_
          
		-- Recupera Informacion del Usuario
		 SELECT p.IdParticipante,p.Identificacion,
			Participante.Par_F_getNombreParticipante(p.IdParticipante) as Nombre, 
			@msg as 'Estado', p.Opident,
			isnull(c.EsClaveNuevo, 'N') as EsClaveNuevo,
			isnull(c.EsClaveCambio, 'N') as EsClaveCambio,
			isnull(c.EsClaveBloqueo, 'N') as EsClaveBloqueo,
			ISNULL(Catalogo.Ctl_F_conCatalogo(207,IdCargo), '') as CargoEmpleado,
			@concat as Rol,
			isNUll(Ruc,'') as rucEmpresa,
			isNUll(NomComercial,'') as nomEmpresa
		FROM	Participante.par_RegistroCliente r
				inner join  Participante.par_participante p
					on r.IdParticipante = p.IdParticipante
				left outer join Seguridad.Seg_Clave c
					on c.IdParticipante = p.IdParticipante
				left outer join Participante.Par_Empleado e
					on e.IdParticipante = p.IdParticipante
				left outer join
				(
				select IdParticipante,su.Ruc,NomComercial from SIPE_PROVEEDOR.Seguridad.Seg_Usuario su 
				inner join SIPE_PROVEEDOR.Proveedor.Pro_Proveedor pp on pp.CodProveedor=su.CodProveedor
				) au on au.IdParticipante=p.IdParticipante
		  WHERE r.IdUsuario = @PV_idUsuario
			
     --END 
      
     --ELSE
     -- -- El usuario no esta activo, retorna solo el mensaje
     -- 	raiserror ('Usuario no esta activo',16,1)	

