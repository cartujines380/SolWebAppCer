
CREATE PROCEDURE [Participante].[Par_P_InicializarModuloNuevaEmpresa]
@PI_IdParticipante int
as
	--Verificar si Secuencias para esta empresa ya fue creada
	if not exists( SELECT 1 FROM Participante.Par_SecuenciaGen WHERE IdEmpresa = @PI_IdParticipante
			and IdTabla = 2)
	BEGIN
		--Inicializa secuencias en Tabla Participante.Par_Organigrama
    	insert into Participante.Par_SecuenciaGen(IdSecuencia,IdEmpresa,IdTabla,Nombre) 
		values(0,@PI_IdParticipante,2,'Participante.Par_Organigrama')

		--Inicializa cargos por empresa
		exec Catalogo.Ctl_P_IngCatalogoEmpresa @PI_IdParticipante,207,1
	END

	
	





