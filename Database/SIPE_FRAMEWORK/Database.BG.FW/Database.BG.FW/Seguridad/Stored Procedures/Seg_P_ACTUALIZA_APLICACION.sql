

CREATE procedure [Seguridad].[Seg_P_ACTUALIZA_APLICACION]
	@PV_xmlDoc VARCHAR(max)
AS            
DECLARE
		@PV_IdAplicacion int,
		@PV_IdEmpresa    int,
		@PV_Nombre       VARCHAR(30),
       	@PV_Descripcion  VARCHAR(100),
       	@PV_TipoServidor VARCHAR(10),
       	@PV_Datagrama	 VARCHAR(100),
       	@PV_Link	     VARCHAR(100),
       	@id_XML as int

exec sp_xml_preparedocument @id_XML OUTPUT, @PV_xmlDoc

SELECT 
		@PV_IdAplicacion =IdAplicacion,
		@PV_IdEmpresa    =IdEmpresa,
		@PV_Nombre       =Nombre,
       	@PV_Descripcion  =Descripcion,
       	@PV_TipoServidor =TipoServidor,
       	@PV_Datagrama	 =Datagrama,
       	@PV_Link         =Link
       	FROM openXML (@id_XML, '//Datos', 1) WITH (IdAplicacion int, 
       								IdEmpresa int, Nombre varchar(30), 
       								Descripcion varchar(100), 
       								TipoServidor varchar(10),
       								Datagrama varchar(100),
       								Link varchar(100))

/* DEBE RECIBIR UN XML CONTENIENDO TODOS LOS PARAMETROS ASOCIADOS
   A LA APLICACION */

if exists(SELECT 1 From Seguridad.Seg_APLICACION Where upper(Nombre) = upper(@PV_nombre) and IdAplicacion != @PV_IdAplicacion)
BEGIN	
	RAISERROR ('Nombre de Aplicacion ya existe',16,1)
	return 
END
BEGIN TRAN

	-- DEBE ACTUALIZAR EN Seg_ParamAplicacion
     	UPDATE Seguridad.Seg_APLICACION 
		set nombre = @PV_nombre,
	            Descripcion = @PV_Descripcion,  
				IdEmpresa = @PV_IdEmpresa,                
	            TipoServidor = @PV_TipoServidor,
	            Datagrama = CASE @PV_Datagrama
							WHEN '' THEN Seguridad.Seg_APLICACION.Datagrama 
							ELSE @PV_Datagrama END,
	            Link = @PV_Link
	Where idaplicacion =  @PV_IdAplicacion	
	
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

	-- Elimina los parametros que no existen
	Delete from Seguridad.Seg_ParamAplicacion 
		Where idaplicacion =  @PV_IdAplicacion and 
			exists (select 1 FROM openXML (@id_XML, '//Parametros/RowD', 1) WITH (Parametro varchar(50)) x
						Where x.Parametro = Seguridad.Seg_ParamAplicacion.Parametro)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
	-- Actualiza los parametros que han sido modicados
	UPDATE Seguridad.Seg_ParamAplicacion
	SET Valor = CASE 
				WHEN x.Encriptado = 0 THEN x.Valor --si no es encriptado se pone el valor dado 
				WHEN x.Encriptado = 1 AND x.Valor = '' THEN pa.Valor --se mantiene si viene blanc0
				ELSE x.Valor END,
		Encriptado = x.Encriptado
	FROM Seguridad.Seg_ParamAplicacion pa INNER JOIN
			openXML (@id_XML, '//Parametros/RowU', 1) WITH (Parametro varchar(50), 
												Valor varchar(200),
												Encriptado bit) x
			ON pa.Parametro = x.Parametro
	WHERE pa.IdAplicacion = @PV_IdAplicacion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
	-- Ingresa Nuevos Parametros 
	Insert into Seguridad.Seg_ParamAplicacion (IdAplicacion, Parametro, Valor, Encriptado)
	select @PV_IdAplicacion, Parametro, Valor , Encriptado
		FROM openXML (@id_XML, '//Parametros/RowI', 1) WITH (Parametro varchar(50), 
													Valor varchar(200),
													Encriptado bit)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- MANTENIMIENTO DE SERVIDORES ASIGNADOS A UNA APLICACION
-- El XML trae todos los servidores
	-- Elimina los servidores que no existen
	Delete from Seguridad.Seg_ServAplicacion 
		Where IdAplicacion =  @PV_IdAplicacion and 
			exists (select 1 FROM openXML (@id_XML, '//Servidores/ServidorD', 1) WITH (IdServidor int) x
						Where x.IdServidor = Seguridad.Seg_ServAplicacion.IdServidor)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
	-- Ingresa Nuevos servidores 
	INSERT Seguridad.Seg_ServAplicacion (IdAplicacion, IdServidor, TipoServidor)
	select @PV_IdAplicacion, IdServidor, TipoServidor
		FROM openXML (@id_XML, '//Servidores/ServidorI', 1) WITH (IdServidor int, 
												  TipoServidor varchar(10))
												  
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

-- MANTENIMIENTO DE PUERTOS DE LA APLICACION
	-- Elimina los puertos que no existen
	Delete from Seguridad.Seg_PuertoAplicacion
		Where idaplicacion =  @PV_IdAplicacion and 
			exists (select 1 FROM openXML (@id_XML, '//Puertos/PuertoD', 1) WITH (Puerto varchar(50)) x
						Where x.Puerto = Seguridad.Seg_PuertoAplicacion.Puerto)
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
	-- Actualiza los puertos que han sido modicados
	UPDATE Seguridad.Seg_PuertoAplicacion
	SET IdRol = x.IdRol,
		QueueIN = x.QueueIN,
		QueueOUT = x.QueueOUT,
		MaxHilos = x.MaxHilos,
		BackLog = x.BackLog,
		Separador = x.Separador,
		MaxReadIdleMs = x.MaxReadIdleMs,
		MaxSendIdleMs = x.MaxSendIdleMs,
		TipoTrama = x.TipoTrama,
		PosTransaccion = x.PosTransaccion
	FROM Seguridad.Seg_PuertoAplicacion pa INNER JOIN
			openXML (@id_XML, '//Puertos/PuertoU', 1)
			 WITH (Puerto int, IdRol int, QueueIN varchar(100), QueueOUT varchar(100), MaxHilos int,
				   BackLog int, Separador varchar(5), MaxReadIdleMs int, MaxSendIdleMs int, TipoTrama varchar(50) , PosTransaccion int) x
			ON pa.Puerto = x.Puerto
	WHERE pa.IdAplicacion = @PV_IdAplicacion
IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 
	-- Ingresa Nuevos puertos 
	INSERT Seguridad.Seg_PuertoAplicacion (IdAplicacion, Puerto, IdRol, QueueIN, QueueOUT,
	         MaxHilos, BackLog, Separador, MaxReadIdleMs, MaxSendIdleMs, TipoTrama , PosTransaccion)
	select @PV_IdAplicacion, Puerto, IdRol, QueueIN, QueueOUT, MaxHilos,
	         BackLog, Separador, MaxReadIdleMs, MaxSendIdleMs, TipoTrama, PosTransaccion
		FROM openXML (@id_XML, '//Puertos/PuertoI', 1)
			 WITH (Puerto int, IdRol int, QueueIN varchar(100), QueueOUT varchar(100), MaxHilos int,
				   BackLog int, Separador varchar(5), MaxReadIdleMs int, MaxSendIdleMs int, TipoTrama varchar(50) , PosTransaccion int)

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

COMMIT TRAN

exec sp_xml_removedocument @id_XML

