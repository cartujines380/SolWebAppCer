
create FUNCTION [Proveedor].[Pro_F_SolMedioContacto]
(
	@IdSolicitud bigint=null,
	@IdContacto bigint=null,
	@Medio varchar(10)
)
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @ResultVar varchar(100)

	--Select top 1  @ResultVar = a.ValorMedioContacto
	--from [Proveedor].[Pro_SolMedioContacto] a
	--where a.IdSolicitud=@IdSolicitud and a.IdSolContacto=@IdContacto and a.TipMedioContacto=@Medio
			if (isnull(@IdContacto,0)=0)
			begin 
			Select top 1  @ResultVar = a.ValorMedioContacto
			from [Proveedor].[Pro_SolMedioContacto] a
			where a.IdSolicitud=@IdSolicitud and a.TipMedioContacto=@Medio
			end
			else
			begin
			Select top 1  @ResultVar = a.ValorMedioContacto
			from [Proveedor].[Pro_SolMedioContacto] a
			where a.IdSolicitud=@IdSolicitud and a.IdSolContacto=@IdContacto and a.TipMedioContacto=@Medio
			end

	RETURN @ResultVar
END



