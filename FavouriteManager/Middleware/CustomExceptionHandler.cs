
using FavouriteManager.Exception;
using System.Text.Json;

namespace FavouriteManager.Middleware
{
    /// <summary>
    /// Custom middleware to handle exceptions and return appropriate JSON error responses.
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the CustomExceptionHandlerMiddleware.
        /// </summary>
        /// <param name="next">The next step in the HTTP request pipeline.</param>
        public CustomExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Handles exceptions that occur while processing the HTTP request.
        /// </summary>
        /// <param name="context">The context of the HTTP request.</param>
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

        /// <summary>
        /// Generates a JSON error response based on the provided HTTP status code and error message.
        /// </summary>
        /// <param name="context">The context of the HTTP request.</param>
        /// <param name="statusCode">The HTTP status code of the error response.</param>
        /// <param name="errorMessage">The error message to include in the JSON response.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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
