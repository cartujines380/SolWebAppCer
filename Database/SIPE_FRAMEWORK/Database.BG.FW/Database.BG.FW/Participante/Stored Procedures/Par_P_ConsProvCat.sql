CREATE PROCEDURE [Participante].[Par_P_ConsProvCat]
@PI_IdEmpresa int,
@PI_IdCategoria int -- Categoria 12 es Proveedores Licencias
as
-- Información General 
SELECT	p.IdParticipante,Catalogo.Ctl_F_conCatalogo(7,p.IdTipoIdentificacion) as TipoIdentificacion,
		p.Identificacion, p.IdUsuario as Alias, em.Nombre
	FROM Participante.Par_Proveedor pr INNER JOIN Participante.Par_Empresa em 
				on pr.IdParticipante = em.IdParticipante INNER JOIN Participante.Par_Participante p
				on em.IdParticipante = p.IdParticipante
	WHERE   pr.IdEmpresa = @PI_IdEmpresa
			AND em.IdCategoriaEmpresa = @PI_IdCategoria

-- Información de sus contactos
SELECT mc.IdParticipante,Catalogo.Ctl_F_conCatalogo(10,mc.IdTipoMedioContacto) as Medio,
		mc.Valor
FROM Participante.Par_Proveedor pr INNER JOIN Participante.Par_Empresa em 
				on pr.IdParticipante = em.IdParticipante INNER JOIN Participante.Par_Direccion di
				on em.IdParticipante = di.IdParticipante INNER JOIN Participante.Par_MedioContacto mc
				on di.IdParticipante = mc.IdParticipante
	WHERE   pr.IdEmpresa = @PI_IdEmpresa
			AND em.IdCategoriaEmpresa = @PI_IdCategoria
		
 





