using App.Application.Configuration;
using App.Infrastructure.Configuration.DataAccess;
using App.Infrastructure.Integration.Client;
using App.Infrastructure.Integration.Requests;

namespace App.Infrastructure.Integration.Usos.Students;

internal class UsosUsersProvider(IUsosHttpClient client, IExecutionContextAccessor context, ICacheProvider cache) : IUsersProvider
{
    public async Task<UserDto> GetUser(string? id = null)
    {
        var request = Request.Get("services/users/user")
            .WithQueryParameter("fields",
                "id|first_name|last_name|sex|student_status|email|phone_numbers|mobile_numbers|photo_urls|student_number|pesel|birth_date|citizenship|student_programmes|postal_addresses|library_card_id|titles|office_hours|course_editions_conducted");

        if (id is not null)
        {
            request.WithQueryParameter("user_id", id);
        }

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<UserDto>();
    }

    public async Task<IDictionary<string, UserDto>> GetMultipleUsers(string[] ids)
    {
        var request = Request.Get("services/users/users")
            .WithQueryParameter("fields", "id|first_name|last_name|sex|student_status|email|phone_numbers|mobile_numbers|photo_urls|student_number|pesel|birth_date|citizenship|student_programmes|postal_addresses|library_card_id|titles|office_hours|course_editions_conducted")
            .WithQueryParameter("user_ids", String.Join('|', ids));

        var response = await client.SendAsync(request);

        if (!response.IsSuccessful())
        {
            throw response.ToException(context.Language);
        }

        return response.Content!.As<IDictionary<string, UserDto>>();
    }
}
