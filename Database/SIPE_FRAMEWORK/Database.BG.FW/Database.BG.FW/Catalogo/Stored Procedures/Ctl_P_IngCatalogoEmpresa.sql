Create Proc [Catalogo].[Ctl_P_IngCatalogoEmpresa]
	( @PI_IdEmpresa int, @PI_IdTabla int, @PI_Codigo varchar(50) ) 
AS

	--Inserto el registro
	insert into Catalogo.Ctl_CatalogoEmpresa( IdEmpresa, IdTabla, Codigo )
	values ( @PI_IdEmpresa, @PI_IdTabla, @PI_Codigo )
	IF (@@error <> 0)
	BEGIN
	  ROLLBACK TRAN
	  RETURN
	END





