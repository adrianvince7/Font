using System;
using System.Threading.Tasks;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Font.Core.Domain.User;
using Font.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Font.Infrastructure.Authentication
{
    public class CognitoAuthenticationService : IAuthenticationService
    {
        private readonly IAmazonCognitoIdentityProvider _cognitoProvider;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly string _clientId;
        private readonly string _userPoolId;

        public CognitoAuthenticationService(
            IAmazonCognitoIdentityProvider cognitoProvider,
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _cognitoProvider = cognitoProvider;
            _userRepository = userRepository;
            _configuration = configuration;
            _clientId = _configuration["AWS:Cognito:ClientId"];
            _userPoolId = _configuration["AWS:Cognito:UserPoolId"];
        }

        public async Task<(bool success, string token, User user)> AuthenticateAsync(string email, string password)
        {
            try
            {
                var authRequest = new AdminInitiateAuthRequest
                {
                    UserPoolId = _userPoolId,
                    ClientId = _clientId,
                    AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH,
                    AuthParameters = new Dictionary<string, string>
                    {
                        { "USERNAME", email },
                        { "PASSWORD", password }
                    }
                };

                var authResponse = await _cognitoProvider.AdminInitiateAuthAsync(authRequest);
                
                if (authResponse.AuthenticationResult != null)
                {
                    var user = await _userRepository.GetByEmailAsync(email);
                    if (user != null)
                    {
                        user.UpdateLastLogin();
                        await _userRepository.UpdateAsync(user);
                        return (true, authResponse.AuthenticationResult.IdToken, user);
                    }
                }

                return (false, null, null);
            }
            catch (NotAuthorizedException)
            {
                return (false, null, null);
            }
            catch (Exception)
            {
                // Log the exception
                return (false, null, null);
            }
        }

        public async Task<(bool success, string message)> RegisterAsync(string username, string email, string password)
        {
            try
            {
                if (await _userRepository.ExistsByEmailAsync(email))
                {
                    return (false, "Email already exists");
                }

                if (await _userRepository.ExistsByUsernameAsync(username))
                {
                    return (false, "Username already exists");
                }

                var signUpRequest = new SignUpRequest
                {
                    ClientId = _clientId,
                    Username = email,
                    Password = password,
                    UserAttributes = new List<AttributeType>
                    {
                        new AttributeType { Name = "email", Value = email },
                        new AttributeType { Name = "preferred_username", Value = username }
                    }
                };

                await _cognitoProvider.SignUpAsync(signUpRequest);

                // Create user in our database
                var user = User.Create(username, email, "COGNITO_MANAGED");
                await _userRepository.AddAsync(user);

                return (true, "Registration successful. Please check your email for verification.");
            }
            catch (UsernameExistsException)
            {
                return (false, "Email already exists");
            }
            catch (InvalidPasswordException)
            {
                return (false, "Password does not meet requirements");
            }
            catch (Exception)
            {
                // Log the exception
                return (false, "An error occurred during registration");
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var request = new GetUserRequest
                {
                    AccessToken = token
                };

                await _cognitoProvider.GetUserAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool success, string message)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return (false, "User not found");
                }

                var request = new ChangePasswordRequest
                {
                    PreviousPassword = currentPassword,
                    ProposedPassword = newPassword,
                    AccessToken = "" // Need to get this from the current session
                };

                await _cognitoProvider.ChangePasswordAsync(request);
                return (true, "Password changed successfully");
            }
            catch (Exception)
            {
                return (false, "Failed to change password");
            }
        }

        public async Task<(bool success, string message)> RequestPasswordResetAsync(string email)
        {
            try
            {
                var request = new ForgotPasswordRequest
                {
                    ClientId = _clientId,
                    Username = email
                };

                await _cognitoProvider.ForgotPasswordAsync(request);
                return (true, "Password reset instructions sent to your email");
            }
            catch (Exception)
            {
                return (false, "Failed to initiate password reset");
            }
        }

        public async Task<(bool success, string message)> ResetPasswordAsync(string email, string token, string newPassword)
        {
            try
            {
                var request = new ConfirmForgotPasswordRequest
                {
                    ClientId = _clientId,
                    Username = email,
                    ConfirmationCode = token,
                    Password = newPassword
                };

                await _cognitoProvider.ConfirmForgotPasswordAsync(request);
                return (true, "Password reset successful");
            }
            catch (Exception)
            {
                return (false, "Failed to reset password");
            }
        }

        public async Task<(bool success, string message)> ConfirmEmailAsync(string email, string token)
        {
            try
            {
                var request = new ConfirmSignUpRequest
                {
                    ClientId = _clientId,
                    Username = email,
                    ConfirmationCode = token
                };

                await _cognitoProvider.ConfirmSignUpAsync(request);
                return (true, "Email confirmed successfully");
            }
            catch (Exception)
            {
                return (false, "Failed to confirm email");
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                var request = new RevokeTokenRequest
                {
                    ClientId = _clientId,
                    Token = token
                };

                await _cognitoProvider.RevokeTokenAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool success, string message)> InitiateExternalAuthAsync(string provider)
        {
            // Implementation depends on the specific external provider (Google, Facebook, etc.)
            throw new NotImplementedException();
        }

        public async Task<(bool success, string token, User user, string message)> CompleteExternalAuthAsync(string provider, string code)
        {
            // Implementation depends on the specific external provider (Google, Facebook, etc.)
            throw new NotImplementedException();
        }
    }
} 