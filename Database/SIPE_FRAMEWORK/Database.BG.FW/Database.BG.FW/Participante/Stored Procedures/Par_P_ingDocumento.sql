create procedure [Participante].[Par_P_ingDocumento]
	@PI_IdParticipante int,
	@PI_IdDocumento int,
	@PI_IdTipoDocumento int,
	@PI_TipoArchivo varchar(100),
	@PI_LongArchivo int,
	@PI_NombreArchivo varchar(100),
	@PI_Descripcion varchar(100),
	@PI_FechaDocumento varchar(10),
	@PI_Documento image
AS
	INSERT Participante.Par_DocumentoParticipante(
	IdParticipante,
	IdDocumento ,
	IdTipoDocumento ,
	TipoArchivo,
	LongArchivo,
	NombreArchivo,Descripcion,FechaDocumento,
	Documento)
	VALUES(	@PI_IdParticipante,
	@PI_IdDocumento ,
	@PI_IdTipoDocumento ,
	@PI_TipoArchivo,
	@PI_LongArchivo,
	@PI_NombreArchivo,@PI_Descripcion,@PI_FechaDocumento,
	@PI_Documento )




