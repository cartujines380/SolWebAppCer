CREATE function [Participante].[Par_F_getPaisProvCiuParticipante](@PI_IdParticipante int)
RETURNS TABLE
AS	

RETURN (SELECT  p.IdPais, Catalogo.Ctl_F_conCatalogo(2,p.IdPais) as Pais,
		p.IdProvincia,Catalogo.Ctl_F_conCatalogo(3,p.IdProvincia) as Provincia,
		p.IdCiudad, Catalogo.Ctl_F_conCatalogo(4,p.IdCiudad) as Ciudad
		FROM Participante.Par_Participante p 
		WHERE p.IdParticipante = @PI_IdParticipante)






