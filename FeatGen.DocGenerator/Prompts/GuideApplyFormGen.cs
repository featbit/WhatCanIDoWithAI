using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideApplyFormGen
    {
        public static string V1(Specification spec, ReportCodeGuide rcg)
        {
            string rawPrompt = """

                ## Task
                
                We have programmed a software named "###{service_name}###". ###{service_desc}###. 

                We've already code for each feature and a speicification. 我们需要写一个申请表来用此软件申请软著。申请表中需要填写的内容如下：

                【软件全称】###{service_name}###
                【版本号】V1.0
                【软件分类】应用软件
                【开发的硬件环境】// please generate randomly a PC or laptop brand name and model, less than 50 characters.
                【运行的硬件环境】// please generate randomly a the server hardware requirement for the system.
                【开发该软件的操作系统】// please generate a os between Windows 11 and modern iOS operation system
                【软件开发环境/开发工具】Visual Studio Code
                【该软件的运行平台/操作系统】Linux, Ubuntu 或 Windows Server
                【软件运行支撑环境/支持软件】服务器需安装 NodeJs, NextJs 和PostgreSQL数据库; 客户端需安装现代Web浏览器;
                【编程语言】JavaScript
                【源程序量】// please generate randomly a code line number, greater than 12000 lines.
                【开发目的】// 请使用小于50字数，定义一个开发目的
                【面向领域/行业】// 请使用小于30个字，精简描述领域/行业
                【软件的主要功能】 // 请基于软件描述，生成一个在100-200字之间的主要功能描述
                【软件的技术特点】 // 请随机生成一个技术特点描述，小于100字

                here's the features of the software:

                ###{page_desc}###

                ## Output format
                
                Return the pure text only without any explaination, table and code.

                ## Output example

                【软件全称】中小企业服务运营平台
                【版本号】V1.0
                【软件分类】应用软件
                【开发的硬件环境】Dell xps 17；Surface pro 8
                【运行的硬件环境】服务器端：CPU主频： 2.0GHz及以上，内存频率： 1333MHz，内存容量： 4GB及以上;客户端：支持现代浏览器的任何非移动设备
                【开发该软件的操作系统】跨平台开发: Windows 11
                【软件开发环境/开发工具】Visual Studio Code
                【该软件的运行平台/操作系统】Linux或Windows Server
                【软件运行支撑环境/支持软件】Node.js, 现代Web浏览器;PostgreSQL
                【编程语言】JavaScript
                【源程序量】13248行
                【开发目的】为中小企业提供一站式SaaS服务平台
                【面向领域/行业】中小企业服务
                【软件的主要功能】中小企业服务运营平台是专为中小企业设计的SaaS平台，集成了业务管理、数据分析、财务管理和客户关系管理等核心模块。系统可实现项目全流程管理、任务分配追踪、数据可视化分析、财务报表生成、预算控制、客户信息集中管理等功能。平台通过提供统一的界面和工作流程，帮助企业高效协同工作，优化资源分配，提升决策效率，降低运营成本，为企业的快速成长和稳定发展提供全方位支持，使企业能够聚焦核心业务创新。
                【软件的技术特点】支持跨平台部署；采用前后端分离的模块化架构技术，可根据企业需求灵活扩展功能。具备良好的安全性和数据保护机制，确保企业数据安全。

                """;

            List<GuidePageItem> pages = new List<GuidePageItem>();
            
            var menuItemsString = rcg.MenuItems;
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages;
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string pageDescription = $"# {spec.Title} \n {spec.Definition} \n";
            for (int i = 0; i < menuItems.Count; i++)
            {
                var menuItem = menuItems[i];
                var page = allPages.FirstOrDefault(p => p.page_id == menuItem.page_id);

                var features = "";
                for(int j=0; j< page.mapping_features.Count; j++)
                {
                    features += "- " + page.mapping_features[j].feature_desc + "\n";
                }

                pageDescription += $"## {menuItem.menu_name} \nFeatures: \n {features}";
            }

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDescription);
            
            return prompt;
        }

    }
}
