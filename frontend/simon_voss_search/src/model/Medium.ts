export type Medium = {
    id : string;
    parentId : string;
    type : string;
    owner : string;
    description : string;
    serialNumber : string;
}

// public Guid Id { get; set; }
// [JsonProperty("groupId")]
// public Guid ParentId { get; set; }
// public string Type { get; set; }
// public string Owner { get; set; }
// [JsonConverter(typeof(JsonNullToEmptyStringConverter))]
// public string Description { get; set; }
// public string SerialNumber { get; set; }