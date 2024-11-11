Create Proc [Catalogo].[Ctl_P_EliCatalogoDependiente]
@PI_CodigoPadre varchar(50), 
@PI_IdTablaPadre int, 
@PI_IdTablaHija int 
AS
BEGIN TRAN
--Eliminar Catalogos Hijos
	DELETE Catalogo.Ctl_Catalogo
        WHERE IdTabla = @PI_IdTablaHija AND DescAlterno = @PI_CodigoPadre 
        IF @@error <> 0
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
--Eliminar Catalogo Padre
	DELETE Catalogo.Ctl_Catalogo
        WHERE IdTabla = @PI_IdTablaPadre AND Codigo = @PI_CodigoPadre 
        IF @@error <> 0
	BEGIN
		ROLLBACK TRAN
		RETURN
	END
COMMIT TRAN




