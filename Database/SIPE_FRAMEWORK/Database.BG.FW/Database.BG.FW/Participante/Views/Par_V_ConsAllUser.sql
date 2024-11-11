
create view [Participante].[Par_V_ConsAllUser]
as
SELECT     IdParticipante, IdEmpresa, IdUsuario, Nombre, Identificacion, Correo, TipoParticipante, 'Interno' AS TipoUsuario
FROM         Participante.Par_V_Cliente
UNION
SELECT     IdParticipante, IdEmpresa, IdUsuario, Nombre, Identificacion, Correo, TipoParticipante, 'Externo' AS TipoUsuario
FROM         Participante.Par_V_Empleado
