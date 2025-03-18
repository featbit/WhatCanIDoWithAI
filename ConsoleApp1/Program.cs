// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


string t = """
    {
        "id": "feature-flag-1",
        "name": "feature flag 1",
        "project_id": "1nxsa-12301-2vzz-2415-1212ak1",
        "env_id": "12301-2vzz-2415-1nxsa-1nxsb",
        "is_enabled": false,
        "type": "bool",
        "values": [
            {
                "value_text": true,
                "id": "true"
            },
            {
                "value_text": false,
                "id": "false"
            }
        ],
        "rules": [
            {
                "rule_name": "rule 1",
                "rule_id": "rule-001-dxa-12",
                "conditions": [            
                    {
                        "property": "",
                        "operation": "user-is-in-segment",
                        "value": [ "segment-001" ]
                    },
                    {
                        "property": "user_id",
                        "operation": "not-included",
                        "value": [ "abe-ase", "feasbe" ]
                    }
                ],
                "return_value": [
                    {
                        "value_id": "true",
                        "percentage": [ 0, 100]
                    },
                    {
                        "value_id": "false",
                        "percentage": [ 0, 0]
                    }
                ]
            }
        ]
    }
    """;

Console.WriteLine(t);

string project = """
    {
        "id": "1nxsa-12301-2vzz-2415-1212ak1",
        "name": "FeatGen",
        "description": "Manage feature flags of FeatGen project",
        "created_time": "2025-03-18T12:23:12",
        "updated_time": "2025-03-18T12:23:12"
    }

    """;

