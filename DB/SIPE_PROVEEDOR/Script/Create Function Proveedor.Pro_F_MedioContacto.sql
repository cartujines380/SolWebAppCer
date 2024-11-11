USE [SIPE_PROVEEDOR]
GO
IF EXISTS (select 1 from sysobjects where name = 'Pro_F_MedioContacto' and type='FN')
begin
    drop function  Proveedor.Pro_F_MedioContacto
    if exists (select 1 from sysobjects where name = 'Pro_F_MedioContacto' and type = 'FN')
      PRINT '<<< DROP FUNCTION Pro_F_MedioContacto -- ERROR -- >>>'
    else
      PRINT '== DROP FUNCTION Pro_F_MedioContacto *OK* =='
end

GO

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

GO

if exists(select 1 from sysobjects where name='Pro_F_MedioContacto' and type = 'FN')
  PRINT '== CREATE FUNCTION Pro_F_MedioContacto *OK* =='
 else
  PRINT '<<< CREATE FUNCTION Pro_F_MedioContacto -- ERROR -- >>>'
GO



