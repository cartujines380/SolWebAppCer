



/*==============================================================*/
/* MANTENIMIENTO: TRANSACCION     */
/*==============================================================*/


 CREATE procedure [Seguridad].[Seg_P_INGRESA_TRANSACCION]
	@IdTransaccion     int,
    @IdOrganizacion      int,
	@Estado            CHAR,
    @Descripcion       VARCHAR(100),
    @Auditable	char,
	@IdServidor int,
	@NombreBase varchar(100),
	@NombreSP varchar(100),
	@Parametros varchar(max),
	@Menu bit,
	@Monitor bit,
	@XmlEntrada varchar(max),
	@XmlSalida varchar(max),
	@XmlValidador varchar(max)
AS 
  
   IF NOT EXISTS(select 1
				from Seguridad.Seg_Transaccion 
				where IdTransaccion=@IdTransaccion and
					  IdOrganizacion=@IdOrganizacion )
     BEGIN
		INSERT INTO Seguridad.Seg_TRANSACCION(idtransaccion,estado,descripcion,idOrganizacion,
										Auditable,Parametros,IdServidor,NombreBase,NombreSP,
										Menu,Monitor,XmlEntrada,XmlSalida,XmlValidador)
            VALUES(@IdTransaccion,@Estado,@Descripcion, @IdOrganizacion,
					@Auditable,@Parametros,@IdServidor,@NombreBase,@NombreSP,
					@Menu,@Monitor,@XmlEntrada,@XmlSalida,@XmlValidador)
     END      
   ELSE
       raiserror ('Transaccion ya existe',16,1)
  

