﻿using System;
using FunnelWeb.Model;
using FunnelWeb.Repositories.Queries;
using FunnelWeb.Tests.Helpers;
using NUnit.Framework;

namespace FunnelWeb.Tests.Integration.Queries
{
    [TestFixture]
    public class EntryByNameAndRevisionQueryMatchesEntryByName : QueryIntegrationTest
    {
        public EntryByNameAndRevisionQueryMatchesEntryByName()
            : base(TheDatabase.CanBeDirty)
        {
        }

        public override void TestQuery()
        {
            var name = "test-" + Guid.NewGuid();

            Database.WithRepository(
                repo =>
                {
                    var entry1 = new Entry { Name = name, Author = "A1" };
                    var revision1 = entry1.Revise();
                    revision1.Body = "Hello";
                    repo.Add(entry1);

                    var entry2 = new Entry { Name = name, Author = "A1" };
                    var revision2 = entry2.Revise();
                    revision2.Body = "Goodbye";
                    repo.Add(entry2);
                });

            Database.WithRepository(
                repo =>
                {
                    var entry = repo.FindFirst(new EntryByNameAndRevisionQuery(name, 1));
                    Assert.AreEqual("A1", entry.Author);
                });
        }
    }
}
