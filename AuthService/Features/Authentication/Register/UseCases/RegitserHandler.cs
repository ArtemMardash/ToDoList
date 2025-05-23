using AuthService.Core.Entities;
using AuthService.Core.ValueObjects;
using AuthService.Features.Authentication.Register.Models;
using AuthService.Features.Authentication.Shared.Repositories;
using FluentValidation;
using Mediator;

namespace AuthService.Features.Authentication.Register.UseCases;

public class RegitserHandler : IRequestHandler<RegisterRequest, RegisterRequestResult>
{
    private readonly IAppUserRepository _userRepository;
    private readonly IValidator<RegisterRequest> _validator;

    public RegitserHandler(IAppUserRepository userRepository, IValidator<RegisterRequest> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    /// <summary>
    /// Handler to register user
    /// </summary>
    public async ValueTask<RegisterRequestResult> Handle(RegisterRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var appUser = new AppUser(request.Email, request.Password, new FullName(request.FirstName, request.LastName));
        var id = await _userRepository.CreateUserAsync(appUser, cancellationToken);
        
        return new RegisterRequestResult
        {
            Id = id
        };
    }
}