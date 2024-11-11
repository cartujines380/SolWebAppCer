

--exec Participante.Par_P_ConClienteOficGes 13, 45

CREATE PROCEDURE [Participante].[Par_P_ConClienteOficGes] 
@PI_IdEmpresa      int,
@PI_IdParticipante int
AS	

select Participante.Par_F_ConClienteOficGes(@PI_IdEmpresa,@PI_IdParticipante) as IdOFicinaGestion





