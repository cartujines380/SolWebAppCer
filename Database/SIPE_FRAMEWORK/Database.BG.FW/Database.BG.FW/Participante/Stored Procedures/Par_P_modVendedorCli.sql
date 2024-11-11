create proc [Participante].[Par_P_modVendedorCli]( 
 @PI_docXML varchar(8000)
)
as
declare @VL_idXML int

--Prepara el documento XML recibido.
exec sp_xml_preparedocument @VL_idXML OUTPUT, @PI_docXML

BEGIN TRAN
update Participante.Par_Cliente
set IdVendedor = dxml.IdVendedor
   FROM  Participante.Par_Cliente c,
OPENXML (@VL_idXML, '/ResultSet/Clientes')WITH Participante.Par_Cliente as dxml
where c.IdEmpresa = dxml.IdEmpresa
and c.IdParticipante = dxml.IdParticipante
 
if @@error <> 0 
begin
   ROLLBACK TRAN
   return 
end
COMMIT TRAN
--Libera el documento XML
EXEC sp_xml_removedocument @VL_idXML
return








