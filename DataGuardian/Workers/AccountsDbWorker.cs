﻿using System;
using System.Collections.Generic;
using System.Linq;
using DataGuardian.DbLevel;
using DataGuardian.Plugins;
using DataGuardian.Plugins.Plugins;

namespace DataGuardian.Workers
{
    public class AccountsDbWorker
    {
        private readonly AccountsDbContext _context;

        public AccountsDbWorker(string dbConnection)
        {
            _context = new AccountsDbContext(dbConnection);
        }

        public void SaveProvider(ICloudProviderAccount cloudProviderAccount)
        {
            _context.Accounts.Add((CloudProviderAccount) cloudProviderAccount);
            _context.SaveChanges();
        }

        public void DeleteProvider(ICloudProviderAccount cloudProviderAccount)
        {
            _context.Accounts.Remove((CloudProviderAccount) cloudProviderAccount);
            _context.SaveChanges();
        }

        public IEnumerable<CloudProviderAccount> ReadAccounts()
        {
            return _context.Accounts.ToList();
        }
    }
}