using Application.DTOs;
using Application.Interfaces;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients.Queries.GetClientById
{
    public class GetClientByIdQuey : IRequest<Response<ClientDto>>
    {
        public int Id { get; set; }
    }

    public class GetClientByIdQueyHandler : IRequestHandler<GetClientByIdQuey, Response<ClientDto>>
    {
        private readonly IRepositoryAsync<Client> _repositoryAsync;
        private readonly IMapper _mapper;

        public GetClientByIdQueyHandler(IRepositoryAsync<Client> repositoryAsync, IMapper mapper)
        {
            _repositoryAsync = repositoryAsync;
            _mapper = mapper;
        }

        public async Task<Response<ClientDto>> Handle(GetClientByIdQuey request, CancellationToken cancellationToken)
        {
            var record = await _repositoryAsync.GetByIdAsync(request.Id);

            if (record == null)
            {
                throw new KeyNotFoundException($"No se encontró el Id = {request.Id}");
            }

            var dto = _mapper.Map<ClientDto>(record);

            return new Response<ClientDto>(dto);
        }
    }
}
