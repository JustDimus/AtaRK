﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace AtaRK.Mobile.Services.Authorization
{
    [ExcludeFromCodeCoverage]
    public class DesignTimeAuthorizationService : IAuthorizationService
    {
        private const bool DEFAULT_AUTHORIZATION_STATUS = true;

        private BehaviorSubject<bool> authorizationStatusSubject = new BehaviorSubject<bool>(DEFAULT_AUTHORIZATION_STATUS);

        public IObservable<bool> AuthorizationStatusObserbavle => this.authorizationStatusSubject.AsObservable();

        public Task<bool> LoginAsync(LoginData loginData)
        {
            return Task.FromResult(DEFAULT_AUTHORIZATION_STATUS);
        }
    }
}
