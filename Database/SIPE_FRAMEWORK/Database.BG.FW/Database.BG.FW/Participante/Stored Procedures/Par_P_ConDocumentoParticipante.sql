CREATE PROCEDURE [Participante].[Par_P_ConDocumentoParticipante]
@PI_IdParticipante int
AS

	SELECT 	d.IdDocumento, d.IdTipoDocumento,
		Catalogo.Ctl_F_conCatalogo(200,d.IdTipoDocumento) TipoDocumento,
		d.Documento,
		d.TipoArchivo, d.LongArchivo, d.NombreArchivo, 
		d.Descripcion,
		convert(varchar,d.FechaDocumento,110) FechaDocumento
	FROM Participante.Par_DocumentoParticipante d
	WHERE d.IdParticipante = @PI_IdParticipante





