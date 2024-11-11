
CREATE Procedure [Catalogo].[Ctl_P_IngItemCatalogo]
(
		@IdTabla int,
		@Codigo varchar(50) = '', --si es nulo se toma el siguiente secuencial
		@Descripcion varchar(max)
	)
AS
	IF @Codigo = ''
		SELECT @Codigo = convert(varchar,MAX( convert(int,Codigo)) + 1) 
			from catalogo.ctl_catalogo where idtabla = @IdTabla
	
	INSERT Catalogo.Ctl_Catalogo(IdTabla,Codigo,Descripcion,Estado)
	VALUES(@IdTabla,@Codigo,@Descripcion,'A')
	
	SELECT @Codigo as Codigo

