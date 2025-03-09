using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeBase.DesktopApp
{
    public class Tempo
    {
        public static string DataAnalysisReport = 
            """
            window.renderDATAANALYSISREPORTPage = function(container) {
                container.innerHTML = `
                    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                        <h1 class="text-2xl font-bold mb-6 text-primary">数据分析与报告模块</h1>
                        <div id="treatmentProgressMonitoring" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                            <h2 class="text-xl font-semibold mb-4 text-gray-800 dark:text-white">患者光疗治疗进度监控模块</h2>

                            <!-- 患者选择区域 -->
                            <div class="mb-6">
                                <div class="flex flex-wrap items-center mb-4">
                                    <label class="w-full sm:w-auto mr-4 mb-2 sm:mb-0 font-medium text-gray-700 dark:text-gray-300">选择患者:</label>
                                    <select id="patientSelect" class="w-full sm:w-64 px-4 py-2 border rounded-md bg-white dark:bg-gray-600 dark:text-white focus:outline-none focus:ring-2 focus:ring-primary">
                                        <option value="">请选择患者...</option>
                                    </select>
                                    <button id="loadPatientData" class="ml-2 px-4 py-2 bg-primary text-white rounded-md hover:bg-blue-600 transition duration-200">
                                        加载数据
                                    </button>
                                </div>
                            </div>

                            <!-- 治疗进度概览 -->
                            <div id="treatmentOverview" class="mb-6 p-4 bg-white dark:bg-gray-600 rounded-lg shadow hidden">
                                <h3 class="text-lg font-semibold mb-3 text-gray-800 dark:text-white">治疗进度概览</h3>
                                <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                                    <div class="p-3 bg-blue-50 dark:bg-gray-700 rounded-lg">
                                        <p class="text-sm text-gray-500 dark:text-gray-400">已完成疗程</p>
                                        <p id="completedSessions" class="text-xl font-bold text-primary">0/0</p>
                                    </div>
                                    <div class="p-3 bg-blue-50 dark:bg-gray-700 rounded-lg">
                                        <p class="text-sm text-gray-500 dark:text-gray-400">总治疗时长</p>
                                        <p id="totalTreatmentTime" class="text-xl font-bold text-primary">0小时</p>
                                    </div>
                                    <div class="p-3 bg-blue-50 dark:bg-gray-700 rounded-lg">
                                        <p class="text-sm text-gray-500 dark:text-gray-400">治疗达标率</p>
                                        <p id="complianceRate" class="text-xl font-bold text-primary">0%</p>
                                    </div>
                                </div>
                            </div>

                            <!-- 治疗详细数据 -->
                            <div id="treatmentDetails" class="mb-6 hidden">
                                <div class="flex justify-between items-center mb-4">
                                    <h3 class="text-lg font-semibold text-gray-800 dark:text-white">治疗详细数据</h3>
                                    <div class="flex space-x-2">
                                        <button id="weekView" class="px-3 py-1 bg-primary text-white rounded hover:bg-blue-600 text-sm">周视图</button>
                                        <button id="monthView" class="px-3 py-1 bg-gray-200 text-gray-700 dark:bg-gray-600 dark:text-white rounded hover:bg-gray-300 dark:hover:bg-gray-500 text-sm">月视图</button>
                                    </div>
                                </div>

                                <!-- 图表区域 -->
                                <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
                                    <div class="bg-white dark:bg-gray-600 rounded-lg shadow p-4">
                                        <h4 class="text-md font-medium mb-3 text-gray-700 dark:text-gray-300">光照强度趋势</h4>
                                        <canvas id="lightIntensityChart" width="400" height="200"></canvas>
                                    </div>
                                    <div class="bg-white dark:bg-gray-600 rounded-lg shadow p-4">
                                        <h4 class="text-md font-medium mb-3 text-gray-700 dark:text-gray-300">治疗时长分布</h4>
                                        <canvas id="treatmentDurationChart" width="400" height="200"></canvas>
                                    </div>
                                </div>
                            </div>

                            <!-- 治疗进度表格 -->
                            <div id="treatmentProgressTable" class="mb-6 hidden">
                                <h3 class="text-lg font-semibold mb-3 text-gray-800 dark:text-white">治疗进度记录</h3>
                                <div class="overflow-x-auto">
                                    <table class="min-w-full bg-white dark:bg-gray-700 rounded-lg overflow-hidden">
                                        <thead class="bg-gray-100 dark:bg-gray-800">
                                            <tr>
                                                <th class="py-3 px-4 text-left text-sm font-medium text-gray-600 dark:text-gray-300">日期</th>
                                                <th class="py-3 px-4 text-left text-sm font-medium text-gray-600 dark:text-gray-300">光照强度</th>
                                                <th class="py-3 px-4 text-left text-sm font-medium text-gray-600 dark:text-gray-300">治疗时长</th>
                                                <th class="py-3 px-4 text-left text-sm font-medium text-gray-600 dark:text-gray-300">状态</th>
                                                <th class="py-3 px-4 text-left text-sm font-medium text-gray-600 dark:text-gray-300">操作</th>
                                            </tr>
                                        </thead>
                                        <tbody id="treatmentRecordsBody">
                                            <!-- 表格数据将通过JavaScript动态填充 -->
                                        </tbody>
                                    </table>
                                </div>
                            </div>

                            <!-- 异常警报区域 -->
                            <div id="alertSection" class="mb-6 hidden">
                                <h3 class="text-lg font-semibold mb-3 text-gray-800 dark:text-white">治疗异常警报</h3>
                                <div id="alertContainer" class="space-y-3">
                                    <!-- 警报将通过JavaScript动态添加 -->
                                </div>
                            </div>

                            <!-- 报告生成区域 -->
                            <div class="mt-8 pt-4 border-t border-gray-200 dark:border-gray-600 hidden" id="reportGeneration">
                                <h3 class="text-lg font-semibold mb-4 text-gray-800 dark:text-white">治疗报告生成</h3>
                                <div class="flex flex-wrap gap-3">
                                    <button id="generateDetailedReport" class="px-4 py-2 bg-primary text-white rounded-md hover:bg-blue-600 transition duration-200">
                                        生成详细报告
                                    </button>
                                    <button id="generateSummaryReport" class="px-4 py-2 bg-green-500 text-white rounded-md hover:bg-green-600 transition duration-200">
                                        生成摘要报告
                                    </button>
                                    <button id="exportData" class="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 transition duration-200">
                                        导出原始数据
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                // 模拟患者数据
                const patients = [
                    { id: 1, name: "张三", age: 45, gender: "男", diagnosis: "银屑病" },
                    { id: 2, name: "李四", age: 32, gender: "女", diagnosis: "白癜风" },
                    { id: 3, name: "王五", age: 56, gender: "男", diagnosis: "湿疹" }
                ];

                // 模拟治疗数据
                const treatmentData = {
                    1: {
                        totalSessions: 12,
                        completedSessions: 8,
                        totalDuration: 16.5,
                        complianceRate: 92,
                        sessions: [
                            { date: "2023-10-01", intensity: 45, duration: 2.0, status: "完成", notes: "患者反映效果良好" },
                            { date: "2023-10-03", intensity: 50, duration: 2.0, status: "完成", notes: "轻微发红，正常反应" },
                            { date: "2023-10-05", intensity: 55, duration: 2.0, status: "完成", notes: "无不适" },
                            { date: "2023-10-08", intensity: 60, duration: 2.0, status: "完成", notes: "皮损面积减少" },
                            { date: "2023-10-10", intensity: 60, duration: 2.5, status: "完成", notes: "延长治疗时间" },
                            { date: "2023-10-12", intensity: 65, duration: 2.0, status: "完成", notes: "治疗反应良好" },
                            { date: "2023-10-15", intensity: 65, duration: 2.0, status: "完成", notes: "持续改善" },
                            { date: "2023-10-17", intensity: 70, duration: 2.0, status: "完成", notes: "患者满意度高" }
                        ],
                        alerts: [
                            { date: "2023-10-03", type: "轻度", message: "患者报告轻微皮肤发红，属正常反应范围", resolved: true },
                            { date: "2023-10-10", type: "提示", message: "治疗时间延长，需观察患者反应", resolved: true }
                        ]
                    },
                    2: {
                        totalSessions: 15,
                        completedSessions: 6,
                        totalDuration: 12.0,
                        complianceRate: 85,
                        sessions: [
                            { date: "2023-10-02", intensity: 35, duration: 2.0, status: "完成", notes: "初次治疗，患者适应良好" },
                            { date: "2023-10-04", intensity: 35, duration: 2.0, status: "完成", notes: "无不适" },
                            { date: "2023-10-06", intensity: 40, duration: 2.0, status: "完成", notes: "患者报告有轻微刺痛感" },
                            { date: "2023-10-09", intensity: 40, duration: 2.0, status: "完成", notes: "刺痛感消失" },
                            { date: "2023-10-11", intensity: 45, duration: 2.0, status: "完成", notes: "开始观察到色素恢复" },
                            { date: "2023-10-16", intensity: 45, duration: 2.0, status: "完成", notes: "持续改善" }
                        ],
                        alerts: [
                            { date: "2023-10-06", type: "轻度", message: "患者报告轻微刺痛感，需继续观察", resolved: true },
                            { date: "2023-10-13", type: "警告", message: "患者未按计划来院治疗，已联系随访", resolved: false }
                        ]
                    },
                    3: {
                        totalSessions: 10,
                        completedSessions: 4,
                        totalDuration: 7.5,
                        complianceRate: 80,
                        sessions: [
                            { date: "2023-10-05", intensity: 30, duration: 1.5, status: "完成", notes: "患者皮肤敏感，降低初始强度" },
                            { date: "2023-10-07", intensity: 30, duration: 2.0, status: "完成", notes: "患者适应良好" },
                            { date: "2023-10-09", intensity: 35, duration: 2.0, status: "完成", notes: "症状有所缓解" },
                            { date: "2023-10-14", intensity: 40, duration: 2.0, status: "完成", notes: "瘙痒感明显减轻" }
                        ],
                        alerts: [
                            { date: "2023-10-05", type: "提示", message: "患者皮肤敏感，治疗强度已调低", resolved: true },
                            { date: "2023-10-11", type: "警告", message: "患者未按计划来院治疗，已联系随访", resolved: true },
                            { date: "2023-10-16", type: "警告", message: "患者连续两次未到院，已电话随访", resolved: false }
                        ]
                    }
                };

                // 填充患者选择下拉框
                const patientSelect = document.getElementById('patientSelect');
                patients.forEach(patient => {
                    const option = document.createElement('option');
                    option.value = patient.id;
                    option.textContent = `${patient.name} (${patient.age}岁, ${patient.gender}) - ${patient.diagnosis}`;
                    patientSelect.appendChild(option);
                });

                // 加载患者数据按钮事件
                const loadPatientDataBtn = document.getElementById('loadPatientData');
                loadPatientDataBtn.addEventListener('click', function() {
                    const patientId = parseInt(patientSelect.value);
                    if (!patientId) {
                        alert('请先选择一名患者');
                        return;
                    }

                    loadPatientTreatmentData(patientId);
                });

                // 加载患者治疗数据函数
                function loadPatientTreatmentData(patientId) {
                    const patientData = treatmentData[patientId];
                    if (!patientData) {
                        alert('无法加载患者数据，请稍后再试');
                        return;
                    }

                    // 显示所有隐藏的部分
                    document.getElementById('treatmentOverview').classList.remove('hidden');
                    document.getElementById('treatmentDetails').classList.remove('hidden');
                    document.getElementById('treatmentProgressTable').classList.remove('hidden');
                    document.getElementById('alertSection').classList.remove('hidden');
                    document.getElementById('reportGeneration').classList.remove('hidden');

                    // 更新治疗进度概览
                    document.getElementById('completedSessions').textContent = 
                        `${patientData.completedSessions}/${patientData.totalSessions}`;
                    document.getElementById('totalTreatmentTime').textContent = 
                        `${patientData.totalDuration}小时`;
                    document.getElementById('complianceRate').textContent = 
                        `${patientData.complianceRate}%`;

                    // 更新治疗记录表格
                    const tableBody = document.getElementById('treatmentRecordsBody');
                    tableBody.innerHTML = '';

                    patientData.sessions.forEach(session => {
                        const row = document.createElement('tr');
                        row.className = 'hover:bg-gray-50 dark:hover:bg-gray-600';

                        row.innerHTML = `
                            <td class="py-3 px-4 border-b border-gray-200 dark:border-gray-600 text-sm text-gray-700 dark:text-gray-300">${session.date}</td>
                            <td class="py-3 px-4 border-b border-gray-200 dark:border-gray-600 text-sm text-gray-700 dark:text-gray-300">${session.intensity} mW/cm²</td>
                            <td class="py-3 px-4 border-b border-gray-200 dark:border-gray-600 text-sm text-gray-700 dark:text-gray-300">${session.duration} 小时</td>
                            <td class="py-3 px-4 border-b border-gray-200 dark:border-gray-600 text-sm">
                                <span class="px-2 py-1 rounded-full text-xs ${session.status === '完成' ? 'bg-green-100 text-green-800 dark:bg-green-700 dark:text-green-100' : 'bg-yellow-100 text-yellow-800 dark:bg-yellow-700 dark:text-yellow-100'}">
                                    ${session.status}
                                </span>
                            </td>
                            <td class="py-3 px-4 border-b border-gray-200 dark:border-gray-600 text-sm">
                                <button class="viewDetails text-primary hover:text-blue-700 mr-2" data-date="${session.date}">查看详情</button>
                            </td>
                        `;

                        tableBody.appendChild(row);
                    });

                    // 添加表格行点击事件
                    document.querySelectorAll('.viewDetails').forEach(button => {
                        button.addEventListener('click', function() {
                            const date = this.getAttribute('data-date');
                            const session = patientData.sessions.find(s => s.date === date);

                            alert(`治疗日期: ${session.date}\n光照强度: ${session.intensity} mW/cm²\n治疗时长: ${session.duration} 小时\n状态: ${session.status}\n备注: ${session.notes}`);
                        });
                    });

                    // 更新警报区域
                    const alertContainer = document.getElementById('alertContainer');
                    alertContainer.innerHTML = '';

                    if (patientData.alerts && patientData.alerts.length > 0) {
                        patientData.alerts.forEach(alert => {
                            const alertElem = document.createElement('div');
                            let alertClass = 'bg-yellow-50 border-l-4 border-yellow-400 dark:bg-gray-700 dark:border-yellow-500';

                            if (alert.type === '轻度') {
                                alertClass = 'bg-blue-50 border-l-4 border-blue-400 dark:bg-gray-700 dark:border-blue-500';
                            } else if (alert.type === '警告') {
                                alertClass = 'bg-red-50 border-l-4 border-red-400 dark:bg-gray-700 dark:border-red-500';
                            }

                            alertElem.className = `${alertClass} p-4 rounded flex justify-between items-center`;

                            alertElem.innerHTML = `
                                <div>
                                    <div class="flex items-center">
                                        <span class="font-medium text-sm text-gray-700 dark:text-gray-300">${alert.date} - ${alert.type}</span>
                                        ${alert.resolved ? 
                                            '<span class="ml-2 px-2 py-0.5 text-xs bg-green-100 text-green-800 dark:bg-green-700 dark:text-green-100 rounded-full">已解决</span>' : 
                                            '<span class="ml-2 px-2 py-0.5 text-xs bg-red-100 text-red-800 dark:bg-red-700 dark:text-red-100 rounded-full">未解决</span>'}
                                    </div>
                                    <p class="text-sm text-gray-600 dark:text-gray-400 mt-1">${alert.message}</p>
                                </div>
                                <button class="resolveAlert text-gray-500 hover:text-gray-700 dark:text-gray-400 dark:hover:text-gray-300 ${alert.resolved ? 'hidden' : ''}" data-date="${alert.date}">
                                    标记为已解决
                                </button>
                            `;

                            alertContainer.appendChild(alertElem);
                        });

                        // 添加解决警报事件
                        document.querySelectorAll('.resolveAlert').forEach(button => {
                            button.addEventListener('click', function() {
                                this.parentNode.querySelector('span.text-xs').className = 'ml-2 px-2 py-0.5 text-xs bg-green-100 text-green-800 dark:bg-green-700 dark:text-green-100 rounded-full';
                                this.parentNode.querySelector('span.text-xs').textContent = '已解决';
                                this.classList.add('hidden');

                                // 在实际应用中，这里应该发送请求更新警报状态
                            });
                        });
                    } else {
                        alertContainer.innerHTML = '<p class="text-gray-500 dark:text-gray-400">当前没有异常警报</p>';
                    }

                    // 生成图表
                    generateCharts(patientData);

                    // 绑定报告按钮事件
                    document.getElementById('generateDetailedReport').addEventListener('click', function() {
                        alert('详细报告生成中，稍后将可在"报告管理"模块中查看');
                    });

                    document.getElementById('generateSummaryReport').addEventListener('click', function() {
                        alert('摘要报告生成中，稍后将可在"报告管理"模块中查看');
                    });

                    document.getElementById('exportData').addEventListener('click', function() {
                        alert('数据导出中，请稍候...');
                    });

                    // 绑定视图切换按钮
                    document.getElementById('weekView').addEventListener('click', function() {
                        this.className = 'px-3 py-1 bg-primary text-white rounded hover:bg-blue-600 text-sm';
                        document.getElementById('monthView').className = 'px-3 py-1 bg-gray-200 text-gray-700 dark:bg-gray-600 dark:text-white rounded hover:bg-gray-300 dark:hover:bg-gray-500 text-sm';
                        generateCharts(patientData, 'week');
                    });

                    document.getElementById('monthView').addEventListener('click', function() {
                        this.className = 'px-3 py-1 bg-primary text-white rounded hover:bg-blue-600 text-sm';
                        document.getElementById('weekView').className = 'px-3 py-1 bg-gray-200 text-gray-700 dark:bg-gray-600 dark:text-white rounded hover:bg-gray-300 dark:hover:bg-gray-500 text-sm';
                        generateCharts(patientData, 'month');
                    });
                }

                // 生成图表函数
                function generateCharts(patientData, viewType = 'week') {
                    // 使用Chart.js (模拟，实际使用时需引入Chart.js库)
                    if (typeof Chart === 'undefined') {
                        // 模拟Chart.js
                        window.Chart = class {
                            constructor(ctx, config) {
                                this.ctx = ctx;
                                this.config = config;

                                // 简单模拟绘制
                                const canvas = document.getElementById(ctx.canvas.id);
                                const context = canvas.getContext('2d');
                                context.fillStyle = '#f0f0f0';
                                context.fillRect(0, 0, canvas.width, canvas.height);
                                context.fillStyle = '#4a90e2';
                                context.font = '14px Arial';
                                context.fillText('模拟图表 - 实际应用需引入Chart.js', 20, 100);
                            }

                            // 更新图表方法
                            update() {}
                        };
                    }

                    // 准备强度图表数据
                    const intensityData = {
                        labels: patientData.sessions.map(s => s.date),
                        datasets: [{
                            label: '光照强度 (mW/cm²)',
                            data: patientData.sessions.map(s => s.intensity),
                            borderColor: '#4a90e2',
                            backgroundColor: 'rgba(74, 144, 226, 0.2)',
                            borderWidth: 2,
                            fill: true
                        }]
                    };

                    // 准备时长图表数据
                    const durationData = {
                        labels: patientData.sessions.map(s => s.date),
                        datasets: [{
                            label: '治疗时长 (小时)',
                            data: patientData.sessions.map(s => s.duration),
                            borderColor: '#34c759',
                            backgroundColor: 'rgba(52, 199, 89, 0.2)',
                            borderWidth: 2,
                            fill: true
                        }]
                    };

                    // 图表配置
                    const chartConfig = {
                        type: 'line',
                        options: {
                            responsive: true,
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        }
                    };

                    // 创建强度图表
                    const intensityCtx = document.getElementById('lightIntensityChart').getContext('2d');
                    const intensityChart = new Chart(intensityCtx, {
                        ...chartConfig,
                        data: intensityData
                    });

                    // 创建时长图表
                    const durationCtx = document.getElementById('treatmentDurationChart').getContext('2d');
                    const durationChart = new Chart(durationCtx, {
                        ...chartConfig,
                        data: durationData
                    });
                }
            };
        
            
            """;



        public static string IntelligentPathPlanning =
            """
            window.renderINTELLIGENTPATHPLANNINGPage = function(container) {
                container.innerHTML = `
                    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                        <h1 class="text-2xl font-bold mb-6 text-primary">智能路径规划模块</h1>
                        <div id="function" class="bg-secondary dark:bg-gray-700 p-4 rounded-lg my-4">
                            <h2 class="text-xl font-semibold mb-4 text-gray-800 dark:text-white">个性化光疗治疗方案自动生成模块</h2>

                            <!-- 患者信息输入表单 -->
                            <div class="mb-6 p-4 bg-white dark:bg-gray-600 rounded-lg shadow">
                                <h3 class="text-lg font-medium mb-3 text-gray-700 dark:text-gray-200">患者基本信息</h3>
                                <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">患者ID</label>
                                        <input type="text" id="patientId" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white" placeholder="请输入或选择患者ID">
                                    </div>
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">年龄</label>
                                        <input type="number" id="patientAge" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white" placeholder="年龄">
                                    </div>
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">性别</label>
                                        <select id="patientGender" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white">
                                            <option value="">请选择</option>
                                            <option value="male">男</option>
                                            <option value="female">女</option>
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">体重(kg)</label>
                                        <input type="number" id="patientWeight" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white" placeholder="体重">
                                    </div>
                                </div>
                            </div>

                            <!-- 诊断和治疗历史 -->
                            <div class="mb-6 p-4 bg-white dark:bg-gray-600 rounded-lg shadow">
                                <h3 class="text-lg font-medium mb-3 text-gray-700 dark:text-gray-200">诊断和治疗历史</h3>
                                <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">疾病类型</label>
                                        <select id="diseaseType" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white">
                                            <option value="">请选择</option>
                                            <option value="psoriasis">银屑病</option>
                                            <option value="vitiligo">白癜风</option>
                                            <option value="dermatitis">皮炎</option>
                                            <option value="eczema">湿疹</option>
                                            <option value="other">其他</option>
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">病情严重程度</label>
                                        <select id="severityLevel" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white">
                                            <option value="">请选择</option>
                                            <option value="mild">轻度</option>
                                            <option value="moderate">中度</option>
                                            <option value="severe">重度</option>
                                            <option value="very-severe">极重度</option>
                                        </select>
                                    </div>
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">患病时长</label>
                                        <div class="flex items-center">
                                            <input type="number" id="illnessDuration" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white" placeholder="时长">
                                            <select id="illnessDurationUnit" class="ml-2 px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white">
                                                <option value="months">月</option>
                                                <option value="years">年</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="mb-3">
                                        <label class="block text-gray-700 dark:text-gray-200 mb-1">以往光疗治疗效果</label>
                                        <select id="previousTreatmentEffectiveness" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white">
                                            <option value="">请选择</option>
                                            <option value="excellent">优秀</option>
                                            <option value="good">良好</option>
                                            <option value="moderate">中等</option>
                                            <option value="poor">较差</option>
                                            <option value="none">无效果</option>
                                            <option value="first-time">首次治疗</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="block text-gray-700 dark:text-gray-200 mb-1">患者皮肤敏感度</label>
                                    <select id="skinSensitivity" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white">
                                        <option value="">请选择</option>
                                        <option value="low">低敏感度</option>
                                        <option value="normal">正常敏感度</option>
                                        <option value="high">高敏感度</option>
                                        <option value="very-high">极高敏感度</option>
                                    </select>
                                </div>
                                <div class="mb-3">
                                    <label class="block text-gray-700 dark:text-gray-200 mb-1">特殊情况/注意事项</label>
                                    <textarea id="specialNotes" class="w-full px-3 py-2 border border-gray-300 dark:border-gray-500 rounded-md dark:bg-gray-700 dark:text-white" rows="3" placeholder="请输入特殊情况或注意事项"></textarea>
                                </div>
                            </div>

                            <!-- 按钮区域 -->
                            <div class="flex flex-col md:flex-row justify-center space-y-2 md:space-y-0 md:space-x-4 mb-6">
                                <button id="generatePlanBtn" class="px-6 py-2 bg-primary hover:bg-blue-600 text-white font-medium rounded-lg transition-colors">
                                    生成治疗方案
                                </button>
                                <button id="resetFormBtn" class="px-6 py-2 bg-gray-200 hover:bg-gray-300 dark:bg-gray-600 dark:hover:bg-gray-500 text-gray-700 dark:text-white font-medium rounded-lg transition-colors">
                                    重置表单
                                </button>
                            </div>

                            <!-- 生成的治疗方案显示区域 -->
                            <div id="treatmentPlan" class="hidden p-4 bg-white dark:bg-gray-600 rounded-lg shadow">
                                <h3 class="text-lg font-bold mb-4 text-primary">个性化光疗治疗方案</h3>
                                <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
                                    <div class="bg-secondary dark:bg-gray-700 p-4 rounded-lg">
                                        <h4 class="font-semibold mb-2 text-gray-700 dark:text-gray-200">治疗周期</h4>
                                        <div id="treatmentCycle" class="text-gray-800 dark:text-white"></div>
                                    </div>
                                    <div class="bg-secondary dark:bg-gray-700 p-4 rounded-lg">
                                        <h4 class="font-semibold mb-2 text-gray-700 dark:text-gray-200">光照强度</h4>
                                        <div id="lightIntensity" class="text-gray-800 dark:text-white"></div>
                                    </div>
                                    <div class="bg-secondary dark:bg-gray-700 p-4 rounded-lg">
                                        <h4 class="font-semibold mb-2 text-gray-700 dark:text-gray-200">治疗频率</h4>
                                        <div id="treatmentFrequency" class="text-gray-800 dark:text-white"></div>
                                    </div>
                                    <div class="bg-secondary dark:bg-gray-700 p-4 rounded-lg">
                                        <h4 class="font-semibold mb-2 text-gray-700 dark:text-gray-200">单次治疗时长</h4>
                                        <div id="sessionDuration" class="text-gray-800 dark:text-white"></div>
                                    </div>
                                </div>

                                <div class="mt-4 bg-secondary dark:bg-gray-700 p-4 rounded-lg">
                                    <h4 class="font-semibold mb-2 text-gray-700 dark:text-gray-200">治疗建议</h4>
                                    <div id="treatmentRecommendations" class="text-gray-800 dark:text-white"></div>
                                </div>

                                <div class="mt-4 bg-secondary dark:bg-gray-700 p-4 rounded-lg">
                                    <h4 class="font-semibold mb-2 text-gray-700 dark:text-gray-200">预期治疗效果</h4>
                                    <div id="expectedOutcome" class="text-gray-800 dark:text-white"></div>
                                </div>

                                <div class="mt-6 flex justify-end">
                                    <button id="printPlanBtn" class="px-4 py-2 bg-primary hover:bg-blue-600 text-white font-medium rounded-lg transition-colors mr-2">
                                        <i class="fas fa-print mr-1"></i> 打印方案
                                    </button>
                                    <button id="exportPlanBtn" class="px-4 py-2 bg-green-500 hover:bg-green-600 text-white font-medium rounded-lg transition-colors mr-2">
                                        <i class="fas fa-file-export mr-1"></i> 导出方案
                                    </button>
                                    <button id="savePlanBtn" class="px-4 py-2 bg-primary hover:bg-blue-600 text-white font-medium rounded-lg transition-colors">
                                        <i class="fas fa-save mr-1"></i> 保存方案
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                // 函数：生成治疗方案
                function generateTreatmentPlan() {
                    const patientId = document.getElementById('patientId').value;
                    const patientAge = document.getElementById('patientAge').value;
                    const patientGender = document.getElementById('patientGender').value;
                    const patientWeight = document.getElementById('patientWeight').value;
                    const diseaseType = document.getElementById('diseaseType').value;
                    const severityLevel = document.getElementById('severityLevel').value;
                    const illnessDuration = document.getElementById('illnessDuration').value;
                    const illnessDurationUnit = document.getElementById('illnessDurationUnit').value;
                    const previousTreatment = document.getElementById('previousTreatmentEffectiveness').value;
                    const skinSensitivity = document.getElementById('skinSensitivity').value;

                    // 验证必填字段
                    if (!patientId || !patientAge || !patientGender || !diseaseType || !severityLevel || !skinSensitivity) {
                        showNotification('请填写所有必要的患者信息', 'error');
                        return;
                    }

                    // 显示加载状态
                    showLoading();

                    // 模拟API调用延迟
                    setTimeout(() => {
                        // 处理生成的治疗方案（这里使用模拟数据）
                        const plan = generateAITreatmentPlan(
                            diseaseType, 
                            severityLevel, 
                            patientAge, 
                            patientGender, 
                            skinSensitivity, 
                            previousTreatment
                        );

                        // 显示治疗方案
                        displayTreatmentPlan(plan);

                        // 隐藏加载状态
                        hideLoading();

                        // 显示成功通知
                        showNotification('治疗方案生成成功！', 'success');
                    }, 1500);
                }

                // 函数：模拟AI生成治疗方案（实际项目中应由后端AI模型生成）
                function generateAITreatmentPlan(diseaseType, severity, age, gender, skinSensitivity, previousTreatment) {
                    // 基于输入参数的算法模拟，实际应由AI模型处理
                    const plans = {
                        'psoriasis': {
                            'mild': {
                                cycle: '4-6周，每周3次',
                                intensity: '初始剂量：70% MED (最小红斑剂量)，逐渐递增5-10%',
                                frequency: '每周3次，间隔至少48小时',
                                duration: '每次治疗2-3分钟，逐渐增加至5-10分钟',
                                recommendations: '1. 治疗期间需保持皮肤清洁干燥\n2. 避免在治疗前使用增敏药物\n3. 治疗后使用保湿剂\n4. 避免治疗当天曝晒阳光',
                                outcome: '预计4-6周后病情显著改善，皮损面积减少约50%，瘙痒症状明显缓解。'
                            },
                            'moderate': {
                                cycle: '8-12周，每周3-4次',
                                intensity: '初始剂量：60% MED，逐渐递增5-10%，密切监测皮肤反应',
                                frequency: '每周3-4次，间隔至少24小时',
                                duration: '每次治疗3-5分钟，逐渐增加至10-15分钟',
                                recommendations: '1. 治疗前后使用医生推荐的专业护肤品\n2. 严格遵循治疗时间表\n3. 记录每次治疗后的皮肤反应\n4. 及时报告任何不适症状\n5. 治疗期间定期复诊评估',
                                outcome: '预计8周后皮损面积减少约60%，12周后可达到80%改善，鳞屑和红斑显著减轻。'
                            },
                            'severe': {
                                cycle: '12-16周，每周4-5次',
                                intensity: '初始剂量：50% MED，谨慎递增3-7%，严密监测',
                                frequency: '每周4-5次，保持规律性',
                                duration: '每次治疗2-3分钟，缓慢增加至最多15分钟',
                                recommendations: '1. 可能需要结合口服药物治疗\n2. 每周评估治疗反应\n3. 避免任何可能的刺激因素\n4. 增加蛋白质和维生素D摄入\n5. 保持良好的作息习惯\n6. 考虑心理疏导',
                                outcome: '预计需要12-16周的持续治疗，期望达到皮损面积减少70%，显著改善生活质量。'
                            }
                        },
                        'vitiligo': {
                            'mild': {
                                cycle: '12-16周，每周2-3次',
                                intensity: '初始剂量：40% MED，缓慢递增',
                                frequency: '每周2-3次，规律治疗',
                                duration: '每次1-2分钟，逐渐增加至5分钟',
                                recommendations: '1. 可搭配局部外用药\n2. 防晒措施至关重要\n3. 保持治疗区域清洁\n4. 记录色素变化情况',
                                outcome: '预计16周后可见初步色素恢复，约30%的白斑区域出现再色素化。'
                            },
                            'moderate': {
                                cycle: '16-24周，每周3次',
                                intensity: '初始剂量：35% MED，缓慢递增，不超过70% MED',
                                frequency: '每周3次，固定时间进行',
                                duration: '每次2-3分钟，最长不超过8分钟',
                                recommendations: '1. 结合光敏剂使用效果更佳\n2. 严格防晒\n3. 补充抗氧化剂\n4. 避免精神压力\n5. 拍摄治疗前后照片比对',
                                outcome: '预计24周后约50%的病变区域可能恢复色素，面部反应优于四肢。'
                            },
                            'severe': {
                                cycle: '24-36周，每周3-4次',
                                intensity: '初始剂量：30% MED，小幅递增',
                                frequency: '每周3-4次，长期坚持',
                                duration: '每次1-2分钟，最长不超过10分钟',
                                recommendations: '1. 考虑结合其他系统治疗\n2. 必要时进行营养素检测\n3. 定期医学摄影记录\n4. 可能需要长期维持治疗\n5. 心理支持非常重要',
                                outcome: '预计36周后可能有30-40%的再色素化，反应个体差异大，需要长期维持治疗。'
                            }
                        },
                        'dermatitis': {
                            'mild': {
                                cycle: '3-4周，每周2次',
                                intensity: '初始剂量：50% MED，缓慢递增',
                                frequency: '每周2次，间隔3天以上',
                                duration: '每次1-2分钟，最长不超过5分钟',
                                recommendations: '1. 保持皮肤湿润\n2. 避免使用刺激性清洁产品\n3. 记录可能的过敏原',
                                outcome: '预计3-4周后瘙痒和炎症症状显著改善，皮肤屏障功能恢复。'
                            },
                            'moderate': {
                                cycle: '4-6周，每周2-3次',
                                intensity: '初始剂量：40% MED，谨慎递增',
                                frequency: '每周2-3次，规律治疗',
                                duration: '每次2-3分钟，不超过8分钟',
                                recommendations: '1. 结合温和类固醇使用\n2. 治疗后立即保湿\n3. 识别并避免触发因素\n4. 使用无香料化妆品和洗护产品',
                                outcome: '预计6周后炎症减轻80%，瘙痒基本消失，皮肤纹理显著改善。'
                            },
                            'severe': {
                                cycle: '6-8周，每周3次',
                                intensity: '初始剂量：30% MED，小幅递增',
                                frequency: '每周3次，避免连续两天治疗',
                                duration: '每次1-2分钟，最长不超过6分钟',
                                recommendations: '1. 可能需要短期系统性治疗\n2. 密切监测皮肤反应\n3. 保持室内湿度\n4. 穿着棉质宽松衣物\n5. 考虑免疫调节剂',
                                outcome: '预计8周后可控制急性症状，但可能需要维持治疗预防复发。'
                            }
                        }
                    };

                    // 基于输入调整治疗方案
                    let plan = plans[diseaseType][severity];

                    // 根据年龄调整
                    if (parseInt(age) > 60) {
                        plan.intensity = plan.intensity.replace(/\d+%/, match => {
                            return Math.max(parseInt(match) - 10, 20) + '%';
                        });
                        plan.recommendations += '\n7. 老年患者需要更温和的治疗方案，密切观察不良反应';
                    } else if (parseInt(age) < 18) {
                        plan.intensity = plan.intensity.replace(/\d+%/, match => {
                            return Math.max(parseInt(match) - 15, 15) + '%';
                        });
                        plan.recommendations += '\n7. 未成年患者应在成人监督下进行治疗，密切观察皮肤反应';
                    }

                    // 根据皮肤敏感度调整
                    if (skinSensitivity === 'high' || skinSensitivity === 'very-high') {
                        plan.intensity = plan.intensity.replace(/\d+%/, match => {
                            return Math.max(parseInt(match) - 20, 10) + '%';
                        });
                        plan.recommendations += '\n8. 高敏感度皮肤需要更温和的治疗方案，治疗后使用舒缓保湿产品';
                    }

                    // 考虑既往治疗效果
                    if (previousTreatment === 'poor' || previousTreatment === 'none') {
                        plan.cycle = plan.cycle.replace(/\d+-\d+/, match => {
                            const [min, max] = match.split('-').map(Number);
                            return `${min + 2}-${max + 4}`;
                        });
                        plan.recommendations += '\n9. 既往治疗效果不佳，可能需要更长疗程，建议结合其他治疗方式';
                    } else if (previousTreatment === 'excellent' || previousTreatment === 'good') {
                        plan.intensity = plan.intensity.replace(/\d+%/, match => {
                            return (parseInt(match) + 5) + '%';
                        });
                        plan.recommendations += '\n9. 既往治疗效果良好，预期本次治疗反应较好';
                    }

                    return plan;
                }

                // 函数：显示治疗方案
                function displayTreatmentPlan(plan) {
                    document.getElementById('treatmentPlan').classList.remove('hidden');
                    document.getElementById('treatmentCycle').textContent = plan.cycle;
                    document.getElementById('lightIntensity').textContent = plan.intensity;
                    document.getElementById('treatmentFrequency').textContent = plan.frequency;
                    document.getElementById('sessionDuration').textContent = plan.duration;
                    document.getElementById('treatmentRecommendations').innerHTML = plan.recommendations.replace(/\n/g, '<br>');
                    document.getElementById('expectedOutcome').textContent = plan.outcome;

                    // 滚动到治疗方案区域
                    document.getElementById('treatmentPlan').scrollIntoView({behavior: 'smooth'});
                }

                // 函数：重置表单
                function resetForm() {
                    const form = document.querySelectorAll('input, select, textarea');
                    form.forEach(element => {
                        element.value = '';
                    });
                    document.getElementById('treatmentPlan').classList.add('hidden');
                }

                // 函数：显示通知
                function showNotification(message, type) {
                    const notificationContainer = document.createElement('div');
                    notificationContainer.className = `fixed top-4 right-4 z-50 p-4 rounded-lg shadow-lg ${
                        type === 'success' ? 'bg-green-500' : 'bg-red-500'
                    } text-white`;
                    notificationContainer.textContent = message;
                    document.body.appendChild(notificationContainer);

                    // 3秒后自动关闭
                    setTimeout(() => {
                        notificationContainer.remove();
                    }, 3000);
                }

                // 函数：显示加载状态
                function showLoading() {
                    const loadingOverlay = document.createElement('div');
                    loadingOverlay.id = 'loadingOverlay';
                    loadingOverlay.className = 'fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50';
                    loadingOverlay.innerHTML = `
                        <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg flex flex-col items-center">
                            <div class="loader mb-4 w-12 h-12 border-4 border-primary border-t-transparent rounded-full animate-spin"></div>
                            <p class="text-gray-700 dark:text-white">AI正在生成个性化治疗方案...</p>
                        </div>
                    `;
                    document.body.appendChild(loadingOverlay);
                }

                // 函数：隐藏加载状态
                function hideLoading() {
                    const loadingOverlay = document.getElementById('loadingOverlay');
                    if (loadingOverlay) {
                        loadingOverlay.remove();
                    }
                }

                // 函数：打印治疗方案
                function printTreatmentPlan() {
                    const printWindow = window.open('', '_blank');
                    const patientId = document.getElementById('patientId').value;
                    const patientAge = document.getElementById('patientAge').value;
                    const patientGender = document.getElementById('patientGender').value;
                    const diseaseType = document.getElementById('diseaseType').value;
                    const diseaseLabel = document.getElementById('diseaseType').options[document.getElementById('diseaseType').selectedIndex].text;

                    printWindow.document.write(`
                        <html>
                        <head>
                            <title>个性化光疗治疗方案 - 患者ID: ${patientId}</title>
                            <style>
                                body { font-family: Arial, sans-serif; margin: 20px; }
                                h1, h2 { color: #4a90e2; }
                                .section { margin-bottom: 15px; }
                                .header { border-bottom: 1px solid #eee; padding-bottom: 10px; margin-bottom: 20px; }
                                .parameter { margin-bottom: 10px; }
                                .label { font-weight: bold; }
                                .footer { margin-top: 30px; border-top: 1px solid #eee; padding-top: 10px; font-size: 0.8em; }
                            </style>
                        </head>
                        <body>
                            <div class="header">
                                <h1>个性化光疗治疗方案</h1>
                                <p>生成日期: ${new Date().toLocaleDateString()}</p>
                            </div>

                            <div class="section">
                                <h2>患者信息</h2>
                                <div class="parameter">
                                    <span class="label">患者ID:</span> ${patientId}
                                </div>
                                <div class="parameter">
                                    <span class="label">年龄:</span> ${patientAge}
                                </div>
                                <div class="parameter">
                                    <span class="label">性别:</span> ${patientGender === 'male' ? '男' : '女'}
                                </div>
                                <div class="parameter">
                                    <span class="label">诊断:</span> ${diseaseLabel}
                                </div>
                            </div>

                            <div class="section">
                                <h2>治疗方案详情</h2>
                                <div class="parameter">
                                    <span class="label">治疗周期:</span> ${document.getElementById('treatmentCycle').textContent}
                                </div>
                                <div class="parameter">
                                    <span class="label">光照强度:</span> ${document.getElementById('lightIntensity').textContent}
                                </div>
                                <div class="parameter">
                                    <span class="label">治疗频率:</span> ${document.getElementById('treatmentFrequency').textContent}
                                </div>
                                <div class="parameter">
                                    <span class="label">单次治疗时长:</span> ${document.getElementById('sessionDuration').textContent}
                                </div>
                            </div>

                            <div class="section">
                                <h2>治疗建议</h2>
                                <p>${document.getElementById('treatmentRecommendations').innerHTML}</p>
                            </div>

                            <div class="section">
                                <h2>预期治疗效果</h2>
                                <p>${document.getElementById('expectedOutcome').textContent}</p>
                            </div>

                            <div class="footer">
                                <p>该治疗方案由光疗临床路径智能规划系统AI自动生成，最终治疗方案需由医生确认。</p>
                                <p>© ${new Date().getFullYear()} 光疗临床路径智能规划系统</p>
                            </div>
                        </body>
                        </html>
                    `);
                    printWindow.document.close();
                    printWindow.print();
                }

                // 函数：导出治疗方案（模拟）
                function exportTreatmentPlan() {
                    showNotification('治疗方案已导出为PDF文件', 'success');
                }

                // 函数：保存治疗方案到系统
                function saveTreatmentPlan() {
                    showLoading();

                    // 模拟API调用延迟
                    setTimeout(() => {
                        hideLoading();
                        showNotification('治疗方案已成功保存至系统', 'success');
                    }, 1000);
                }

                // 事件监听
                document.getElementById('generatePlanBtn').addEventListener('click', generateTreatmentPlan);
                document.getElementById('resetFormBtn').addEventListener('click', resetForm);
                document.getElementById('printPlanBtn').addEventListener('click', printTreatmentPlan);
                document.getElementById('exportPlanBtn').addEventListener('click', exportTreatmentPlan);
                document.getElementById('savePlanBtn').addEventListener('click', saveTreatmentPlan);

                // 添加疾病类型改变时的联动效果
                document.getElementById('diseaseType').addEventListener('change', function() {
                    const diseaseType = this.value;
                    const severitySelect = document.getElementById('severityLevel');

                    // 清空当前选项
                    severitySelect.innerHTML = '<option value="">请选择</option>';

                    // 根据疾病类型动态添加不同的严重程度选项
                    if (diseaseType === 'psoriasis') {
                        severitySelect.innerHTML += `
                            <option value="mild">轻度 (PASI < 5)</option>
                            <option value="moderate">中度 (PASI 5-10)</option>
                            <option value="severe">重度 (PASI > 10)</option>
                            <option value="very-severe">极重度 (PASI > 20)</option>
                        `;
                    } else if (diseaseType === 'vitiligo') {
                        severitySelect.innerHTML += `
                            <option value="mild">局限型 (< 5% 体表面积)</option>
                            <option value="moderate">散发型 (5-20% 体表面积)</option>
                            <option value="severe">泛发型 (> 20% 体表面积)</option>
                        `;
                    } else if (diseaseType === 'dermatitis' || diseaseType === 'eczema') {
                        severitySelect.innerHTML += `
                            <option value="mild">轻度 (轻微红斑、瘙痒)</option>
                            <option value="moderate">中度 (明显红斑、脱屑、持续瘙痒)</option>
                            <option value="severe">重度 (严重红斑、渗出、剧烈瘙痒)</option>
                        `;
                    } else {
                        severitySelect.innerHTML += `
                            <option value="mild">轻度</option>
                            <option value="moderate">中度</option>
                            <option value="severe">重度</option>
                        `;
                    }
                });
            }
            """;



        public static string TreatmentMonitoring =
            """
            window.renderTREATMENTMONITORINGPage = function(container) {
                // Sample treatment data for demonstration
                const treatmentData = {
                    patientId: "P12345",
                    patientName: "张三",
                    treatmentPlan: {
                        intensity: 85, // 光疗强度 (%)
                        duration: 20, // 治疗时间 (分钟)
                        frequency: 3, // 治疗频率 (每周次数)
                        totalSessions: 12, // 总疗程次数
                        completedSessions: 5, // 已完成次数
                    },
                    currentSession: {
                        startTime: new Date(Date.now() - 1000 * 60 * 8), // 8分钟前开始
                        intensity: 83, // 当前光疗强度
                        duration: 20, // 计划治疗时间
                        elapsedTime: 8, // 已经过时间
                        status: "进行中"
                    },
                    vitalSigns: [
                        { time: new Date(Date.now() - 1000 * 60 * 8), intensity: 85, temp: 36.5 },
                        { time: new Date(Date.now() - 1000 * 60 * 6), intensity: 84, temp: 36.6 },
                        { time: new Date(Date.now() - 1000 * 60 * 4), intensity: 83, temp: 36.7 },
                        { time: new Date(Date.now() - 1000 * 60 * 2), intensity: 83, temp: 36.8 }
                    ],
                    alerts: [
                        { time: new Date(Date.now() - 1000 * 60 * 6), message: "光疗强度轻微下降", severity: "info" },
                        { time: new Date(Date.now() - 1000 * 60 * 3), message: "患者体温轻微升高", severity: "warning" }
                    ]
                };

                container.innerHTML = `
                    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                        <h1 class="text-2xl font-bold mb-6 text-primary">治疗监控与提醒功能</h1>

                        <div class="grid grid-cols-1 lg:grid-cols-3 gap-6 mb-6">
                            <!-- 患者信息卡片 -->
                            <div class="bg-white dark:bg-gray-700 rounded-lg shadow p-4">
                                <h2 class="text-lg font-semibold mb-4 text-gray-800 dark:text-white">患者信息</h2>
                                <div class="space-y-2">
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">患者ID:</span>
                                        <span class="font-medium">${treatmentData.patientId}</span>
                                    </div>
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">姓名:</span>
                                        <span class="font-medium">${treatmentData.patientName}</span>
                                    </div>
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">治疗进度:</span>
                                        <span class="font-medium">${treatmentData.treatmentPlan.completedSessions}/${treatmentData.treatmentPlan.totalSessions} 次</span>
                                    </div>
                                </div>
                            </div>

                            <!-- 当前会话信息 -->
                            <div class="bg-white dark:bg-gray-700 rounded-lg shadow p-4">
                                <h2 class="text-lg font-semibold mb-4 text-gray-800 dark:text-white">当前治疗会话</h2>
                                <div class="space-y-2">
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">开始时间:</span>
                                        <span class="font-medium">${treatmentData.currentSession.startTime.toLocaleTimeString()}</span>
                                    </div>
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">已进行时间:</span>
                                        <span class="font-medium" id="elapsedTime">${treatmentData.currentSession.elapsedTime} 分钟</span>
                                    </div>
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">状态:</span>
                                        <span class="font-medium text-green-500">${treatmentData.currentSession.status}</span>
                                    </div>
                                </div>
                            </div>

                            <!-- 治疗参数 -->
                            <div class="bg-white dark:bg-gray-700 rounded-lg shadow p-4">
                                <h2 class="text-lg font-semibold mb-4 text-gray-800 dark:text-white">治疗参数</h2>
                                <div class="space-y-2">
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">计划光疗强度:</span>
                                        <span class="font-medium">${treatmentData.treatmentPlan.intensity}%</span>
                                    </div>
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">当前光疗强度:</span>
                                        <span class="font-medium" id="currentIntensity">${treatmentData.currentSession.intensity}%</span>
                                    </div>
                                    <div class="flex justify-between">
                                        <span class="text-gray-600 dark:text-gray-300">计划治疗时间:</span>
                                        <span class="font-medium">${treatmentData.treatmentPlan.duration} 分钟</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- 实时监控仪表板 -->
                        <div id="function" class="bg-secondary dark:bg-gray-700 p-6 rounded-lg my-6">
                            <h2 class="text-xl font-semibold mb-4 text-gray-800 dark:text-white">光疗治疗进程实时监控</h2>

                            <!-- 进度条 -->
                            <div class="mb-6">
                                <div class="flex justify-between mb-1">
                                    <span class="text-gray-700 dark:text-gray-300">治疗进度</span>
                                    <span class="text-gray-700 dark:text-gray-300" id="progressPercent">${Math.round((treatmentData.currentSession.elapsedTime / treatmentData.treatmentPlan.duration) * 100)}%</span>
                                </div>
                                <div class="w-full bg-gray-200 rounded-full h-4 dark:bg-gray-600">
                                    <div id="progressBar" class="bg-primary h-4 rounded-full" style="width: ${Math.round((treatmentData.currentSession.elapsedTime / treatmentData.treatmentPlan.duration) * 100)}%"></div>
                                </div>
                            </div>

                            <!-- 参数监控图表 -->
                            <div class="bg-white dark:bg-gray-800 rounded-lg p-4 mb-6">
                                <h3 class="text-lg font-semibold mb-2 text-gray-800 dark:text-white">实时参数监控</h3>
                                <div id="monitoringChart" class="w-full h-64"></div>
                            </div>

                            <!-- 参数监控卡片 -->
                            <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
                                <div class="bg-white dark:bg-gray-800 rounded-lg p-4 shadow-sm">
                                    <h3 class="text-md font-semibold mb-2 text-gray-800 dark:text-white">光疗强度</h3>
                                    <div class="flex items-center">
                                        <div class="w-12 h-12 rounded-full flex items-center justify-center bg-blue-100 dark:bg-blue-900 mr-3">
                                            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-blue-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" />
                                            </svg>
                                        </div>
                                        <div>
                                            <p class="text-gray-500 dark:text-gray-400 text-sm">当前</p>
                                            <p class="text-2xl font-bold" id="intensityValue">${treatmentData.currentSession.intensity}%</p>
                                        </div>
                                    </div>
                                    <div class="mt-2">
                                        <div class="flex justify-between text-xs">
                                            <span>目标: ${treatmentData.treatmentPlan.intensity}%</span>
                                            <span class="text-yellow-500" id="intensityStatus">轻微偏低</span>
                                        </div>
                                    </div>
                                </div>

                                <div class="bg-white dark:bg-gray-800 rounded-lg p-4 shadow-sm">
                                    <h3 class="text-md font-semibold mb-2 text-gray-800 dark:text-white">治疗时间</h3>
                                    <div class="flex items-center">
                                        <div class="w-12 h-12 rounded-full flex items-center justify-center bg-green-100 dark:bg-green-900 mr-3">
                                            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-green-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" />
                                            </svg>
                                        </div>
                                        <div>
                                            <p class="text-gray-500 dark:text-gray-400 text-sm">已进行</p>
                                            <p class="text-2xl font-bold" id="durationValue">${treatmentData.currentSession.elapsedTime}分钟</p>
                                        </div>
                                    </div>
                                    <div class="mt-2">
                                        <div class="flex justify-between text-xs">
                                            <span>计划: ${treatmentData.treatmentPlan.duration}分钟</span>
                                            <span class="text-green-500" id="durationStatus">正常</span>
                                        </div>
                                    </div>
                                </div>

                                <div class="bg-white dark:bg-gray-800 rounded-lg p-4 shadow-sm">
                                    <h3 class="text-md font-semibold mb-2 text-gray-800 dark:text-white">患者体温</h3>
                                    <div class="flex items-center">
                                        <div class="w-12 h-12 rounded-full flex items-center justify-center bg-red-100 dark:bg-red-900 mr-3">
                                            <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-red-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                                            </svg>
                                        </div>
                                        <div>
                                            <p class="text-gray-500 dark:text-gray-400 text-sm">当前</p>
                                            <p class="text-2xl font-bold" id="tempValue">${treatmentData.vitalSigns[treatmentData.vitalSigns.length-1].temp}°C</p>
                                        </div>
                                    </div>
                                    <div class="mt-2">
                                        <div class="flex justify-between text-xs">
                                            <span>正常范围: 36.3-37.0°C</span>
                                            <span class="text-yellow-500" id="tempStatus">轻微升高</span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- 警报和提醒 -->
                            <div class="bg-white dark:bg-gray-800 rounded-lg p-4">
                                <h3 class="text-lg font-semibold mb-2 text-gray-800 dark:text-white">警报与提醒</h3>
                                <div id="alertsContainer" class="space-y-2">
                                    ${treatmentData.alerts.map(alert => `
                                        <div class="flex items-start p-3 rounded-md ${alert.severity === 'warning' ? 'bg-yellow-50 dark:bg-yellow-900 text-yellow-800 dark:text-yellow-100' : 'bg-blue-50 dark:bg-blue-900 text-blue-800 dark:text-blue-100'}">
                                            <div class="flex-shrink-0 mr-2">
                                                ${alert.severity === 'warning' ? `
                                                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-yellow-500" viewBox="0 0 20 20" fill="currentColor">
                                                        <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
                                                    </svg>
                                                ` : `
                                                    <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-blue-500" viewBox="0 0 20 20" fill="currentColor">
                                                        <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd" />
                                                    </svg>
                                                `}
                                            </div>
                                            <div>
                                                <p class="text-sm font-medium">${alert.message}</p>
                                                <p class="text-xs">${alert.time.toLocaleTimeString()}</p>
                                            </div>
                                        </div>
                                    `).join('')}
                                </div>
                            </div>
                        </div>

                        <!-- 操作按钮 -->
                        <div class="flex justify-end space-x-4">
                            <button id="pauseBtn" class="bg-yellow-500 hover:bg-yellow-600 text-white font-medium py-2 px-4 rounded-lg">
                                暂停治疗
                            </button>
                            <button id="stopBtn" class="bg-red-500 hover:bg-red-600 text-white font-medium py-2 px-4 rounded-lg">
                                终止治疗
                            </button>
                            <button id="noteBtn" class="bg-primary hover:bg-blue-600 text-white font-medium py-2 px-4 rounded-lg">
                                添加备注
                            </button>
                        </div>
                    </div>
                `;

                // 模拟实时数据更新
                let elapsedTime = treatmentData.currentSession.elapsedTime;
                let currentIntensity = treatmentData.currentSession.intensity;
                let currentTemp = treatmentData.vitalSigns[treatmentData.vitalSigns.length-1].temp;

                // 创建图表
                renderMonitoringChart(treatmentData.vitalSigns);

                // 定时更新模拟数据
                const updateInterval = setInterval(() => {
                    // 更新时间
                    elapsedTime += 1/60; // 增加1秒 (以分钟为单位)
                    document.getElementById('elapsedTime').textContent = elapsedTime.toFixed(1) + ' 分钟';

                    // 更新进度条
                    const progressPercent = Math.min(Math.round((elapsedTime / treatmentData.treatmentPlan.duration) * 100), 100);
                    document.getElementById('progressPercent').textContent = progressPercent + '%';
                    document.getElementById('progressBar').style.width = progressPercent + '%';
                    document.getElementById('durationValue').textContent = elapsedTime.toFixed(1) + '分钟';

                    // 随机波动光疗强度
                    const intensityChange = Math.random() > 0.7 ? (Math.random() > 0.5 ? 1 : -1) : 0;
                    currentIntensity = Math.max(80, Math.min(89, currentIntensity + intensityChange));
                    document.getElementById('intensityValue').textContent = currentIntensity + '%';
                    document.getElementById('currentIntensity').textContent = currentIntensity + '%';

                    // 更新状态提示
                    if (currentIntensity < treatmentData.treatmentPlan.intensity - 1) {
                        document.getElementById('intensityStatus').textContent = '轻微偏低';
                        document.getElementById('intensityStatus').className = 'text-yellow-500';
                    } else if (currentIntensity > treatmentData.treatmentPlan.intensity + 1) {
                        document.getElementById('intensityStatus').textContent = '轻微偏高';
                        document.getElementById('intensityStatus').className = 'text-yellow-500';
                    } else {
                        document.getElementById('intensityStatus').textContent = '正常';
                        document.getElementById('intensityStatus').className = 'text-green-500';
                    }

                    // 随机波动体温
                    const tempChange = Math.random() > 0.8 ? (Math.random() > 0.5 ? 0.1 : -0.1) : 0;
                    currentTemp = Math.max(36.3, Math.min(37.1, currentTemp + tempChange));
                    document.getElementById('tempValue').textContent = currentTemp.toFixed(1) + '°C';

                    // 更新体温状态
                    if (currentTemp > 36.8) {
                        document.getElementById('tempStatus').textContent = '轻微升高';
                        document.getElementById('tempStatus').className = 'text-yellow-500';
                    } else if (currentTemp < 36.4) {
                        document.getElementById('tempStatus').textContent = '轻微偏低';
                        document.getElementById('tempStatus').className = 'text-yellow-500';
                    } else {
                        document.getElementById('tempStatus').textContent = '正常';
                        document.getElementById('tempStatus').className = 'text-green-500';
                    }

                    // 添加新的数据点到图表
                    updateMonitoringChart({
                        time: new Date(),
                        intensity: currentIntensity,
                        temp: currentTemp
                    });

                    // 随机添加警报
                    if (Math.random() > 0.95) {
                        addAlert({
                            time: new Date(),
                            message: currentIntensity < treatmentData.treatmentPlan.intensity ? 
                                "光疗强度低于目标值" : 
                                (currentTemp > 36.8 ? "患者体温持续升高" : "光疗强度波动"),
                            severity: currentIntensity < treatmentData.treatmentPlan.intensity - 3 || currentTemp > 36.9 ? 
                                "warning" : "info"
                        });
                    }
                }, 1000);

                // 清除定时器，防止内存泄漏
                window.addEventListener('beforeunload', () => {
                    clearInterval(updateInterval);
                });

                // 处理按钮点击事件
                document.getElementById('pauseBtn').addEventListener('click', function() {
                    alert('治疗已暂停，请在确认后继续');
                });

                document.getElementById('stopBtn').addEventListener('click', function() {
                    if (confirm('确定要终止当前治疗吗？')) {
                        alert('治疗已终止');
                        clearInterval(updateInterval);
                    }
                });

                document.getElementById('noteBtn').addEventListener('click', function() {
                    const note = prompt('请输入治疗备注：');
                    if (note) {
                        addAlert({
                            time: new Date(),
                            message: "医生备注: " + note,
                            severity: "info"
                        });
                    }
                });

                function renderMonitoringChart(initialData) {
                    // 使用简单的Canvas API绘制图表，实际项目中可以替换为Chart.js等库
                    const canvas = document.createElement('canvas');
                    canvas.width = document.getElementById('monitoringChart').clientWidth;
                    canvas.height = document.getElementById('monitoringChart').clientHeight;
                    document.getElementById('monitoringChart').appendChild(canvas);

                    const ctx = canvas.getContext('2d');
                    const data = [...initialData];

                    // 绘制图表的函数
                    function drawChart() {
                        ctx.clearRect(0, 0, canvas.width, canvas.height);

                        const padding = 40;
                        const chartWidth = canvas.width - padding * 2;
                        const chartHeight = canvas.height - padding * 2;

                        // 绘制坐标轴
                        ctx.beginPath();
                        ctx.strokeStyle = '#ccc';
                        ctx.moveTo(padding, padding);
                        ctx.lineTo(padding, canvas.height - padding);
                        ctx.lineTo(canvas.width - padding, canvas.height - padding);
                        ctx.stroke();

                        // 绘制标签
                        ctx.font = '12px Arial';
                        ctx.fillStyle = '#666';
                        ctx.fillText('时间', canvas.width / 2, canvas.height - 10);
                        ctx.save();
                        ctx.translate(15, canvas.height / 2);
                        ctx.rotate(-Math.PI / 2);
                        ctx.fillText('参数值', 0, 0);
                        ctx.restore();

                        // 没有足够的数据点
                        if (data.length < 2) return;

                        // 绘制光疗强度线
                        ctx.beginPath();
                        ctx.strokeStyle = '#4a90e2';
                        ctx.lineWidth = 2;

                        const timeRange = data[data.length - 1].time - data[0].time;

                        for (let i = 0; i < data.length; i++) {
                            const x = padding + (data[i].time - data[0].time) / timeRange * chartWidth;
                            const y = canvas.height - padding - (data[i].intensity - 80) / 10 * chartHeight;

                            if (i === 0) {
                                ctx.moveTo(x, y);
                            } else {
                                ctx.lineTo(x, y);
                            }
                        }
                        ctx.stroke();

                        // 绘制体温线
                        ctx.beginPath();
                        ctx.strokeStyle = '#ff6b6b';
                        ctx.lineWidth = 2;

                        for (let i = 0; i < data.length; i++) {
                            const x = padding + (data[i].time - data[0].time) / timeRange * chartWidth;
                            const y = canvas.height - padding - (data[i].temp - 36) / 1.5 * chartHeight;

                            if (i === 0) {
                                ctx.moveTo(x, y);
                            } else {
                                ctx.lineTo(x, y);
                            }
                        }
                        ctx.stroke();

                        // 绘制图例
                        ctx.font = '12px Arial';
                        ctx.fillStyle = '#4a90e2';
                        ctx.fillRect(padding, 20, 15, 10);
                        ctx.fillStyle = '#333';
                        ctx.fillText('光疗强度', padding + 20, 28);

                        ctx.fillStyle = '#ff6b6b';
                        ctx.fillRect(padding + 100, 20, 15, 10);
                        ctx.fillStyle = '#333';
                        ctx.fillText('体温', padding + 120, 28);
                    }

                    // 首次绘制
                    drawChart();

                    // 返回更新图表的函数
                    window.updateMonitoringChart = function(newDataPoint) {
                        data.push(newDataPoint);
                        if (data.length > 10) data.shift(); // 保持最多10个数据点
                        drawChart();
                    };
                }

                // 添加警报
                function addAlert(alert) {
                    const alertsContainer = document.getElementById('alertsContainer');
                    const alertElement = document.createElement('div');
                    alertElement.className = `flex items-start p-3 rounded-md ${alert.severity === 'warning' ? 'bg-yellow-50 dark:bg-yellow-900 text-yellow-800 dark:text-yellow-100' : 'bg-blue-50 dark:bg-blue-900 text-blue-800 dark:text-blue-100'}`;

                    alertElement.innerHTML = `
                        <div class="flex-shrink-0 mr-2">
                            ${alert.severity === 'warning' ? `
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-yellow-500" viewBox="0 0 20 20" fill="currentColor">
                                    <path fill-rule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clip-rule="evenodd" />
                                </svg>
                            ` : `
                                <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 text-blue-500" viewBox="0 0 20 20" fill="currentColor">
                                    <path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd" />
                                </svg>
                            `}
                        </div>
                        <div>
                            <p class="text-sm font-medium">${alert.message}</p>
                            <p class="text-xs">${alert.time.toLocaleTimeString()}</p>
                        </div>
                    `;

                    alertsContainer.prepend(alertElement);

                    // 如果是警告，播放提示音
                    if (alert.severity === 'warning') {
                        playAlertSound();
                    }

                    // 限制显示的警报数量
                    if (alertsContainer.children.length > 5) {
                        alertsContainer.removeChild(alertsContainer.lastChild);
                    }
                }

                // 播放警报声音
                function playAlertSound() {
                    const audio = new Audio('data:audio/wav;base64,UklGRnoGAABXQVZFZm10IBAAAAABAAEAQB8AAEAfAAABAAgAZGF0YWoGAACBhYqFbF1fdJivrJBhNjVgodDbq2EcBj2q7/vIgjoFLIng8POwhDcDIW2wyuDMv6qCNwkYLn3a//3aqHg5DRIbU8n///nsy6J5NwgPHFvV//b18tioeUAZFyFl0ffVz9LZuZRfNRUULpj28uC9rqGQdzwQBweH9f/9yJuCdHl+bCkFDHPq//7RfxkHJ4zq+9OWPAgjptD/7q1PChyR6//1tVwMGJHr//ezWxAbiOv/9rpgDhiE7P/4v2IOFXnp//fFaA4Tc+f/98ttEBBp4//4z3cSDV7d//fVgRYKUtX/9tqMGghKzP/13JIcBkLI//Xflh0GP8L/9OGZHgU8u//z4pwhBDq1//LiniIDOLD/8OShJAI3r//v5aMmATWu/+7MgzAUN7T/7a1OGhtHu//t3J4iAje8/+zkqicAMbT/6+asKAAvr//q560qACup/+jmrywBK6j/6OewLAEooP/l57EtAiaa/+ToiDMPMah/9rA4AxOm//H2tTkCEaD/7/e3OgIQnP/u+Lk8AQ2a/+35uj0BC5j/6/m7PwEKlP/q+rxAAgiU/+n7vkECBpL/6Py/QgIFkP/n/MBDAgWP/+b9wUQCBI7/5v3CRQICjf/l/sNGAgGL/+X+xEcCAYj/4v7ERwIBhv/i/8VHAwGE/+H/xkcDAYP/4P/GSAMAg//g/8dJAwCC/9//x0oDAIH/3//HSgQAgP/eAMhLBAB//94AyEwEAH7/3QDJTQQAff/dAMlOBQB8/9wAyk8FAHv/3ADKUAUAev/bAMtRBQB5/9sAy1EGAHj/2gDMUgYAd//aAMxTBgB2/9kAzVQGAHX/2QDNVQcAdP/YAM5WBwBz/9cAzlYIAHL/1wDPVwgAcf/WAM9YCQBw/9YA0FkJAG//1QDQWQoAbv/VANFaCgBt/9QA0VsLAGz/1ADSWwsAa//TANJcDABq/9IA01wMAGn/0gDTXQ0AaP/RANReDQBn/9EA1F4OAGb/0ADVXw4AZf/QANVgDwBk/88A1mAPAGP/zwDWYRAAYv/OANdiEABh/84A12MRAGAm2hURAF8r2BR9RAFeNNsUP2wAXTndFEFgAFw/3xRETwBbQ+IVRkAAWkXmFUgxAFlI6RZJIwBYSu0WShcAV0zwF0wIAFZO8xdM+gBVUPYYTewAVFH6GU7fAFNT/RlP0gBSVQAaTsUAUVYDGk+5AEkPYyBp7AD/M5JJ6wIID2clA3dEgaeSC4v/oAoHgv+gCwaD/6EMBIb/oQ4Dh/+hDwKI/6IQAon/ohEBiv+iEQCL/6MSAI3/oxMAjf+jFACI/KIXAH/5JBkAeuo1GgDchWYfAL0o0SAABpX/pBQABZX/pBUABJb/pRYAA5b/pRcAApf/phgAAZf/phgAAJj/pxkA/5j/pxoA/pn/qBoA/Zn/qBsA/Jr/qRwA+5r/qR0A+pv/qh4A+Zv/qh4A+Jz/qh8A95z/qyAA9p3/qyEA9Z3/rCEA85v/qyEA9Z3/rCMA9J7/rSQA8p7/rSQA857/riUA8p//riYA8Z//ryYA8KD/sCcA76D/sCgA7qH/sCkA7aH/sSkA7KL/sioA6qP/sisA6aP/sisA6KT/tCwA56T/tC0A5qX/tS0A5aX/tS4A5Kb/ti8A46b/tjAA4qf/tzEA4Kj/tzEA4Kj/uDIA36n/uTMA3qn/uTQA3ar/ujUA3Kr/ujUA3Kv/uzYA26v/uzcA2az/vDgA2Kz/vDkA16z/vTgA2Kz/vTgA2K3/vjkA1q3/vjoA1a7/vzsA1K7/wDwA06//wD0A0q//wT4A0LD/wT8A0LD/wkAA0LD/w0EAz7H/w0IAz7H/xEMAzrL/xUQAzbL/xUUAy7P/xkYAyrT/x0cAybT/x0gAyLP/x0cAyLT/yEcAyLT/yUgAxrX/yUkAxbX/ykoAxLb/y0sAw7b/zEwAwrf/zU0Awbj/zU4AwLj/zk8Av7n/zlAAv7n/z1EAvbr/z1IAvrr/0FMAvLv/0VQAu7v/0VUAur3/0lcAuL3/01cAt77/01gAtb//1FkAtL//1VoAs8D/1VsAsb//1VoAsb//1lkAs8D/1VsAs8D/1lsAsb//1lsAsb//11wAsL//11wAsb//110ArLf93V4AqrX+3mEAqLP+32EAp7H+32IAprH+4GMAprH+4WQApbH+4WUApLD+4mYAo7D+4mcAo6//42gAo6//42kAoq//5GoAoq//5WsAoa7/5WwAoK7/5m0An67/5m4An63/528Anq3/6HAAnKz/6HEAm6z/6XIAmqv/6XMAmar/6nQAmav/6nUAmKv/63YAl6r/63cAl6r/7HgAlqn/7HkAlKj/7XoAk6j/7nsAk6f/7nwAkqf/738AkKX/8IAAj6X/8IEAj6X/8YIAjqT/8YMAjaT/8oQAjaP/8oUAjKP/84YAi6L/84cAi6L/9IcAi6L/9YgAiqH/9YkAiKD/9ooAiKD/9osAh5//9owAhp//940Ahp//+I4Ahp//+I8AhZ7/+JAAhZ7/+ZEAhJ3/+ZIAgpv/+pMAgJn/+pQAf5n/+5UA8nid/JYA7nSX/ZcA63GS/ZgA6XGN/pkA5m2H/poA5WuC/psA4maA/pwA4GR8/p0AGUlsanV8goeQm6exvczd7vgdAxEcJzE6QkpPU1VZXF9iZGZpbG5wcnR2eHp8fn+BgoSFh4iKi4yOj5GSk5WWmJmbnJ6foaKkpaipq6yur7Gys7W2uLm7vL7Jqg0GwXU9EcZ5Pwq8cjwMw3c+C75zPQvAdT0MyXxBC9OER8z/');
                    audio.play().catch(e => console.error('Failed to play alert sound:', e));
                }
            }
            """;

        public static string UserManagement =
            """
            window.renderUSERMANAGEMENTPage = function(container) {
                // Predefined role types and permissions
                const roles = [
                    { id: 1, name: '管理员', description: '系统管理员，拥有所有权限' },
                    { id: 2, name: '主治医师', description: '负责诊断和治疗方案制定' },
                    { id: 3, name: '住院医师', description: '协助主治医师进行治疗' },
                    { id: 4, name: '护士', description: '执行治疗和患者护理' },
                    { id: 5, name: '技师', description: '负责设备操作和维护' }
                ];

                const permissionCategories = [
                    { id: 'patient', name: '患者管理' },
                    { id: 'treatment', name: '治疗方案' },
                    { id: 'equipment', name: '设备管理' },
                    { id: 'reports', name: '报告查看' },
                    { id: 'system', name: '系统管理' }
                ];

                const permissions = [
                    { id: 'patient_view', name: '查看患者信息', category: 'patient' },
                    { id: 'patient_edit', name: '编辑患者信息', category: 'patient' },
                    { id: 'patient_add', name: '添加患者', category: 'patient' },
                    { id: 'patient_delete', name: '删除患者', category: 'patient' },

                    { id: 'treatment_view', name: '查看治疗方案', category: 'treatment' },
                    { id: 'treatment_edit', name: '编辑治疗方案', category: 'treatment' },
                    { id: 'treatment_approve', name: '审批治疗方案', category: 'treatment' },

                    { id: 'equipment_view', name: '查看设备', category: 'equipment' },
                    { id: 'equipment_operate', name: '操作设备', category: 'equipment' },
                    { id: 'equipment_maintain', name: '维护设备', category: 'equipment' },

                    { id: 'reports_view', name: '查看报告', category: 'reports' },
                    { id: 'reports_generate', name: '生成报告', category: 'reports' },
                    { id: 'reports_export', name: '导出报告', category: 'reports' },

                    { id: 'system_users', name: '用户管理', category: 'system' },
                    { id: 'system_roles', name: '角色管理', category: 'system' },
                    { id: 'system_logs', name: '日志查看', category: 'system' },
                    { id: 'system_settings', name: '系统设置', category: 'system' }
                ];

                // Default role permissions
                const rolePermissions = {
                    1: permissions.map(p => p.id), // Admin has all permissions
                    2: ['patient_view', 'patient_edit', 'patient_add', 'treatment_view', 'treatment_edit', 'treatment_approve', 'equipment_view', 'reports_view', 'reports_generate', 'reports_export'],
                    3: ['patient_view', 'patient_edit', 'treatment_view', 'treatment_edit', 'equipment_view', 'reports_view', 'reports_generate'],
                    4: ['patient_view', 'treatment_view', 'equipment_view', 'equipment_operate', 'reports_view'],
                    5: ['equipment_view', 'equipment_operate', 'equipment_maintain']
                };

                // Sample users
                const users = [
                    { id: 1, name: '张管理', role: 1, email: 'admin@example.com', department: '行政部' },
                    { id: 2, name: '李医生', role: 2, email: 'doctor.li@example.com', department: '皮肤科' },
                    { id: 3, name: '王医生', role: 3, email: 'doctor.wang@example.com', department: '皮肤科' },
                    { id: 4, name: '赵护士', role: 4, email: 'nurse.zhao@example.com', department: '光疗室' },
                    { id: 5, name: '刘技师', role: 5, email: 'technician.liu@example.com', department: '设备科' }
                ];

                container.innerHTML = `
                    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg p-6">
                        <h1 class="text-2xl font-bold mb-6 text-primary">用户管理与权限控制</h1>

                        <div class="flex mb-6">
                            <button id="tab-users" class="px-4 py-2 text-primary font-medium border-b-2 border-primary">用户管理</button>
                            <button id="tab-roles" class="px-4 py-2 text-gray-500 font-medium border-b-2 border-transparent hover:text-primary">角色管理</button>
                            <button id="tab-permissions" class="px-4 py-2 text-gray-500 font-medium border-b-2 border-transparent hover:text-primary">权限设置</button>
                            <button id="tab-logs" class="px-4 py-2 text-gray-500 font-medium border-b-2 border-transparent hover:text-primary">操作日志</button>
                        </div>

                        <div id="content-users" class="tab-content">
                            <div class="flex justify-between mb-4">
                                <h2 class="text-xl font-bold">用户列表</h2>
                                <button id="add-user-btn" class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition">添加用户</button>
                            </div>

                            <div class="overflow-x-auto">
                                <table class="min-w-full bg-white dark:bg-gray-700 rounded-lg overflow-hidden">
                                    <thead class="bg-gray-100 dark:bg-gray-600">
                                        <tr>
                                            <th class="py-3 px-4 text-left">ID</th>
                                            <th class="py-3 px-4 text-left">姓名</th>
                                            <th class="py-3 px-4 text-left">角色</th>
                                            <th class="py-3 px-4 text-left">邮箱</th>
                                            <th class="py-3 px-4 text-left">部门</th>
                                            <th class="py-3 px-4 text-left">操作</th>
                                        </tr>
                                    </thead>
                                    <tbody id="users-table-body">
                                        <!-- User rows will be populated dynamically -->
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <div id="content-roles" class="tab-content hidden">
                            <div class="flex justify-between mb-4">
                                <h2 class="text-xl font-bold">角色列表</h2>
                                <button id="add-role-btn" class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition">添加角色</button>
                            </div>

                            <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4" id="roles-container">
                                <!-- Role cards will be populated dynamically -->
                            </div>
                        </div>

                        <div id="content-permissions" class="tab-content hidden">
                            <h2 class="text-xl font-bold mb-4">权限设置</h2>

                            <div class="mb-4">
                                <label class="block text-gray-700 dark:text-gray-300 mb-2">选择角色：</label>
                                <select id="role-selector" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600">
                                    <!-- Role options will be populated dynamically -->
                                </select>
                            </div>

                            <div id="permissions-container" class="space-y-6">
                                <!-- Permission categories will be populated dynamically -->
                            </div>

                            <div class="mt-6 flex justify-end">
                                <button id="save-permissions-btn" class="bg-primary text-white px-6 py-2 rounded-lg hover:bg-blue-600 transition">保存权限设置</button>
                            </div>
                        </div>

                        <div id="content-logs" class="tab-content hidden">
                            <h2 class="text-xl font-bold mb-4">操作日志</h2>

                            <div class="mb-4 flex flex-wrap gap-4">
                                <div class="flex items-center">
                                    <label class="mr-2">用户：</label>
                                    <select id="log-user-filter" class="p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600">
                                        <option value="">全部用户</option>
                                        <!-- User options will be populated dynamically -->
                                    </select>
                                </div>

                                <div class="flex items-center">
                                    <label class="mr-2">操作类型：</label>
                                    <select id="log-action-filter" class="p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600">
                                        <option value="">全部操作</option>
                                        <option value="login">登录</option>
                                        <option value="logout">登出</option>
                                        <option value="create">创建</option>
                                        <option value="update">更新</option>
                                        <option value="delete">删除</option>
                                        <option value="view">查看</option>
                                    </select>
                                </div>

                                <div class="flex items-center">
                                    <label class="mr-2">日期范围：</label>
                                    <input type="date" id="log-date-start" class="p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600">
                                    <span class="mx-2">至</span>
                                    <input type="date" id="log-date-end" class="p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600">
                                </div>
                            </div>

                            <div class="overflow-x-auto">
                                <table class="min-w-full bg-white dark:bg-gray-700 rounded-lg overflow-hidden">
                                    <thead class="bg-gray-100 dark:bg-gray-600">
                                        <tr>
                                            <th class="py-3 px-4 text-left">ID</th>
                                            <th class="py-3 px-4 text-left">用户</th>
                                            <th class="py-3 px-4 text-left">操作类型</th>
                                            <th class="py-3 px-4 text-left">操作内容</th>
                                            <th class="py-3 px-4 text-left">IP地址</th>
                                            <th class="py-3 px-4 text-left">时间</th>
                                        </tr>
                                    </thead>
                                    <tbody id="logs-table-body">
                                        <!-- Log rows will be populated dynamically -->
                                    </tbody>
                                </table>
                            </div>

                            <div class="mt-4 flex justify-between items-center">
                                <div>
                                    <span>总计: <span id="total-logs">0</span> 条记录</span>
                                </div>
                                <div class="flex space-x-2">
                                    <button id="prev-page" class="px-3 py-1 border rounded-md disabled:opacity-50">上一页</button>
                                    <span class="px-3 py-1">第 <span id="current-page">1</span> 页</span>
                                    <button id="next-page" class="px-3 py-1 border rounded-md disabled:opacity-50">下一页</button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Modal for adding/editing users -->
                    <div id="user-modal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center hidden z-50">
                        <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md">
                            <h2 id="user-modal-title" class="text-xl font-bold mb-4">添加用户</h2>

                            <form id="user-form" class="space-y-4">
                                <input type="hidden" id="user-id">

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">姓名</label>
                                    <input type="text" id="user-name" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" required>
                                </div>

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">角色</label>
                                    <select id="user-role" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" required>
                                        <!-- Role options will be populated dynamically -->
                                    </select>
                                </div>

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">邮箱</label>
                                    <input type="email" id="user-email" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" required>
                                </div>

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">部门</label>
                                    <input type="text" id="user-department" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" required>
                                </div>

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">密码</label>
                                    <input type="password" id="user-password" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" required>
                                </div>

                                <div class="flex justify-end space-x-3 pt-4">
                                    <button type="button" id="close-user-modal" class="px-4 py-2 border rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition">取消</button>
                                    <button type="submit" class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition">保存</button>
                                </div>
                            </form>
                        </div>
                    </div>

                    <!-- Modal for adding/editing roles -->
                    <div id="role-modal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center hidden z-50">
                        <div class="bg-white dark:bg-gray-800 p-6 rounded-lg shadow-lg w-full max-w-md">
                            <h2 id="role-modal-title" class="text-xl font-bold mb-4">添加角色</h2>

                            <form id="role-form" class="space-y-4">
                                <input type="hidden" id="role-id">

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">角色名称</label>
                                    <input type="text" id="role-name" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" required>
                                </div>

                                <div>
                                    <label class="block text-gray-700 dark:text-gray-300 mb-1">描述</label>
                                    <textarea id="role-description" class="w-full p-2 border rounded-lg dark:bg-gray-700 dark:border-gray-600" rows="3" required></textarea>
                                </div>

                                <div class="flex justify-end space-x-3 pt-4">
                                    <button type="button" id="close-role-modal" class="px-4 py-2 border rounded-lg hover:bg-gray-100 dark:hover:bg-gray-700 transition">取消</button>
                                    <button type="submit" class="bg-primary text-white px-4 py-2 rounded-lg hover:bg-blue-600 transition">保存</button>
                                </div>
                            </form>
                        </div>
                    </div>
                `;

                // Sample logs
                const logs = [
                    { id: 1, userId: 1, userName: '张管理', action: 'login', content: '登录系统', ip: '192.168.1.100', time: '2023-08-15 08:30:45' },
                    { id: 2, userId: 2, userName: '李医生', action: 'view', content: '查看患者#12345信息', ip: '192.168.1.101', time: '2023-08-15 09:15:22' },
                    { id: 3, userId: 2, userName: '李医生', action: 'update', content: '更新患者#12345治疗方案', ip: '192.168.1.101', time: '2023-08-15 09:20:15' },
                    { id: 4, userId: 4, userName: '赵护士', action: 'view', content: '查看患者#12345治疗记录', ip: '192.168.1.102', time: '2023-08-15 10:05:30' },
                    { id: 5, userId: 1, userName: '张管理', action: 'create', content: '创建新用户账号：陈医生', ip: '192.168.1.100', time: '2023-08-15 11:45:18' },
                    { id: 6, userId: 3, userName: '王医生', action: 'login', content: '登录系统', ip: '192.168.1.103', time: '2023-08-15 13:10:25' },
                    { id: 7, userId: 3, userName: '王医生', action: 'view', content: '查看患者#54321信息', ip: '192.168.1.103', time: '2023-08-15 13:15:40' },
                    { id: 8, userId: 5, userName: '刘技师', action: 'update', content: '更新设备#E001维护记录', ip: '192.168.1.104', time: '2023-08-15 14:30:12' },
                    { id: 9, userId: 2, userName: '李医生', action: 'logout', content: '登出系统', ip: '192.168.1.101', time: '2023-08-15 17:45:55' },
                    { id: 10, userId: 1, userName: '张管理', action: 'delete', content: '删除过期治疗记录', ip: '192.168.1.100', time: '2023-08-15 18:20:30' }
                ];

                // Initialize tabs
                const tabs = ['users', 'roles', 'permissions', 'logs'];
                tabs.forEach(tab => {
                    const tabButton = document.getElementById(`tab-${tab}`);
                    tabButton.addEventListener('click', () => {
                        // Hide all tab contents
                        tabs.forEach(t => {
                            document.getElementById(`content-${t}`).classList.add('hidden');
                            document.getElementById(`tab-${t}`).classList.remove('border-primary', 'text-primary');
                            document.getElementById(`tab-${t}`).classList.add('border-transparent', 'text-gray-500');
                        });

                        // Show selected tab content
                        document.getElementById(`content-${tab}`).classList.remove('hidden');
                        document.getElementById(`tab-${tab}`).classList.add('border-primary', 'text-primary');
                        document.getElementById(`tab-${tab}`).classList.remove('border-transparent', 'text-gray-500');
                    });
                });

                // Populate users table
                const populateUsersTable = () => {
                    const tableBody = document.getElementById('users-table-body');
                    tableBody.innerHTML = '';

                    users.forEach(user => {
                        const role = roles.find(r => r.id === user.role);
                        const row = document.createElement('tr');
                        row.className = 'border-t dark:border-gray-600';
                        row.innerHTML = `
                            <td class="py-3 px-4">${user.id}</td>
                            <td class="py-3 px-4">${user.name}</td>
                            <td class="py-3 px-4">${role ? role.name : '未知'}</td>
                            <td class="py-3 px-4">${user.email}</td>
                            <td class="py-3 px-4">${user.department}</td>
                            <td class="py-3 px-4">
                                <button class="edit-user-btn text-blue-500 hover:text-blue-700 mr-2" data-id="${user.id}">编辑</button>
                                <button class="delete-user-btn text-red-500 hover:text-red-700" data-id="${user.id}">删除</button>
                            </td>
                        `;
                        tableBody.appendChild(row);
                    });

                    // Add event listeners to edit and delete buttons
                    document.querySelectorAll('.edit-user-btn').forEach(btn => {
                        btn.addEventListener('click', () => {
                            const userId = parseInt(btn.getAttribute('data-id'));
                            openUserModal(userId);
                        });
                    });

                    document.querySelectorAll('.delete-user-btn').forEach(btn => {
                        btn.addEventListener('click', () => {
                            const userId = parseInt(btn.getAttribute('data-id'));
                            if (confirm('确定要删除此用户吗？')) {
                                // In a real application, you would make an API call here
                                const index = users.findIndex(u => u.id === userId);
                                if (index !== -1) {
                                    users.splice(index, 1);
                                    populateUsersTable();
                                }
                            }
                        });
                    });
                };

                // Populate roles container
                const populateRolesContainer = () => {
                    const rolesContainer = document.getElementById('roles-container');
                    rolesContainer.innerHTML = '';

                    roles.forEach(role => {
                        const card = document.createElement('div');
                        card.className = 'bg-white dark:bg-gray-700 p-4 rounded-lg shadow';

                        const permCount = rolePermissions[role.id] ? rolePermissions[role.id].length : 0;

                        card.innerHTML = `
                            <div class="flex justify-between items-start">
                                <h3 class="text-lg font-bold">${role.name}</h3>
                                <div class="flex space-x-2">
                                    <button class="edit-role-btn text-blue-500 hover:text-blue-700" data-id="${role.id}">
                                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                            <path d="M13.586 3.586a2 2 0 112.828 2.828l-.793.793-2.828-2.828.793-.793zM11.379 5.793L3 14.172V17h2.828l8.38-8.379-2.83-2.828z" />
                                        </svg>
                                    </button>
                                    <button class="delete-role-btn text-red-500 hover:text-red-700" data-id="${role.id}">
                                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                            <path fill-rule="evenodd" d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z" clip-rule="evenodd" />
                                        </svg>
                                    </button>
                                </div>
                            </div>
                            <p class="text-gray-600 dark:text-gray-300 mt-2">${role.description}</p>
                            <div class="mt-4 flex justify-between items-center">
                                <span class="text-sm text-gray-500 dark:text-gray-400">拥有 ${permCount} 项权限</span>
                                <button class="view-permissions-btn text-primary hover:text-blue-700 text-sm" data-id="${role.id}">查看权限 &rarr;</button>
                            </div>
                        `;

                        rolesContainer.appendChild(card);
                    });

                    // Add event listeners to role card buttons
                    document.querySelectorAll('.edit-role-btn').forEach(btn => {
                        btn.addEventListener('click', () => {
                            const roleId = parseInt(btn.getAttribute('data-id'));
                            openRoleModal(roleId);
                        });
                    });

                    document.querySelectorAll('.delete-role-btn').forEach(btn => {
                        btn.addEventListener('click', () => {
                            const roleId = parseInt(btn.getAttribute('data-id'));
                            if (confirm('确定要删除此角色吗？注意：这将影响所有使用此角色的用户。')) {
                                // In a real application, you would make an API call here
                                const index = roles.findIndex(r => r.id === roleId);
                                if (index !== -1) {
                                    // Check if there are users with this role
                                    const usersWithRole = users.filter(u => u.role === roleId);
                                    if (usersWithRole.length > 0) {
                                        alert(`无法删除此角色，因为有 ${usersWithRole.length} 个用户正在使用它。`);
                                    } else {
                                        roles.splice(index, 1);
                                        delete rolePermissions[roleId];
                                        populateRolesContainer();
                                    }
                                }
                            }
                        });
                    });

                    document.querySelectorAll('.view-permissions-btn').forEach(btn => {
                        btn.addEventListener('click', () => {
                            const roleId = parseInt(btn.getAttribute('data-id'));
                            // Switch to permissions tab and select this role
                            document.getElementById('tab-permissions').click();
                            document.getElementById('role-selector').value = roleId;
                            populatePermissions(roleId);
                        });
                    });
                };

                // Populate role selector for permissions tab
                const populateRoleSelector = () => {
                    const roleSelector = document.getElementById('role-selector');
                    roleSelector.innerHTML = '';

                    roles.forEach(role => {
                        const option = document.createElement('option');
                        option.value = role.id;
                        option.textContent = role.name;
                        roleSelector.appendChild(option);
                    });

                    // Initial population of permissions for the first role
                    if (roles.length > 0) {
                        populatePermissions(roles[0].id);
                    }

                    roleSelector.addEventListener('change', () => {
                        const roleId = parseInt(roleSelector.value);
                        populatePermissions(roleId);
                    });
                };

                // Populate permissions for selected role
                const populatePermissions = (roleId) => {
                    const permissionsContainer = document.getElementById('permissions-container');
                    permissionsContainer.innerHTML = '';

                    // Get the selected role's permissions
                    const rolePerms = rolePermissions[roleId] || [];

                    // Group permissions by category
                    permissionCategories.forEach(category => {
                        const categoryPerms = permissions.filter(p => p.category === category.id);

                        const categoryDiv = document.createElement('div');
                        categoryDiv.className = 'border dark:border-gray-600 rounded-lg p-4';

                        categoryDiv.innerHTML = `
                            <h3 class="font-bold text-lg mb-3">${category.name}</h3>
                            <div class="space-y-2 permission-group" data-category="${category.id}">
                                ${categoryPerms.map(perm => `
                                    <div class="flex items-center">
                                        <input type="checkbox" id="perm-${perm.id}" class="permission-checkbox mr-2" 
                                               data-perm-id="${perm.id}" ${rolePerms.includes(perm.id) ? 'checked' : ''}>
                                        <label for="perm-${perm.id}">${perm.name}</label>
                                    </div>
                                `).join('')}
                            </div>
                        `;

                        permissionsContainer.appendChild(categoryDiv);
                    });
                };

                // Populate logs table
                const populateLogsTable = () => {
                    const tableBody = document.getElementById('logs-table-body');
                    tableBody.innerHTML = '';

                    // Apply filters (in a real app, this would be done server-side)
                    let filteredLogs = [...logs];

                    const userFilter = document.getElementById('log-user-filter').value;
                    if (userFilter) {
                        filteredLogs = filteredLogs.filter(log => log.userId === parseInt(userFilter));
                    }

                    const actionFilter = document.getElementById('log-action-filter').value;
                    if (actionFilter) {
                        filteredLogs = filteredLogs.filter(log => log.action === actionFilter);
                    }

                    const startDate = document.getElementById('log-date-start').value;
                    if (startDate) {
                        const startTimestamp = new Date(startDate).getTime();
                        filteredLogs = filteredLogs.filter(log => {
                            const logDate = new Date(log.time.split(' ')[0]).getTime();
                            return logDate >= startTimestamp;
                        });
                    }

                    const endDate = document.getElementById('log-date-end').value;
                    if (endDate) {
                        const endTimestamp = new Date(endDate).getTime() + 86400000; // Add one day to include the end date
                        filteredLogs = filteredLogs.filter(log => {
                            const logDate = new Date(log.time.split(' ')[0]).getTime();
                            return logDate < endTimestamp;
                        });
                    }

                    // In a real app, you would implement pagination
                    document.getElementById('total-logs').textContent = filteredLogs.length;

                    // For simplicity, we'll just show all logs without pagination
                    filteredLogs.forEach(log => {
                        const row = document.createElement('tr');
                        row.className = 'border-t dark:border-gray-600';

                        let actionClass = '';
                        switch (log.action) {
                            case 'login':
                            case 'logout':
                                actionClass = 'text-blue-500';
                                break;
                            case 'create':
                                actionClass = 'text-green-500';
                                break;
                            case 'update':
                                actionClass = 'text-yellow-500';
                                break;
                            case 'delete':
                                actionClass = 'text-red-500';
                                break;
                            case 'view':
                                actionClass = 'text-purple-500';
                                break;
                        }

                        row.innerHTML = `
                            <td class="py-3 px-4">${log.id}</td>
                            <td class="py-3 px-4">${log.userName}</td>
                            <td class="py-3 px-4"><span class="${actionClass}">${getActionName(log.action)}</span></td>
                            <td class="py-3 px-4">${log.content}</td>
                            <td class="py-3 px-4">${log.ip}</td>
                            <td class="py-3 px-4">${log.time}</td>
                        `;

                        tableBody.appendChild(row);
                    });
                };

                // Helper function to get action name
                const getActionName = (action) => {
                    const actionMap = {
                        'login': '登录',
                        'logout': '登出',
                        'create': '创建',
                        'update': '更新',
                        'delete': '删除',
                        'view': '查看'
                    };
                    return actionMap[action] || action;
                };

                // Populate user selector for logs tab
                const populateLogUserFilter = () => {
                    const userFilter = document.getElementById('log-user-filter');
                    userFilter.innerHTML = '<option value="">全部用户</option>';

                    users.forEach(user => {
                        const option = document.createElement('option');
                        option.value = user.id;
                        option.textContent = user.name;
                        userFilter.appendChild(option);
                    });

                    // Add event listeners for log filters
                    userFilter.addEventListener('change', populateLogsTable);
                    document.getElementById('log-action-filter').addEventListener('change', populateLogsTable);
                    document.getElementById('log-date-start').addEventListener('change', populateLogsTable);
                    document.getElementById('log-date-end').addEventListener('change', populateLogsTable);
                };

                // Open user modal for adding or editing
                const openUserModal = (userId = null) => {
                    const modal = document.getElementById('user-modal');
                    const modalTitle = document.getElementById('user-modal-title');
                    const form = document.getElementById('user-form');
                    const idInput = document.getElementById('user-id');
                    const nameInput = document.getElementById('user-name');
                    const roleInput = document.getElementById('user-role');
                    const emailInput = document.getElementById('user-email');
                    const departmentInput = document.getElementById('user-department');
                    const passwordInput = document.getElementById('user-password');

                    // Clear the form
                    form.reset();

                    // Populate role dropdown
                    roleInput.innerHTML = '';
                    roles.forEach(role => {
                        const option = document.createElement('option');
                        option.value = role.id;
                        option.textContent = role.name;
                        roleInput.appendChild(option);
                    });

                    if (userId) {
                        // Edit existing user
                        modalTitle.textContent = '编辑用户';
                        const user = users.find(u => u.id === userId);
                        if (user) {
                            idInput.value = user.id;
                            nameInput.value = user.name;
                            roleInput.value = user.role;
                            emailInput.value = user.email;
                            departmentInput.value = user.department;
                            passwordInput.value = '********'; // Placeholder for password
                            passwordInput.required = false; // Password not required for edit
                        }
                    } else {
                        // Add new user
                        modalTitle.textContent = '添加用户';
                        idInput.value = '';
                        passwordInput.required = true;
                    }

                    modal.classList.remove('hidden');
                };

                // Open role modal for adding or editing
                const openRoleModal = (roleId = null) => {
                    const modal = document.getElementById('role-modal');
                    const modalTitle = document.getElementById('role-modal-title');
                    const form = document.getElementById('role-form');
                    const idInput = document.getElementById('role-id');
                    const nameInput = document.getElementById('role-name');
                    const descriptionInput = document.getElementById('role-description');

                    // Clear the form
                    form.reset();

                    if (roleId) {
                        // Edit existing role
                        modalTitle.textContent = '编辑角色';
                        const role = roles.find(r => r.id === roleId);
                        if (role) {
                            idInput.value = role.id;
                            nameInput.value = role.name;
                            descriptionInput.value = role.description;
                        }
                    } else {
                        // Add new role
                        modalTitle.textContent = '添加角色';
                        idInput.value = '';
                    }

                    modal.classList.remove('hidden');
                };

                // Initialize page
                const initialize = () => {
                    // Populate data
                    populateUsersTable();
                    populateRolesContainer();
                    populateRoleSelector();
                    populateLogsTable();
                    populateLogUserFilter();

                    // Set up user modal events
                    document.getElementById('add-user-btn').addEventListener('click', () => openUserModal());
                    document.getElementById('close-user-modal').addEventListener('click', () => {
                        document.getElementById('user-modal').classList.add('hidden');
                    });

                    document.getElementById('user-form').addEventListener('submit', (e) => {
                        e.preventDefault();

                        const userId = document.getElementById('user-id').value;
                        const userData = {
                            name: document.getElementById('user-name').value,
                            role: parseInt(document.getElementById('user-role').value),
                            email: document.getElementById('user-email').value,
                            department: document.getElementById('user-department').value
                        };

                        if (userId) {
                            // Update existing user
                            const index = users.findIndex(u => u.id === parseInt(userId));
                            if (index !== -1) {
                                users[index] = { ...users[index], ...userData };
                            }
                        } else {
                            // Add new user
                            const newId = users.length > 0 ? Math.max(...users.map(u => u.id)) + 1 : 1;
                            users.push({
                                id: newId,
                                ...userData,
                                // In a real app, you would hash the password
                                password: document.getElementById('user-password').value
                            });
                        }

                        // Update UI and close modal
                        populateUsersTable();
                        populateLogUserFilter();
                        document.getElementById('user-modal').classList.add('hidden');
                    });

                    // Set up role modal events
                    document.getElementById('add-role-btn').addEventListener('click', () => openRoleModal());
                    document.getElementById('close-role-modal').addEventListener('click', () => {
                        document.getElementById('role-modal').classList.add('hidden');
                    });

                    document.getElementById('role-form').addEventListener('submit', (e) => {
                        e.preventDefault();

                        const roleId = document.getElementById('role-id').value;
                        const roleData = {
                            name: document.getElementById('role-name').value,
                            description: document.getElementById('role-description').value
                        };

                        if (roleId) {
                            // Update existing role
                            const index = roles.findIndex(r => r.id === parseInt(roleId));
                            if (index !== -1) {
                                roles[index] = { ...roles[index], ...roleData };
                            }
                        } else {
                            // Add new role
                            const newId = roles.length > 0 ? Math.max(...roles.map(r => r.id)) + 1 : 1;
                            roles.push({
                                id: newId,
                                ...roleData
                            });
                            // Initialize empty permissions for new role
                            rolePermissions[newId] = [];
                        }

                        // Update UI and close modal
                        populateRolesContainer();
                        populateRoleSelector();
                        document.getElementById('role-modal').classList.add('hidden');
                    });

                    // Save permissions button
                    document.getElementById('save-permissions-btn').addEventListener('click', () => {
                        const roleId = parseInt(document.getElementById('role-selector').value);
                        const checkedPermissions = [];

                        document.querySelectorAll('.permission-checkbox:checked').forEach(checkbox => {
                            checkedPermissions.push(checkbox.getAttribute('data-perm-id'));
                        });

                        // Update role permissions
                        rolePermissions[roleId] = checkedPermissions;

                        // Show success message
                        alert('权限设置已保存');

                        // Update roles container to reflect new permission counts
                        populateRolesContainer();
                    });

                    // Pagination buttons (for demo only, not functional)
                    document.getElementById('prev-page').addEventListener('click', () => {
                        alert('在实际应用中，这将加载上一页的日志记录');
                    });

                    document.getElementById('next-page').addEventListener('click', () => {
                        alert('在实际应用中，这将加载下一页的日志记录');
                    });
                };

                // Initialize the page
                initialize();
            }
            """;
    }
}
