

CREATE procedure [Seguridad].[Seg_P_CONSULTA_GENTRANPORUSER] 
@PV_Codusuario      VARCHAR(20 )
AS
         SELECT distinct 1 Sec,  convert(char,tu.fechainicial, 110) FechaInicial, convert(char,tu.fechafinal, 110) FechaFinal,
                tu.usrreemplaza, tu.idorganizacion "o-idorg", 
                tu.idtransaccion "o-idtransacc", tu.idopcion "o-idopcion",
                u.nombre "o-nomUsuario"
         
         FROM Seguridad.Seg_TransUsuario tu, Seguridad.Seg_Usuario u
         WHERE tu.idusuario = @PV_Codusuario
               and u.idusuario = tu.usrreemplaza
        
  





