namespace ProyectoRepuestos.Helpers;

public static class Messages
{
    public static class Repuesto
    {
        public const string NotFound = "Repuesto no encontrado.";
        public const string AlreadyExists = "El repuesto ya existe.";
        public const string InvalidData = "Datos del repuesto inválidos.";

        public const string Deleted = "Repuesto eliminado exitosamente.";
    }

    public static class General
    {
        public const string NotFound = "Elemento no encontrado.";
        public const string AlreadyExists = "El elemento ya existe.";
        public const string InvalidData = "Datos inválidos.";

        public const string InternalError = "Error interno del servidor.";

        public const string Deleted = "Elemento eliminado exitosamente.";
    }
}