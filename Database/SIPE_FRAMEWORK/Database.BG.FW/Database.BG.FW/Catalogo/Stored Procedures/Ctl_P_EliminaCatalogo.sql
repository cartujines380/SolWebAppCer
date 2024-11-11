Create Proc [Catalogo].[Ctl_P_EliminaCatalogo]
@PI_Codigo_tbl int,
@PI_Codigo varchar(10)
AS
	DELETE Catalogo.Ctl_Catalogo
	WHERE idTabla = @PI_Codigo_tbl and Codigo = @PI_Codigo




