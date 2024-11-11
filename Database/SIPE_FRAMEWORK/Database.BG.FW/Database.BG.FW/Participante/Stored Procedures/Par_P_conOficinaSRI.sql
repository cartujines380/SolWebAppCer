create proc [Participante].[Par_P_conOficinaSRI]
(
    @PI_IdOficina int,
    @PI_IdTipoParametro tinyint,
    @PO_Valor varchar(100) output
)
as

SET NOCOUNT ON

DECLARE 
    @VL_Valor varchar(100)

SELECT @VL_Valor = ''

if exists (select 1 from Participante.Par_ParametroParticipante
                where IdParticipante =  @PI_IdOficina and 
                      IdTipoParametro = @PI_IdTipoParametro)
begin

    select 
        @VL_Valor = Valor
    from 
        Participante.Par_ParametroParticipante
    where 
        IdParticipante =  @PI_IdOficina and 
        IdTipoParametro = @PI_IdTipoParametro
end

SELECT @PO_Valor = @VL_Valor
return




