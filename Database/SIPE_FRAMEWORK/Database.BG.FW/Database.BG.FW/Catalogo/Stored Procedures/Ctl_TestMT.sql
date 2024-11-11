create proc [Catalogo].[Ctl_TestMT](@PI_Dato varchar(100))
AS
Select @PI_Dato Dato,convert(varchar(8),getdate(),112) FechaFormato
