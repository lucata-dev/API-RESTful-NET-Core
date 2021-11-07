using Application.Parameters;

namespace Application.Features.Clients.Queries.GetAllClients
{
    public class GetAllClientsParameters : RequestParameter
    {
        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
