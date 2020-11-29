﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using DataGuardian.Plugins.Backups;
using DataGuardian.Plugins.Core;
using DataGuardian.Plugins.Plugins;

namespace DataGuardian.Plugins
{
    [Table("Accounts")]
    public class CloudProviderAccount : ICloudProviderAccount
    {
        public ICloudStorageProvider CloudStorageProvider { get; set; }

        [Column("CloudId")]
        public Guid CloudStorageProviderId
        {
            get => CloudStorageProvider.Id;
            set => CloudStorageProvider = CoreStatic.Instance.CloudStorageProviders.FirstOrDefault(x => x.Id == value);
        }

        [Column("ConnectionSettings")] public string ConnectionSettings { get; set; }

        [Column("Name")] public string Name { get; set; }

        [Key, Column("Id")] public int Id { get; set; }

        public byte[] Logo => CloudStorageProvider?.Logo;

        public Task<RemoteBackupsState> GetRemoteBackups()
        {
            return CloudStorageProvider.GetBackupState();
        }

        public Task UploadBackup(LocalArchivedBackup localBackup)
        {
            return CloudStorageProvider.UploadBackupAsync(localBackup);
        }

        public CloudProviderAccount()
        {
        }

        public CloudProviderAccount(string name, ICloudStorageProvider cloudStorageProvider, string connectionSettings)
        {
            CloudStorageProvider = cloudStorageProvider;
            ConnectionSettings = connectionSettings;
            Name = name;
        }

        public void SetId(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is CloudProviderAccount conn)
                return IsSame(this, conn);
            return false;
        }

        private bool IsSame(ICloudProviderAccount conn1, ICloudProviderAccount conn2)
        {
            return conn1.Id.Equals(conn2.Id);
        }
    }
}