
-- EXEC [Seguridad].[Seg_P_ConsBandejaUsrAdicional]  '<Root IdEmpresa="1" Ruc="0987654321001" />'

CREATE PROCEDURE [Seguridad].[Seg_P_ConsBandejaUsrAdicional]
	( @PI_XmlParam xml )
AS
BEGIN

	DECLARE
		 @IdEmpresa			INT
		,@Ruc				VARCHAR(13)
		,@Usuario			VARCHAR(13)
		,@Nombre			VARCHAR(13)
		,@Apellido			VARCHAR(13)
		,@Estado			VARCHAR(13)
		,@RecActas			VARCHAR(1)

	SELECT
		@IdEmpresa			= nref.value('@IdEmpresa','INT'),
		@Ruc				= nref.value('@Ruc','VARCHAR(13)'),
		@Usuario			= nref.value('@Usuario','VARCHAR(13)'),
		@Nombre				= nref.value('@Nombre','VARCHAR(13)'),
		@Apellido			= nref.value('@Apellido','VARCHAR(13)'),
		@Estado				= nref.value('@Estado','VARCHAR(13)'),
		@RecActas			= nref.value('@RecActas','VARCHAR(1)')
	FROM @PI_XmlParam.nodes('/Root') AS R(nref)
	
	IF(@Usuario IS NULL)
	BEGIN
		SET @Usuario='';
		SET @Usuario='%'+@Usuario+'%'
	END
	ELSE
		SET @Usuario='%'+@Usuario+'%'

	IF(@Nombre IS NULL)
	BEGIN
		SET @Nombre='';
		SET @Nombre='%'+@Nombre+'%'
	END
	ELSE
		SET @Nombre='%'+@Nombre+'%'
	
	IF(@Apellido IS NULL)
	BEGIN
		SET @Apellido='';
		SET @Apellido='%'+@Apellido+'%'
	END
	ELSE
		SET @Apellido='%'+@Apellido+'%'

	IF(@Estado='' or @Estado is null or @Estado='undefined')
	BEGIN
		SET @Estado='A';
	END
	IF(@RecActas='T' )
	BEGIN
		SET @RecActas= NULL
	END
	

	SELECT	a.Identificacion, u.Usuario,
			a.Nombre1 + ' ' + ISNULL(a.Nombre2, '') as Nombre,
			a.Apellido1 + ' ' + ISNULL(a.Apellido2, '') as Apellido,
			u.CorreoE, u.Celular, u.UsrAutorizador, u.Estado,pa.Clave, isnull(a.RecibeActas,0) as RecibeActas
		FROM [Seguridad].[Seg_Usuario] u
		INNER JOIN [Seguridad].[Seg_UsuarioAdicional] a ON 
			u.IdEmpresa = a.IdEmpresa AND u.Ruc = a.Ruc AND u.Usuario = a.Usuario
		INNER JOIN SIPE_FRAMEWORK.Participante.Par_Participante PA ON PA.IdParticipante=U.IdParticipante
		WHERE u.EsAdmin = 0 AND u.IdEmpresa = @IdEmpresa AND u.Ruc = @Ruc
			AND	U.Usuario LIKE(@Usuario) AND A.Nombre1 LIKE(@Nombre) AND A.Apellido1 LIKE(@Apellido)
			AND U.Estado=ISNULL(@Estado,U.Estado) AND U.ESTADO<>'M'
			AND isnull(a.RecibeActas,convert(bit,0)) = ISNULL(@RecActas, isnull(a.RecibeActas,convert(bit,0)))

        select case p.ApoderadoIdFiscal when '' then p.Ruc else isnull(p.ApoderadoIdFiscal,p.Ruc) end as Apoderado , 
		            p.NomComercial 
	    from proveedor.Pro_Proveedor p
		where p.Ruc = @Ruc

END


