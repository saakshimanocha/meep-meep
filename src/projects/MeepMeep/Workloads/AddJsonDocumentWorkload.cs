﻿using System.Diagnostics;
using Couchbase;
using Enyim.Caching.Memcached;
using MeepMeep.Docs;

namespace MeepMeep.Workloads
{
    /// <summary>
    /// Workload representing the use-case of adding JSON documents.
    /// </summary>
    public class AddJsonDocumentWorkload : WorkloadBase
    {
        private readonly string _description;

        protected readonly string SampleDocument;

        public const string DefaultKeyGenerationPart = "ajdw";

        public override string Description
        {
            get { return _description; }
        }

        public AddJsonDocumentWorkload(IWorkloadDocKeyGenerator docKeyGenerator, int workloadSize, int warmupMs, string sampleDocument = null)
            : base(docKeyGenerator, workloadSize, warmupMs)
        {
            SampleDocument = sampleDocument ?? SampleDocuments.Default;
            _description = string.Format("ExecuteStore (Add) of {0} JSON doc(s) with doc size: {1}.",
                WorkloadSize,
                SampleDocument.Length);
        }

        protected override WorkloadOperationResult OnExecuteStep(ICouchbaseClient client, int workloadIndex, int docIndex, Stopwatch sw)
        {
            var key = DocKeyGenerator.Generate(workloadIndex, docIndex);

            sw.Start();
            //saakshi
            //var storeOpResult = client.ExecuteStore(StoreMode.Add, key, SampleDocument);
            var storeOpResult = client.ExecuteStore(StoreMode.Set, key, SampleDocument);
            sw.Stop();

            return new WorkloadOperationResult(storeOpResult.Success, storeOpResult.Message, sw.Elapsed)
            {
                DocSize = SampleDocument.Length
            };
        }
    }
}