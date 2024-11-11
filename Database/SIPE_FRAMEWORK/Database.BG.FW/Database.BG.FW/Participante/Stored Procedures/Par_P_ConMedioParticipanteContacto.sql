Create procedure [Participante].[Par_P_ConMedioParticipanteContacto]
@PI_IdParticipante int,
@PI_IdDireccion int
as
select distinct  m.IdTipoMedioContacto,m.valor
from Participante.Par_MedioContacto m
where m.IdParticipante = @PI_IdParticipante and m.IdDireccion=@PI_IdDireccion





