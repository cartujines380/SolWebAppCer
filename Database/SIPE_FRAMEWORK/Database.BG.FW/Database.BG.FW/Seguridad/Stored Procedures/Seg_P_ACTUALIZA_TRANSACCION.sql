



CREATE procedure [Seguridad].[Seg_P_ACTUALIZA_TRANSACCION]
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
     UPDATE Seguridad.Seg_TRANSACCION
            SET estado=Estado,
                descripcion=descripcion,
		Auditable = Auditable,
		Parametros = Parametros,
		IdServidor = @IdServidor,
		NombreBase = @NombreBase,
		NombreSP = @NombreSP,
		Menu = @Menu,
		Monitor = @Monitor,
		XmlEntrada = @XmlEntrada,
		XmlSalida = @XmlSalida,
		XmlValidador = @XmlValidador
     WHERE idtransaccion=@IdTransaccion
           AND IdOrganizacion =@IdOrganizacion

