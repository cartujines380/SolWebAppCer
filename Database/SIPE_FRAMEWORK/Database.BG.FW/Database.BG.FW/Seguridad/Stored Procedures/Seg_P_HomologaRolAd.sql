
CREATE proc [Seguridad].[Seg_P_HomologaRolAd](
@PS_IdRol   varchar(20),
@PS_Codigo  varchar(50),
@PS_Sesion  varchar(100),
@PS_Tipo	char(1)
)
as

begin
	declare @cod int ,
			@mensaje varchar(100)

	if @PS_Tipo = '1'
	begin
		begin try
			if exists(select 1 
					from Seguridad.Seg_HomologacionRoles
					where IdRol = @PS_IdRol 
			)
			begin
				update Seguridad.Seg_HomologacionRoles
				set CodAD = @PS_Codigo, 
					FechaModificacion = GETDATE(),
					UsuarioModificacion = @PS_Sesion
				where IdRol = @PS_IdRol

				if @@ROWCOUNT > 0
				begin
					select @cod = 0, @mensaje = 'Registro guardado con éxito'
				end 
			end
			else
			begin
				INSERT INTO Seguridad.Seg_HomologacionRoles
				(IdRol, CodAD, FechaCreacion, UsuarioCreacion, Estado)
				values
				(@PS_IdRol, @PS_Codigo, GETDATE(), @PS_Sesion, 'A')

				if @@ROWCOUNT > 0
				begin
					select @cod = 0, @mensaje = 'Registro guardado con éxito'
				end 
			end
		end try
		begin catch
			select @cod = @@ERROR, @mensaje = ERROR_MESSAGE()
		end catch

		return @cod --@mensaje

	end

	if @PS_Tipo = '2'
	begin
		select 
			H.IdRol Rol,
			R.Descripcion Descripcion,
			H.CodAD CodigoAd,
			H.FechaCreacion FechaCreacion,
			R.Abreviatura Abreviatura
		from Seguridad.Seg_HomologacionRoles H
		inner join Seguridad.Seg_Rol R
		on H.IdRol = R.IdRol
		where H.Estado = 'A'
		and R.Status = 'ACTIVE'
		--order by H.Id desc

		return 0
	end

	if @PS_Tipo = '3'
	begin
		begin try
			update Seguridad.Seg_HomologacionRoles
			set Estado = 'E', 
				FechaModificacion = GETDATE(),
				UsuarioModificacion = @PS_Sesion
			where IdRol = @PS_IdRol
			and CodAD = @PS_Codigo
			and Estado = 'A'

			if @@ROWCOUNT > 0
			begin
				select @cod = 0, @mensaje = 'Registro eliminado con éxito'
			end 
		end try
		begin catch
			select @cod = @@ERROR, @mensaje = ERROR_MESSAGE()
		end catch

		return @cod
	end
end

