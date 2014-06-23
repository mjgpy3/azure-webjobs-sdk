﻿using System;
using Microsoft.Azure.Jobs.Protocols;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Dashboard.Data
{
    public class RecentInvocationIndexByJobRunWriter : IRecentInvocationIndexByJobRunWriter
    {
        private readonly IVersionedTextStore _store;

        [CLSCompliant(false)]
        public RecentInvocationIndexByJobRunWriter(CloudBlobClient client)
            : this(VersionedTextStore.CreateBlobStore(client, DashboardContainerNames.RecentFunctionsContainerName))
        {
        }

        private RecentInvocationIndexByJobRunWriter(IVersionedTextStore store)
        {
            _store = store;
        }

        public void CreateOrUpdate(WebJobRunIdentifier webJobRunId, DateTimeOffset timestamp, Guid id)
        {
            string innerId = CreateInnerId(webJobRunId, timestamp, id);
            _store.CreateOrUpdate(innerId, String.Empty);
        }

        public void DeleteIfExists(WebJobRunIdentifier webJobRunId, DateTimeOffset timestamp, Guid id)
        {
            string innerId = CreateInnerId(webJobRunId, timestamp, id);
            _store.DeleteIfExists(innerId);
        }

        private static string CreateInnerId(WebJobRunIdentifier webJobRunId, DateTimeOffset timestamp, Guid id)
        {
            return DashboardBlobPrefixes.CreateByJobRunPrefix(webJobRunId) +
                RecentInvocationEntry.Format(timestamp, id);
        }
    }
}