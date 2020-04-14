﻿using System.Threading.Tasks;
using MoneyOps.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MoneyOps.Features.Auth
{
    [Route("auth")]
    public class AuthController
    {
        private readonly IMediator _mediator;

        public AuthController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<AuthUserDto> Login(
            [FromBody] Login.Command command)
        {
            return await _mediator.Send(command);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Unit> Register(
            [FromBody] Register.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("refreshtoken")]
        public async Task<AuthUserDto> RefreshToken(
            [FromBody] ExchangeRefreshToken.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}