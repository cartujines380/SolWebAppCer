CREATE PROCEDURE [Participante].[Par_P_TreeEmpresa]
@PI_IdEmpresaPadre int
AS

DECLARE @Tabla table (Codigo int,Descripcion varchar(100), Nivel int,CodigoPadre int, 
					IdUsuario varchar(100), Estado char, TipoParticipante char, 
					Identificacion varchar(50) )
DECLARE @CodigoPadre int, @MinNivel int;

with descendientes(Codigo,Descricion, Nivel,CodigoPadre, 
					IdUsuario, Estado, TipoParticipante, Identificacion) AS
	(SELECT e.IdParticipante as Codigo, e.Nombre as Descripcion,
			e.Nivel, e.IdEmpresaPadre as CodigoPadre, p.IdUsuario, 
			p.Estado, p.TipoParticipante, p.Identificacion
	FROM Participante.Par_Empresa e INNER JOIN Participante.Par_Participante p 
		ON e.IdParticipante = p.IdParticipante
	where e.IdParticipante = @PI_IdEmpresaPadre
	UNION ALL
	SELECT e.IdParticipante as Codigo, e.Nombre as Descripcion,
			e.Nivel, e.IdEmpresaPadre as CodigoPadre, p.IdUsuario, 
			p.Estado, p.TipoParticipante, p.Identificacion
	FROM Participante.Par_Empresa e INNER JOIN Participante.Par_Participante p 
		ON e.IdParticipante = p.IdParticipante
		 INNER JOIN descendientes as A 
				ON a.Codigo = e.IdEmpresaPadre )
INSERT @Tabla
SELECT Codigo,Descricion, Nivel,CodigoPadre, 
					IdUsuario, Estado, TipoParticipante, Identificacion 
FROM descendientes
SELECT @MinNivel = Min(Nivel) FROM @Tabla
SELECT @CodigoPadre = CodigoPadre FROM @Tabla Where Nivel = @MinNivel
IF @MinNivel <> 1 -- NOT EXISTS (SELECT 1 FROM @Tabla WHERE Nivel = 1 )
BEGIN
	-- Actualizo los niveles
	UPDATE @Tabla 
		SET Nivel = Nivel - @MinNivel + 2
	INSERT @Tabla VALUES(@CodigoPadre,'Empresas Definidas',1,-1,'','A','E','')
END

SELECT * FROM @Tabla ORDER BY Nivel



