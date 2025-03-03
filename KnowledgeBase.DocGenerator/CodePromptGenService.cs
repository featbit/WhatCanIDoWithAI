using KnowledgeBase.Models.ReportGenerator;
using KnowledgeBase.OpenAI;
using KnowledgeBase.ReportGenerator.Models;
using System.Text.Json;

namespace KnowledgeBase.ReportGenerator
{
    public interface ICodePromptGenService
    {
        Task<Functionality> FunctionalityGenAsync(
            string serviceName, string serviceDescription, string featureDescription, string moduleDescription);

        Task<string> MenuItemCodeGenAsync(Specification spec);
    }

    public class CodePromptGenService(IOpenAiChatService openaiChatService) : ICodePromptGenService
    {
        public async Task<Functionality> FunctionalityGenAsync(
            string serviceName, string serviceDescription, string featureDescription, string moduleDescription)
        {
            string exampleCode = "window.renderDATAANALYSISPage = function(container) {\n    container.innerHTML = `\n        <div class=\"bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6\">\n            <h1 class=\"text-2xl font-bold mb-6 text-primary\">数据分析与报告生成模块</h1>\n            <div id=\"function\" class=\"bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4\">\n                <h2 class=\"text-xl font-semibold mb-4 text-gray-800 dark:text-white\">数据收集功能</h2>\n                \n                <!-- 数据收集控制面板 -->\n                <div class=\"mb-6 p-4 bg-white dark:bg-gray-600 rounded-lg shadow\">\n                    <h3 class=\"text-lg font-medium mb-3 text-primary\">数据收集控制</h3>\n                    <div class=\"grid grid-cols-1 md:grid-cols-2 gap-4\">\n                        <div>\n                            <label class=\"block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2\">选择患者</label>\n                            <select id=\"patient-select\" class=\"w-full p-2 border border-gray-300 dark:border-gray-500 rounded-md bg-white dark:bg-gray-700 text-gray-800 dark:text-white\">\n                                <option value=\"\">选择患者...</option>\n                                <option value=\"1\">张三 (ID: PT1001)</option>\n                                <option value=\"2\">李四 (ID: PT1002)</option>\n                                <option value=\"3\">王五 (ID: PT1003)</option>\n                            </select>\n                        </div>\n                        <div>\n                            <label class=\"block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2\">选择设备</label>\n                            <select id=\"device-select\" class=\"w-full p-2 border border-gray-300 dark:border-gray-500 rounded-md bg-white dark:bg-gray-700 text-gray-800 dark:text-white\">\n                                <option value=\"\">选择设备...</option>\n                                <option value=\"1\">紫外线光疗设备 (ID: DEV001)</option>\n                                <option value=\"2\">红外线光疗设备 (ID: DEV002)</option>\n                                <option value=\"3\">蓝光治疗设备 (ID: DEV003)</option>\n                            </select>\n                        </div>\n                    </div>\n                    <div class=\"mt-4\">\n                        <button id=\"start-collection\" class=\"bg-primary text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors\">\n                            开始数据收集\n                        </button>\n                        <button id=\"stop-collection\" class=\"bg-gray-500 text-white px-4 py-2 rounded hover:bg-gray-600 transition-colors ml-2\" disabled>\n                            停止数据收集\n                        </button>\n                    </div>\n                </div>\n                \n                <!-- 实时数据展示 -->\n                <div class=\"mb-6 p-4 bg-white dark:bg-gray-600 rounded-lg shadow\">\n                    <h3 class=\"text-lg font-medium mb-3 text-primary\">实时数据</h3>\n                    <div id=\"real-time-data\" class=\"grid grid-cols-1 md:grid-cols-3 gap-4\">\n                        <div class=\"p-3 bg-secondary dark:bg-gray-700 rounded-lg\">\n                            <h4 class=\"text-sm font-medium text-gray-600 dark:text-gray-300\">设备参数</h4>\n                            <div id=\"device-params\" class=\"text-xl font-bold text-gray-800 dark:text-white\">- -</div>\n                        </div>\n                        <div class=\"p-3 bg-secondary dark:bg-gray-700 rounded-lg\">\n                            <h4 class=\"text-sm font-medium text-gray-600 dark:text-gray-300\">治疗时长</h4>\n                            <div id=\"treatment-duration\" class=\"text-xl font-bold text-gray-800 dark:text-white\">00:00:00</div>\n                        </div>\n                        <div class=\"p-3 bg-secondary dark:bg-gray-700 rounded-lg\">\n                            <h4 class=\"text-sm font-medium text-gray-600 dark:text-gray-300\">皮肤温度</h4>\n                            <div id=\"skin-temperature\" class=\"text-xl font-bold text-gray-800 dark:text-white\">- - °C</div>\n                        </div>\n                    </div>\n                </div>\n                \n                <!-- 患者反馈收集 -->\n                <div class=\"mb-6 p-4 bg-white dark:bg-gray-600 rounded-lg shadow\">\n                    <h3 class=\"text-lg font-medium mb-3 text-primary\">患者反馈</h3>\n                    <div class=\"space-y-4\">\n                        <div>\n                            <label class=\"block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2\">疼痛等级 (0-10)</label>\n                            <input type=\"range\" id=\"pain-level\" min=\"0\" max=\"10\" value=\"0\" class=\"w-full\">\n                            <div class=\"flex justify-between text-xs text-gray-500 dark:text-gray-400\">\n                                <span>无痛 (0)</span>\n                                <span>中度 (5)</span>\n                                <span>剧痛 (10)</span>\n                            </div>\n                        </div>\n                        <div>\n                            <label class=\"block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2\">舒适度 (0-10)</label>\n                            <input type=\"range\" id=\"comfort-level\" min=\"0\" max=\"10\" value=\"5\" class=\"w-full\">\n                            <div class=\"flex justify-between text-xs text-gray-500 dark:text-gray-400\">\n                                <span>不适 (0)</span>\n                                <span>一般 (5)</span>\n                                <span>舒适 (10)</span>\n                            </div>\n                        </div>\n                        <div>\n                            <label class=\"block text-sm font-medium text-gray-700 dark:text-gray-300 mb-2\">其他症状描述</label>\n                            <textarea id=\"symptoms-desc\" rows=\"2\" class=\"w-full p-2 border border-gray-300 dark:border-gray-500 rounded-md bg-white dark:bg-gray-700 text-gray-800 dark:text-white\" placeholder=\"请描述其他症状或感受...\"></textarea>\n                        </div>\n                        <button id=\"submit-feedback\" class=\"bg-primary text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors\">\n                            提交反馈\n                        </button>\n                    </div>\n                </div>\n                \n                <!-- 数据收集历史 -->\n                <div class=\"p-4 bg-white dark:bg-gray-600 rounded-lg shadow\">\n                    <h3 class=\"text-lg font-medium mb-3 text-primary\">数据收集历史</h3>\n                    <div class=\"overflow-x-auto\">\n                        <table class=\"min-w-full divide-y divide-gray-200 dark:divide-gray-600\">\n                            <thead>\n                                <tr>\n                                    <th class=\"px-4 py-2 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider\">日期时间</th>\n                                    <th class=\"px-4 py-2 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider\">患者</th>\n                                    <th class=\"px-4 py-2 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider\">设备</th>\n                                    <th class=\"px-4 py-2 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider\">治疗时长</th>\n                                    <th class=\"px-4 py-2 text-left text-xs font-medium text-gray-500 dark:text-gray-300 uppercase tracking-wider\">操作</th>\n                                </tr>\n                            </thead>\n                            <tbody id=\"history-data\" class=\"divide-y divide-gray-200 dark:divide-gray-600\">\n                                <tr>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">2023-06-15 09:30</td>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">张三</td>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">紫外线光疗设备</td>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">00:15:30</td>\n                                    <td class=\"px-4 py-3 text-sm\">\n                                        <button class=\"view-data-btn text-primary hover:text-blue-700\">查看</button>\n                                    </td>\n                                </tr>\n                                <tr>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">2023-06-14 14:45</td>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">李四</td>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">红外线光疗设备</td>\n                                    <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">00:20:15</td>\n                                    <td class=\"px-4 py-3 text-sm\">\n                                        <button class=\"view-data-btn text-primary hover:text-blue-700\">查看</button>\n                                    </td>\n                                </tr>\n                            </tbody>\n                        </table>\n                    </div>\n                </div>\n            </div>\n        </div>\n    `;\n\n    // 模拟数据变量\n    let isCollecting = false;\n    let timerInterval = null;\n    let seconds = 0;\n\n    // DOM 元素\n    const startBtn = document.getElementById('start-collection');\n    const stopBtn = document.getElementById('stop-collection');\n    const deviceParams = document.getElementById('device-params');\n    const treatmentDuration = document.getElementById('treatment-duration');\n    const skinTemperature = document.getElementById('skin-temperature');\n    const submitFeedbackBtn = document.getElementById('submit-feedback');\n    const viewDataBtns = document.querySelectorAll('.view-data-btn');\n    \n    // 开始数据收集\n    startBtn.addEventListener('click', function() {\n        const patientSelect = document.getElementById('patient-select');\n        const deviceSelect = document.getElementById('device-select');\n        \n        if (patientSelect.value === '' || deviceSelect.value === '') {\n            alert('请先选择患者和设备');\n            return;\n        }\n        \n        isCollecting = true;\n        startBtn.disabled = true;\n        stopBtn.disabled = false;\n        \n        // 更新界面显示\n        deviceParams.textContent = \"功率: 80%, 波长: 350nm\";\n        \n        // 开始计时\n        seconds = 0;\n        timerInterval = setInterval(function() {\n            seconds++;\n            const hours = Math.floor(seconds / 3600).toString().padStart(2, '0');\n            const minutes = Math.floor((seconds % 3600) / 60).toString().padStart(2, '0');\n            const secs = (seconds % 60).toString().padStart(2, '0');\n            treatmentDuration.textContent = `${hours}:${minutes}:${secs}`;\n            \n            // 模拟温度变化\n            const temp = 36 + Math.random().toFixed(1);\n            skinTemperature.textContent = `${temp} °C`;\n        }, 1000);\n    });\n    \n    // 停止数据收集\n    stopBtn.addEventListener('click', function() {\n        isCollecting = false;\n        startBtn.disabled = false;\n        stopBtn.disabled = true;\n        \n        clearInterval(timerInterval);\n        \n        // 添加到历史记录\n        const historyData = document.getElementById('history-data');\n        const patientSelect = document.getElementById('patient-select');\n        const deviceSelect = document.getElementById('device-select');\n        \n        const patientText = patientSelect.options[patientSelect.selectedIndex].text;\n        const deviceText = deviceSelect.options[deviceSelect.selectedIndex].text;\n        \n        const now = new Date();\n        const dateStr = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-${String(now.getDate()).padStart(2, '0')} ${String(now.getHours()).padStart(2, '0')}:${String(now.getMinutes()).padStart(2, '0')}`;\n        \n        const newRow = document.createElement('tr');\n        newRow.innerHTML = `\n            <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">${dateStr}</td>\n            <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">${patientText}</td>\n            <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">${deviceText}</td>\n            <td class=\"px-4 py-3 text-sm text-gray-700 dark:text-gray-300\">${treatmentDuration.textContent}</td>\n            <td class=\"px-4 py-3 text-sm\">\n                <button class=\"view-data-btn text-primary hover:text-blue-700\">查看</button>\n            </td>\n        `;\n        \n        historyData.prepend(newRow);\n        \n        // 重置显示\n        deviceParams.textContent = \"- -\";\n        skinTemperature.textContent = \"- - °C\";\n        \n        alert('数据收集已完成，记录已保存');\n    });\n    \n    // 提交患者反馈\n    submitFeedbackBtn.addEventListener('click', function() {\n        const painLevel = document.getElementById('pain-level').value;\n        const comfortLevel = document.getElementById('comfort-level').value;\n        const symptomsDesc = document.getElementById('symptoms-desc').value;\n        \n        // 显示确认消息\n        alert(`反馈已提交!\\n疼痛等级: ${painLevel}\\n舒适度: ${comfortLevel}\\n症状描述: ${symptomsDesc}`);\n        \n        // 重置表单\n        document.getElementById('pain-level').value = 0;\n        document.getElementById('comfort-level').value = 5;\n        document.getElementById('symptoms-desc').value = '';\n    });\n    \n    // 查看历史数据\n    viewDataBtns.forEach(btn => {\n        btn.addEventListener('click', function() {\n            const row = this.closest('tr');\n            const cells = row.querySelectorAll('td');\n            \n            alert(`数据详情:\\n日期: ${cells[0].textContent}\\n患者: ${cells[1].textContent}\\n设备: ${cells[2].textContent}\\n时长: ${cells[3].textContent}\\n\\n详细报告即将生成...`);\n        });\n    });\n    \n    // 动态添加的行也要绑定事件\n    document.getElementById('history-data').addEventListener('click', function(e) {\n        if (e.target && e.target.classList.contains('view-data-btn')) {\n            const row = e.target.closest('tr');\n            const cells = row.querySelectorAll('td');\n            \n            alert(`数据详情:\\n日期: ${cells[0].textContent}\\n患者: ${cells[1].textContent}\\n设备: ${cells[2].textContent}\\n时长: ${cells[3].textContent}\\n\\n详细报告即将生成...`);\n        }\n    });\n}";

            string rawPrompt = """

                ## Task

                In a javascript + html + tailwind project, generate the code for the functionality and feature described below:

                - Software: 光疗临床路径智能规划系统是一款基于人工智能和大数据技术的SaaS产品，旨在帮助医疗机构为患者制定个性化的光疗方案，并根据患者的健康状况和治疗反馈调整治疗路径，优化治疗效果。
                - Feature Description: 数据分析与报告生成模块。数据分析与报告生成模块通过收集患者在光疗过程中的各项治疗数据，结合治疗前后的反馈信息，生成详细的分析报告。这些报告不仅帮助医生评估治疗效果，还能为未来的治疗路径提供决策支持。该模块能够实时监控患者的治疗进度，分析光疗方案的有效性，并对治疗方案进行动态调整，以确保每位患者获得最佳的治疗效果。
                - Functionality Description: 数据收集功能, 数据收集功能是光疗临床路径智能规划系统中的核心模块之一，主要负责自动化收集患者在光疗过程中各项关键数据，包括但不限于治疗设备参数、患者主观反馈（如疼痛、舒适度等）、生理数据（如皮肤反应、温度变化等）、以及病情变化（如症状改善情况、治疗效果等）。通过与治疗设备的实时连接，该模块能够确保数据的及时性与准确性，并支持数据的长期跟踪与记录。所收集的数据将用于后续治疗路径的优化和调整，为医生提供全面的信息支持，帮助制定个性化的治疗方案。此外，该模块还具备一定的智能分析能力，能够基于患者的历史数据与治疗反馈，预测治疗效果并提出建议。所有数据会在后台系统中进行汇总分析，形成易于理解和参考的报告。

                You should generate the code for the Functionality described above. The code you will generate will be in a restricted .js file "path-planning.js" that have already the code below:

                ```javascript
                window.renderDATAANALYSISPage = function(container) {
                    container.innerHTML = `
                        <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                            <h1 class="text-2xl font-bold mb-6 text-primary">数据分析与报告生成模块</h1>
                            <div id="function" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                                <h2>数据收集功能</h2>
                            </div>
                        </div>
                    `;
                }
                ```

                Here's the theme code will help you to generate the correct theme for the functionality:

                ```javascript
                tailwind.config = {
                    darkMode: 'class',
                    theme: {
                        extend: {
                            colors: {
                                primary: '#4a90e2',
                                secondary: '#f8f8f8',
                            },
                            fontFamily: {
                                'roboto': ['Roboto', 'sans-serif'],
                            },
                        }
                    }
                }
                ```

                Here's the exisitng files in the project:

                - index.html
                - tailwind.config.js
                - components/main.js
                - components/footer.js
                - components/leftbar.js
                - components/topbar.js
                - components/pages/data-analysis.js
                - components/pages/data-analysis.js
                - components/pages/data-analysis.js
                - components/pages/path-planning.js

                ## Output Format

                Return the pure code in a json filed, only code without any explaination, markdown symboles and other characters.

                {
                    "functionality_code": "" // js + html + tailwind css code in a .js file
                }

                """;

                //## Output Example
                
                //{
                //    "functionality_code": "window.renderDATAANALYSISPage = function(container) {container.innerHTML = `<div class=\"bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6\"><h1 class=\"text-2xl font-bold mb-6 text-primary\">数据分析与报告生成模坠</h1><div id=\"function\" class=\"bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4\"><h2>数据收集功能</h2><div id="function" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">{/* 数据收集功能 functionnalities code */}</div></div></div>`;}"
                //}

                //""";

            string prompt = rawPrompt
                .Replace("###{service_name}###", serviceName)
                .Replace("###{service_desc}###", serviceDescription)
                .Replace("###{feature_desc}###", featureDescription)
                .Replace("###{module_desc}###", moduleDescription);

            string codeJson = await openaiChatService.CompleteChatAsync(prompt, "spec-gen-module");

            return JsonSerializer.Deserialize<Functionality>(codeJson);
        }

        public async Task<string> MenuItemCodeGenAsync(Specification spec)
        {
            string rawPrompt = """
                ## Task

                Modify the existing code, change the menu items with feature data. Each feature corresponds to a menu item.
                    
                Return a the new code as output.

                ### Existing code

                ```js
                window.menuItems = [
                    { id: 'home', label: 'Dashboard', icon: 'cog' }
                ];
                ```

                ### feature data 

                Here are name of features, each feature corresponds to a menu item:

                ###{feature_data}###

                ## Output Format
                    
                Return the pure code only without any explaination, markdown symboles and other characters.

                ## Output Example

                window.menuItems = [
                    { id: 'item-1', label: 'Item one', icon: 'cog' },
                    { id: 'item-2', label: 'Item one', icon: 'cog' },
                    { id: 'item-3', label: 'Item one', icon: 'cog' }
                ];

                """;

            List<string> menuItems = spec.Features.Select(f => "- " + f.MenuItem).ToList();
            string prompt = rawPrompt
                .Replace("###{feature_data}###", string.Join("\n", menuItems));

            return await openaiChatService.CompleteChatAsync(prompt, "spec-gen-module");
        }
    }
}
