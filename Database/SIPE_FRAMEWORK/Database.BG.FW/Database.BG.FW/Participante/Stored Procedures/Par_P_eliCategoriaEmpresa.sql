CREATE  procedure [Participante].[Par_P_eliCategoriaEmpresa]
@PI_IdCategoria int 
AS
if exists(SELECT 1 From Participante.Par_CategoriaEmpresa Where IdCategoriaEmpPadre = @PI_IdCategoria)
	BEGIN
		raiserror (52103,16,1,@PI_IdCategoria)
		return
	END
--Pregunto si esta siendo referenciada en la tabla Participante.Par_Empresa
if exists(SELECT 1 From Participante.Par_Empresa Where IdCategoriaEmpresa = @PI_IdCategoria)
	BEGIN
		raiserror (52104,16,1,@PI_IdCategoria)
		return
	END

--Pregunto si existe para ser eliminar
IF NOT EXISTS(SELECT 1 FROM Participante.Par_CategoriaEmpresa WHERE IdCategoriaEmpresa = @PI_IdCategoria)
BEGIN
	raiserror (52102,16,1)	
	return
END

--Elimina Categoria
DELETE Participante.Par_CategoriaEmpresa
WHERE IdCategoriaEmpresa = @PI_IdCategoria
IF (@@error <> 0)
BEGIN
  	RAISERROR (52106,16,1)	
	RETURN
END 






