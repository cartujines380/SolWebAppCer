
CREATE FUNCTION [Proveedor].[Pro_F_MedioContacto]
(
	@CodProveedor varchar(10)=null,
	@IdContacto bigint=null,
	@Medio varchar(10)
)
RETURNS VARCHAR(100)
AS
BEGIN
	DECLARE @ResultVar varchar(100)

			if (isnull(@IdContacto,0)=0)
			begin 
			Select top 1  @ResultVar = a.ValorMedioContacto
			from [Proveedor].[Pro_MedioContacto] a
			where a.CodProveedor=@CodProveedor and a.TipMedioContacto=@Medio
			end
			else
			begin
			Select top 1  @ResultVar = a.ValorMedioContacto
			from [Proveedor].[Pro_MedioContacto] a
			where a.CodProveedor=@CodProveedor and a.IdContacto=@IdContacto and a.TipMedioContacto=@Medio
			end

	RETURN @ResultVar
END

