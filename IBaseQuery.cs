using MediatR;

namespace UserManagement.Api.Common.Interfaces;

public interface IBaseQuery<TResponse> : IRequest<TResponse> { }
