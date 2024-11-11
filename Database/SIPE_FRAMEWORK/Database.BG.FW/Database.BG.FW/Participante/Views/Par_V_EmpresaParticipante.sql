

CREATE VIEW [Participante].[Par_V_EmpresaParticipante]
AS
SELECT     p.IdParticipante, p.Identificacion, p.IdUsuario, p.Estado, CONVERT(varchar, p.FechaRegistro, 110) AS FechaRegistro, p.IdPais, p.IdProvincia, 
                      p.IdCiudad, p.CuentaContable, p.Comentario, e.Nombre, e.IdCategoriaEmpresa, e.Nivel, p.IdNaturalezaNegocio, e.IdEmpresaPadre, e.Licencia, 
                      e.Marca
FROM         Participante.Par_Participante AS p INNER JOIN
                      Participante.Par_Empresa AS e ON p.IdParticipante = e.IdParticipante






