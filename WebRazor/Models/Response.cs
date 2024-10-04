namespace WebRazor.Models
{
    public record Response(
    int error,
    String message,
    object? data
    );
}
