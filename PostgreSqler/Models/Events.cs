using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostgreSqler.Models
{
    public class Event
    {
        public Guid Id { get; set; }

        [Column("env_id")]
        public Guid EnvId { get; set; }

        [Column("event")]
        public string EventType { get; set; }

        [Column(TypeName = "jsonb")]
        public EventData Data { get; set; }

        public DateTime Timestamp { get; set; }
    }

    public class EventData
    {
        public string Route { get; set; }
        public string FlagId { get; set; }
        public string EnvId { get; set; }
        public string AccountId { get; set; }
        public string ProjectId { get; set; }
        public string FeatureFlagKey { get; set; }
        public bool SendToExperiment { get; set; }
        public string UserKeyId { get; set; }
        public string UserName { get; set; }
        public string VariationId { get; set; }
        public string Tag0 { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
    }
}
