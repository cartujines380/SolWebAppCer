CREATE PROCEDURE [Catalogo].[Ctl_P_conPuntoVenta]
@PI_IdParticipante int,
@PI_IdDocumento varchar(20)
AS
if exists(select 1 
	  from 	.Catalogo.Ctl_DocumentoAutorizado 
	  where IdOficina=@PI_IdParticipante and 
		IdDocumento=@PI_IdDocumento)
begin
	Select	p.PuntoVenta, p.NumDigitosOficina,p.NumDigitos,p.OficinaRef
	FROM 	.Catalogo.Ctl_DocumentoAutorizado p
	WHERE 	p.IdOficina = @PI_IdParticipante and
	      	p.IdDocumento=@PI_IdDocumento and
	      	p.Estado='A'
end
else
	--Select 0 as IdParticipante, 'N/A' as PuntoVenta
	Select 0 as IdParticipante, 'N/A' as PuntoVenta,'0' as NumDigitosOficina,'0' as NumDigitos, '0' as OficinaRef






