CREATE FUNCTION [Participante].[Par_F_DireccionParticipante](
	@PI_IdParticipante int,
	@PI_IdTipoDireccion int)
RETURNS TABLE
AS
RETURN(SELECT IdDireccion,Direccion,
	IdPais, Catalogo.Ctl_F_conCatalogo(2,IdPais) Pais,
	IdProvincia,Catalogo.Ctl_F_conCatalogo(3,IdProvincia) Provincia,
	IdCiudad,Catalogo.Ctl_F_conCatalogo(4,IdCiudad) Ciudad,
	isnull(NombreContacto,'') NombreContacto,
	isnull(HorarioContacto,'') HorarioContacto
	FROM Participante.Par_Direccion
	where IdParticipante = @PI_IdParticipante
		and IdTipoDireccion = @PI_IdTipoDireccion)





