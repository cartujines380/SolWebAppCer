

CREATE PROCEDURE [Seguridad].[Seg_P_ConsLoginUser]
@IdUsuario varchar(50),
@IdAplicacion int  
 AS
	Declare @UrlAD varchar(200), @MAC char
	-- Recupera el Url de donde se definio el usuario en el Active Directory
	SELECT @UrlAD = pa.Valor
	FROM Participante.Par_RegistroCliente rc
			INNER JOIN Participante.Par_Participante p
			ON p.IdParticipante = rc.IdParticipante
			  INNER JOIN Catalogo.Ctl_Catalogo c
			  ON c.IdTabla = 24 AND c.Codigo = p.TipoPartRegistro
                --DMUNOZ  ERROR CONVERSION DE STRING TO INT 
                --INNNER JOIN Seguridad.Seg_ParamAplicacion pa VERSION ANTERIOR
                -- convert(int,0)
				INNER JOIN Seguridad.Seg_ParamAplicacion pa
				ON convert(varchar,pa.IdAplicacion) =
                convert(varchar,isnull(c.DescAlterno,'')) AND pa.Parametro = 'Url'

	WHERE rc.IdUsuario = @IdUsuario 

	-- Recupera MAC = S si a la aplicacion que ingresa tiene un rol asociado a una MAC Adrees
	IF EXISTS (SELECT 1 FROM Seguridad.Seg_Organizacion o
							INNER JOIN Seguridad.Seg_opciontransrol otr
							ON otr.IdOrganizacion = o.IdOrganizacion
							  INNER JOIN Seguridad.Seg_RolUsuario ru
							  ON ru.IdRol = otr.IdRol
				WHERE o.IdAplicacion = @IdAplicacion
					AND ru.IdUsuario = @IdUsuario 
					AND ru.TipoIdentificacion = 2 AND ru.IdIdentificacion <> '')
		SET @MAC = 'S'
	ELSE
		SET @MAC = 'N'
-- Retorna datos
	SELECT @UrlAD as UrlAD, @MAC as MAC 














