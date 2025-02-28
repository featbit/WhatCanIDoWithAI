## Task

I am writing a specification for software "光疗临床路径智能规划系统", please help me to generate the following data:

1. main features of the software. maximum 4 main features.
2. sub features in each main feature. maximum 3 sub features for each main feature.
3. the menu item name for each main feature.

## Information

The 光疗临床路径智能规划系统 (Light Therapy Clinical Pathway Intelligent Planning System) is a cloud-based SaaS platform designed to streamline and optimize light therapy treatment planning in clinical settings. By leveraging AI and clinical guidelines, it helps healthcare providers create personalized light therapy plans, monitor patient progress, and ensure effective treatments. The system integrates seamlessly with patient data, providing real-time insights and adjustments to enhance treatment outcomes while ensuring data privacy and security.

It has mainly for features:

- AI-driven Personalized Treatment Plans: Generates customized light therapy schedules based on individual patient health data.
- Real-Time Treatment Monitoring: Tracks patient progress and provides real-time recommendations to adjust therapy for better results.
- Clinical Pathway Integration: Incorporates established clinical guidelines and best practices for light therapy into the treatment planning process.
- Comprehensive Reporting: Generates detailed treatment reports for healthcare providers to assess therapy effectiveness and make data-driven decisions.

## Output

Output should be in json format:


{
    "service_description": "" // define what the SaaS "光疗临床路径智能规划系统" should looks like
    "saas_features": [
        {
            "feature": "", // main feature of the SaaS "光疗临床路径智能规划系统"
            "sub_features": [] // list 1,2,or 3 sub features of the main feature, describing its functionalities with details as a specification
            "menu_item": "" // menu item name for the main feature
        }
    ] // list 3-5 main features of the SaaS "光疗临床路径智能规划系统"
}


---------

{
    "service_description": "光疗临床路径智能规划系统 is a cloud-based SaaS platform that streamlines and optimizes light therapy treatment planning in clinical settings. It leverages AI and established clinical guidelines to generate personalized treatment plans, monitor patient progress in real time, integrate seamlessly with patient data, and provide comprehensive reporting for data-driven decision making while ensuring data privacy and security.",
    "saas_features": [
        {
            "feature": "AI-driven Personalized Treatment Plans",
            "sub_features": [
                "Generates customized light therapy schedules based on individual patient health data",
                "Utilizes advanced AI algorithms to tailor treatment intensity and timing",
                "Incorporates patient history and real-time data for dynamic plan adjustments"
            ],
            "menu_item": "个性化治疗"
        },
        {
            "feature": "Real-Time Treatment Monitoring",
            "sub_features": [
                "Continuously tracks patient progress during therapy",
                "Provides real-time recommendations and alerts for therapy adjustments",
                "Integrates with monitoring devices for accurate treatment feedback"
            ],
            "menu_item": "实时监控"
        },
        {
            "feature": "Clinical Pathway Integration",
            "sub_features": [
                "Integrates established clinical guidelines and best practices for light therapy",
                "Ensures compliance with regulatory and clinical standards",
                "Seamlessly connects with existing electronic health records"
            ],
            "menu_item": "临床路径"
        },
        {
            "feature": "Comprehensive Reporting",
            "sub_features": [
                "Generates detailed treatment outcome reports for healthcare providers",
                "Offers data visualization tools to track therapy effectiveness",
                "Enables data-driven decision making with historical and real-time analytics"
            ],
            "menu_item": "报告中心"
        }
    ]
}
