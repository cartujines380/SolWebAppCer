CREATE VIEW [Participante].[Par_V_TelefonoCorreoPart]
AS

	SELECT	distinct p.Idparticipante,d.Direccion,d.IdPais,
			d.IdProvincia, d.IdCiudad, mc1.Valor Telefono,mc1.ValorAlt DDI, 
			mc2.Valor Correo
	FROM	Participante.Par_Participante p
				left outer join Participante.Par_Direccion d
					on  p.IdParticipante = d.IdParticipante and d.IdDireccion = '1'
				left outer join Participante.Par_MedioContacto mc1
					on  p.IdParticipante = mc1.IdParticipante AND d.IdDireccion = mc1.IdDireccion
						AND  mc1.IdTipoMedioContacto = '1'--Telefono
				left outer join Participante.Par_MedioContacto mc2 
					on p.IdParticipante = mc2.IdParticipante AND d.IdDireccion = mc2.IdDireccion
						AND mc2.IdTipoMedioContacto = '3' --Correo	
	




