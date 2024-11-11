
CREATE PROCEDURE [Participante].[Par_P_ConDireccion]
@PI_IdParticipante int
AS
	SELECT d.IdDireccion, d.IdTipoDireccion,Catalogo.Ctl_F_conCatalogo(9,d.IdTipoDireccion) as TipoDireccion
		,d.Direccion, d.NumCasa, d.CallePrincipal, d.CalleTransversal, d.NombreContacto,d.HorarioContacto
		,d.IdPais,Catalogo.Ctl_F_conCatalogo(2,d.IdPais) as Pais
		,d.IdProvincia,Catalogo.Ctl_F_conCatalogo(3,d.IdProvincia) as Provincia
		,d.IdCiudad,Catalogo.Ctl_F_conCatalogo(4,d.IdCiudad) as Ciudad	
		,d.IdParroquia,Catalogo.Ctl_F_conCatalogo(18,d.IdParroquia) as Parroquia
		,d.IdBarrio,Catalogo.Ctl_F_conCatalogo(19,d.IdBarrio) as Barrio	
	FROM Participante.Par_Direccion d
	WHERE d.IdParticipante = @PI_IdParticipante





