
using FavouriteManager.Exception;
using System.Text.Json;

namespace FavouriteManager.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                // Handle the exception here, log it, and return an appropriate response.
                await HandleExceptionAsync(context, StatusCodes.Status404NotFound, "Entity Not Found");
            }
            catch (FavoriteAlreadyExistsException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status409Conflict, "Favourite Already Exists");
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status400BadRequest, "Invalid Format");
            }
        }

        private Task HandleExceptionAsync(HttpContext context, int statusCode, string errorMessage)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                error = new
                {
                    code = statusCode,
                    message = errorMessage
                }
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);

            return context.Response.WriteAsync(jsonResponse);
        }
    }

}
