// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using Microsoft.IdentityModel.Logging;
using TokenLogMessages = Microsoft.IdentityModel.Tokens.LogMessages;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// Validators meant to be kept internal
    /// </summary>
    internal static class InternalValidators
    {
        /// <summary>
        /// Called after signature validation has failed. Will always throw an exception.
        /// </summary>
        /// <exception cref="SecurityTokenSignatureKeyNotFoundException">
        /// If the lifetime and issuer are valid otherwise
        /// the exception returned from ValidateLifetime and ValidateIssuer.
        /// </exception>
        internal static void ValidateLifetimeAndIssuerAfterSignatureFailed(
            SecurityToken securityToken,
            DateTime? notBefore,
            DateTime? expires,
            string kid,
            TokenValidationParameters validationParameters,
            BaseConfiguration configuration,
            StringBuilder exceptionStrings,
            int numKeysInConfiguration,
            int numKeysInTokenValidationParameters)
        {
            bool validIssuer = false;
            bool validLifetime = false;
            Exception exception = null;

            try
            {
                Validators.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
                validLifetime = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception == null)
            {
                try
                {
                    Validators.ValidateIssuer(securityToken.Issuer, securityToken, validationParameters, configuration);
                    validIssuer = true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            if (validLifetime && validIssuer)
                throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TokenLogMessages.IDX10501,
                    LogHelper.MarkAsNonPII(kid),
                    LogHelper.MarkAsNonPII(numKeysInTokenValidationParameters),
                    LogHelper.MarkAsNonPII(numKeysInConfiguration),
                    exceptionStrings,
                    securityToken)));
            else if (exception != null)
                throw LogHelper.LogExceptionMessage(exception);
        }

        /// <summary>
        /// Called after signature validation has failed. Will always throw an exception.
        /// </summary>
        /// <exception cref="SecurityTokenSignatureKeyNotFoundException">
        /// If the lifetime and issuer are valid otherwise
        /// the exception returned from ValidateLifetime and ValidateIssuer.
        /// </exception>
        internal static void ValidateLifetimeAndIssuerAfterSignatureNotValidatedSaml(SecurityToken securityToken, DateTime? notBefore, DateTime? expires, string keyInfo, TokenValidationParameters validationParameters, StringBuilder exceptionStrings)
        {
            bool validIssuer = false;
            bool validLifetime = false;
            Exception exception = null;
            try
            {
                Validators.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
                validLifetime = true;
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            if (exception == null)
            {
                try
                {
                    Validators.ValidateIssuer(securityToken.Issuer, securityToken, validationParameters);
                    validIssuer = true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            if (validLifetime && validIssuer)
                throw LogHelper.LogExceptionMessage(new SecurityTokenSignatureKeyNotFoundException(LogHelper.FormatInvariant(TokenLogMessages.IDX10513, keyInfo, exceptionStrings, securityToken)));
            else if (exception != null)
                throw LogHelper.LogExceptionMessage(exception);
        }
    }
}
