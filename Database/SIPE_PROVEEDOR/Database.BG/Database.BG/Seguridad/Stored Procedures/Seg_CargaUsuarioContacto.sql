-- =============================================
-- Author:		Miguel Rodriguez
-- Create date: 18-09-2015
-- Description:	ACTUALIZAR CARGA DE ARCHIVO
-- 319
-- =============================================

CREATE PROCEDURE [Seguridad].[Seg_CargaUsuarioContacto]
	@PI_ParamXML xml
AS
BEGIN
	DECLARE @W_TIPO INT
	DECLARE @W_CODSAP VARCHAR(15) 
	SET NOCOUNT ON;
	SELECT	
					
			    @W_TIPO=nref.value('@Tipo','INT'),
				@W_CODSAP=nref.value('@CodSap','VARCHAR(15)')
				FROM  @PI_ParamXML.nodes('/Root') as item(nref)
	
	IF(@W_TIPO=1)
	BEGIN
		SELECT DISTINCT SEG.CodProveedor
		FROM [Seguridad].[Seg_Usuario] SEG 
			INNER JOIN [Seguridad].[Seg_UsuarioAdicional] SEGA
				ON SEG.Usuario=SEGA.Usuario AND SEGA.Ruc=SEG.Ruc AND  SEG.Estado='A' 
				AND SEG.CodProveedor IS NOT NULL 
				AND SEG.CodProveedor<>''
		WHERE (SEG.UsrSubido IS NULL OR SEG.UsrSubido=0) AND CodProveedor='104867'
	END
	
	IF(@W_TIPO=2)
	BEGIN
		SELECT TipoIdentificacion=SEGA.TipoIdent
		,Identificacion=SEG.Usuario,
				Nombre1=SEGA.Nombre1,Nombre2=ISNULL(SEGA.Nombre2,''),Apellido1=SEGA.Apellido1,Apellido2=ISNULL(SEGA.Apellido2,''),TelfFijo=SEG.Telefono,TelfMovil=SEG.Celular,
				Correo=SEG.CorreoE,Departamento=SEG.UsrCargo,Funcion=seg.UsrFuncion,Tratamiento='SEÑOR (A)'
		FROM [Seguridad].[Seg_Usuario] SEG 
			INNER JOIN [Seguridad].[Seg_UsuarioAdicional] SEGA
				ON SEG.Usuario=SEGA.Usuario AND SEGA.Ruc=SEG.Ruc
		WHERE (SEG.UsrSubido IS NULL OR SEG.UsrSubido=0) 
		AND   SEG.Estado='A'
		AND CodProveedor=@W_CODSAP

		UPDATE	[Seguridad].[Seg_Usuario] SET UsrSubido=1
		FROM	[Seguridad].[Seg_Usuario] SEG 
		WHERE	SEG.Estado='A'
		AND		(SEG.UsrSubido IS NULL OR SEG.UsrSubido=0)
		AND		 CodProveedor=@W_CODSAP


	END

	
END


