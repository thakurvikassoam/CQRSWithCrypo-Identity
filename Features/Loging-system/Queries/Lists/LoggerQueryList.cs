﻿using CQRSWithDocker_Identity.Features.Loging_system.Dtos;
using CQRSWithDocker_Identity.Features.Users.Dtos;
using MediatR;

namespace CQRSWithDocker_Identity.Features.Loging_system.Queries.Lists
{
    
    public record LoggerQueryList : IRequest<List<LoggerDto>>;
}