
CREATE procedure [Seguridad].[Seg_P_INGRESA_APLICACION]
	@PV_xmlDoc VARCHAR(max)
AS            
DECLARE
		@PO_IdAplicacion int,
		@PV_IdEmpresa    int,
		@PV_Nombre       VARCHAR(30),
       	@PV_Descripcion  VARCHAR(100),
       	@PV_TipoServidor VARCHAR(10),
       	@PV_Datagrama	 VARCHAR(100),
       	@PV_Link	 VARCHAR(100),
       	@id_XML as int

exec sp_xml_preparedocument @id_XML OUTPUT, @PV_xmlDoc

SELECT 
		@PV_IdEmpresa    =IdEmpresa,
		@PV_Nombre       =Nombre,
       	@PV_Descripcion  =Descripcion,
       	@PV_TipoServidor =TipoServidor,
       	@PV_Datagrama	 =Datagrama,
       	@PV_Link         =Link
       	FROM openXML (@id_XML, '//Datos', 1) WITH (IdEmpresa int, Nombre varchar(30), 
       											Descripcion varchar(100), 
       											TipoServidor varchar(10),
       											Datagrama varchar(100),
       											Link varchar(100))

/* DEBE RECIBIR UN XML CONTENIENDO TODOS LOS PARAMETROS ASOCIADOS
   A LA APLICACION */
	if exists(SELECT 1 From Seguridad.Seg_APLICACION Where upper(Nombre) = upper(@PV_nombre))
	BEGIN
		RAISERROR ('Nombre de Aplicacion ya existe',16,1)
		return
	END

SELECT @PO_IdAplicacion = Max(IdAplicacion) From Seguridad.Seg_APLICACION
SET @PO_IdAplicacion = isnull( @PO_IdAplicacion, 0 ) + 1

BEGIN TRAN

	Insert into Seguridad.Seg_APLICACION (IdAplicacion, IdEmpresa, Nombre, Descripcion, TipoServidor, Datagrama, Link) 
		Values (@PO_IdAplicacion, @PV_IdEmpresa, @PV_Nombre, @PV_Descripcion, @PV_TipoServidor, @PV_Datagrama, @PV_Link) 

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

/*
	--DEBE INGRESAR LOS PARAMETRO EN Seg_ParamAplicacion
	
	Insert Into Seguridad.Seg_APLICACION (IdAplicacion, nombre,descripcion,servidor,
								usuario,clave,TipoServidor, IdEmpresa,
								Puerto,MinPool, MaxPool)
	Values(@PO_IdAplicacion, @PV_nombre,@PV_Descripcion, @PV_Servidor, 
								@PV_Usuario, @PV_Clave,@PV_TipoServidor,1,
								@PV_Puerto,@PV_MinPool, @PV_MaxPool)
*/

	Insert into Seguridad.Seg_ParamAplicacion (IdAplicacion, Parametro, Valor, Encriptado)
	select @PO_IdAplicacion, Parametro, Valor, Encriptado
		FROM openXML (@id_XML, '//Parametros/RowI', 1) WITH (Parametro varchar(50), 
													Valor varchar(200),
													Encriptado bit)

-- INGRESA LOS SERVIDORES ASOCIADOS A UNA APLICACION
	INSERT Seguridad.Seg_ServAplicacion (IdAplicacion, IdServidor, TipoServidor)
	select @PO_IdAplicacion, IdServidor, TipoServidor
		FROM openXML (@id_XML, '//Servidores/ServidorI', 1) WITH (IdServidor int, 
												  TipoServidor varchar(10))

-- INGRESA LOS PUERTOS DE A UNA APLICACION
	INSERT Seguridad.Seg_PuertoAplicacion (IdAplicacion, Puerto, IdRol, QueueIN, QueueOUT,
	         MaxHilos, BackLog, Separador, MaxReadIdleMs, MaxSendIdleMs, TipoTrama, PosTransaccion)
	select @PO_IdAplicacion, Puerto, IdRol, QueueIN, QueueOUT, MaxHilos,
	         BackLog, Separador, MaxReadIdleMs, MaxSendIdleMs, TipoTrama, PosTransaccion
		FROM openXML (@id_XML, '//Puertos/PuertoI', 1)
			 WITH (Puerto int, IdRol int, QueueIN varchar(100), QueueOUT varchar(100), MaxHilos int,
				   BackLog int, Separador varchar(5), MaxReadIdleMs int, MaxSendIdleMs int, TipoTrama varchar(50), PosTransaccion int)

IF (@@error <> 0)
BEGIN
  ROLLBACK TRAN
  RETURN
END 

COMMIT TRAN

SELECT PO_IdAplicacion = @PO_IdAplicacion

exec sp_xml_removedocument @id_XML


