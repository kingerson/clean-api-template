namespace MsClean.Application;
using System;
using MediatR;

public sealed record GetGovernmentEntityByIdQuery(Guid Id) : IRequest<GovernmentEntityViewModel>;
