
CREATE PROCEDURE [Participante].[Par_P_ConIdUsuario]
@PI_TipoId char,
@PI_NumId varchar(15),
@PO_IdUsuario varchar(50) output,
@PO_Correo varchar(100) output
AS

-- Asume Correo = 3
-- Se ejecuta desde VS2003
	SELECT @PO_IdUsuario = p.IdUsuario, 
	       @PO_Correo = mc.Valor
	FROM Participante.Par_RegistroCliente rc, Participante.Par_Participante p, 
	     Participante.Par_MedioContacto mc
	WHERE rc.TipoIdent = @PI_TipoId
	    AND rc.NumIdent = @PI_NumId
	    AND rc.IdParticipante = p.IdParticipante
	    AND rc.Estado = 1
	    AND mc.IdParticipante = p.IdParticipante
	    AND mc.IdDireccion = 1
	    AND mc.IdTipoMedioContacto = 3 -- Correo






