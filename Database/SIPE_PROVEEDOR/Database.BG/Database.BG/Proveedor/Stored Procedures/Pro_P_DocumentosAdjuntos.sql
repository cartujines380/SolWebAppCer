
CREATE PROCEDURE [Proveedor].[Pro_P_DocumentosAdjuntos](
	@PI_ParamXML xml	
)
AS
BEGIN
	declare @Tipo		int,
	        @TipoPersona varchar(5),
			@vTipoPersona varchar(5),
			@ProcesoSoporte varchar(5)
		
		select @Tipo=nref.value('@Tipo','int'),
			   @TipoPersona=nref.value('@TipoPersona','varchar(2)'),
			   @ProcesoSoporte= nref.value('@ProcesoSoporte','varchar(2)')
		from @PI_ParamXML.nodes('/Root') AS R(nref) 
		
		

	--Consulta General
	if @Tipo = '2'
	begin

		set @vTipoPersona = @TipoPersona;
		if @TipoPersona is not null and @TipoPersona <> 'PN' and @TipoPersona <> 'PJ'
			set @vTipoPersona = 'PJ'
		


		select Codigo, 
			   Descripcion, 
			   case when EsObligatorio ='S' then 'SI' else 'NO'end as EsObligatorio 
	      from [Proveedor].[Pro_Documentos] d 
		 where d.CodTipoPersona = @vTipoPersona
		and d.Estado = 'A'

	end

	--Consulta de documentos condicionales por proceso critico
	if @Tipo = '3'
	begin
		select Codigo, 
			   Descripcion, 
			   case when EsObligatorio ='S' then 'SI' else 'NO'end as EsObligatorio 
	      from [Proveedor].[Pro_Documentos] d 
		 where d.CodTipoPersona = 'CO' and d.Codigo = case @ProcesoSoporte when 'P3' then 'EH'
		                                                                   when 'P4' then 'CV'
																		   else '' end
		and d.Estado = 'A'
	end



	--return 0
END

