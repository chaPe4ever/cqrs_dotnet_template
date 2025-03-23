using MediatR;

namespace UserManagement.Api.Common.Interfaces;

public interface IBaseCommand<TResponse> : IRequest<TResponse> { }
public interface IBaseCommand : IRequest { }
