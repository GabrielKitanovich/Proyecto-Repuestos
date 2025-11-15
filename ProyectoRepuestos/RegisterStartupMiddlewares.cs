namespace ProyectoRepuestos;
public static class RegisterStartupMiddlewares
{
    public static WebApplication SetupMiddleware(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Agregar middleware de autenticación y autorización
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}