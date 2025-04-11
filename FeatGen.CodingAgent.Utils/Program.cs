using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;



string code = """

        
    "use client";

    import React, { useState, useEffect, useCallback, useMemo } from 'react';
    import {
      getAnalysisDataSources,
      getFilterOptions,
      runDataAnalysis,
      // saveChartTemplate, // Not implemented in UI for brevity
      // getChartTemplates, // Not implemented in UI for brevity
      generateAndSaveReport,
      getReportList,
      getReportDetails,
      updateReport,
      deleteReport,
      getReportDownloadLink
    } from '@/app/apis/data-analysis-report';

    import { Bar, Line, Pie } from 'react-chartjs-2';
    import {
      Chart as ChartJS,
      CategoryScale,
      LinearScale,
      BarElement,
      LineElement,
      PointElement,
      ArcElement,
      Title,
      Tooltip,
      Legend,
      Filler,
    } from 'chart.js';

    import {
      Tabs,
      TabsContent,
      TabsList,
      TabsTrigger,
    } from "@/components/ui/tabs";
    import {
      Select,
      SelectContent,
      SelectItem,
      SelectTrigger,
      SelectValue,
    } from "@/components/ui/select";
    import { Button } from "@/components/ui/button";
    import { Input } from "@/components/ui/input";
    import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card";
    import { Checkbox } from "@/components/ui/checkbox";
    import { Label } from "@/components/ui/label";
    import {
      Table,
      TableBody,
      TableCell,
      TableHead,
      TableHeader,
      TableRow,
    } from "@/components/ui/table";
    import {
      Pagination,
      PaginationContent,
      PaginationEllipsis,
      PaginationItem,
      PaginationLink,
      PaginationNext,
      PaginationPrevious,
    } from "@/components/ui/pagination"
    import {
      AlertDialog,
      AlertDialogAction,
      AlertDialogCancel,
      AlertDialogContent,
      AlertDialogDescription,
      AlertDialogFooter,
      AlertDialogHeader,
      AlertDialogTitle,
      AlertDialogTrigger,
    } from "@/components/ui/alert-dialog"
    import { useToast } from "@/components/ui/use-toast"
    import { Toaster } from "@/components/ui/toaster"


    import { ListFilter, BarChart, LineChart, PieChart, FileDown, TableIcon, Play, Trash2, Edit, Download, RefreshCw, Database, Filter, FileText, Settings, BarChart3, Workflow } from 'lucide-react';

    ChartJS.register(
      CategoryScale,
      LinearScale,
      BarElement,
      LineElement,
      PointElement,
      ArcElement,
      Title,
      Tooltip,
      Legend,
      Filler
    );

    // --- Helper Components ---

    // Filter Panel Component
    const FilterPanel = ({ dataSourceId, onFilterChange }) => {
      const [filterOptions, setFilterOptions] = useState(null);
      const [appliedFilters, setAppliedFilters] = useState({});
      const [isLoading, setIsLoading] = useState(false);
      const { toast } = useToast();

      useEffect(() => {
        if (dataSourceId) {
          setIsLoading(true);
          setFilterOptions(null); // Clear previous options
          setAppliedFilters({}); // Clear applied filters when source changes
          getFilterOptions(dataSourceId)
            .then(options => {
              setFilterOptions(options);
              // Initialize appliedFilters with empty arrays for multi-select options
              const initialFilters = {};
              Object.keys(options).forEach(key => {
                if (Array.isArray(options[key])) {
                  initialFilters[key] = [];
                } else {
                  initialFilters[key] = ''; // Initialize single select/input
                }
              });
              setAppliedFilters(initialFilters);
              onFilterChange(initialFilters); // Notify parent of initial empty filters
            })
            .catch(error => {
              console.error("Error fetching filter options:", error);
              toast({ variant: "destructive", title: "错误", description: "加载筛选条件失败。" });
              setFilterOptions({}); // Set empty object to prevent errors
            })
            .finally(() => setIsLoading(false));
        } else {
          setFilterOptions(null); // Clear options if no data source selected
          setAppliedFilters({});
          onFilterChange({}); // Notify parent
        }
      }, [dataSourceId, toast, onFilterChange]);

      const handleMultiSelectChange = (filterKey, value) => {
        const currentSelection = appliedFilters[filterKey] || [];
        const newSelection = currentSelection.includes(value)
          ? currentSelection.filter(item => item !== value)
          : [...currentSelection, value];

        const newFilters = { ...appliedFilters, [filterKey]: newSelection };
        setAppliedFilters(newFilters);
        onFilterChange(newFilters);
      };

      const handleInputChange = (filterKey, value) => {
        const newFilters = { ...appliedFilters, [filterKey]: value };
        setAppliedFilters(newFilters);
        onFilterChange(newFilters);
      };


      const renderFilterControl = (key, options) => {
        const ALL_YEARS_VALUE = "__ALL__"; // Constant for the "All Years" value

        if (key === 'years') {
           return (
            <div key={key} className="space-y-2">
              <Label className="font-medium text-sm text-primary">{key === 'years' ? '年份' : key}</Label>
              <Select
                // If the current filter value is empty (meaning all years), set Select value to our special constant
                // Otherwise, convert the number year to string for the Select value
                value={appliedFilters[key] === '' || appliedFilters[key] === null || appliedFilters[key] === undefined ? ALL_YEARS_VALUE : String(appliedFilters[key])}
                // When the value changes, if it's our special constant, set the actual filter value to empty string
                // Otherwise, parse the selected string year back to an integer
                onValueChange={(value) => handleInputChange(key, value === ALL_YEARS_VALUE ? '' : (value ? parseInt(value) : ''))}
              >
                <SelectTrigger className="w-full">
                  <SelectValue placeholder={`选择${key === 'years' ? '年份' : key}...`} />
                </SelectTrigger>
                <SelectContent>
                  {/* Use the special constant as the value for "All Years" */}
                  <SelectItem value={ALL_YEARS_VALUE}>所有年份</SelectItem>
                  {options.map(option => (
                    <SelectItem key={option} value={String(option)}>{option}</SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          );
        } else if (Array.isArray(options)) {
          // Render checkboxes for arrays (multi-select)
          const labelMapping = {
            regions: '区域',
            insuranceTypes: '医保类型',
            hospitalLevels: '医院等级',
            genders: '性别',
            drugTypes: '药品类型',
            dosageForms: '剂型',
            diseaseTypes: '疾病类型',
            departments: '科室',
          };
          return (
            <div key={key} className="space-y-2">
              <Label className="font-medium text-sm text-primary">{labelMapping[key] || key}</Label>
              <div className="max-h-40 overflow-y-auto space-y-1 pr-2">
                {options.map(option => (
                  <div key={option} className="flex items-center space-x-2">
                    <Checkbox
                      id={`${key}-${option}`}
                      checked={(appliedFilters[key] || []).includes(option)}
                      onCheckedChange={() => handleMultiSelectChange(key, option)}
                    />
                    <Label htmlFor={`${key}-${option}`} className="text-sm font-normal cursor-pointer">{option}</Label>
                  </div>
                ))}
              </div>
            </div>
          );
        }
        // Add more specific controls if needed (e.g., date range)
        return null; // Or a default input if structure varies widely
      };

      if (isLoading) {
        return <div className="p-4 text-center text-muted-foreground"><RefreshCw className="h-4 w-4 animate-spin mr-2 inline" /> 加载筛选条件...</div>;
      }

      if (!filterOptions || Object.keys(filterOptions).length === 0) {
        return <div className="p-4 text-center text-muted-foreground text-sm">{dataSourceId ? "该数据源无可用筛选条件。" : "请先选择数据源。"}</div>;
      }

      return (
        <div className="p-4 space-y-4 border-t">
          <h3 className="text-md font-semibold flex items-center text-primary"><Filter className="h-4 w-4 mr-2" /> 数据筛选</h3>
          {Object.entries(filterOptions).map(([key, options]) => renderFilterControl(key, options))}
        </div>
      );
    };

    // Analysis Results Display Component
    const AnalysisResultsDisplay = ({ results, onGenerateReport, isLoadingReport }) => {
       const [displayType, setDisplayType] = useState('table'); // 'table', 'bar', 'line', 'pie'
       const [reportFormat, setReportFormat] = useState("PDF"); // Add state for report format
       const { toast } = useToast();

       const chartData = useMemo(() => {
        if (!results || !results.data || results.data.length === 0 || !results.columns) {
          return null;
        }

        const labels = results.data.map(item => item[results.columns[0]?.key]); // Assuming first column is label
        const dataKey = results.columns.find(col => col.key !== results.columns[0]?.key)?.key; // Find the first data column
        if (!dataKey) return null;
        const dataValues = results.data.map(item => item[dataKey]);

        const backgroundColors = [
          'rgba(0, 102, 204, 0.6)',
          'rgba(74, 173, 224, 0.6)',
          'rgba(0, 204, 153, 0.6)',
          'rgba(255, 126, 71, 0.6)',
          'rgba(245, 158, 11, 0.6)',
          'rgba(59, 130, 246, 0.6)',
          'rgba(96, 165, 250, 0.6)',
          'rgba(52, 211, 153, 0.6)',
          'rgba(255, 153, 102, 0.6)',
          'rgba(252, 211, 77, 0.6)'
        ];
        const borderColors = backgroundColors.map(color => color.replace('0.6', '1'));


        return {
          labels,
          datasets: [{
            label: results.columns.find(col => col.key === dataKey)?.label || dataKey,
            data: dataValues,
            backgroundColor: displayType === 'pie' ? backgroundColors.slice(0, dataValues.length) : backgroundColors[0],
            borderColor: displayType === 'pie' ? borderColors.slice(0, dataValues.length) : borderColors[0],
            borderWidth: 1,
            fill: displayType === 'line', // Fill area under line chart
            tension: 0.1 // Slight curve for line chart
          }]
        };
      }, [results, displayType]);

       const chartOptions = useMemo(() => ({
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: `分析结果可视化 (${results?.summary?.filterDescription || '无筛选'})`,
            font: { size: 16 }
          },
          tooltip: {
            callbacks: {
               label: function(context) {
                    let label = context.dataset.label || '';
                    if (label) {
                        label += ': ';
                    }
                    if (context.parsed.y !== null && displayType !== 'pie') {
                        label += new Intl.NumberFormat('zh-CN').format(context.parsed.y);
                    }
                     if (context.parsed !== null && displayType === 'pie') {
                         label += new Intl.NumberFormat('zh-CN').format(context.parsed);
                     }

                     // Add proportion for pie chart if available
                     if (displayType === 'pie' && results?.columns?.find(c => c.key === 'proportion')) {
                         const item = results.data[context.dataIndex];
                         if(item && item.proportion){
                             label += ` (${(item.proportion * 100).toFixed(1)}%)`;
                         }
                     }
                    return label;
                }
            }
          }
        },
        scales: (displayType !== 'pie') ? {
            x: {
                title: { display: true, text: results?.columns?.[0]?.label || 'Category' },
                grid: { display: false }
            },
            y: {
                title: { display: true, text: results?.columns?.find(col => col.key !== results.columns[0]?.key)?.label || 'Value' },
                beginAtZero: true
            }
        } : undefined
      }), [results, displayType]);


      if (!results) {
        return <div className="p-6 text-center text-muted-foreground">请先运行分析。</div>;
      }

      return (
        <Card className="mt-4">
          <CardHeader>
            <CardTitle className="flex justify-between items-center">
                <span>分析结果: {results.summary?.dataSource} ({results.summary?.analysisType})</span>
                <div className="flex items-center space-x-2">
                    <Button variant={displayType === 'table' ? "default" : "outline"} size="sm" onClick={() => setDisplayType('table')}><TableIcon className="h-4 w-4 mr-1" /> 表格</Button>
                    <Button variant={displayType === 'bar' ? "default" : "outline"} size="sm" onClick={() => setDisplayType('bar')} disabled={!chartData}><BarChart className="h-4 w-4 mr-1" /> 柱状图</Button>
                    <Button variant={displayType === 'line' ? "default" : "outline"} size="sm" onClick={() => setDisplayType('line')} disabled={!chartData}><LineChart className="h-4 w-4 mr-1" /> 折线图</Button>
                    <Button variant={displayType === 'pie' ? "default" : "outline"} size="sm" onClick={() => setDisplayType('pie')} disabled={!chartData}><PieChart className="h-4 w-4 mr-1" /> 饼图</Button>
                    <AlertDialog>
                      <AlertDialogTrigger asChild>
                         <Button size="sm" variant="secondary" disabled={isLoadingReport}>
                            {isLoadingReport ? <RefreshCw className="h-4 w-4 mr-2 animate-spin" /> : <FileDown className="h-4 w-4 mr-1" />}
                            生成报告
                          </Button>
                      </AlertDialogTrigger>
                      <AlertDialogContent>
                        <AlertDialogHeader>
                          <AlertDialogTitle>确认生成报告?</AlertDialogTitle>
                          <AlertDialogDescription>
                            将根据当前分析参数和筛选条件生成报告记录。选择报告格式：
                             <Select value={reportFormat} onValueChange={setReportFormat}>
                                <SelectTrigger className="mt-2">
                                    <SelectValue placeholder="选择格式" />
                                </SelectTrigger>
                                <SelectContent>
                                    <SelectItem value="PDF">PDF</SelectItem>
                                    <SelectItem value="Excel">Excel</SelectItem>
                                    <SelectItem value="CSV">CSV</SelectItem>
                                </SelectContent>
                            </Select>
                          </AlertDialogDescription>
                        </AlertDialogHeader>
                        <AlertDialogFooter>
                          <AlertDialogCancel>取消</AlertDialogCancel>
                          <AlertDialogAction onClick={() => onGenerateReport(reportFormat)} disabled={isLoadingReport}>
                            {isLoadingReport ? "生成中..." : "确认生成"}
                          </AlertDialogAction>
                        </AlertDialogFooter>
                      </AlertDialogContent>
                    </AlertDialog>

                </div>
            </CardTitle>
            <CardDescription>
              {results.summary?.filterDescription ? `筛选条件: ${results.summary.filterDescription}` : '无筛选条件'}
              <br/>
              {/* Display other summary points */}
              {Object.entries(results.summary)
                .filter(([key]) => !['filterDescription', 'dataSource', 'analysisType'].includes(key))
                .map(([key, value]) => (
                  <span key={key} className="mr-4 text-sm text-muted-foreground">
                    <span className="font-medium text-foreground">{key}:</span> {typeof value === 'number' ? value.toLocaleString() : value}
                  </span>
              ))}
            </CardDescription>
          </CardHeader>
          <CardContent>
            {displayType === 'table' && (
              <div className="max-h-[400px] overflow-auto">
                 <Table>
                  <TableHeader className="sticky top-0 bg-background">
                    <TableRow>
                      {results.columns?.map(col => <TableHead key={col.key}>{col.label}</TableHead>)}
                    </TableRow>
                  </TableHeader>
                  <TableBody>
                    {results.data?.length > 0 ? (
                      results.data.map((row, rowIndex) => (
                        <TableRow key={rowIndex}>
                          {results.columns.map(col => (
                            <TableCell key={`${rowIndex}-${col.key}`}>
                                {typeof row[col.key] === 'number' ? row[col.key].toLocaleString() : row[col.key]}
                                {/* Special formatting for proportion */}
                                {col.key === 'proportion' && typeof row[col.key] === 'number' ? ` (${(row[col.key]*100).toFixed(1)}%)` : ''}
                            </TableCell>
                          ))}
                        </TableRow>
                      ))
                    ) : (
                      <TableRow>
                        <TableCell colSpan={results.columns?.length || 1} className="text-center h-24 text-muted-foreground">无数据</TableCell>
                      </TableRow>
                    )}
                  </TableBody>
                </Table>
              </div>
            )}
            {displayType !== 'table' && chartData && (
                <div className="h-[400px] w-full relative">
                    {displayType === 'bar' && <Bar options={chartOptions} data={chartData} />}
                    {displayType === 'line' && <Line options={chartOptions} data={chartData} />}
                    {displayType === 'pie' && <Pie options={chartOptions} data={chartData} />}
                </div>
            )}
             {displayType !== 'table' && !chartData && (
                <div className="h-[400px] flex items-center justify-center text-muted-foreground">无法生成图表，请检查数据或分析类型。</div>
             )}
          </CardContent>
        </Card>
      );
    };

    // Report Management Table Component
    const ReportManagementTable = ({ reports, totalCount, pagination, onPageChange, onSortChange, onDelete, onRename, onDownload }) => {
        const { toast } = useToast();
        const [renamingReport, setRenamingReport] = useState(null);
        const [newName, setNewName] = useState("");
        const [isRenaming, setIsRenaming] = useState(false);
        const [isDeleting, setIsDeleting] = useState(false);
        const [deletingReportId, setDeletingReportId] = useState(null);

        const totalPages = Math.ceil(totalCount / pagination.limit);

        const handleRenameClick = (report) => {
            setRenamingReport(report);
            setNewName(report.name);
        };

        const handleRenameConfirm = async () => {
            if (!renamingReport || !newName.trim()) return;
            setIsRenaming(true);
            try {
                await onRename(renamingReport.reportId, newName.trim());
                toast({ title: "成功", description: `报告 "${renamingReport.name}" 已重命名为 "${newName.trim()}".` });
                setRenamingReport(null);
                setNewName("");
            } catch (error) {
                 toast({ variant: "destructive", title: "错误", description: `重命名报告失败: ${error.message}` });
            } finally {
                 setIsRenaming(false);
            }
        };

         const handleDeleteClick = (reportId) => {
            setDeletingReportId(reportId);
        };

        const handleDeleteConfirm = async () => {
            if (!deletingReportId) return;
            setIsDeleting(true);
            try {
                await onDelete(deletingReportId);
                toast({ title: "成功", description: "报告已删除。" });
                setDeletingReportId(null); // Close dialog implicitly
            } catch (error) {
                 toast({ variant: "destructive", title: "错误", description: `删除报告失败: ${error.message}` });
            } finally {
                setIsDeleting(false);
                // Ensure dialog closes even on error if needed, depends on AlertDialog behavior
                // Might need explicit state control for the dialog open/closed status
            }
        };

         const handleDownloadClick = async (reportId) => {
             toast({ title: "处理中", description: "正在生成下载链接..." });
             try {
                 const result = await onDownload(reportId);
                 if (result.success) {
                     // Simulate download click
                     const link = document.createElement('a');
                     link.href = result.url; // Use the simulated URL
                     link.setAttribute('download', result.fileName || `report_${reportId}`); // Use suggested filename
                     document.body.appendChild(link);
                     link.click();
                     document.body.removeChild(link);
                     toast({ title: "成功", description: "下载已开始 (模拟)。" });
                 } else {
                     toast({ variant: "destructive", title: "错误", description: result.message || "无法获取下载链接。" });
                 }
             } catch (error) {
                  toast({ variant: "destructive", title: "错误", description: `获取下载链接失败: ${error.message}` });
             }
        };


        return (
            <Card>
                <CardContent className="p-0">
                    <div className="overflow-x-auto">
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead onClick={() => onSortChange('name')} className="cursor-pointer">报告名称</TableHead>
                                    <TableHead onClick={() => onSortChange('format')} className="cursor-pointer">格式</TableHead>
                                    <TableHead onClick={() => onSortChange('analysisType')} className="cursor-pointer">分析类型</TableHead>
                                    <TableHead onClick={() => onSortChange('dataSourceId')} className="cursor-pointer">数据源</TableHead>
                                    <TableHead onClick={() => onSortChange('generatedAt')} className="cursor-pointer">生成时间</TableHead>
                                    <TableHead onClick={() => onSortChange('userId')} className="cursor-pointer">创建者</TableHead>
                                    <TableHead>操作</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {reports && reports.length > 0 ? (
                                    reports.map((report) => (
                                        <TableRow key={report.reportId}>
                                            <TableCell className="font-medium">{report.name}</TableCell>
                                            <TableCell>{report.format}</TableCell>
                                            <TableCell>{report.analysisType}</TableCell>
                                            <TableCell>{report.dataSourceId}</TableCell>
                                            <TableCell>{new Date(report.generatedAt).toLocaleString('zh-CN')}</TableCell>
                                            <TableCell>{report.userId}</TableCell>
                                            <TableCell className="space-x-1">
                                                 <Button variant="ghost" size="icon" onClick={() => handleDownloadClick(report.reportId)}>
                                                    <Download className="h-4 w-4" />
                                                </Button>
                                                 <AlertDialog open={renamingReport?.reportId === report.reportId} onOpenChange={(isOpen) => !isOpen && setRenamingReport(null)}>
                                                    <AlertDialogTrigger asChild>
                                                        <Button variant="ghost" size="icon" onClick={() => handleRenameClick(report)}>
                                                            <Edit className="h-4 w-4" />
                                                        </Button>
                                                    </AlertDialogTrigger>
                                                    <AlertDialogContent>
                                                        <AlertDialogHeader>
                                                            <AlertDialogTitle>重命名报告</AlertDialogTitle>
                                                            <AlertDialogDescription>
                                                                输入报告的新名称: "{renamingReport?.name}"
                                                            </AlertDialogDescription>
                                                             <Input
                                                                value={newName}
                                                                onChange={(e) => setNewName(e.target.value)}
                                                                placeholder="新报告名称"
                                                                className="mt-2"
                                                            />
                                                        </AlertDialogHeader>
                                                        <AlertDialogFooter>
                                                            <AlertDialogCancel onClick={() => setRenamingReport(null)}>取消</AlertDialogCancel>
                                                            <AlertDialogAction onClick={handleRenameConfirm} disabled={!newName.trim() || isRenaming}>
                                                                 {isRenaming ? <RefreshCw className="h-4 w-4 animate-spin mr-2"/> : null} 保存
                                                            </AlertDialogAction>
                                                        </AlertDialogFooter>
                                                    </AlertDialogContent>
                                                </AlertDialog>

                                                <AlertDialog open={deletingReportId === report.reportId} onOpenChange={(isOpen) => !isOpen && setDeletingReportId(null)}>
                                                    <AlertDialogTrigger asChild>
                                                        <Button variant="ghost" size="icon" className="text-error-red hover:text-destructive" onClick={() => handleDeleteClick(report.reportId)}>
                                                            <Trash2 className="h-4 w-4" />
                                                        </Button>
                                                    </AlertDialogTrigger>
                                                    <AlertDialogContent>
                                                        <AlertDialogHeader>
                                                            <AlertDialogTitle>确认删除报告?</AlertDialogTitle>
                                                            <AlertDialogDescription>
                                                                此操作无法撤销。您确定要删除报告 "{report.name}" 吗?
                                                            </AlertDialogDescription>
                                                        </AlertDialogHeader>
                                                        <AlertDialogFooter>
                                                             <AlertDialogCancel onClick={() => setDeletingReportId(null)}>取消</AlertDialogCancel>
                                                            <AlertDialogAction
                                                                onClick={handleDeleteConfirm}
                                                                disabled={isDeleting}
                                                                className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
                                                             >
                                                               {isDeleting ? <RefreshCw className="h-4 w-4 animate-spin mr-2"/> : null} 删除
                                                            </AlertDialogAction>
                                                        </AlertDialogFooter>
                                                    </AlertDialogContent>
                                                </AlertDialog>
                                            </TableCell>
                                        </TableRow>
                                    ))
                                ) : (
                                    <TableRow>
                                        <TableCell colSpan={7} className="text-center h-24 text-muted-foreground">未找到报告记录。</TableCell>
                                    </TableRow>
                                )}
                            </TableBody>
                        </Table>
                    </div>
                </CardContent>
                {totalPages > 1 && (
                    <CardFooter className="flex justify-center pt-4">
                        <Pagination>
                            <PaginationContent>
                                <PaginationItem>
                                    <PaginationPrevious href="#" onClick={(e) => { e.preventDefault(); onPageChange(pagination.page - 1); }} aria-disabled={pagination.page <= 1} tabIndex={pagination.page <= 1 ? -1 : undefined}
                 className={pagination.page <= 1 ? "pointer-events-none opacity-50" : undefined} />
                                </PaginationItem>
                                 {/* Simple pagination display - enhance if needed */}
                                <PaginationItem>
                                    <PaginationLink href="#" isActive>
                                        {pagination.page}
                                    </PaginationLink>
                                </PaginationItem>
                                 {totalPages > pagination.page && (
                                     <PaginationItem>
                                         <PaginationLink href="#" onClick={(e) => { e.preventDefault(); onPageChange(pagination.page + 1); }}>
                                             {pagination.page + 1}
                                         </PaginationLink>
                                     </PaginationItem>
                                 )}
                                 {totalPages > pagination.page + 1 && <PaginationItem><PaginationEllipsis /></PaginationItem>}
                                <PaginationItem>
                                    <PaginationNext href="#" onClick={(e) => { e.preventDefault(); onPageChange(pagination.page + 1); }} aria-disabled={pagination.page >= totalPages} tabIndex={pagination.page >= totalPages ? -1 : undefined}
                 className={pagination.page >= totalPages ? "pointer-events-none opacity-50" : undefined} />
                                </PaginationItem>
                            </PaginationContent>
                        </Pagination>
                    </CardFooter>
                )}
            </Card>
        );
    };


    // Main Page Component
    export default function DATAANALYSISREPORT() {
      const [activeTab, setActiveTab] = useState("analysis"); // analysis, management
      const [dataSources, setDataSources] = useState([]);
      const [selectedDataSource, setSelectedDataSource] = useState('');
      const [appliedFilters, setAppliedFilters] = useState({});
      const [analysisType, setAnalysisType] = useState('trend'); // 'trend', 'regional', 'distribution', 'timeseries'
      const [analysisParams, setAnalysisParams] = useState({ valueMetric: 'value', timeDimension: 'month' }); // Example params
      const [analysisResults, setAnalysisResults] = useState(null);
      const [isLoadingAnalysis, setIsLoadingAnalysis] = useState(false);
      const [isLoadingSources, setIsLoadingSources] = useState(true);
      const [isLoadingReportGeneration, setIsLoadingReportGeneration] = useState(false);

      // Report Management State
      const [reports, setReports] = useState([]);
      const [reportTotalCount, setReportTotalCount] = useState(0);
      const [reportPagination, setReportPagination] = useState({ page: 1, limit: 10 });
      const [reportSort, setReportSort] = useState({ sortBy: 'generatedAt', sortOrder: 'desc' });
      const [reportFilters, setReportFilters] = useState({ filterByName: '' }); // Add other filters if needed
      const [isLoadingReports, setIsLoadingReports] = useState(false);

      const { toast } = useToast();

      // Fetch Data Sources
      useEffect(() => {
        setIsLoadingSources(true);
        getAnalysisDataSources()
          .then(setDataSources)
          .catch(error => {
            console.error("Error fetching data sources:", error);
            toast({ variant: "destructive", title: "错误", description: "加载数据源列表失败。" });
          })
          .finally(() => setIsLoadingSources(false));
      }, [toast]);

       // Fetch Reports List
       const fetchReports = useCallback(() => {
        setIsLoadingReports(true);
        const params = {
          ...reportPagination,
          ...reportSort,
          ...reportFilters,
        };
        getReportList(params)
          .then(data => {
            setReports(data.reports);
            setReportTotalCount(data.totalCount);
          })
          .catch(error => {
            console.error("Error fetching reports:", error);
            toast({ variant: "destructive", title: "错误", description: "加载报告列表失败。" });
          })
          .finally(() => setIsLoadingReports(false));
      }, [reportPagination, reportSort, reportFilters, toast]);

      // Fetch reports when tab is active or dependencies change
      useEffect(() => {
        if (activeTab === 'management') {
          fetchReports();
        }
      }, [activeTab, fetchReports]);


      const handleFilterChange = useCallback((filters) => {
        setAppliedFilters(filters);
      }, []);

      const handleRunAnalysis = () => {
        if (!selectedDataSource || !analysisType) {
          toast({ variant: "warning", title: "提示", description: "请选择数据源和分析类型。" });
          return;
        }
        setIsLoadingAnalysis(true);
        setAnalysisResults(null); // Clear previous results
        runDataAnalysis({
          dataSourceId: selectedDataSource,
          analysisType,
          parameters: analysisParams,
          filters: appliedFilters,
        })
          .then(results => {
            setAnalysisResults(results);
             toast({ title: "成功", description: `已完成 ${analysisType} 分析。` });
          })
          .catch(error => {
            console.error("Error running analysis:", error);
            toast({ variant: "destructive", title: "错误", description: `执行分析失败: ${error.message}` });
            setAnalysisResults({ summary: { error: `分析失败: ${error.message}` }, data: [], columns: [] }); // Show error in results area
          })
          .finally(() => setIsLoadingAnalysis(false));
      };

       const handleGenerateReport = async (format = 'PDF') => {
            if (!analysisResults || !selectedDataSource || !analysisType) {
                toast({ variant: "warning", title: "提示", description: "请先成功运行一次分析。" });
                return;
            }
            setIsLoadingReportGeneration(true);
            try {
                // Generate a default name (can be customized later)
                 const reportName = `${new Date().toLocaleDateString('zh-CN')} ${dataSources.find(ds=>ds.id === selectedDataSource)?.name} ${analysisType}分析报告`;

                const result = await generateAndSaveReport({
                    name: reportName,
                    format: format,
                    analysisType: analysisType,
                    dataSourceId: selectedDataSource,
                    parameters: analysisParams,
                    filters: appliedFilters,
                    userId: 'user123' // Replace with actual logged-in user ID
                });
                 toast({ title: "成功", description: `报告元数据已保存 (ID: ${result.reportId?.substring(0, 8)}...). 可在报告管理中查看。` });
                 // Optionally switch to management tab or refresh list if already there
                 if (activeTab === 'management') {
                     fetchReports(); // Refresh the list
                 } else {
                     setActiveTab('management'); // Switch to management tab
                 }

            } catch (error) {
                 console.error("Error generating report:", error);
                toast({ variant: "destructive", title: "错误", description: `生成报告失败: ${error.message}` });
            } finally {
                setIsLoadingReportGeneration(false);
            }
        };

         // --- Report Management Handlers ---
        const handleReportPageChange = (newPage) => {
            setReportPagination(prev => ({ ...prev, page: newPage }));
        };

        const handleReportSortChange = (columnKey) => {
            setReportSort(prev => ({
                sortBy: columnKey,
                sortOrder: prev.sortBy === columnKey && prev.sortOrder === 'desc' ? 'asc' : 'desc'
            }));
        };

         const handleReportDelete = async (reportId) => {
            // Confirmation is handled within ReportManagementTable via AlertDialog
            await deleteReport(reportId);
            fetchReports(); // Refresh list after delete
        };

         const handleReportRename = async (reportId, newName) => {
             // Confirmation is handled within ReportManagementTable via AlertDialog
            await updateReport(reportId, { name: newName });
            fetchReports(); // Refresh list after rename
        };

        const handleReportDownload = async (reportId) => {
            // Logic handled within ReportManagementTable
            return getReportDownloadLink(reportId);
        };

         const handleReportFilterChange = (e) => {
            setReportFilters(prev => ({ ...prev, filterByName: e.target.value }));
            // Optional: Add debounce here if needed
            // Reset page to 1 when filtering
            setReportPagination(prev => ({ ...prev, page: 1 }));
        };

         // Trigger fetchReports when filter changes (after potential debounce)
         useEffect(() => {
             if (activeTab === 'management') {
                 const timer = setTimeout(() => {
                     fetchReports();
                 }, 500); // Debounce filter input
                 return () => clearTimeout(timer);
             }
         }, [reportFilters.filterByName, activeTab, fetchReports]);


      return (
        <div className="p-4 md:p-6 flex flex-col h-full">
          <h1 className="text-2xl font-semibold mb-4 text-primary flex items-center">
            <BarChart3 className="h-6 w-6 mr-2" />数据分析与报告
          </h1>
           <Tabs value={activeTab} onValueChange={setActiveTab} className="flex-1 flex flex-col">
             <TabsList className="mb-4 self-start">
               <TabsTrigger value="analysis" className="text-base px-4 py-2">
                  <Settings className="h-4 w-4 mr-2" />分析配置与执行
                </TabsTrigger>
               <TabsTrigger value="management" className="text-base px-4 py-2">
                  <FileText className="h-4 w-4 mr-2" />报告管理
               </TabsTrigger>
               {/* <TabsTrigger value="export" className="text-base px-4 py-2"><FileDown className="h-4 w-4 mr-2" />报表导出</TabsTrigger> */}
             </TabsList>

             <TabsContent value="analysis" className="flex-1 flex flex-row gap-4 overflow-hidden">
                {/* Left Sidebar: Data Source & Filters */}
                <Card className="w-1/4 flex flex-col overflow-hidden min-w-[300px]">
                     <CardHeader className="p-4 border-b">
                        <CardTitle className="text-lg flex items-center"><Database className="h-5 w-5 mr-2 text-primary"/> 数据源选择</CardTitle>
                     </CardHeader>
                     <CardContent className="p-4 flex-1 flex flex-col overflow-y-auto">
                        {isLoadingSources ? (
                             <div className="text-center p-4"><RefreshCw className="h-4 w-4 animate-spin mr-2 inline" /> 加载中...</div>
                         ) : (
                             <Select onValueChange={setSelectedDataSource} value={selectedDataSource}>
                                 <SelectTrigger className="w-full mb-4">
                                     <SelectValue placeholder="请选择数据源..." />
                                 </SelectTrigger>
                                 <SelectContent>
                                     {dataSources.map(ds => (
                                         <SelectItem key={ds.id} value={ds.id}>{ds.name}</SelectItem>
                                     ))}
                                 </SelectContent>
                             </Select>
                         )}
                        <div className="flex-1 overflow-y-auto border-t -mx-4">
                            <FilterPanel dataSourceId={selectedDataSource} onFilterChange={handleFilterChange} />
                        </div>
                     </CardContent>
                </Card>

                 {/* Main Content: Analysis Config & Results */}
                 <div className="flex-1 flex flex-col overflow-hidden">
                     <Card className="mb-4">
                         <CardHeader>
                             <CardTitle className="text-lg flex items-center"><Workflow className="h-5 w-5 mr-2 text-primary"/> 分析配置</CardTitle>
                         </CardHeader>
                         <CardContent className="flex flex-wrap items-end gap-4">
                             <div className="flex-1 min-w-[200px]">
                                 <Label htmlFor="analysisType">分析类型</Label>
                                 <Select id="analysisType" value={analysisType} onValueChange={setAnalysisType}>
                                     <SelectTrigger>
                                         <SelectValue placeholder="选择分析类型" />
                                     </SelectTrigger>
                                     <SelectContent>
                                         <SelectItem value="trend">趋势分析</SelectItem>
                                         <SelectItem value="timeseries">时间序列分析</SelectItem>
                                         <SelectItem value="regional">区域分布分析</SelectItem>
                                         <SelectItem value="distribution">构成/分布分析</SelectItem>
                                     </SelectContent>
                                 </Select>
                             </div>
                             {/* Add more parameter inputs based on analysisType if needed */}
                             <div className="flex-1 min-w-[150px]">
                                <Label htmlFor="valueMetric">度量指标</Label>
                                <Input id="valueMetric" placeholder="e.g., total_cost" value={analysisParams.valueMetric || ''} onChange={e => setAnalysisParams(p => ({...p, valueMetric: e.target.value}))} />
                             </div>
                             {(analysisType === 'trend' || analysisType === 'timeseries') && (
                                  <div className="flex-1 min-w-[150px]">
                                     <Label htmlFor="timeDimension">时间维度</Label>
                                      <Select id="timeDimension" value={analysisParams.timeDimension || 'month'} onValueChange={val => setAnalysisParams(p => ({...p, timeDimension: val}))}>
                                         <SelectTrigger>
                                             <SelectValue placeholder="选择时间维度" />
                                         </SelectTrigger>
                                         <SelectContent>
                                             <SelectItem value="month">月</SelectItem>
                                             <SelectItem value="year">年</SelectItem>
                                             {/* <SelectItem value="day">日</SelectItem> */}
                                         </SelectContent>
                                     </Select>
                                  </div>
                             )}

                             <Button onClick={handleRunAnalysis} disabled={isLoadingAnalysis || !selectedDataSource}>
                                {isLoadingAnalysis ? <RefreshCw className="h-4 w-4 mr-2 animate-spin" /> : <Play className="h-4 w-4 mr-2" />}
                                运行分析
                            </Button>
                         </CardContent>
                     </Card>

                      <div className="flex-1 overflow-y-auto">
                         {isLoadingAnalysis ? (
                             <div className="flex items-center justify-center h-full text-muted-foreground">
                                 <RefreshCw className="h-5 w-5 animate-spin mr-2" /> 分析正在运行中...
                             </div>
                         ) : (
                             <AnalysisResultsDisplay
                                 results={analysisResults}
                                 onGenerateReport={handleGenerateReport}
                                 isLoadingReport={isLoadingReportGeneration}
                                 />
                         )}
                     </div>
                 </div>
             </TabsContent>

             <TabsContent value="management" className="flex-1 flex flex-col overflow-hidden">
                 <Card className="flex-1 flex flex-col">
                     <CardHeader>
                         <CardTitle className="text-lg flex justify-between items-center">
                            <span>历史分析报告</span>
                             <div className="flex items-center space-x-2 w-1/3">
                                 <Input
                                     placeholder="按名称搜索报告..."
                                     value={reportFilters.filterByName}
                                     onChange={handleReportFilterChange}
                                     className="max-w-sm"
                                 />
                                 <Button variant="outline" size="icon" onClick={fetchReports} disabled={isLoadingReports}>
                                     <RefreshCw className={`h-4 w-4 ${isLoadingReports ? 'animate-spin' : ''}`} />
                                 </Button>
                             </div>
                         </CardTitle>
                     </CardHeader>
                     <CardContent className="flex-1 overflow-y-auto p-0">
                         {isLoadingReports ? (
                             <div className="flex items-center justify-center h-full text-muted-foreground">
                                 <RefreshCw className="h-5 w-5 animate-spin mr-2" /> 加载报告列表中...
                             </div>
                         ) : (
                            <ReportManagementTable
                                reports={reports}
                                totalCount={reportTotalCount}
                                pagination={reportPagination}
                                onPageChange={handleReportPageChange}
                                onSortChange={handleReportSortChange}
                                onDelete={handleReportDelete}
                                onRename={handleReportRename}
                                onDownload={handleReportDownload}
                                />
                         )}
                     </CardContent>
                 </Card>
             </TabsContent>

             {/* <TabsContent value="export" className="flex-1">
                <Card>
                  <CardHeader><CardTitle>报表导出</CardTitle></CardHeader>
                  <CardContent><p>此功能待实现。通常在此处选择报告模板、配置导出内容并生成文件。</p></CardContent>
                </Card>
              </TabsContent> */}
           </Tabs>
           <Toaster />
        </div>
      );
    }
    
    """;

string isseurPrompt = "1. 左侧《数据源选择》看板需要再宽一点。2.左侧《数据源选择》的高度应该有限制，里面的内容应该用 scroll 可以查看到。具体高度可以是比如 700px";

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7009/api/code-fixing/single-file");
client.Timeout = TimeSpan.FromSeconds(600);
var content = new StringContent(JsonConvert.SerializeObject(new
{
    FileCode = code,
    RequirementPrompt = isseurPrompt,
}), null, "application/json");

request.Content = content;
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();
string responseContent = await response.Content.ReadAsStringAsync();

// Save the response content to a file
string outputPath = "response_content.json";
File.WriteAllText(outputPath, responseContent);
Console.WriteLine($"Response content saved to {outputPath}");
